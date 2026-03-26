/*
 * 这个示例演示如何接收相机事件。
 * This program shows how to receive camera events.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvCameraControl;

namespace Events_Camera
{
    class Events_Camera
    {

        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
         | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static void DeviceEventGrabedHandler(object sender, DeviceEventArgs e)
        {
            Console.WriteLine("EventName[{0}], EventID[{1}]", e.EventInfo.EventName, e.EventInfo.EventID);
        }

        static void Main(string[] args)
        {
            int ret = MvError.MV_OK;
            IDevice device = null;

            SDKSystem.Initialize();

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                // ch:打开设备 | en:Open device
                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:设置触发模式为off | en:set trigger mode as off
                ret = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                // ch:开启设备指定事件Event | en:Set Event of ExposureEnd On
                ret = device.EventNotificationOn("ExposureEnd");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("EventNotificationOn failed!:{0:x8}", ret);
                    return;
                }

                // ch:注册回调函数 | en:Register Event callback
                device.EventGrabber.DeviceEvent += DeviceEventGrabedHandler;
                device.EventGrabber.SubscribeEvent("ExposureEnd");

                // ch:开启抓图 || en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}
/*
 * 这个示例演示如何通过IP连接GigE相机。
 * This program shows how to connect a GigE camera through IP.
 */

using MvCameraControl;
using System;

namespace GigE_ConnectCameraByIP
{
    class GigE_ConnectCameraByIP
    {
        static void Main(string[] args)
        {

            int result = MvError.MV_OK;
            IDevice device = null;
            int packetSize;
          
            try
            {
                //ch: 初始化SDK |  en: Initialize SDK
                SDKSystem.Initialize();

                // ch:需要连接的相机ip(根据实际填充) 
               //en:The camera IP that needs to be connected (based on actual padding)
                Console.Write("Please input Device Ip : ");
                string deviceIp = Convert.ToString(Console.ReadLine());

                // ch:相机对应的网卡ip(根据实际填充)
                // en:The pc IP that needs to be connected (based on actual padding)
                Console.Write("Please input Net Export Ip : ");
                string netExport = Convert.ToString(Console.ReadLine());

                //ch: 创建设备 | en: Create device
                device = DeviceFactory.CreateDeviceByIp(deviceIp, netExport);
                if(null == device)
                {
                    Console.WriteLine("Create device failed!");
                    return;
                }

                //ch: 打开设备 | en:Open device
                result = device.Open();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Open device failed:{0:x8}", result);
                    return;
                }

                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = (IGigEDevice)device;


                    // ch:探测网络最佳包大小(只对GigE相机有效) 
                    // en:Detection network optimal package size(It only works for the GigE camera)
                    result = gigEDevice.GetOptimalPacketSize(out packetSize);
                    if (MvError.MV_OK != result)
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", result);
                    }
                    else
                    {
                       
                        result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", result);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(" Device is not gigEDevice!");
                    return;
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                result = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", result);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                result = device.StreamGrabber.StartGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", result);
                    return;
                }

