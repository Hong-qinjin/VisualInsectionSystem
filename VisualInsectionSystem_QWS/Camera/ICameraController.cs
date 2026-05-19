using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static iMVS_6000PlatformSDKCS.SyncPlatformSDKCS.ImvsSdkPFSync;

namespace VisualInsectionSystem.Camera
{
    public interface ICameraController : IDisposable
    {
        bool IsConnected {  get; }
        bool IsGrabbing {  get; }
        string CurrentCameraSN {  get; }
        bool Initialize();
        void Shutdown();
        //Task<List<CameraDeviceInfo>> EnumerateDevicesAsync();
        Task<List<CameraInfo>> EnumerateDevicesAsync();
        Task<bool> ConnectAsync(string serialNumber);
        Task DisconnectAsync();

        Task<bool> StartGrabbingAsync();
        Task StopGrabbingAsync();
        Task<FrameCaptureResult> CaptureOneFrameAsync(int timeoutMs = 5000);

        Task SetExposureTimeAsync(float value);
        Task SetGainAsync(float value);
        Task SetPixelFormatAsync(string format);
        Task SetTriggerModeAsync(bool isOn);

        event EventHandler<CameraErrorEventArgs> CameraError;
    }
    // 相机信息，控制接口
    public class CameraInfo
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string IPAddress { get; set; }
        public IDeviceInfo RawInfo { get; set; }
        public string UserDefinedName { get; set; }
        public string DeviceVersion { get; set; }
        public string AccessStatus { get; set; }
    }
    // 相机结果类
    public class FrameCaptureResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ImageFilePath { get; set; }        
        public int Width { get; set; }
        public int Height { get; set; }

        public DateTime CaptureTime { get; set; }
    }
    // 相机错误事件参数
    public class CameraErrorEventArgs : EventArgs
    {
        public string Method { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorTime { get; set; }
    }
}
