using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvCameraControl;
using VisualInsectionSystem.Configuration;
using static MvCameraControl.MyCamera;

namespace VisualInsectionSystem.Camera
{
    public class HKCameraController : ICameraController
    {
        private IDevice _device;
        private IStreamGrabber _grabber;
        private bool _isGrabbing;
        private bool _disposed;

        private readonly object _lock = new object();
        private CancellationTokenSource _grabCts;

        public bool IsConnected => _device != null;
        public bool IsGrabbing => _isGrabbing;
        public string CurrentCameraSN {  get; private set; }

        public event EventHandler<CameraErrorEventArgs> CameraError;
        public event EventHandler<CameraErrorEventArgs> CameraErrorOccurred;
        
        private string _connectedCameraSN = "";

        // 图像临时存储路径
        private readonly string _tempImageDirectory;

        public string ConnectedCameraSN => _connectedCameraSN;

        public HKCameraController()
        {
            // 初始化临时图像目录
            _tempImageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempImages");
            if (!Directory.Exists(_tempImageDirectory))
            {
                Directory.CreateDirectory(_tempImageDirectory);
            }
        }

        public bool Initialize()      
        {
            try 
            { 
                int ret = SDKSystem.Initialize(); 
                LogHelper.Info($"SDK初始化: {(ret == 0 ? "成功" : $"失败 0x{ret:X8}")}"); 
                return ret == 0; 
            }
            catch (Exception ex) 
            { 
                OnError("Initialize", -1, ex.Message); 
                return false; 
            }
            try
            {
                // 初始化SDK
                int result = MyCamera.MV_CC_Initialize_NET();               
                if (result == MyCamera.MV_OK)
                {
                    LogHelper.Info("海康SDK初始化成功");       
                    return true;
                }
                else
                {
                    LogHelper.Error($"海康SDK初始化失败，错误码: {result}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"海康SDK初始化异常: {ex.Message}", ex);
                return false;
            }
        }

        public void Shutdown()
        {
            try 
            { 
                SDKSystem.Finalize();                          
                MyCamera.MV_CC_Finalize_NET();
                LogHelper.Info("海康SDK已反初始化");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"海康SDK反初始化失败: {ex.Message}", ex);
            }
        }

        public async Task<List<CameraInfo>> EnumerateDevicesAsync()
        {
            return await Task.Run(() =>
            {
                var list = new List<CameraInfo>();
                try
                {
                    DeviceTLayerType type = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice;
                    List<IDeviceInfo> infos;
                    if (DeviceEnumerator.EnumDevicesEx2(type, SortMethod.SortBySerialNumber, null, out infos) == MvError.MV_OK)
                    {
                        foreach (var info in infos)
                            list.Add(new CameraInfo { SerialNumber = info.SerialNumber, Model = info.ModelName, IPAddress = (info is IGigEDeviceInfo gige) ? IPConvert(gige.CurrentIp) : "", RawInfo = info });
                    }
                }
                catch (Exception ex)
                {
                    OnError("EnumerateDevices", -1, ex.Message);
                }
                return list;
            });
        }

        private string IPConvert(uint ip)
        {
            var bytes = BitConverter.GetBytes(ip);
            return $"{bytes[0]}.{bytes[1]}.{bytes[2]}.{bytes[3]}";
        }

        public async Task<bool> ConnectAsync(string serialNumber)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    if (_device != null)
                    {
                        await DisconnectAsync();
                    }
                    var cameras = await EnumerateDevicesAsync();
                    var info = cameras.FirstOrDefault(c => c.SerialNumber == serialNumber)?.RawInfo;
                    if (info == null) return false;

                    _device = DeviceFactory.CreateDevice(info);
                    if (info == null) return false;

                    int ret = _device.Open();
                    if (ret != MvError.MV_OK)
                    {
                        _device.Dispose();
                        _device = null;
                        return false;
                    }

                    _grabber = _device.StreamGrabber;
                    CurrentCameraSN = serialNumber;
                    if (_device is IGigEDevice gige)
                    {
                        gige.GetOptimalPacketSize(out int ps);
                        if (ps > 0) gige.Parameters.SetIntValue("GevSCPSPacketSize", ps);
                    }
                    if (_device is IUSBDevice usb)
                    {
                        usb.SetSyncTimeOut(1000);
                    }
                    await SetTriggerModeAsync(false);
                    LogHelper.Info($"相机连接成功: {serialNumber}");
                    return true;
                }
                catch (Exception ex)
                {
                    OnError("Connect", -1, ex.Message);
                    return false;
                }
            });
        }

        public async Task DisconnectAsync()
        {
            await Task.Run(async () =>
            {
                if (_isGrabbing)
                {
                    await StopGrabbingAsync();
                }
                _device?.Close();
                _device?.Dispose();
                _device = null;
                _grabber = null;
                CurrentCameraSN = null;
                LogHelper.Info("相机断开连接");
            });
        }

        public async Task<bool> StartGrabbingAsync()
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    if (_isGrabbing || _device == null)
                    {
                        LogHelper.Error("相机未连接，无法启动采集");
                        return false;
                    }
                    _grabber.SetImageNodeNum(5);
                    if (_grabber.StartGrabbing() != MvError.MV_OK)
                    {
                        LogHelper.Error($"启动图像采集失败");
                        return false;
                    }
                    _isGrabbing = true;
                    LogHelper.Info("图像采集启动成功");
                    _grabCts = new CancellationTokenSource();
                    return true;
                }
            });
            //LogHelper.Error($"启动图像采集失败: {ex.Message}", ex);
            //OnCameraErrorOccurred("GRAB_START_ERROR", ex.Message);
            //return false;
        }

        public async Task StopGrabbingAsync()
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    if (!_isGrabbing)
                    {
                        LogHelper.Error($"停止图像采集失败");
                        return;
                    }
                    _grabCts?.Cancel();
                    _grabber?.StopGrabbing();
                    _isGrabbing = false;
                    LogHelper.Info("图像采集停止成功");
                }
            });
        }

        public async Task<FrameCaptureResult> CaptureOneFrameAsync(int timeoutMs = 5000)
        {
            return await Task.Run(() =>
            {
                var result = new FrameCaptureResult();
                if (_device == null)
                {
                    result.ErrorMessage = "相机未连接";
                    LogHelper.Error(result.ErrorMessage);
                    return result;
                }
                ;
                // 等待图像采集
                DateTime startTime = DateTime.Now;
                IFrameOut frameOut = null;
                bool gotFrame = false;      // 是否拿到图像
                try
                {
                    // 发送软触发命令  
                    _device.Parameters.SetEnumValueByString("TriggerMode", "On");
                    _device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                    _device.Parameters.SetCommandValue("TriggerSoftware");

                    int triggerResult = _device.Parameters.SetCommandValue("TriggerSoftware");
                    if (triggerResult != MyCamera.MV_OK)
                    {
                        result.ErrorMessage = $"软触发命令执行失败，错误码: {triggerResult}";
                        LogHelper.Error(result.ErrorMessage);
                        return result;
                    }

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    while (sw.ElapsedMilliseconds < timeoutMs)
                    //while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
                    {
                        if (_grabber.GetImageBuffer(100, out frameOut) == MvError.MV_OK && frameOut != null) break;
                        Thread.Sleep(100);
                    }
                    if (frameOut == null)
                    {
                        result.ErrorMessage = "取图超时,获取到的图像帧为空";
                        LogHelper.Error(result.ErrorMessage);
                        return result;
                    }
                    result.Width = (int)frameOut.Image.Width;
                    result.Height = (int)frameOut.Image.Height;
                    string tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageTemp");
                    Directory.CreateDirectory(tempDir);


                    // 使用SDK的图像保存功能
                    MyCamera.MV_SAVE_IMAGE_PARAM_EX3 saveParam = new MyCamera.MV_SAVE_IMAGE_PARAM_EX3();
                    saveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Jpeg;
                    saveParam.nJpgQuality = 90; // JPEG质量


                    // 保存图像到临时文件
                    string filePath = Path.Combine(tempDir, $"capture_{DateTime.Now:yyyyMMddHHmmssfff}.jpg");
                    var formatInfo = new ImageFormatInfo { FormatType = ImageFormatType.Jpeg, JpegQuality = 90 };
                    if (_device.ImageSaver.SaveImageToFile(filePath, frameOut.Image, formatInfo, CFAMethod.Optimal) == MvError.MV_OK)
                        result.ImageFilePath = filePath;
                    else
                        result.ErrorMessage = "图像保存失败";

                    result.Success = string.IsNullOrEmpty(result.ErrorMessage);
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                    OnError("CaptureOneFrame", -1, ex.Message);
                }
                finally
                {
                    if (frameOut != null)
                    {
                        _grabber.FreeImageBuffer(frameOut);
                    }

                }
                return result;
            });
        }

        public async Task SetExposureTimeAsync(float value) => await Task.Run(() => _device?.Parameters.SetFloatValue("ExposureTime", value));
        public async Task SetGainAsync(float value) => await Task.Run(() => _device?.Parameters.SetFloatValue("Gain", value));
        public async Task SetPixelFormatAsync(string format) => await Task.Run(() => _device?.Parameters.SetEnumValueByString("PixelFormat", format));
        public async Task SetTriggerModeAsync(bool isOn) => await Task.Run(() => _device?.Parameters.SetEnumValueByString("TriggerMode", isOn ? "On" : "Off"));

        private void OnError(string method, int code, string msg) => CameraError?.Invoke(this, new CameraErrorEventArgs { Method = method, ErrorCode = code, Message = msg });

        public void Dispose()
        {
            // 停止采集
            StopGrabbingAsync().Wait();

            // 断开连接
            DisconnectAsync().Wait();

            // 反初始化SDK
            try
            {
                if (_disposed) return;
                DisconnectAsync().Wait(TimeSpan.FromSeconds(3));
                Shutdown();
                _disposed = true;
                // MyCamera.MV_CC_Finalize_NET();
                LogHelper.Info("HKCameraController已释放资源");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"SDK反初始化失败: {ex.Message}", ex);
            }
        }
    }
}
   