                int count = 0;
                IFrameOut frameOut;
                while(count++ != 10)
                {
                    // ch:获取一帧图像 | en:Get one image
                    result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                    if (MvError.MV_OK == result)
                    {
                        Console.WriteLine("Get Image Buffer: Width[{0}] , Height[{1}] , FrameNum[{2}]"
                            , frameOut.Image.Width, frameOut.Image.Height, frameOut.FrameNum);

                        ImageFormatInfo info = new ImageFormatInfo();
                        info.FormatType = ImageFormatType.Jpeg;
                        info.JpegQuality = 80;

                        string filePath = "Image" + "_w_" + frameOut.Image.Width + "_h" + frameOut.Image.Height + "_p" + frameOut.Image.PixelType.ToString() + "_" + frameOut.FrameNum;
                        filePath += "." + info.FormatType;

                        //ch: 保持图像到文件 | en: Save image to file
                        result = device.ImageSaver.SaveImageToFile(filePath, frameOut.Image, info, CFAMethod.Equilibrated);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Save Image failed:{0:x8}", result);                       
                        }

                        //ch: 释放图像缓存 | en: Release image buffer
                        device.StreamGrabber.FreeImageBuffer(frameOut);
                    }
                    else
                    {
                        Console.WriteLine("Get Image failed:{0:x8}", result);
                    }
                }

                // ch:停止抓图 | en:Stop grabbing
                result = device.StreamGrabber.StopGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", result);
                    return;
                }

                // ch:关闭设备 | en:Close device
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Close device failed:{0:x8}", result);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                if (MvError.MV_OK != result)
                {
                    // ch:销毁设备 | en:Destroy device
                    device.Dispose();
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
      
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MvCameraControl;
using System.Net;

namespace GigE_ForceIP
{
    public partial class GigE_ForceIP : Form
    {
        List<IDeviceInfo> _devInfoList;
        IDevice _device = null;

        public GigE_ForceIP()
        {
            InitializeComponent();
            this.Load += new EventHandler(GigE_ForceIP_Load);
        }

        private void enumButton_Click(object sender, EventArgs e)
        {
            DeviceListAcq();
        }

        private void DeviceListAcq()
        {
            int result = 0;

            // ch:创建设备列表 | en:Create Device List
            deviceListComboBox.Items.Clear();
            result = DeviceEnumerator.EnumDevices(DeviceTLayerType.MvGigEDevice, out _devInfoList);
            if (MvError.MV_OK != result)
            {
                ShowErrorMsg("DeviceList Acquire Failed!", result);
                return;
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < _devInfoList.Count; i++)
            {
                if (_devInfoList[i].TLayerType == DeviceTLayerType.MvGigEDevice)
                {
                    if (_devInfoList[i].UserDefinedName.Length > 0)
                    {
                        deviceListComboBox.Items.Add("GEV: " + _devInfoList[i].UserDefinedName + " (" + _devInfoList[i].SerialNumber + ")");
                    }
                    else
                    {
                        deviceListComboBox.Items.Add("GEV: " + _devInfoList[i].ManufacturerName + " " + _devInfoList[i].ModelName + "(" + _devInfoList[i].SerialNumber + ")");
                    }
                }
            }

            if (_devInfoList.Count > 0)
            {
                deviceListComboBox.SelectedIndex = 0;
            }
        }

        private void GigE_ForceIP_Load(object sender, EventArgs e)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            // ch: 枚举设备 | en: Enum Device List
            DeviceListAcq();
        }


        private void GigE_ForceIP_Closing(object sender, FormClosingEventArgs e)
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }
            // ch: 反初始化SDK | en: Finalize SDK
            SDKSystem.Finalize();
        }

        // ch:显示错误信息 | en:Show error message
        private void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0)
            {
                errorMsg = csMessage;
            }
            else
            {
                errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);
            }

            switch (nErrorNum)
            {
                case MvError.MV_E_HANDLE: errorMsg += " Error or invalid handle "; break;
                case MvError.MV_E_SUPPORT: errorMsg += " Not supported function "; break;
                case MvError.MV_E_BUFOVER: errorMsg += " Cache is full "; break;
                case MvError.MV_E_CALLORDER: errorMsg += " Function calling order error "; break;
                case MvError.MV_E_PARAMETER: errorMsg += " Incorrect parameter "; break;
                case MvError.MV_E_RESOURCE: errorMsg += " Applying resource failed "; break;
                case MvError.MV_E_NODATA: errorMsg += " No data "; break;
                case MvError.MV_E_PRECONDITION: errorMsg += " Precondition error, or running environment changed "; break;
                case MvError.MV_E_VERSION: errorMsg += " Version mismatches "; break;
                case MvError.MV_E_NOENOUGH_BUF: errorMsg += " Insufficient memory "; break;
                case MvError.MV_E_UNKNOW: errorMsg += " Unknown error "; break;
                case MvError.MV_E_GC_GENERIC: errorMsg += " General error "; break;
                case MvError.MV_E_GC_ACCESS: errorMsg += " Node accessing condition error "; break;
                case MvError.MV_E_ACCESS_DENIED: errorMsg += " No permission "; break;
                case MvError.MV_E_BUSY: errorMsg += " Device is busy, or network disconnected "; break;
                case MvError.MV_E_NETER: errorMsg += " Network error "; break;
            }

            MessageBox.Show(errorMsg, "PROMPT");
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (_devInfoList.Count == 0)
            {
                ShowErrorMsg("No Device", 0);
                return;
            }

            // ch:IP转换 | en:IP conversion
            IPAddress clsIpAddr;
            if (false == IPAddress.TryParse(ipTextBox.Text, out clsIpAddr))
            {
                ShowErrorMsg("Please enter correct IP", 0);
                return;
            }
            long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);

            // ch:掩码转换 | en:Mask conversion
            IPAddress clsSubMask;
            if (false == IPAddress.TryParse(subnetTextBox.Text, out clsSubMask))
            {
                ShowErrorMsg("Please enter correct IP", 0);
                return;
            }
            long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);

            // ch:网关转换 | en:Gateway conversion
            IPAddress clsDefaultWay;
            if (false == IPAddress.TryParse(gatewayTextBox.Text, out clsDefaultWay))
            {
                ShowErrorMsg("Please enter correct IP", 0);
                return;
            }
            long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);

            if (_devInfoList == null || _devInfoList.Count == 0 || _devInfoList[deviceListComboBox.SelectedIndex] ==null)
            {
                return;
            }

            int ret = MvError.MV_OK;
            // ch:创建设备 | en:Create device
            _device = DeviceFactory.CreateDevice(_devInfoList[deviceListComboBox.SelectedIndex]);
            IGigEDevice gigeDevice = _device as IGigEDevice;
            // ch:判断设备IP是否可达 | en: If device ip is accessible
            bool accessible = DeviceEnumerator.IsDeviceAccessible(_devInfoList[deviceListComboBox.SelectedIndex], DeviceAccessMode.AccessExclusive);
            if (accessible)
            {
               

                ret = gigeDevice.SetIpConfig(IpConfigType.Static);
                if (MvError.MV_OK != ret)
                {
                    ShowErrorMsg("Set Ip config fail", ret);
                    gigeDevice.Dispose();
                    _device = null;
                    return;
                }

                ret = gigeDevice.ForceIp((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
                if (MvError.MV_OK != ret)
                {
                    ShowErrorMsg("ForceIp fail", ret);
                    gigeDevice.Dispose();
                    _device = null;
                    return;
                }
            }
            else
            {
                ret = gigeDevice.ForceIp((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
                if (MvError.MV_OK != ret)
                {
                    ShowErrorMsg("ForceIp fail", ret);
                    gigeDevice.Dispose();
                    _device = null;
                    return;
                }
                gigeDevice.Dispose();

               IDeviceInfo deviceInfo = _devInfoList[deviceListComboBox.SelectedIndex];
               IGigEDeviceInfo gigeDevInfo = deviceInfo as IGigEDeviceInfo;

               uint nIp1 = ((gigeDevInfo.NetExport & 0xff000000) >> 24);
               uint nIp2 = ((gigeDevInfo.NetExport & 0x00ff0000) >> 16);
               uint nIp3 = ((gigeDevInfo.NetExport & 0x0000ff00) >> 8);
               uint nIp4 = (gigeDevInfo.NetExport & 0x000000ff);
               string netExportIp = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
                //ch:需要重新创建句柄，设置为静态IP方式进行保存 | en:  Need to recreate the handle and set it to static IP mode for saving
                //ch: 创建设备 | en: Create device
               _device = DeviceFactory.CreateDeviceByIp(ipTextBox.Text, netExportIp);
               if (null == _device)
                {
                    ShowErrorMsg("Create handle fail", 0);
                    return;
                }
                gigeDevice = _device as IGigEDevice;
                ret = gigeDevice.SetIpConfig(IpConfigType.Static);
                if (MvError.MV_OK != ret)
                {
                    ShowErrorMsg("Set Ip config fail", ret);
                    gigeDevice.Dispose();
                    _device = null;
                    return;
                }
            }
            ShowErrorMsg("IP Set Succeed!", 0);
        }

        private void deviceListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_devInfoList.Count == 0)
            {
                ShowErrorMsg("No Device", 0);
                return;
            }
            IDeviceInfo deviceInfo = _devInfoList[deviceListComboBox.SelectedIndex];
            IGigEDeviceInfo gigeDevInfo = deviceInfo as IGigEDeviceInfo;

            // ch:网口IP | en:Net IP
            UInt32 nNetIp1 = (gigeDevInfo.NetExport & 0xFF000000) >> 24;
            UInt32 nNetIp2 = (gigeDevInfo.NetExport & 0x00FF0000) >> 16;
            UInt32 nNetIp3 = (gigeDevInfo.NetExport & 0x0000FF00) >> 8;
            UInt32 nNetIp4 = (gigeDevInfo.NetExport & 0x000000FF);

            // ch:显示IP | en:Display IP
            uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
            uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
            uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
            uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);

            rangeLabel.Text = nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nNetIp3.ToString() + "." 
                + "0" + "~" + nNetIp1.ToString() + "." + nNetIp2.ToString() + "." + nNetIp3.ToString() + "." + "255";

            ipTextBox.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();

            // ch:显示掩码 | en:Display mask
            nIp1 = (gigeDevInfo.CurrentSubNetMask & 0xFF000000) >> 24;
            nIp2 = (gigeDevInfo.CurrentSubNetMask & 0x00FF0000) >> 16;
            nIp3 = (gigeDevInfo.CurrentSubNetMask & 0x0000FF00) >> 8;
            nIp4 = (gigeDevInfo.CurrentSubNetMask & 0x000000FF);

            subnetTextBox.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();

            // ch:显示网关 | en:Display gateway
            nIp1 = (gigeDevInfo.DefultGateWay & 0xFF000000) >> 24;
            nIp2 = (gigeDevInfo.DefultGateWay & 0x00FF0000) >> 16;
            nIp3 = (gigeDevInfo.DefultGateWay & 0x0000FF00) >> 8;
            nIp4 = (gigeDevInfo.DefultGateWay & 0x000000FF);

            gatewayTextBox.Text = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
        }

    }
}
/*
 * 这个示例演示如何使用GigE相机的ActionCommand触发功能。
 * This program shows how to use the ActionCommand trigger function of a GigE camera.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvCameraControl;
namespace Grab_ActionCommand
{
    class Grab_ActionCommand
    {
        static bool _bExit = false;
        static uint _deviceKey = 1;
        static uint _groupKey = 1;
        static uint _groupMask = 1;

        public static void ActionCommandWorkThread()
        {          
            int ret = MvError.MV_OK;
            ActionCmdInfo actionCmdInfo = new ActionCmdInfo();
            List<ActionCmdResult> actionCmdResults;

            actionCmdInfo.DeviceKey = _deviceKey;
            actionCmdInfo.GroupKey = _groupKey;
            actionCmdInfo.GroupMask = _groupMask;
            actionCmdInfo.BroadcastAddress = "255.255.255.255";
            actionCmdInfo.TimeOut = 100;
            actionCmdInfo.ActionTimeEnable = 0;

            while(!_bExit)
            {
                ret = DeviceEnumerator.GigEIssueActionCommand(actionCmdInfo, out actionCmdResults);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Issue Action Command failed! nRet {0:x8}", ret);
                    continue;
                }

                if (actionCmdResults != null)
                {
                    for (int i = 0; i < actionCmdResults.Count; i++)
                    {
                        Console.WriteLine("Ip == {0}, Status == {1}", actionCmdResults[i].DeviceAddress, actionCmdResults[i].Status);
                    }
                }
            }
           
        }

        public static void ReceiveImageWorkThread(object obj)
        {
            IStreamGrabber streamGrabber = (IStreamGrabber)obj;

            while (true)
            {
                IFrameOut frame;

                //ch：获取一帧图像 | en: Get one frame
                int ret = streamGrabber.GetImageBuffer(1000, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //ch: 释放图像缓存  | en: Release the image buffer
                    streamGrabber.FreeImageBuffer(frame);                  
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                }

                if (_bExit)
                {
                    break;
                }
            }

        }

        static void Main(string[] args)
        {
            int ret = MvError.MV_OK;

            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();
            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(DeviceTLayerType.MvGigEDevice, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                   
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("[Device {0}]:", devIndex);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                        Console.WriteLine("ModelName: {0}\n",devInfo.ModelName);
                    }
                    else
                    {
                        Console.Write("Not Support!\n");
                        break;
                    }
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                // ch:打开设备 | en:Open device
                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:打开触发模式 || en:set trigger mode as On
                ret = device.Parameters.SetEnumValue("TriggerMode", 1);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                // ch:设置触发源为Action1 | en:Set trigger source as Action1
                ret = device.Parameters.SetEnumValueByString("TriggerSource", "Action1");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Trigger Source failed! {0:x8}", ret);
                    return;
                }

                // ch:设置Action Device Key | en:Set Action Device Key
                ret = device.Parameters.SetIntValue("ActionDeviceKey", _deviceKey);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Action Device Key failed! {0:x8}", ret);
                    return;
                }

                // ch:设置Action Group Key | en:Set Action Group Key
                ret = device.Parameters.SetIntValue("ActionGroupKey", _groupKey);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Action Group Key failed! {0:x8}", ret);
                    return;
                }

                // ch:设置Action Group Mask | en:Set Action Group Mask
                ret = device.Parameters.SetIntValue("ActionGroupMask", _groupMask);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Action Group Mask fail! {0:x8}", ret);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                Thread hActionCommandThreadHandle = new Thread(ActionCommandWorkThread);
                hActionCommandThreadHandle.Start();

                Thread hReceiveImageThreadHandle = new Thread(ReceiveImageWorkThread);
                hReceiveImageThreadHandle.Start(device.StreamGrabber);

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();

                _bExit = true;
                hActionCommandThreadHandle.Join();
                hReceiveImageThreadHandle.Join();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null || ret != MvError.MV_OK)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }

          
        }
    }
}

/*
 * 这个示例演示如何通过回调方式取图（FrameGrabedEvent），回调方式下，SDK内部开启取流线程，通过event方式回调给上层。
 * This program shows how to grab images using event-driven method. The SDK starts a thread to grab images and uses the FrameGrabedEvent to return the images.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;

namespace Grab_Callback
{
    class Grab_Callback
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static void FrameGrabedEventHandler(object sender, FrameGrabbedEventArgs e)
        {
            Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , ImageSize[{2}], FrameNum[{3}]", e.FrameOut.Image.Width, e.FrameOut.Image.Height, e.FrameOut.Image.ImageSize, e.FrameOut.FrameNum);

            // ch: 手动释放缓存 | en: Dispose frame manually
            e.FrameOut.Dispose();
        }

        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            IDevice device = null;

           try
           {
               List<IDeviceInfo> devInfoList;

               // ch:枚举设备 | en:Enum device
               int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Enum device failed:{0:x8}", ret);
                   return;
               }

               Console.WriteLine("Enum device count : {0}", devInfoList.Count);

               if (0 == devInfoList.Count)
               {
                   Console.WriteLine("No device!");
                   return;
               }

               // ch:打印设备信息 en:Print device info
               int devIndex = 0;
               foreach (var devInfo in devInfoList)
               {
                   Console.WriteLine("[Device {0}]:", devIndex);
                   if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                   {
                       IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                       uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                       uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                       uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                       uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                       Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                   }

                   Console.WriteLine("ModelName:" + devInfo.ModelName);
                   Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                   Console.WriteLine();
                   devIndex++;
               }

               Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

               devIndex = Convert.ToInt32(Console.ReadLine());

               if (devIndex > devInfoList.Count - 1 || devIndex < 0)
               {
                   Console.Write("Input Error!\n");
                   return;
               }

               // ch:创建设备 | en:Create device
               device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

               ret = device.Open();
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Open device failed:{0:x8}", ret);
                   return;
               }

               // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
               if (device is IGigEDevice)
               {
                   int packetSize;
                   ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                   if (packetSize > 0)
                   {
                       ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                       if (ret != MvError.MV_OK)
                       {
                           Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                       }
                       else
                       {
                           Console.WriteLine("Set PacketSize to {0}", packetSize);
                       }
                   }
                   else
                   {
                       Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                   }
               }

               // ch:设置触发模式为off || en:set trigger mode as off
               ret = device.Parameters.SetEnumValue("TriggerMode", 0);
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                   return;
               }

               //ch: 设置合适的缓存节点数量 | en: Setting the appropriate number of image nodes
               device.StreamGrabber.SetImageNodeNum(5);

               // ch:注册回调函数 | en:Register image callback
               device.StreamGrabber.FrameGrabedEventEx += FrameGrabedEventHandler;
               // ch:开启抓图 || en: start grab image
               ret = device.StreamGrabber.StartGrabbing();
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                   return;
               }

               Console.WriteLine("Press enter to exit");
               Console.ReadLine();


               // ch:停止抓图 | en:Stop grabbing
               ret = device.StreamGrabber.StopGrabbing();
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                   return;
               }

               // ch:关闭设备 | en:Close device
               ret = device.Close();
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Close device failed:{0:x8}", ret);
                   return;
               }

               // ch:销毁设备 | en:Destroy device
               device.Dispose();
               device = null;
           }
            catch(Exception e)
           {
               Console.Write("Exception: " + e.Message);
           }
            finally
           {
               // ch:销毁设备 | en:Destroy device
               if (device != null)
               {
                   device.Dispose();
                   device = null;
               }

               // ch: 反初始化SDK | en: Finalize SDK
               SDKSystem.Finalize();

               Console.WriteLine("Press enter to exit");
               Console.ReadKey();
           }
        }
    }
}

/*
 * 这个示例演示如何解析相机发送的Chunk信息。
 * This program shows how to parse Chunk information sent by a camera.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvCameraControl;

namespace Grab_ChunkData
{
    class Grab_ChunkData
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
          | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static void FrameGrabedEventHandler(object sender, FrameGrabbedEventArgs e)
        {
            Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", e.FrameOut.Image.Width, e.FrameOut.Image.Height, e.FrameOut.FrameNum);
            
            // ch: chunkData有两种获取方法，一种是通过遍历获取，另一种是通过chunkId获取 
            // en: There are two ways to get chunkData, one is  traversal, and the other is by chunkId

            //ch:方法1：遍历 | en: Way1:traversal
            foreach (IChunkData chunkData in e.FrameOut.ChunkInfo)
            {
                Console.WriteLine("ChunkInfo:" + "ChunkID[0x{0:x8}],ChunkLen[{1}]", chunkData.ChunkID, chunkData.Length);
            }

            //ch:方法2：通过chunkId | en: Way2: byChunkId
            {
                //ch: 获取宽 | en: Get width
                IChunkData widthChunkData = e.FrameOut.ChunkInfo[0xa5a5010a];
                int width = BitConverter.ToInt32(widthChunkData.Data, 0);

                //ch: 获取高 | en: Get heigth
                IChunkData heightChunkData = e.FrameOut.ChunkInfo[0xa5a5010b];
                int height = BitConverter.ToInt32(heightChunkData.Data, 0);

                Console.WriteLine("ChunkInfo:" + "width[{0}],height[{1}]", width, height);
            }

        }

        static void Main(string[] args)
        {
            int ret = MvError.MV_OK;
            IDevice device = null;

            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                // ch:打开设备 | en:Open device
                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:注册回调函数 | en:Register image callback
                device.StreamGrabber.FrameGrabedEvent += FrameGrabedEventHandler;

                // ch:开启Chunk Mode | en:Open Chunk Mode
                ret = device.Parameters.SetBoolValue("ChunkModeActive", true);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Chunk Mode failed:{0:x8}", ret);
                    return;
                }

                // ch:Chunk Selector设为Exposure | en: Chunk Selector set as Exposure
                ret = device.Parameters.SetEnumValueByString("ChunkSelector", "Exposure");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Chunk Enable failed:{0:x8}", ret);
                    return;
                }

                // ch:开启Chunk Enable | en:Open Chunk Enable
                ret = device.Parameters.SetBoolValue("ChunkEnable", true);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Exposure Chunk failed:{0:x8}", ret);
                    return;
                }

                // ch:Chunk Selector设为Timestamp | en: Chunk Selector set as Timestamp
                ret = device.Parameters.SetEnumValueByString("ChunkSelector", "Timestamp");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Timestamp Chunk failed:{0:x8}", ret);
                    return;
                }

                // ch:开启Chunk Enable | en:Open Chunk Enable
                ret = device.Parameters.SetBoolValue("ChunkEnable", true);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Chunk Enable failed:{0:x8}", ret);
                    return;
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                ret = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {

                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null || ret != MvError.MV_OK)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}


/*
 * 这个示例演示了主动取图方式，由调用者创建取图线程，并循环调用GetImageBuffer/FreeImageBuffer获取和释放图片。
 * This sample shows the active acquisition mode. The caller creates a thread to grab images and continuously calls 
 * GetImageBuffer/FreeImageBuffer to get and release image buffers.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;
using System.Threading;

namespace Grab_GetImageBuffer
{
    class Grab_GetImageBuffer
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static volatile bool _grabThreadExit = false; 
       static void FrameGrabThread(object obj)
        {
            IStreamGrabber streamGrabber = (IStreamGrabber)obj;

           while(!_grabThreadExit)
           {
               IFrameOut frame;

               //ch：获取一帧图像 | en: Get one frame
               int ret = streamGrabber.GetImageBuffer(1000, out frame);
               if (ret != MvError.MV_OK)
               {
                   Console.WriteLine("Get Image failed:{0:x8}", ret);
                   continue;
               }

               Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , ImageSize[{2}], FrameNum[{3}]", frame.Image.Width, frame.Image.Height, frame.Image.ImageSize, frame.FrameNum);
               //Do some thing
               

               //ch: 释放图像缓存  | en: Release the image buffer
               streamGrabber.FreeImageBuffer(frame);
           }
        }

        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:设置触发模式为off | en:set trigger mode as off
                ret = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                //ch: 设置合适的缓存节点数量 | en: Setting the appropriate number of image nodes
                device.StreamGrabber.SetImageNodeNum(5);

                // ch:开启抓图 | en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:开启抓图线程 | en: Start the grabbing thread
                Thread GrabThread = new Thread(FrameGrabThread);
                GrabThread.Start(device.StreamGrabber);

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                //ch: 通知线程退出 | en: Notify the grab thread to exit
                _grabThreadExit = true;
                GrabThread.Join();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);

            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}

/*
 * 这个示例演示如何使用GigE相机的组播功能。
 * This program shows how to use the multicast feature of GigE cameras.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvCameraControl;

namespace Grab_GigEMultiCast
{
    class Grab_GigEMultiCast
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice;
        public static bool _bExit = false;

        static void WorkThread(object obj)
        {
            IStreamGrabber streamGrabber = (IStreamGrabber)obj;
            while (true)
            {
                IFrameOut frame;

                //ch：获取一帧图像 | en: Get one frame
                int ret = streamGrabber.GetImageBuffer(1000, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //ch: 释放图像缓存  | en: Release the image buffer
                    streamGrabber.FreeImageBuffer(frame);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                }

                if (_bExit)
                {
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            int ret = MvError.MV_OK;
            IDevice device = null;

            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                // ch:查询用户使用的模式
                // Query the user for the mode to use.
                bool monitorMode = false;
                {
                    string key = "";

                    // Ask the user to launch the multicast controlling application or the multicast monitoring application.
                    Console.WriteLine("Start multicast sample in (c)ontrol or in (m)onitor mode? (c/m)");
                    do
                    {
                        key = Convert.ToString(Console.ReadLine());
                    } 
                    while ((key != "c") && (key != "m") && (key != "C") && (key != "M"));
                    monitorMode = (key == "m") || (key == "M");
                }

                 // ch:打开设备 | en:Open device
                if (monitorMode)
                {
                    ret = device.Open(DeviceAccessMode.AccessMonitor, 0);
                    if (ret != MvError.MV_OK)
                    {
                        Console.WriteLine("Open device failed:{0:x8}", ret);
                        return;
                    }
                }
                else
                {
                    ret = device.Open(DeviceAccessMode.AccessControl, 0);
                    if (ret != MvError.MV_OK)
                    {
                        Console.WriteLine("Open device failed:{0:x8}", ret);
                        return;
                    }
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:指定组播ip | en:The specified multicast IP
                string strIp = "239.192.1.1";
                var parts = strIp.Split('.');
                int nDestIp1 = Convert.ToInt32(parts[0]);
                int nDestIp2 = Convert.ToInt32(parts[1]);
                int nDestIp3 = Convert.ToInt32(parts[2]);
                int nDestIp4 = Convert.ToInt32(parts[3]);
                int nDestIp = (nDestIp1 << 24) | (nDestIp2 << 16) | (nDestIp3 << 8) | nDestIp4;

                ret = (device as IGigEDevice).SetTransmissionType(TransmissionType.Multicast, (uint)nDestIp, 1024);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set transmission type failed:{0:x8}", ret);
                    return;
                }

                // ch:开启抓图 | en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                Thread thr = new Thread(WorkThread);
                thr.Start(device.StreamGrabber);

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                _bExit = true;
                thr.Join();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null || ret != MvError.MV_OK)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }

          
        }
    }
}

/*
 * 这个示例演示了取图时深拷贝图像，将图像放入队列后异步处理。
 * This sample shows how to perform a deep copy of the image during image acquisition, place the image in a queue, and process it asynchronously.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;
using System.Threading;

namespace Grab_ImageClone
{
    class Grab_ImageClone
    {
        private const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        /// <summary>
        /// ch: 帧缓存队列 | en: frame queue for process
        /// </summary>
        private Queue<IFrameOut> _frameQueue = null;

        /// <summary>
        /// ch: 队列图像数量上限 | en: maximum number of frames in the queue
        /// </summary>
        private const uint _maxQueueSize = 10; 

        /// <summary>
        /// ch: 异步处理线程 | asynchronous processing thread
        /// </summary>
        private Thread _asyncProcessThread = null;
        /// <summary>
        /// ch: 信号，通知异步处理线程处理 | Used to notify the processing thread
        /// </summary>
        private Semaphore _frameGrabSem = null;
        /// <summary>
        /// ch: 异步处理线程退出标志 | en: Flag to notify the  processing thread to exit
        /// </summary>
        private volatile bool _processThreadExit = false;

        public Grab_ImageClone()
        {
            _frameQueue = new Queue<IFrameOut>();
            _frameGrabSem = new Semaphore(0, Int32.MaxValue);
        }


        public void Run()
        {
            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                ret = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                //ch: 设置合适的缓存节点数量 | en: Setting the appropriate number of image nodes
                device.StreamGrabber.SetImageNodeNum(5);

                //ch：创建异步处理线程 | en: Create an asynchronous processing thread
                _processThreadExit = false;
                _asyncProcessThread = new Thread(AsyncProcessThread);
                _asyncProcessThread.Start();

                // ch:注册回调函数 | en:Register image callback
                device.StreamGrabber.FrameGrabedEvent += FrameGrabedEventHandler;
                // ch:开启抓图 || en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Press enter to stop grabbing");
                Console.ReadLine();

                //ch: 通知异步处理线程退出 | en: Notify the thread to exit
                _processThreadExit = true;
                _asyncProcessThread.Join();

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

            }
        }

        void AsyncProcessThread()
        {
            try
            {
                while (!_processThreadExit)
                {
                    if (_frameGrabSem.WaitOne(100))
                    {
                        using (IFrameOut frame = _frameQueue.Dequeue())
                        {
                            Console.WriteLine("AsyncProcessThread: process one frame, Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                            //Processing the image data, such as algorithms
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AsyncProcessThread exception: " + e.Message);
            }

        }

        void FrameGrabedEventHandler(object sender, FrameGrabbedEventArgs e)
        {
            Console.WriteLine("FrameGrabedEventHandler: Get one frame, Width[{0}] , Height[{1}] , FrameNum[{2}]", e.FrameOut.Image.Width, e.FrameOut.Image.Height, e.FrameOut.FrameNum);

            try
            {
                
                lock (this)
                {
                    if (_frameQueue.Count <= _maxQueueSize)
                    {
                        // ch: 克隆图像数据（深拷贝） | en :Clone frame data using deep copy
                        IFrameOut frameCopy = (IFrameOut)e.FrameOut.Clone();

                        //ch: 添加到队列并通知处理线程 | en: Add the frame to the queue and notify the processing thread
                        _frameQueue.Enqueue(frameCopy);
                        _frameGrabSem.Release();
                    }
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("FrameGrabedEventHandler exception: " + exception.Message);
            }

        }

        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            Grab_ImageClone program = new Grab_ImageClone();
            program.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();

            // ch: 反初始化SDK | en: Finalize SDK
            SDKSystem.Finalize();
        }
    }
}


/*
 * 这个示例演示了如何使用取图策略
 * This sample demonstrates how to use the image acquisition strategy
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;
using System.Threading;

namespace Grab_Strategy
{
    class Grab_Strategy
    {
        //Only the GigE and USB device support the grab strategy now!
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice;

        static volatile bool _grabThreadExit = false;
        static void FrameGrabThread(object obj)
        {
            IStreamGrabber streamGrabber = (IStreamGrabber)obj;

            while (!_grabThreadExit)
            {
                IFrameOut frame;

                //ch：获取一帧图像 | en: Get one frame
                int ret = streamGrabber.GetImageBuffer(1000, out frame);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    continue;
                }

                Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                //Do something


                //ch: 释放图像缓存  | en: Release the image buffer
                streamGrabber.FreeImageBuffer(frame);
            }
        }

        static void StrategyOneByOne(IDevice device)
        {
            int ret = MvError.MV_OK;

            // ch:开启抓图 | en: start grab image
            ret = device.StreamGrabber.StartGrabbing(StreamGrabStrategy.OneByOne);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                return;
            }

            // ch:软触发5次 | en: 5 software triggers
            for (UInt32 i = 0; i < 5; i++)
            {
                //ch: 软触发 | en: Software trigger
                ret = device.Parameters.SetCommandValue("TriggerSoftware");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Send Trigger Software command fail:{0:x8}", ret);
                }
                else
                {
                    Console.WriteLine("Send Trigger Software command:{0}", i);
                }

                Thread.Sleep(500);//如果帧率过小或TriggerDelay很大，可能会出现软触发命令没有全部起效而导致取不到数据的情况
            }


            //ch: 获取到连续的5帧图像 | en: Obtain a continuous sequence of 5 frames
            while (true)
            {
                IFrameOut frame;
                ret = device.StreamGrabber.GetImageBuffer(10, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //Do something

                    //ch: 释放图像缓存  | en: Release the image buffer
                    device.StreamGrabber.FreeImageBuffer(frame);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    break;
                }
            }

            // ch:停止抓图 | en:Stop grabbing
            ret = device.StreamGrabber.StopGrabbing();
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                return;
            }
        }


        static void StrategyLastImageOnly(IDevice device)
        {
            int ret = MvError.MV_OK;

            // ch:开启抓图 | en: start grab image
            ret = device.StreamGrabber.StartGrabbing(StreamGrabStrategy.LatestImageOnly);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                return;
            }

            // ch:软触发5次 | en: 5 software triggers
            for (UInt32 i = 0; i < 5; i++)
            {
                //ch: 软触发 | en: Software trigger
                ret = device.Parameters.SetCommandValue("TriggerSoftware");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Send Trigger Software command fail:{0:x8}", ret);
                }
                else
                {
                    Console.WriteLine("Send Trigger Software command:{0}", i);
                }

                Thread.Sleep(500);//如果帧率过小或TriggerDelay很大，可能会出现软触发命令没有全部起效而导致取不到数据的情况
            }


            //ch：获取到最新一帧图像  | en：Obtain the latest frame
            while (true)
            {
                IFrameOut frame;
                ret = device.StreamGrabber.GetImageBuffer(10, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //Do something

                    //ch: 释放图像缓存  | en: Release the image buffer
                    device.StreamGrabber.FreeImageBuffer(frame);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    break;
                }
            }

            // ch:停止抓图 | en:Stop grabbing
            ret = device.StreamGrabber.StopGrabbing();
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                return;
            }
        }

        static void StrategyLastImages(IDevice device)
        {
            int ret = MvError.MV_OK;

            //ch: 设置输出2帧图像 | Set out put queue to 2
            ret = device.StreamGrabber.SetOutputQueueSize(2);

            // ch:开启抓图 | en: start grab image
            ret = device.StreamGrabber.StartGrabbing(StreamGrabStrategy.LatestImages);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                return;
            }

            // ch:软触发5次 | en: 5 software triggers
            for (UInt32 i = 0; i < 5; i++)
            {
                //ch: 软触发 | en: Software trigger
                ret = device.Parameters.SetCommandValue("TriggerSoftware");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Send Trigger Software command fail:{0:x8}", ret);
                }
                else
                {
                    Console.WriteLine("Send Trigger Software command:{0}", i);
                }

                Thread.Sleep(500);//如果帧率过小或TriggerDelay很大，可能会出现软触发命令没有全部起效而导致取不到数据的情况
            }

            //ch：获取到最新2帧图像  | en：Obtain the latest two frames
            while (true)
            {
                IFrameOut frame;
                ret = device.StreamGrabber.GetImageBuffer(10, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //Do something

                    //ch: 释放图像缓存  | en: Release the image buffer
                    device.StreamGrabber.FreeImageBuffer(frame);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    break;
                }
            }

            // ch:停止抓图 | en:Stop grabbing
            ret = device.StreamGrabber.StopGrabbing();
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                return;
            }
        }

        static void StrategyUpcomingImage(IDevice device)
        {
            int ret = MvError.MV_OK;

            // ch:开启抓图 | en: start grab image
            ret = device.StreamGrabber.StartGrabbing(StreamGrabStrategy.UpcomingImage);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                return;
            }

            // ch:软触发5次 | en: 5 software triggers
            for (UInt32 i = 0; i < 5; i++)
            {
                //ch: 软触发 | en: Software trigger
                ret = device.Parameters.SetCommandValue("TriggerSoftware");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Send Trigger Software command fail:{0:x8}", ret);
                }
                else
                {
                    Console.WriteLine("Send Trigger Software command:{0}", i);
                }

                Thread.Sleep(500);//如果帧率过小或TriggerDelay很大，可能会出现软触发命令没有全部起效而导致取不到数据的情况
            }

            Thread triggerThread = new Thread(() =>
                {
                    // ch: 3秒后触发一次 | en: trigger after 3s
                    Thread.Sleep(3000);
                    device.Parameters.SetCommandValue("TriggerSoftware");
                    Console.WriteLine("TriggerThread：Send Trigger Software command");
                });
            triggerThread.Start();

            //ch：获取到3秒后触发的那一帧  | en: Retrieve the frame triggered 3 seconds later
            while (true)
            {
                IFrameOut frame;
                ret = device.StreamGrabber.GetImageBuffer(5000, out frame);
                if (ret == MvError.MV_OK)
                {
                    Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                    //Do something

                    //ch: 释放图像缓存  | en: Release the image buffer
                    device.StreamGrabber.FreeImageBuffer(frame);

                    break;
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                }
            }

            // ch:停止抓图 | en:Stop grabbing
            ret = device.StreamGrabber.StopGrabbing();
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                return;
            }
        }

        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:设置为软触发模式 | en:Set Trigger Mode and Set Trigger Source
                ret = device.Parameters.SetEnumValueByString("TriggerMode", "On");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                ret = device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set Trigger Source failed:{0:x8}", ret);
                    return;
                }

                //ch: 设置合适的缓存节点数量 | en: Setting the appropriate number of image nodes
                device.StreamGrabber.SetImageNodeNum(10);


                Console.WriteLine("\n**************************************************************************");
                Console.WriteLine("* 0.GrabStrategy_OneByOne;       1.GrabStrategy_LatestImagesOnly;  *");
                Console.WriteLine("* 2.GrabStrategy_LatestImages;   3.GrabStrategy_UpcomingImage;     *");
                Console.WriteLine("**************************************************************************");

                Console.Write("Please Input Grab Strategy:");
                Int32 nGrabStrategy = 0;
                try
                {
                    nGrabStrategy = (Int32)Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.Write("Invalid Input!");
                    return;
                }

                // ch:U3V相机不支持MV_GrabStrategy_UpcomingImage | en:U3V device not support UpcomingImage
                if (nGrabStrategy == (Int32)StreamGrabStrategy.UpcomingImage
                    && device is IUSBDevice)
                {
                    Console.Write("U3V device not support UpcomingImage");
                    return;
                }

                if (nGrabStrategy == (Int32)StreamGrabStrategy.OneByOne)
                {
                    StrategyOneByOne(device);
                }
                else if (nGrabStrategy == (Int32)StreamGrabStrategy.LatestImageOnly)
                {
                    StrategyLastImageOnly(device);
                }
                else if (nGrabStrategy == (Int32)StreamGrabStrategy.LatestImages)
                {
                    StrategyLastImages(device);
                }
                else if (nGrabStrategy == (Int32)StreamGrabStrategy.UpcomingImage)
                {
                    StrategyUpcomingImage(device);
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}

/*
 * 这个示例演示了如何使用对比度调节功能
 * This sample demonstrates how to use the image contrast function of ImageProcess
 */

