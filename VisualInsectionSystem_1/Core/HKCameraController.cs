using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MvCameraControl;
using NLog;

namespace VisionInspectionSystem
{
    public class HKCameraController : ICameraController
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private MyCamera _camera;
        private bool _isSdkInitialized;
        private bool _isCapturing;
        private readonly object _lockObj = new object();
        private string _defaultImageSaveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapturedImages");

        // 事件实现
        public event EventHandler<CameraInitializedEventArgs> CameraInitialized;
        public event EventHandler<CameraScannedEventArgs> CameraScanned;
        public event EventHandler<CameraConnectionChangedEventArgs> CameraConnected;
        public event EventHandler<CameraConnectionChangedEventArgs> CameraDisconnected;
        public event EventHandler<CaptureResultEventArgs> CameraCaptured;
        public event EventHandler<CameraErrorEventArgs> CameraErrorOccurred;

        // 属性
        public bool IsCameraConnected => _camera != null && _camera.IsOpen();
        public CameraInfo CurrentCamera { get; private set; }
        public bool IsCapturing => _isCapturing;

        // SDK初始化
        public bool InitializeSDK()
        {
            try
            {
                lock (_lockObj)
                {
                    if (_isSdkInitialized) return true;

                    var sdkVersion = SDKSystem.GetSDKVersion();
                    var initResult = SDKSystem.Initialize();
                    _isSdkInitialized = initResult == 0;

                    _logger.Info($"SDK初始化{(initResult == 0 ? "成功" : "失败")}，版本：{sdkVersion}");
                    CameraInitialized?.Invoke(this, new CameraInitializedEventArgs
                    {
                        IsSuccess = _isSdkInitialized,
                        SdkVersion = sdkVersion
                    });

                    // 创建默认图像存储目录
                    if (!Directory.Exists(_defaultImageSaveDir))
                        Directory.CreateDirectory(_defaultImageSaveDir);

                    return _isSdkInitialized;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SDK初始化异常");
                OnCameraError("InitializeSDK", -1, ex.Message);
                return false;
            }
        }

        // SDK反初始化
        public bool FinalizeSDK()
        {
            try
            {
                lock (_lockObj)
                {
                    if (!_isSdkInitialized) return true;

                    // 先断开相机
                    if (IsCameraConnected)
                        DisconnectCamera();

                    var result = SDKSystem.Finalize();
                    _isSdkInitialized = result != 0;
                    _logger.Info($"SDK反初始化{(result == 0 ? "成功" : "失败")}");
                    return result == 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SDK反初始化异常");
                OnCameraError("FinalizeSDK", -1, ex.Message);
                return false;
            }
        }

        // 枚举相机
        public List<CameraInfo> ScanCameras()
        {
            var cameraList = new List<CameraInfo>();
            try
            {
                if (!_isSdkInitialized)
                {
                    OnCameraError("ScanCameras", -2, "SDK未初始化");
                    return cameraList;
                }

                var deviceEnum = new DeviceEnumerator();
                var gigEDevices = new List<MV_GIGE_DEVICE_INFO>();
                var usbDevices = new List<MV_USB_DEVICE_INFO>();

                // 枚举GigE相机
                var gigECount = deviceEnum.EnumGigEDevices(gigEDevices);
                foreach (var dev in gigEDevices)
                {
                    cameraList.Add(new CameraInfo
                    {
                        SerialNumber = dev.chSerialNumber,
                        IpAddress = dev.chIpAddress,
                        Model = dev.chModelName,
                        GigEInfo = dev,
                        DeviceType = DeviceType.GigE
                    });
                }

                // 枚举USB相机
                var usbCount = deviceEnum.EnumUSBDevices(usbDevices);
                foreach (var dev in usbDevices)
                {
                    cameraList.Add(new CameraInfo
                    {
                        SerialNumber = dev.chSerialNumber,
                        Model = dev.chModelName,
                        UsbInfo = dev,
                        DeviceType = DeviceType.USB
                    });
                }

                _logger.Info($"枚举相机完成：GigE({gigECount})台，USB({usbCount})台，总计{cameraList.Count}台");
                CameraScanned?.Invoke(this, new CameraScannedEventArgs { CameraList = cameraList });
                return cameraList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "枚举相机异常");
                OnCameraError("ScanCameras", -1, ex.Message);
                return cameraList;
            }
        }

        // 连接相机
        public bool ConnectCamera(CameraInfo cameraInfo)
        {
            try
            {
                lock (_lockObj)
                {
                    if (cameraInfo == null)
                    {
                        OnCameraError("ConnectCamera", -3, "相机信息为空");
                        return false;
                    }

                    // 断开已有连接
                    if (IsCameraConnected)
                        DisconnectCamera();

                    // 创建相机实例
                    _camera = DeviceFactory.CreateDevice(cameraInfo.DeviceType == DeviceType.GigE
                        ? (object)cameraInfo.GigEInfo
                        : cameraInfo.UsbInfo);

                    if (_camera == null)
                    {
                        OnCameraError("ConnectCamera", -4, "创建相机实例失败");
                        return false;
                    }

                    // 打开相机
                    var openResult = _camera.Open();
                    if (openResult != 0)
                    {
                        OnCameraError("ConnectCamera", openResult, $"打开相机失败，错误码：{openResult}");
                        _camera = null;
                        return false;
                    }

                    // 差异化配置
                    if (cameraInfo.DeviceType == DeviceType.GigE)
                    {
                        // GigE相机配置最佳包大小
                        _camera.SetIntValue("GevSCPSPacketSize", _camera.GetOptimalPacketSize());
                        _logger.Info($"GigE相机最佳包大小配置完成：{_camera.GetOptimalPacketSize()}");
                    }
                    else if (cameraInfo.DeviceType == DeviceType.USB)
                    {
                        // USB相机配置超时
                        _camera.SetIntValue("USBTimeout", 1000);
                        _logger.Info("USB相机超时配置完成：1000ms");
                    }

                    CurrentCamera = cameraInfo;
                    _logger.Info($"相机连接成功：{cameraInfo.SerialNumber}({cameraInfo.Model})");
                    CameraConnected?.Invoke(this, new CameraConnectionChangedEventArgs
                    {
                        CameraInfo = cameraInfo,
                        IsConnected = true,
                        ErrorCode = 0
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "连接相机异常");
                OnCameraError("ConnectCamera", -1, ex.Message);
                CameraConnected?.Invoke(this, new CameraConnectionChangedEventArgs
                {
                    CameraInfo = cameraInfo,
                    IsConnected = false,
                    ErrorMsg = ex.Message,
                    ErrorCode = -1
                });
                return false;
            }
        }

        // 断开相机
        public bool DisconnectCamera()
        {
            try
            {
                lock (_lockObj)
                {
                    if (!IsCameraConnected) return true;

                    // 停止采集
                    if (_isCapturing)
                        StopContinuousCapture();

                    // 关闭相机
                    var closeResult = _camera.Close();
                    var isSuccess = closeResult == 0;

                    if (isSuccess)
                    {
                        _logger.Info($"相机断开成功：{CurrentCamera.SerialNumber}");
                        CameraDisconnected?.Invoke(this, new CameraConnectionChangedEventArgs
                        {
                            CameraInfo = CurrentCamera,
                            IsConnected = false,
                            ErrorCode = 0
                        });
                    }
                    else
                    {
                        OnCameraError("DisconnectCamera", closeResult, $"关闭相机失败，错误码：{closeResult}");
                    }

                    _camera = null;
                    CurrentCamera = null;
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "断开相机异常");
                OnCameraError("DisconnectCamera", -1, ex.Message);
                CameraDisconnected?.Invoke(this, new CameraConnectionChangedEventArgs
                {
                    CameraInfo = CurrentCamera,
                    IsConnected = false,
                    ErrorMsg = ex.Message,
                    ErrorCode = -1
                });
                return false;
            }
        }

        // 单帧采集（异步）
        public async Task<CaptureResult> CaptureSingleFrameAsync(int timeoutMs = 5000)
        {
            var result = new CaptureResult { CaptureTime = DateTime.Now };
            try
            {
                if (!IsCameraConnected)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "相机未连接";
                    OnCameraError("CaptureSingleFrameAsync", -5, result.ErrorMsg);
                    return result;
                }

                // 异步执行采集
                await Task.Run(() =>
                {
                    lock (_lockObj)
                    {
                        // 开始取流
                        var startResult = _camera.StartGrabbing();
                        if (startResult != 0)
                        {
                            result.IsSuccess = false;
                            result.ErrorMsg = $"开始取流失败，错误码：{startResult}";
                            OnCameraError("CaptureSingleFrameAsync", startResult, result.ErrorMsg);
                            return;
                        }

                        // 获取图像帧
                        var frame = new MV_FRAME_OUT();
                        var getFrameResult = _camera.GetImageBuffer(ref frame, timeoutMs);
                        if (getFrameResult != 0)
                        {
                            result.IsSuccess = false;
                            result.ErrorMsg = $"获取图像失败，错误码：{getFrameResult}";
                            OnCameraError("CaptureSingleFrameAsync", getFrameResult, result.ErrorMsg);
                            _camera.StopGrabbing();
                            return;
                        }

                        // 深拷贝图像数据
                        var clonedFrame = frame.Clone();
                        result.ImageData = clonedFrame.pBufAddr;
                        result.DataLength = clonedFrame.nBufSize;
                        result.IsSuccess = true;

                        // 释放原始缓存
                        _camera.FreeImageBuffer(ref frame);
                        // 停止取流
                        _camera.StopGrabbing();

                        // 生成图像路径
                        var fileName = $"{CurrentCamera.SerialNumber}_{DateTime.Now:yyyyMMddHHmmssfff}.bmp";
                        result.ImagePath = Path.Combine(_defaultImageSaveDir, fileName);
                        // 保存图像
                        SaveImage(result, result.ImagePath);
                    }
                });

                _logger.Info($"单帧采集完成：{result.ImagePath}");
                CameraCaptured?.Invoke(this, new CaptureResultEventArgs { CaptureResult = result });
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                _logger.Error(ex, "单帧采集异常");
                OnCameraError("CaptureSingleFrameAsync", -1, ex.Message);
                return result;
            }
        }

        // 连续采集
        public bool StartContinuousCapture()
        {
            try
            {
                lock (_lockObj)
                {
                    if (!IsCameraConnected || _isCapturing) return false;

                    var startResult = _camera.StartGrabbing();
                    if (startResult != 0)
                    {
                        OnCameraError("StartContinuousCapture", startResult, $"开始连续采集失败，错误码：{startResult}");
                        return false;
                    }

                    _isCapturing = true;
                    _logger.Info("连续采集已启动");
                    // 后台轮询取帧
                    Task.Run(async () =>
                    {
                        while (_isCapturing && IsCameraConnected)
                        {
                            await CaptureSingleFrameAsync(3000);
                            await Task.Delay(10); // 采集间隔
                        }
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "启动连续采集异常");
                OnCameraError("StartContinuousCapture", -1, ex.Message);
                return false;
            }
        }

        // 停止连续采集
        public bool StopContinuousCapture()
        {
            try
            {
                lock (_lockObj)
                {
                    if (!_isCapturing) return true;

                    _isCapturing = false;
                    if (IsCameraConnected)
                    {
                        var stopResult = _camera.StopGrabbing();
                        if (stopResult != 0)
                        {
                            OnCameraError("StopContinuousCapture", stopResult, $"停止连续采集失败，错误码：{stopResult}");
                            return false;
                        }
                    }

                    _logger.Info("连续采集已停止");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "停止连续采集异常");
                OnCameraError("StopContinuousCapture", -1, ex.Message);
                return false;
            }
        }

        // 参数设置（整型）
        public bool SetParameter(string paramName, int value)
        {
            return SetParameterInternal(paramName, value);
        }

        // 参数设置（浮点型）
        public bool SetParameter(string paramName, float value)
        {
            return SetParameterInternal(paramName, value);
        }

        // 参数设置（布尔型）
        public bool SetParameter(string paramName, bool value)
        {
            return SetParameterInternal(paramName, value ? 1 : 0);
        }

        // 参数设置（字符串）
        public bool SetParameter(string paramName, string value)
        {
            return SetParameterInternal(paramName, value);
        }

        // 参数获取
        public T GetParameter<T>(string paramName, T defaultValue = default)
        {
            try
            {
                if (!IsCameraConnected)
                {
                    OnCameraError("GetParameter", -6, "相机未连接");
                    return defaultValue;
                }

                object result = defaultValue;
                if (typeof(T) == typeof(int))
                {
                    int val = 0;
                    var getResult = _camera.GetIntValue(paramName, ref val);
                    result = getResult == 0 ? val : defaultValue;
                }
                else if (typeof(T) == typeof(float))
                {
                    float val = 0;
                    var getResult = _camera.GetFloatValue(paramName, ref val);
                    result = getResult == 0 ? val : defaultValue;
                }
                else if (typeof(T) == typeof(bool))
                {
                    int val = 0;
                    var getResult = _camera.GetIntValue(paramName, ref val);
                    result = getResult == 0 ? (val == 1) : defaultValue;
                }
                else if (typeof(T) == typeof(string))
                {
                    var val = new string('\0', 256);
                    var getResult = _camera.GetStringValue(paramName, val, 256);
                    result = getResult == 0 ? val.Trim('\0') : defaultValue;
                }

                _logger.Debug($"获取参数[{paramName}]：{(result ?? "默认值")}");
                return (T)result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"获取参数[{paramName}]异常");
                OnCameraError("GetParameter", -1, ex.Message);
                return defaultValue;
            }
        }

        // 保存图像
        public bool SaveImage(CaptureResult captureResult, string savePath)
        {
            try
            {
                if (captureResult == null || !captureResult.IsSuccess || captureResult.ImageData == IntPtr.Zero)
                {
                    OnCameraError("SaveImage", -7, "采集结果无效");
                    return false;
                }

                var saver = new ImageSaver();
                var saveResult = saver.SaveImage(captureResult.ImageData, (uint)captureResult.DataLength, savePath);
                if (saveResult != 0)
                {
                    OnCameraError("SaveImage", saveResult, $"保存图像失败，错误码：{saveResult}");
                    return false;
                }

                _logger.Info($"图像保存成功：{savePath}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "保存图像异常");
                OnCameraError("SaveImage", -1, ex.Message);
                return false;
            }
        }

        // 图像显示
        public bool ShowImageInControl(IntPtr controlHandle, CaptureResult captureResult)
        {
            try
            {
                if (controlHandle == IntPtr.Zero || captureResult == null || !captureResult.IsSuccess)
                {
                    OnCameraError("ShowImageInControl", -8, "参数无效");
                    return false;
                }

                var render = new ImageRender();
                var renderResult = render.Render(controlHandle, captureResult.ImageData, (uint)captureResult.DataLength);
                if (renderResult != 0)
                {
                    OnCameraError("ShowImageInControl", renderResult, $"图像显示失败，错误码：{renderResult}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "图像显示异常");
                OnCameraError("ShowImageInControl", -1, ex.Message);
                return false;
            }
        }

        // 资源释放
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 释放托管资源
                DisconnectCamera();
                FinalizeSDK();
            }
            // 释放非托管资源（如果有）
        }

        // 私有辅助方法
        private bool SetParameterInternal(string paramName, object value)
        {
            try
            {
                if (!IsCameraConnected)
                {
                    OnCameraError("SetParameterInternal", -6, "相机未连接");
                    return false;
                }

                int result = -1;
                if (value is int intVal)
                {
                    result = _camera.SetIntValue(paramName, intVal);
                }
                else if (value is float floatVal)
                {
                    result = _camera.SetFloatValue(paramName, floatVal);
                }
                else if (value is string strVal)
                {
                    result = _camera.SetStringValue(paramName, strVal);
                }

                var isSuccess = result == 0;
                if (isSuccess)
                {
                    _logger.Debug($"设置参数[{paramName}] = {value} 成功");
                }
                else
                {
                    OnCameraError("SetParameterInternal", result, $"设置参数[{paramName}]失败，错误码：{result}");
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"设置参数[{paramName}]异常");
                OnCameraError("SetParameterInternal", -1, ex.Message);
                return false;
            }
        }

        // 触发相机错误事件
        private void OnCameraError(string methodName, int errorCode, string errorMsg)
        {
            CameraErrorOccurred?.Invoke(this, new CameraErrorEventArgs
            {
                MethodName = methodName,
                ErrorCode = errorCode,
                ErrorMsg = errorMsg
            });
        }
    }
}
