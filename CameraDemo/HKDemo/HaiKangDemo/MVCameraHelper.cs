using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MvCamCtrl.NET;
using HalconDotNet;
using System.Threading;

namespace HaiKangDemo
{
    /// <summary>
    /// 海康SDK
    /// </summary>
    class MVCameraHelper
    {
        MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();//设备列表(sdk)
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();//图像属性
        MyCamera.cbOutputExdelegate ImageCallback;//回调对象
        string _DeviceName;//设备名称
        List<string> _DeviceList = new List<string>();//设备列表
        public static HObject _Image;//采集的图片
        private MyCamera m_MyCamera = new MyCamera();//选中的相机对象
        int _Mode = 0;//0主动采图/1回调采图
        bool CameraGrabState = false;
        Thread InitiativeThread;//主动采图线程

        #region 构造方法
        public MVCameraHelper(string DeviceName, int Mode = 0)
        {
            _DeviceName = DeviceName;
            if (Mode == 0 || Mode == 1)
            {
                _Mode = Mode;
            }

        }
        #endregion

        #region 遍历设备
        /// <summary>
        /// 遍历设备
        /// </summary>
        public void EnumCamera()
        {
            // ch:创建设备列表 | en:Create Device List
            System.GC.Collect();
            _DeviceList.Clear();// 设备列表
            m_stDeviceList.nDeviceNum = 0;
            // 枚举设备
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                return;//设备连接失败
            }