using MvCameraControl;
using System;
using System.Collections.Generic;

namespace Image_Contrast
{
    class Image_Contrast
    {
        static void Main(string[] args)
        {
            DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
     | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;


            int result = MvError.MV_OK;
            IDevice device = null;
            List<IDeviceInfo> deviceInfos;
            int packetSize;

            try
            {
                //ch: 初始化SDK |  en: Initialize SDK
                SDKSystem.Initialize();

                result = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfos);
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Enumerate device failed, result: {0:x8}", result);
                    return;
                }

                if (deviceInfos.Count == 0)
                {
                    Console.WriteLine("No device");
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in deviceInfos)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                // ch:需要连接的相机索引 || en:Select a device that want to connect
                Console.Write("Please input index(0-{0:d}):", deviceInfos.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > deviceInfos.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                //ch: 创建设备 | en: Create device
                device = DeviceFactory.CreateDevice(deviceInfos[devIndex]);
                if (device == null)
                {
                    Console.WriteLine("Create device failed!");
                    return;
                }

                //ch: 打开设备 | en:Open device
                result = device.Open();
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", result);
                    return;
                }

                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = (IGigEDevice)device;

                    // ch:探测网络最佳包大小(只对GigE相机有效) 
                    // en:Detection network optimal package size(It only works for the GigE camera)
                    result = gigEDevice.GetOptimalPacketSize(out packetSize);
                    if (MvError.MV_OK != result)
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", result);
                    }
                    else
                    {
                        result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", result);
                        }
                    }
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                result = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", result);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                result = device.StreamGrabber.StartGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", result);
                    return;
                }

                IFrameOut frameOut;
                // ch:获取一帧图像 | en:Get one image
                result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == result)
                {
                    Console.WriteLine("Get Image Buffer: Width[{0}] , Height[{1}] , FrameNum[{2}]"
                        , frameOut.Image.Width, frameOut.Image.Height, frameOut.FrameNum);

                    IImage inputImage = frameOut.Image;
                    IImage outImage;

                    // ch:对比度值，[1, 10000] | en:Image Contrast Factor[1, 10000]
                    uint contrastFactor = 300;

                    // ch:对比度调节 | en:Image Contrast Process
                    result = device.ImageProcess.ImageContrast(inputImage, out outImage, contrastFactor);
                    if (result != MvError.MV_OK)
                    {
                        Console.WriteLine("Image Contrast failed:{0:x8}", result);
                        return;
                    }
                    Console.WriteLine("Image Contrast success!");

                    ImageFormatInfo info = new ImageFormatInfo();
                    info.FormatType = ImageFormatType.Bmp;

                    string inputFilePath = string.Format("InputImage.{0}", info.FormatType);
                    string outputFilePath = string.Format("OutputImage_ContrastFactor{0}.{1}", contrastFactor, info.FormatType);

                    //ch: 保持图像到文件 | en: Save image to file
                    device.ImageSaver.SaveImageToFile(inputFilePath, frameOut.Image, info, CFAMethod.Equilibrated);
                    Console.WriteLine("Save inputImage: {0}!", inputFilePath);

                    device.ImageSaver.SaveImageToFile(outputFilePath, outImage, info, CFAMethod.Equilibrated);
                    Console.WriteLine("Save OutputImage: {0}!", outputFilePath);

                    //ch: 图像使用完及时释放，防止内存快速上涨导致频繁GC |en：Release image promptly to prevent rapid memory increase leading to frequent GC.
                    outImage.Dispose();

                    //ch: 释放图像缓存 | en: Release image buffer
                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", result);
                }

                // ch:停止抓图 | en:Stop grabbing
                result = device.StreamGrabber.StopGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", result);
                    return;
                }

                // ch:关闭设备 | en:Close device
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Close device failed:{0:x8}", result);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (device != null)
                {
                    // ch:销毁设备 | en:Destroy device
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}

