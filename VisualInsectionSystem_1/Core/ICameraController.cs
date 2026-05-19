using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvCameraControl;
using static MvCameraControl.MyCamera;

namespace VisionInspectionSystem
{
    // 相机信息实体
    public class CameraInfo
    {
        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }
        public string Model { get; set; }
        public MV_GIGE_DEVICE_INFO GigEInfo { get; set; }
        public MV_USB3_DEVICE_INFO UsbInfo { get; set; }    //USB->USB3
        public DeviceType DeviceType { get; set; }
    }

    public enum DeviceType { GigE, USB, Unknown }

    // 采集结果实体
    public class CaptureResult
    {
        public bool IsSuccess { get; set; }
        public string ImagePath { get; set; }
        public IntPtr ImageData { get; set; }
        public long DataLength { get; set; }
        public DateTime CaptureTime { get; set; }
        public string ErrorMsg { get; set; }
    }

    // 相机控制核心接口
    public interface ICameraController : IDisposable
    {
        // 事件定义
        event EventHandler<CameraInitializedEventArgs> CameraInitialized;
        event EventHandler<CameraScannedEventArgs> CameraScanned;
        event EventHandler<CameraConnectionChangedEventArgs> CameraConnected;
        event EventHandler<CameraConnectionChangedEventArgs> CameraDisconnected;
        event EventHandler<CaptureResultEventArgs> CameraCaptured;
        event EventHandler<CameraErrorEventArgs> CameraErrorOccurred;

        // 生命周期管理
        bool InitializeSDK();
        bool FinalizeSDK();
        List<CameraInfo> ScanCameras();
        bool ConnectCamera(CameraInfo cameraInfo);
        bool DisconnectCamera();
        bool IsCameraConnected { get; }
        CameraInfo CurrentCamera { get; }

        // 图像采集
        Task<CaptureResult> CaptureSingleFrameAsync(int timeoutMs = 5000);
        bool StartContinuousCapture();
        bool StopContinuousCapture();
        bool IsCapturing { get; }

        // 参数配置
        bool SetParameter(string paramName, int value);
        bool SetParameter(string paramName, float value);
        bool SetParameter(string paramName, bool value);
        bool SetParameter(string paramName, string value);
        T GetParameter<T>(string paramName, T defaultValue = default);

        // 辅助功能
        bool SaveImage(CaptureResult captureResult, string savePath);
        bool ShowImageInControl(IntPtr controlHandle, CaptureResult captureResult);
    }

    // 自定义事件参数
    public class CameraInitializedEventArgs : EventArgs
    {
        public bool IsSuccess { get; set; }
        public string SdkVersion { get; set; }
    }

    public class CameraScannedEventArgs : EventArgs
    {
        public List<CameraInfo> CameraList { get; set; }
    }

    public class CameraConnectionChangedEventArgs : EventArgs
    {
        public CameraInfo CameraInfo { get; set; }
        public bool IsConnected { get; set; }
        public string ErrorMsg { get; set; }
        public int ErrorCode { get; set; }
    }

    public class CaptureResultEventArgs : EventArgs
    {
        public CaptureResult CaptureResult { get; set; }
    }

    public class CameraErrorEventArgs : EventArgs
    {
        public string ErrorMsg { get; set; }
        public int ErrorCode { get; set; }
        public string MethodName { get; set; }
    }
}