            // 遍历设备添加进设备列表
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (gigeInfo.chUserDefinedName != "")
                    {

                        _DeviceList.Add(gigeInfo.chSerialNumber);
                    }
                    else
                    {
                        _DeviceList.Add(gigeInfo.chSerialNumber);
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        _DeviceList.Add(usbInfo.chSerialNumber);
                    }
                    else
                    {
                        _DeviceList.Add(usbInfo.chSerialNumber);
                    }
                }
            }
        }

        #endregion

        #region 初始化设备
        public bool OpenCamera()
        {
            try
            {
                EnumCamera();
                int Index = 0;
                if (_DeviceList.Contains(_DeviceName))
                {
                    Index = _DeviceList.IndexOf(_DeviceName);
                }
                else
                {
                    return false;//集合中没有设备名
                }
                // 2获取选择的设备信息 | en:Get selected device information
                MyCamera.MV_CC_DEVICE_INFO device =
        (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[Index],
                                                      typeof(MyCamera.MV_CC_DEVICE_INFO));
                //3创建某一个设备
                int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref device);
                if (MyCamera.MV_OK != nRet)
                {
                    return false;//创建失败
                }

                //4打开某一个设备
                nRet = m_MyCamera.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    m_MyCamera.MV_CC_DestroyDevice_NET();
                    return false; //打开失败
                }

                // 5探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    int nPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                    if (nPacketSize > 0)
                    {
                        //设置数据包的大小
                        nRet = m_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                        if (nRet != MyCamera.MV_OK)
                        {
                            return false;//设置失败
                        }
                    }
                    else
                    {
                        return false;//获取失败
                    }
                }
                // 6设置采集连续模式 | en:Set Continues Aquisition Mode
                m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                //设置触发模式
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                //m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                //m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                //m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);

                // ch:注册回调函数 | en:Register image callback
                if (_Mode == 1)
                {
                    ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackFunc);
                    nRet = m_MyCamera.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);
                    GC.KeepAlive(ImageCallback);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 开始采集
        public bool startGrab()
        {
            try
            {
                if (_Mode == 0)
                {
                    if (!InitiativeThread.IsAlive)
                    {
                        CameraGrabState = true;
                        ImageInitiativeFunc();
                    }
                    else
                    {
                        return false;
                    }
                }
                m_stFrameInfo.nFrameLen = 0;//取流之前先清除帧长度
                m_stFrameInfo.enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;
                ////开始采集
                int nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 停止采集
        public bool StopGrab()
        {
            try
            {
                // ch:停止采集 | en:Stop Grabbing
                CameraGrabState = false;
                int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
                if (nRet != MyCamera.MV_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

        #region 关闭相机
        public bool ColseCamera()
        {
            try
            {
                StopGrab();
                if (InitiativeThread != null)
                {
                    InitiativeThread.Abort();//主动采集线程释放
                }
                m_MyCamera.MV_CC_CloseDevice_NET();
                m_MyCamera.MV_CC_DestroyDevice_NET();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 回调函数
        public void ImageCallbackFunc(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
            {
                int bytesbylineGray = (pFrameInfo.nWidth + 3) / 4 * 4;//灰度图像每行所占字节数
                byte[] ImageGray = new byte[bytesbylineGray * pFrameInfo.nHeight];
                //黑白相机要使用ImageGray  存储
                Marshal.Copy(pData, ImageGray, 0, bytesbylineGray * pFrameInfo.nHeight);
                unsafe
                {
                    fixed (byte* p = ImageGray)
                    {
                        HOperatorSet.GenImage1Extern(out _Image, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, new IntPtr(p), 0);
                    }
                }
            }
            else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed)
            {
                int bytesbylineGray = (pFrameInfo.nWidth + 3) / 4 * 4;//灰度图像每行所占字节数
                int bytesbylineRGB = (pFrameInfo.nHeight * 3 + 3) / 4 * 4;//彩色图像每行所占字节数
                byte[] BufferR = new byte[bytesbylineGray * pFrameInfo.nHeight];
                byte[] BufferG = new byte[bytesbylineGray * pFrameInfo.nHeight];
                byte[] BufferB = new byte[bytesbylineGray * pFrameInfo.nHeight];
                byte[] ImageBuffer = new byte[bytesbylineRGB * pFrameInfo.nHeight];
                byte[] ImageGray = new byte[bytesbylineGray * pFrameInfo.nHeight];

                for (int i = 0; i < pFrameInfo.nHeight; i++)
                {
                    for (int j = 0; j < pFrameInfo.nWidth; j++)
                    {
                        BufferB[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 0];
                        BufferG[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 1];
                        BufferR[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 2];
                        //三通转单通道
                        // ImageGray[i * bytesbylineGray + j] = (byte)(BufferB[i * bytesbylineGray + j] * 0.11 + BufferG[i * bytesbylineGray + j] * 0.5 + BufferR[i * bytesbylineGray + j] * 0.3);
                    }
                }
                unsafe
                {
                    fixed (byte* pr = BufferR, pg = BufferG, pb = BufferB)
                    {
                        HOperatorSet.GenImage3Extern(out _Image, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, new IntPtr(pr), new IntPtr(pg), new IntPtr(pb), 0);
                    }
                }
            }
        }

        #endregion

        #region 主动采图线程
        public void ImageInitiativeFunc()
        {
            try
            {
                MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();
                int nRet = m_MyCamera.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
                UInt32 nPayloadSize = stParam.nCurValue;
                IntPtr m_BufForDriver = Marshal.AllocHGlobal((Int32)nPayloadSize);
                InitiativeThread = new Thread(() =>
                {
                    while (CameraGrabState)
                    {
                        nRet = m_MyCamera.MV_CC_GetOneFrameTimeout_NET(m_BufForDriver, nPayloadSize, ref m_stFrameInfo, 1000);
                        if (nRet != MyCamera.MV_OK)
                        {
                            Thread.Sleep(10);
                            continue;
                        }
                        if (m_stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                        {
                            int bytesbylineGray = (m_stFrameInfo.nWidth + 3) / 4 * 4;//灰度图像每行所占字节数
                            byte[] ImageGray = new byte[bytesbylineGray * m_stFrameInfo.nHeight];
                            //黑白相机要使用ImageGray  存储
                            Marshal.Copy(m_BufForDriver, ImageGray, 0, bytesbylineGray * m_stFrameInfo.nHeight);

                            unsafe
                            {
                                fixed (byte* p = ImageGray)
                                {
                                    HOperatorSet.GenImage1Extern(out _Image, "byte", m_stFrameInfo.nWidth, m_stFrameInfo.nHeight, new IntPtr(p), 0);
                                }
                            }
                        }
                        else if (m_stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed)
                        {
                            int bytesbylineGray = (m_stFrameInfo.nWidth + 3) / 4 * 4;//灰度图像每行所占字节数
                            int bytesbylineRGB = (m_stFrameInfo.nHeight * 3 + 3) / 4 * 4;//彩色图像每行所占字节数
                            byte[] BufferR = new byte[bytesbylineGray * m_stFrameInfo.nHeight];
                            byte[] BufferG = new byte[bytesbylineGray * m_stFrameInfo.nHeight];
                            byte[] BufferB = new byte[bytesbylineGray * m_stFrameInfo.nHeight];
                            byte[] ImageBuffer = new byte[bytesbylineRGB * m_stFrameInfo.nHeight];
                            byte[] ImageGray = new byte[bytesbylineGray * m_stFrameInfo.nHeight];

                            for (int i = 0; i < m_stFrameInfo.nHeight; i++)
                            {
                                for (int j = 0; j < m_stFrameInfo.nWidth; j++)
                                {
                                    BufferB[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 0];
                                    BufferG[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 1];
                                    BufferR[i * bytesbylineGray + j] = ImageBuffer[i * bytesbylineRGB + j * 3 + 2];
                                    //三通转单通道
                                    // ImageGray[i * bytesbylineGray + j] = (byte)(BufferB[i * bytesbylineGray + j] * 0.11 + BufferG[i * bytesbylineGray + j] * 0.5 + BufferR[i * bytesbylineGray + j] * 0.3);
                                }
                            }
                            unsafe
                            {
                                fixed (byte* pr = BufferR, pg = BufferG, pb = BufferB)
                                {
                                    HOperatorSet.GenImage3Extern(out _Image, "byte", m_stFrameInfo.nWidth, m_stFrameInfo.nHeight, new IntPtr(pr), new IntPtr(pg), new IntPtr(pb), 0);
                                }
                            }
                        }
                    }
                })
                {
                    IsBackground = true,
                };
                InitiativeThread.Start();
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }

}