/*
 * 这个示例演示了如何使用格式转换功能
 * This sample demonstrates how to use the PixelTypeConverter function
 */

using MvCameraControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace Image_ConvertPixelType
{
    class Image_ConvertPixelType
    {
        static bool IsMonoPixelFormat(MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        static bool IsColorPixelFormat(MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                case MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerRBGG8:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        static void Main(string[] args)
        {
            DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
     | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;

            int result = MvError.MV_OK;
            IDevice device = null;
            List<IDeviceInfo> deviceInfos;
            int packetSize;

            try
            {
                //ch: 初始化SDK |  en: Initialize SDK
                SDKSystem.Initialize();

                result = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfos);
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Enumerate device failed, result: {0:x8}", result);
                    return;
                }

                if (deviceInfos.Count == 0)
                {
                    Console.WriteLine("No device");
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in deviceInfos)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                // ch:需要连接的相机索引 || en:Select a device that want to connect
                Console.Write("Please input index(0-{0:d}):", deviceInfos.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > deviceInfos.Count - 1 || devIndex < 0)
                {
                    Console.WriteLine("Input Error!\n");
                    return;
                }

                //ch: 创建设备 | en: Create device
                device = DeviceFactory.CreateDevice(deviceInfos[devIndex]);
                if (device == null)
                {
                    Console.WriteLine("Create device failed!");
                    return;
                }

                //ch: 打开设备 | en:Open device
                result = device.Open();
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", result);
                    return;
                }

                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = (IGigEDevice)device;

                    // ch:探测网络最佳包大小(只对GigE相机有效) 
                    // en:Detection network optimal package size(It only works for the GigE camera)
                    result = gigEDevice.GetOptimalPacketSize(out packetSize);
                    if (MvError.MV_OK != result)
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", result);
                    }
                    else
                    {
                        result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", result);
                        }
                    }
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                result = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", result);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                result = device.StreamGrabber.StartGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", result);
                    return;
                }

                IFrameOut frameOut;
                // ch:获取一帧图像 | en:Get one image
                result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == result)
                {
                    Console.WriteLine("Get Image Buffer: Width[{0}] , Height[{1}] , FrameNum[{2}]"
                        , frameOut.Image.Width, frameOut.Image.Height, frameOut.FrameNum);

                    IImage inputImage = frameOut.Image;
                    IImage outImage;
                    MvGvspPixelType dstPixelType = MvGvspPixelType.PixelType_Gvsp_Undefined;

                    if (IsColorPixelFormat(frameOut.Image.PixelType))
                    {
                        dstPixelType = MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                    }
                    else if (IsMonoPixelFormat(frameOut.Image.PixelType))
                    {
                        dstPixelType = MvGvspPixelType.PixelType_Gvsp_Mono8;
                    }
                    else
                    {
                        Console.WriteLine("Don't need to convert!");
                    }

                    if (dstPixelType != MvGvspPixelType.PixelType_Gvsp_Undefined)
                    {
                        // ch:像素格式转换 | en:Pixel type convert 
                        result = device.PixelTypeConverter.ConvertPixelType(inputImage, out outImage, dstPixelType);
                        if (result != MvError.MV_OK)
                        {
                            Console.WriteLine("Image Convert failed:{0:x8}", result);
                            return;
                        }
                        Console.WriteLine("Image Convert success!");

                        string inputFilePath = string.Format("InputImage_w{0}_h{1}_{2}.raw", inputImage.Width, inputImage.Height, inputImage.PixelType);
                        string outputFilePath = string.Format("OutputImage_w{0}_h{1}_{2}.raw", outImage.Width, outImage.Height, outImage.PixelType);

                        //ch: 保持图像到文件 | en: Save image to file
                        using (FileStream fs = new FileStream(inputFilePath, FileMode.Create))
                        {
                            fs.Write(inputImage.PixelData, 0, inputImage.PixelData.Length);
                        }
                        Console.WriteLine("Save inputImage: {0}!", inputFilePath);

                        using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
                        {
                            fs.Write(outImage.PixelData, 0, outImage.PixelData.Length);
                        }
                        Console.WriteLine("Save OutputImage: {0}!", outputFilePath);

                        //ch: 图像使用完及时释放，防止内存快速上涨导致频繁GC |en：Release image promptly to prevent rapid memory increase leading to frequent GC.
                        outImage.Dispose();
                    }

                    //ch: 释放图像缓存 | en: Release image buffer
                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", result);
                }

                // ch:停止抓图 | en:Stop grabbing
                result = device.StreamGrabber.StopGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", result);
                    return;
                }

                // ch:关闭设备 | en:Close device
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Close device failed:{0:x8}", result);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (MvError.MV_OK != result)
                {
                    // ch:销毁设备 | en:Destroy device
                    device.Dispose();
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}

/*
 * 这个示例演示了如何使用无损压缩解码功能
 * This sample demonstrates how to use the HBDecode function
 */

using MvCameraControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace Image_HBDecode
{
    class Image_HBDecode
    {
        static void Main(string[] args)
        {
            DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
     | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;


            int result = MvError.MV_OK;
            IDevice device = null;
            List<IDeviceInfo> deviceInfos;
            int packetSize;

            try
            {
                //ch: 初始化SDK |  en: Initialize SDK
                SDKSystem.Initialize();

                result = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfos);
                if (result != MvError.MV_OK)
                {
                    Console.Write("Enumerate device failed, result: {0:x8}", result);
                    return;
                }

                if (deviceInfos.Count == 0)
                {
                    Console.WriteLine("No device");
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in deviceInfos)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                // ch:需要连接的相机索引 || en:Select a device that want to connect
                Console.Write("Please input index(0-{0:d}):", deviceInfos.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > deviceInfos.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                //ch: 创建设备 | en: Create device
                device = DeviceFactory.CreateDevice(deviceInfos[devIndex]);
                if (device == null)
                {
                    Console.WriteLine("Create device failed!");
                    return;
                }

                //ch: 打开设备 | en:Open device
                result = device.Open();
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", result);
                    return;
                }

                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = (IGigEDevice)device;

                    // ch:探测网络最佳包大小(只对GigE相机有效) 
                    // en:Detection network optimal package size(It only works for the GigE camera)
                    result = gigEDevice.GetOptimalPacketSize(out packetSize);
                    if (MvError.MV_OK != result)
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", result);
                    }
                    else
                    {
                        result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", result);
                        }
                    }
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                result = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", result);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                result = device.StreamGrabber.StartGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", result);
                    return;
                }

                IFrameOut frameOut;
                // ch:获取一帧图像 | en:Get one image
                result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == result)
                {
                    Console.WriteLine("Get Image Buffer: Width[{0}] , Height[{1}] , FrameNum[{2}]"
                        , frameOut.Image.Width, frameOut.Image.Height, frameOut.FrameNum);

                    IFrameOut frameDecode;

                    // ch:图像解码 | en:Image Decode Process
                    result = device.ImageDecoder.HBDecode(frameOut, out frameDecode);
                    if (result != MvError.MV_OK)
                    {
                        Console.WriteLine("Image HBDecode failed:{0:x8}", result);
                        return;
                    }
                    Console.WriteLine("Image HBDecode success!");

                    //ch: 保持图像到文件 | en: Save image to file
                    string outputFilePath = string.Format("HBDecode_w{0}_h{1}_{2}.bmp", frameDecode.Image.Width, frameDecode.Image.Height, frameDecode.Image.PixelType);
                    ImageFormatInfo imageFormatInfo = new ImageFormatInfo();
                    imageFormatInfo.FormatType = ImageFormatType.Bmp;
                    result = device.ImageSaver.SaveImageToFile(outputFilePath, frameDecode.Image, imageFormatInfo, CFAMethod.Equilibrated);
                    if (result != MvError.MV_OK)
                    {
                         Console.WriteLine("Image Save failed:{0:x8}", result);
                         return;
                    }
                    Console.WriteLine("Save image success, {0}!", outputFilePath);

                    //ch: 图像使用完及时释放，防止内存快速上涨导致频繁GC | en：Release image promptly to prevent rapid memory increase leading to frequent GC.
                    frameDecode.Dispose();

                    //ch: 释放图像缓存 | en: Release image buffer
                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
                else
                {
                    Console.WriteLine("Get Image failed:{0:x8}", result);
                }

                // ch:停止抓图 | en:Stop grabbing
                result = device.StreamGrabber.StopGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", result);
                    return;
                }

                // ch:关闭设备 | en:Close device
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Close device failed:{0:x8}", result);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (device != null)
                {
                    // ch:销毁设备 | en:Destroy device
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}


/*
 * 这个示例演示了录像功能。
 * This sample shows the how to recording.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;
using System.Threading;

namespace Image_Recording
{
    class Program
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static volatile bool _grabThreadExit = false;
        static void FrameGrabThread(object obj)
        {
            int ret = MvError.MV_OK;

            IDevice device = obj as IDevice;
            IVideoRecorder recorder = device.VideoRecorder;
            IStreamGrabber streamGrabber = device.StreamGrabber;

            while (!_grabThreadExit)
            {
                IFrameOut frame;

                //ch：获取一帧图像 | en: Get one frame
                ret = streamGrabber.GetImageBuffer(1000, out frame);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    continue;
                }

                Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);

                //ch：图像添加到录像文件 | en: Record the frame
                ret = recorder.InputOneFrame(frame.Image);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Input one frame failed:{0:x8}", ret);
                }

                //ch: 释放图像缓存  | en: Release the image buffer
                streamGrabber.FreeImageBuffer(frame);
            }
        }

        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                // ch:设置触发模式为off | en:set trigger mode as off
                ret = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                    return;
                }

                //ch: 设置合适的缓存节点数量 | en: Setting the appropriate number of image nodes
                device.StreamGrabber.SetImageNodeNum(5);

                // ch:开启抓图 | en: start grab image
                ret = device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:获取录像所需的参数 | en: Get record parameters
                IIntValue widthValue;
                IIntValue heightValue;
                IEnumValue pixelTypeValue;
                IFloatValue frameRateValue;
                device.Parameters.GetIntValue("Width", out widthValue);
                device.Parameters.GetIntValue("Height", out heightValue);
                device.Parameters.GetEnumValue("PixelFormat", out pixelTypeValue);
                device.Parameters.GetFloatValue("ResultingFrameRate", out frameRateValue);

                // ch:开启录像 | en: start record
                RecordParam recordParam = new RecordParam();
                recordParam.Width = (uint)widthValue.CurValue;
                recordParam.Height = (uint)heightValue.CurValue;
                recordParam.PixelType = (MvGvspPixelType)pixelTypeValue.CurEnumEntry.Value;

                // ch:帧率(大于1/16)fps | en:Frame Rate (>1/16)fps
                recordParam.FrameRate = frameRateValue.CurValue;
                // ch:码率kbps(128kbps-16Mbps) | en:Bitrate kbps(128kbps-16Mbps)
                recordParam.BitRate = 1000;
                // ch:录像格式(仅支持AVI) | en:Record Format(AVI is only supported)
                recordParam.FormatType = VideoFormatType.AVI;

                ret = device.VideoRecorder.StartRecord("./Recording.avi", recordParam);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start record failed:{0:x8}", ret);
                    return;
                }

                // ch:开启抓图线程 | en: Start the grabbing thread
                Thread GrabThread = new Thread(FrameGrabThread);
                GrabThread.Start(device);

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                //ch: 通知线程退出 | en: Notify the grab thread to exit
                _grabThreadExit = true;
                GrabThread.Join();

                // ch:停止录像 | en:Stop record
                ret = device.VideoRecorder.StopRecord();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop record failed:{0:x8}", ret);
                }

                // ch:停止抓图 | en:Stop grabbing
                ret = device.StreamGrabber.StopGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
                    return;
                }

                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}
/*
 * 这个示例演示了如何保存图像数据
 * This sample demonstrates how to save image data
 */

using MvCameraControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Image_Save
{
    class Image_Save
    {
        const DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLGigEDevice
    | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;
        public void Run()
        {
            IDevice device = null;
            List<IDeviceInfo> deviceInfos;

            try
            {
                int result = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfos);
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Enumerate device failed, result: {0:x8}", result);
                    return;
                }

                if (deviceInfos.Count == 0)
                {
                    Console.WriteLine("No device");
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in deviceInfos)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                // ch:需要连接的相机索引 || en:Select a device that want to connect
                Console.Write("Please input index(0-{0:d}):", deviceInfos.Count - 1);
                devIndex = Convert.ToInt32(Console.ReadLine());
                if (devIndex > deviceInfos.Count - 1 || devIndex < 0)
                {
                    Console.WriteLine("Input Error!\n");
                    return;
                }

                // ch:选择要保存的文件类型 | en: Select the file type for image save
                Console.WriteLine("Please select the file type to save:");
                Console.WriteLine("0: Raw");
                Console.WriteLine("1: " + ImageFormatType.Bmp);
                Console.WriteLine("2: " + ImageFormatType.Jpeg);
                Console.WriteLine("3: " + ImageFormatType.Png);
                Console.WriteLine("4: " + ImageFormatType.Tiff);
                int imageFormatType = Convert.ToInt32(Console.ReadLine());
                if (imageFormatType < 0 || imageFormatType > 4)
                {
                    Console.WriteLine("Input Error!\n");
                    return;
                }

                //ch: 创建设备 | en: Create device
                device = DeviceFactory.CreateDevice(deviceInfos[devIndex]);
                if (device == null)
                {
                    Console.WriteLine("Create device failed!");
                    return;
                }

                //ch: 打开设备 | en:Open device
                result = device.Open();
                if (result != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", result);
                    return;
                }

                //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
                if (device is IGigEDevice)
                {
                    //ch: 转换为gigE设备 | en: Convert to Gige device
                    IGigEDevice gigEDevice = (IGigEDevice)device;

                    // ch:探测网络最佳包大小(只对GigE相机有效) 
                    // en:Detection network optimal package size(It only works for the GigE camera)
                    int packetSize;
                    result = gigEDevice.GetOptimalPacketSize(out packetSize);
                    if (MvError.MV_OK != result)
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", result);
                    }
                    else
                    {
                        result = gigEDevice.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (MvError.MV_OK != result)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", result);
                        }
                    }
                }

                // ch:设置触发模式为off || en:set trigger mode as off
                result = device.Parameters.SetEnumValue("TriggerMode", 0);
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Set TriggerMode failed:{0:x8}", result);
                    return;
                }

                // ch:开启抓图 || en: start grab image
                result = device.StreamGrabber.StartGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", result);
                    return;
                }

                IFrameOut frameOut;
                // ch:获取一帧图像 | en:Get one image
                result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (MvError.MV_OK == result)
                {
                    Console.WriteLine("Get Image Buffer: Width[{0}] , Height[{1}] , FrameNum[{2}]"
                        , frameOut.Image.Width, frameOut.Image.Height, frameOut.FrameNum);

                    // ch:如果图像是HB格式，需要先解码 | en:If the image is HB format, should to be decoded first
                    IFrameOut frameForSave = frameOut;
                    if (frameOut.Image.PixelType.ToString().Contains("HB"))
                    {
                        result = device.ImageDecoder.HBDecode(frameOut, out frameForSave);
                        if (result != MvError.MV_OK)
                        {
                            Console.WriteLine("HB Decode failed:{0:x8}", result);
                            return;
                        }
                    }

                    // ch:保存图像 | en:Save image
                    string fileName = string.Format("Image_w{0}_h{1}_p{2}", frameForSave.Image.Width, frameForSave.Image.Height, frameForSave.Image.PixelType);
                    switch(imageFormatType)
                    {
                        case 0:
                            {
                                // ch:保存raw数据 | en:Save raw data
                                fileName += ".raw";
                                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                                {
                                    // ch:图像超过int.MaxValue时, 分段保存 | en:The save image in segments When byte array is large than int.MaxValue
                                    UInt64 intMaxValue = (UInt64)int.MaxValue;
                                    if (frameForSave.Image.ImageSize > intMaxValue)
                                    {
                                        int blockLen = 1024 * 1024;
                                        byte[] newData = new byte[blockLen];

                                        UInt64 remain = frameForSave.Image.ImageSize;

                                        IntPtr ptrSourceTemp = frameForSave.Image.PixelDataPtr;
                                        for (UInt64 i = 0; i < remain; i++)
                                        {
                                            int writeLen = (int)(remain > (UInt64)blockLen ? (UInt64)blockLen : remain);
                                            Marshal.Copy(ptrSourceTemp, newData, 0, writeLen); 
                                            fs.Write(newData, 0, writeLen);

                                            remain -= (UInt64)writeLen;
                                            ptrSourceTemp = new IntPtr(ptrSourceTemp.ToInt64() + writeLen);
                                        }
                                    }
                                    else
                                    {
                                        fs.Write(frameForSave.Image.PixelData, 0, (int)frameForSave.Image.ImageSize);
                                    }
                                }
                                Console.WriteLine("Save image success! " + fileName);
                            }
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            {
                                ImageFormatInfo imageFormatInfo = new ImageFormatInfo();
                                imageFormatInfo.FormatType = (ImageFormatType)imageFormatType;

                                // ch:JPEG格式需要配置图像质量 | en:Set JpegQuality for JPEG file
                                if (imageFormatInfo.FormatType == ImageFormatType.Jpeg)
                                {
                                    imageFormatInfo.JpegQuality = 80;
                                }

                                // ch:图像保存的文件名 | en:Save image to file
                                fileName += "." + imageFormatInfo.FormatType.ToString();

                                // ch:保存图像 | en:Save image to file 
                                result = device.ImageSaver.SaveImageToFile(fileName, frameForSave.Image, imageFormatInfo, CFAMethod.Equilibrated);
                                if (result != MvError.MV_OK)
                                {
                                    Console.WriteLine("SaveImageToFile failed:{0:x8}", result);
                                    return;
                                }
                                Console.WriteLine("Save image success! " + fileName);
                            }
                            break;
                        default:
                            Console.WriteLine("Input file type error");
                            return;
                    }

                    //ch: 图像使用完及时释放，防止内存快速上涨导致频繁GC | en：Release image promptly to prevent rapid memory increase leading to frequent GC.
                    if (frameForSave != frameOut)
                    {
                        frameForSave.Image.Dispose();
                    }
                }

                //ch: 释放图像缓存 | en: Release image buffer
                device.StreamGrabber.FreeImageBuffer(frameOut);

                // ch:停止抓图 | en:Stop grabbing
                result = device.StreamGrabber.StopGrabbing();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Stop grabbing failed:{0:x8}", result);
                    return;
                }

                // ch:关闭设备 | en:Close device
                result = device.Close();
                if (MvError.MV_OK != result)
                {
                    Console.WriteLine("Close device failed:{0:x8}", result);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
                device = null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (device != null)
                {
                    // ch:销毁设备 | en:Destroy device
                    device.Dispose();
                    device = null;
                }
            }
        }
        static void Main(string[] args)
        {
            //ch: 初始化SDK |  en: Initialize SDK
            SDKSystem.Initialize();

            Image_Save program = new Image_Save();
            program.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();

            // ch: 反初始化SDK | en: Finalize SDK
            SDKSystem.Finalize();
        }
    }
}


/*
 * 这个示例演示了从相机中获取配置文件。
 * This sample shows how to obtain configuration files from a camera.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;
using System.Threading;

namespace ParameterCamera_FileAccess
{
    class ParameterCamera_FileAccess
    {
        private const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
           | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        public void Run()
        {
            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }


                //Ch: 读设备文件 | Read device file
                Thread readThread = new Thread(() => 
                {
                    int readRet = device.Parameters.FileAccessRead("UserSet1", "UserSet1.bin");
                    if (readRet != MvError.MV_OK)
                    {
                        Console.WriteLine("FileAccessRead failed {0:x8}", readRet);
                    }
                    else
                    {
                        Console.WriteLine("FileAccessRead success");
                    }
                });


                //ch:获取文件存取进度 |en:Get progress of file access
                Thread readProgressThread = new Thread(() =>
                {
                    while (true)
                    {
                        Int64 completed;
                        Int64 total;
                        int progressRet = device.Parameters.GetFileAccessProgress(out completed, out total);
                        if (progressRet != MvError.MV_OK)
                        {
                            Console.WriteLine("GetFileAccessProgress failed {0:x8}", progressRet);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("GetFileAccessProgress: Completed = {0}, Totoal = {1}", completed, total);

                            if (completed == total && total != 0)
                            {
                                break;
                            }
                        }

                        Thread.Sleep(50);
                    }

                });

                
                readThread.Start();
                readProgressThread.Start();
                readThread.Join();
                readProgressThread.Join();

                Console.WriteLine("");

                //Ch: 写设备文件 | Write file to device
                Thread writeThread = new Thread(() =>
                {
                    int readRet = device.Parameters.FileAccessWrite("UserSet1", "UserSet1.bin");
                    if (readRet != MvError.MV_OK)
                    {
                        Console.WriteLine("FileAccessWrite failed {0:x8}", readRet);
                    }
                    else
                    {
                        Console.WriteLine("FileAccessWrite success");
                    }
                });

                //ch:获取文件存取进度 |en:Get progress of file access
                Thread writeProgressThread = new Thread(() =>
                {
                    while (true)
                    {
                        Int64 completed;
                        Int64 total;
                        int progressRet = device.Parameters.GetFileAccessProgress(out completed, out total);
                        if (progressRet != MvError.MV_OK)
                        {
                            Console.WriteLine("GetFileAccessProgress failed {0:x8}", progressRet);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("GetFileAccessProgress: Completed = {0}, Totoal = {1}", completed, total);

                            if (completed == total && total != 0)
                            {
                                break;
                            }
                        }

                        Thread.Sleep(50);
                    }

                });

                writeThread.Start();
                writeProgressThread.Start();
                writeThread.Join();
                writeProgressThread.Join();
                
                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

            }
        }


        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            ParameterCamera_FileAccess program = new ParameterCamera_FileAccess();
            program.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();

            // ch: 反初始化SDK | en: Finalize SDK
            SDKSystem.Finalize();
        }
    }
}

/*
 * 这个示例演示了从相机中导入导出配置文件。
 * This sample shows how to importing and exporting configuration files from a camera.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvCameraControl;

namespace ParameterCamera_LoadAndSave
{
    class ParameterCamera_LoadAndSave
    {
        private const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
           | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        public void Run()
        {
            IDevice device = null;

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());

                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                // ch:创建设备 | en:Create device
                device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device is IGigEDevice)
                {
                    int packetSize;
                    ret = (device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                Console.WriteLine("Start export the camera properties to the file");
                Console.WriteLine("Wait......");
                // ch:将相机属性导出到文件中
                // en:Export the camera properties to the file
                ret = device.Parameters.FeatureSave("CameraFile.mfs");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("FeatureSave failed!");
                    return;
                }

                Console.WriteLine("Finish export the camera properties to the file\n");

                Console.WriteLine("Start import the camera properties from the file");
                Console.WriteLine("Wait......");
                // ch:从文件中导入相机属性
                // en:Import the camera properties from the file
                ret = device.Parameters.FeatureLoad("CameraFile.mfs");
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("FeatureLoad failed!");
                    return;
                }

                Console.WriteLine("Finish import the camera properties from the file");

               
                // ch:关闭设备 | en:Close device
                ret = device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                device.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

            }
        }


        static void Main(string[] args)
        {
            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            ParameterCamera_LoadAndSave program = new ParameterCamera_LoadAndSave();
            program.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadKey();

            // ch: 反初始化SDK | en: Finalize SDK
            SDKSystem.Finalize();
        }
    }
}

/*
 * 这个示例演示了如何在相机异常断开时重新连接相机。
 * This sample shows how to reconnect the camera when it disconnects abnormally.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MvCameraControl;

namespace Reconnect
{
    class Reconnect
    {
        const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
        | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        static bool _bExit = false;
        static bool _bConnect = false;
        static IDevice _device = null;
        static string _serialNumber;

        static void FrameGrabThread(object obj)
        {
            IStreamGrabber streamGrabber = (IStreamGrabber)obj;

            while (true)
            {
                if (!_bConnect)
                {
                    break;
                }
                IFrameOut frame;

                //ch：获取一帧图像 | en: Get one frame
                int ret = streamGrabber.GetImageBuffer(1000, out frame);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Get Image failed:{0:x8}", ret);
                    continue;
                }

                Console.WriteLine("Get one frame: Width[{0}] , Height[{1}] , FrameNum[{2}]", frame.Image.Width, frame.Image.Height, frame.FrameNum);
           
                //ch: 释放图像缓存  | en: Release the image buffer
                streamGrabber.FreeImageBuffer(frame);
            }
        }

        static void ExceptionEventHandler(object sender, DeviceExceptionArgs e)
        {
            if (e.MsgType == DeviceExceptionType.DisConnect)
            {
                Console.WriteLine("Device disconnect!");
                _bConnect = false;
            }
        }

        public static void ReconnectProcess()
        {
            int ret = MvError.MV_OK;

            while (true)
            {
                if (_bConnect)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (_bExit)
                {
                    break;
                }

                if (_device != null)
                {
                    _device.StreamGrabber.StopGrabbing();
                    _device.Close();
                    _device.Dispose();
                    _device = null;
                }

                Console.WriteLine("connecting, please wait...");
                List<IDeviceInfo> devInfoList;
                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    continue;
                }

                if (0 == devInfoList.Count)
                {
                    continue;
                }

                //ch:根据序列号选择相机 | en: Select camera by serial number 
                int devIndex = 0;
                bool findDevice = false;
                foreach (var devInfo in devInfoList)
                {
                    if (_serialNumber == devInfo.SerialNumber)
                    {
                        findDevice = true;
                        break;
                    }
                    devIndex++;
                }

                if (!findDevice)
                {
                    continue;
                }
                // ch:创建设备 | en:Create device
                _device = DeviceFactory.CreateDevice(devInfoList[devIndex]);

                // ch:打开设备 | en:Open device
                ret = _device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return;
                }

                _bConnect = true;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (_device is IGigEDevice)
                {
                    int packetSize;
                    ret = (_device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                    if (packetSize > 0)
                    {
                        ret = _device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                        if (ret != MvError.MV_OK)
                        {
                            Console.WriteLine("Warning: Set Packet Size failed {0:x8}", ret);
                        }
                        else
                        {
                            Console.WriteLine("Set PacketSize to {0}", packetSize);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Warning: Get Packet Size failed {0:x8}", ret);
                    }
                }

                _device.DeviceExceptionEvent += ExceptionEventHandler;

                Console.WriteLine("connect succeed!");

                // ch:开启抓图 | en: start grab image
                ret = _device.StreamGrabber.StartGrabbing();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", ret);
                    continue;
                }

                // ch:开启抓图线程 | en: Start the grabbing thread
                Thread GrabThread = new Thread(FrameGrabThread);
                GrabThread.Start(_device.StreamGrabber);
            }
        }

        static void Main(string[] args)
        {
            int ret = MvError.MV_OK;
           

            // ch: 初始化 SDK | en: Initialize SDK
            SDKSystem.Initialize();

            try
            {
                List<IDeviceInfo> devInfoList;

                // ch:枚举设备 | en:Enum device
                ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", ret);
                    return;
                }

                Console.WriteLine("Enum device count : {0}", devInfoList.Count);

                if (0 == devInfoList.Count)
                {
                    return;
                }

                // ch:打印设备信息 en:Print device info
                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}]:", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);
                        Console.WriteLine("DevIP: {0}.{1}.{2}.{3}", nIp1, nIp2, nIp3, nIp4);
                    }

                    Console.WriteLine("ModelName:" + devInfo.ModelName);
                    Console.WriteLine("SerialNumber:" + devInfo.SerialNumber);
                    Console.WriteLine();
                    devIndex++;
                }

                Console.Write("Please input index(0-{0:d}):", devInfoList.Count - 1);

                devIndex = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                if (devIndex > devInfoList.Count - 1 || devIndex < 0)
                {
                    Console.Write("Input Error!\n");
                    return;
                }

                _serialNumber = devInfoList[devIndex].SerialNumber;

                Thread reconnectThread = new Thread(ReconnectProcess);
                reconnectThread.Start();
                               
                Console.WriteLine("Press enter to exit");
                Console.ReadKey();

                _bConnect = false;
                _bExit = true;
                reconnectThread.Join();

                // ch:关闭设备 | en:Close device
                ret = _device.Close();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Close device failed:{0:x8}", ret);
                    return;
                }

                // ch:销毁设备 | en:Destroy device
                _device.Dispose();
                _device = null;

            }
            catch (Exception e)
            {

                Console.Write("Exception: " + e.Message);
            }
            finally
            {
                // ch:销毁设备 | en:Destroy device
                if (_device != null || ret != MvError.MV_OK)
                {
                    _device.Dispose();
                    _device = null;
                }

                // ch: 反初始化SDK | en: Finalize SDK
                SDKSystem.Finalize();

                Console.WriteLine("Press enter to exit");
                Console.ReadKey();
            }
        }
    }
}

















