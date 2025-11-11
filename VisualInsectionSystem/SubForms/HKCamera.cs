//using MvCamCtrl.NET;
using IfModuleCs;
using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace VisualInsectionSystem.SubForms
{
    public partial class HKCamera : Form  
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count); // 内存拷贝函数 | EN: Memory copy function
        public const Int32 CUSTOMER_PIXEL_FORMAT = unchecked((Int32)0x80000000);   // 判断用户自定义像素格式  | EN: Determine user-defined pixel format

        // 设备列表
        MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();  // 图像帧信息
        private MyCamera m_MyCamera = null;     //相机对象
        public bool m_bGrabbing = false;        // 取流标志，是否开始抓图
        public bool IsConnect { get; private set; }  // 相机连接状态
        Thread m_hReceiveThread = null;     // 图像取图线程

        // 从驱动获取图像数据的缓存 | EN: Cache for obtaining image data from the driver
        UInt32 m_nBufSizeForDriver = 0;
        IntPtr m_pBufForDriver = IntPtr.Zero;
        // R通道数据
        byte[] m_pDataForRed = null;
        // G通道数据
        byte[] m_pDataForGreen = null;
        // B通道数据
        byte[] m_pDataForBlue = null;
        
        // ch:Bitmap及其像素格式 | en:Bitmap and Pixel Format
        Bitmap m_bitmap = null;
        PixelFormat m_bitmapPixelFormat = PixelFormat.DontCare;
        IntPtr m_ConvertDstBuf = IntPtr.Zero;   // 图像转换后的数据缓存 
        UInt32 m_nConvertDstBufLen = 0;

        private static Object BufForDriverLock = new Object();  // 读写图像时锁定
        private static Object BufForImageLock = new Object();

        IntPtr displayHandle = IntPtr.Zero;     // 显示句柄 | EN: Display handle

        private int caotureCount = 0;   // 拍摄计数
        private bool isBatchCapture = false; // 是否正在批量拍摄
         public PLCCommunicator PlcCommunicator { get; set; }  // 接收从MainForm传递的PLC实例

        public HKCamera()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Load += new EventHandler(HKCamera_Load);

        }
        private void HKCamera_Load(object sender, EventArgs e)
        {
            // 初始化SDK
            MyCamera.MV_CC_Initialize_NET();
            // 枚举相机（保持原有逻辑）
            // DeviceListAcq();  // 根据需要决定是否默认枚举
        }

        private void HKCamera_FormClosed(object sender, FormClosedEventArgs e)
        {
            // ch: 关闭设备 | en: Close Device
            button4_Click(sender, e);

            // ch: 反初始化SDK | en: Finalize SDK
            MyCamera.MV_CC_Finalize_NET();
        }

        // 删除字符串尾部的空字符 | EN: Delete the null character at the end of the string
        private string DeleteTail(string strstrUserDefinedName)
        {
            strstrUserDefinedName = Regex.Unescape(strstrUserDefinedName);
            int nIndex = strstrUserDefinedName.IndexOf('\0');
            if (nIndex >= 0)
            {
                strstrUserDefinedName = strstrUserDefinedName.Remove(nIndex);
            }
            return strstrUserDefinedName;
        }

        // 判断是否为黑白相机像素格式 | EN: Determine whether it is a monochrome camera pixel format
        private Boolean IsMonoData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono16:
                    return true;
                default:
                    return false;
            }
        }
        /************************************************************************
         *  @fn     IsColorData()
         *  @brief  判断是否是彩色数据，颜色通道       
         *  @return 成功，返回0；错误，返回-1 
         *************************************************************************/
        private Boolean IsColorData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        // ch:查找相机
        private void button1_Click(object sender, EventArgs e)
        {
            // ch：枚举相机 | en: Enumerate cameras
            DeviceListAcq();
        }

        // ch:创建设备列表 | EN:Create device list
        public void DeviceListAcq()
        {
            // CH:枚举设备列表 | EN:Create device list
            System.GC.Collect();
            comboBox1.Items.Clear();        // 设备列表
            m_stDeviceList.nDeviceNum = 0;  // 设备数量清零

            // ch:枚举了所有类型，根据实际情况，选择合适的枚举类型即可 |
            // en:Enumerate all types, select the appropriate enumeration type according to the actual situation
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE | MyCamera.MV_GENTL_GIGE_DEVICE
                | MyCamera.MV_GENTL_CAMERALINK_DEVICE | MyCamera.MV_GENTL_CXP_DEVICE | MyCamera.MV_GENTL_XOF_DEVICE, ref m_stDeviceList);

            if (0 != nRet)
            {
                ShowErrorMsg("Enum Devices Fail!", nRet);
                return;
            }
            else
            {
                // todo list
            }
            //ch:在combobox列表中显示相机
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i],
                    typeof(MyCamera.MV_CC_DEVICE_INFO));
                string strUserDefinedName = "";
                // 1. 普通GigE设备（非GenTL抽象的GigE相机）
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO_EX gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO_EX)MyCamera.ByteToStruct(
                        device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO_EX));
                    if ((gigeInfo.chUserDefinedName.Length > 0) && (gigeInfo.chUserDefinedName[0] != '\0'))
                    {
                        //if (MyCamera.IsTextUTF8(gigeInfo.chUserDefinedName))
                        //{
                        //    strUserDefinedName = Encoding.UTF8.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0');
                        //}
                        //else
                        //{
                        //    strUserDefinedName = Encoding.Default.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0');
                        //}
                        strUserDefinedName = MyCamera.IsTextUTF8(gigeInfo.chUserDefinedName)
                            ? Encoding.UTF8.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0')
                            : Encoding.Default.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0');
                        comboBox1.Items.Add("GEV: " + DeleteTail(strUserDefinedName) + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        comboBox1.Items.Add("GEV: " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                }
                // 2. USB3.0 Vision设备
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO_EX usbInfo = (MyCamera.MV_USB3_DEVICE_INFO_EX)
                        MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO_EX));

                    if ((usbInfo.chUserDefinedName.Length > 0) && (usbInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(usbInfo.chUserDefinedName)
                            ? Encoding.UTF8.GetString(usbInfo.chUserDefinedName).TrimEnd('\0')
                            : Encoding.Default.GetString(usbInfo.chUserDefinedName).TrimEnd('\0');
                        comboBox1.Items.Add("U3V: " + DeleteTail(strUserDefinedName) + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        comboBox1.Items.Add("U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }
                // 3. 基于GenTL的CameraLink设备（工业CameraLink接口，通过GenTL抽象）
                else if (device.nTLayerType == MyCamera.MV_GENTL_CAMERALINK_DEVICE)
                {
                    MyCamera.MV_CML_DEVICE_INFO CMLInfo = (MyCamera.MV_CML_DEVICE_INFO)
                        MyCamera.ByteToStruct(device.SpecialInfo.stCMLInfo, typeof(MyCamera.MV_CML_DEVICE_INFO));

                    if ((CMLInfo.chUserDefinedName.Length > 0) && (CMLInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(CMLInfo.chUserDefinedName)
                            ? Encoding.UTF8.GetString(CMLInfo.chUserDefinedName).TrimEnd('\0')
                            : Encoding.Default.GetString(CMLInfo.chUserDefinedName).TrimEnd('\0');
                        comboBox1.Items.Add("CML: " + DeleteTail(strUserDefinedName) + " (" + CMLInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        comboBox1.Items.Add("CML: " + CMLInfo.chManufacturerInfo + " " + CMLInfo.chModelName + " (" + CMLInfo.chSerialNumber + ")");
                    }
                }
                // 4. 基于GenTL的CoaXPress设备（高速同轴接口，通过GenTL抽象）
                else if (device.nTLayerType == MyCamera.MV_GENTL_CXP_DEVICE)
                {
                    MyCamera.MV_CXP_DEVICE_INFO CXPInfo = (MyCamera.MV_CXP_DEVICE_INFO)
                        MyCamera.ByteToStruct(device.SpecialInfo.stCXPInfo, typeof(MyCamera.MV_CXP_DEVICE_INFO));

                    if ((CXPInfo.chUserDefinedName.Length > 0) && (CXPInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(CXPInfo.chUserDefinedName)
                            ? Encoding.UTF8.GetString(CXPInfo.chUserDefinedName).TrimEnd('\0')
                            : Encoding.Default.GetString(CXPInfo.chUserDefinedName).TrimEnd('\0');
                        comboBox1.Items.Add("CXP: " + DeleteTail(strUserDefinedName) + " (" + CXPInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        comboBox1.Items.Add("CXP: " + CXPInfo.chManufacturerInfo + " " + CXPInfo.chModelName + " (" + CXPInfo.chSerialNumber + ")");
                    }
                }
                // 5. 基于GenTL的Xover Fiber设备（光纤接口，通过GenTL抽象）
                else if (device.nTLayerType == MyCamera.MV_GENTL_XOF_DEVICE)
                {
                    MyCamera.MV_XOF_DEVICE_INFO XOFInfo = (MyCamera.MV_XOF_DEVICE_INFO)
                        MyCamera.ByteToStruct(device.SpecialInfo.stXoFInfo, typeof(MyCamera.MV_XOF_DEVICE_INFO));

                    if ((XOFInfo.chUserDefinedName.Length > 0) && (XOFInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(XOFInfo.chUserDefinedName)
                            ? Encoding.UTF8.GetString(XOFInfo.chUserDefinedName).TrimEnd('\0')
                            : Encoding.Default.GetString(XOFInfo.chUserDefinedName).TrimEnd('\0');
                        comboBox1.Items.Add("XOF: " + DeleteTail(strUserDefinedName) + " (" + XOFInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        comboBox1.Items.Add("XOF: " + XOFInfo.chManufacturerInfo + " " + XOFInfo.chModelName + " (" + XOFInfo.chSerialNumber + ")");
                    }
                }
                
            }
            //默认第一项 | EN: Default first item
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                button2.Enabled = true;
            }
        }
        private void SetCtrlWhenOpen()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            comboBox1.Enabled = false;
        }
        private void SetCtrlWhenClose()
        {
            button1.Enabled = true;
            button2.Enabled = false;
            comboBox1.Enabled = true;
        }

        ///<summary>显示设置窗口</summary>
        //public void DisPlaySet()
        //{
        //    HOperatorSet.OpenWindow(0, 0, pictureBox1.Width, pictureBox1.Height, pictureBox1.Handle, "visible", "", out HTuple hv_WindowHandle);
        //    HDevWindowStack.Push(hv_WindowHandle);
        //}
        public void InitDisplayHandle()
        {
            // 将PictureBox的句柄作为显示目标
            displayHandle = pictureBox1.Handle;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //输入一个PLC的IP即可连接
        }

        ///<summary>打开相机</summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (m_stDeviceList.nDeviceNum == 0 || comboBox1.SelectedIndex == -1)
            {
                ShowErrorMsg("No Device Found!", -1);
                return;
            }
            InitDisplayHandle();
            //other code ...

            ///<summary>获取设备信息</summary>
            MyCamera.MV_CC_DEVICE_INFO deviceInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(
                m_stDeviceList.pDeviceInfo[comboBox1.SelectedIndex], typeof(MyCamera.MV_CC_DEVICE_INFO));

            if (null == m_MyCamera)
            {
                m_MyCamera = new MyCamera();    // 创建设备实例
                if (null == m_MyCamera)
                {
                    ShowErrorMsg("初始化相机对象失败!", MyCamera.MV_E_RESOURCE);
                    return;
                }
            }

            int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref deviceInfo);  // 创建设备
            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Create Device Fail!", nRet);
                return;
            }
            nRet = m_MyCamera.MV_CC_OpenDevice_NET(); // 打开设备
            if (MyCamera.MV_OK != nRet)
            {
                m_MyCamera.MV_CC_DestroyDevice_NET();
                ShowErrorMsg("Device open fail!", nRet);
                return;
            }


            // 待更新代码...
            //ch:探测网络最佳包大小(只对GigE相机有效) | en:Detect the optimal packet size for the network (only valid for GigE cameras)
            if (deviceInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                // 1. 获取当前网络最佳包大小
                int optimalPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                // 2.检测包大小,并校验
                if (optimalPacketSize > 0)
                {
                    const int MIN_PACKET_SIZE = 512;
                    const int MAX_PACKET_SIZE = 9000;   // 部分工业网络支持Jumbo Frame（巨型帧）
                    if (optimalPacketSize < MIN_PACKET_SIZE || optimalPacketSize > MAX_PACKET_SIZE)
                    {
                        ShowErrorMsg($"获取的最佳包大小({optimalPacketSize}字节)超出合理范围[{MIN_PACKET_SIZE}-{MAX_PACKET_SIZE}]", optimalPacketSize);
                        return; // 超出范围时终止设置，避免无效操作
                    }
                    //nRet = m_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)optimalPacketSize);
                    nRet = m_MyCamera.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", optimalPacketSize);
                    if (nRet != MyCamera.MV_OK)
                    {
                        ShowErrorMsg($"设置最佳包大小({optimalPacketSize}字节)失败！错误码：{nRet}", nRet);
                    }
                    else
                    {
                        //TODO list// 补充成功日志（可选，便于调试）
                        // Console.WriteLine($"最佳包大小({optimalPacketSize}字节)设置成功");
                    }
                }
                else
                {
                    // 3.获取最佳包大小失败时的错误处理，错误码（如-1代表未知错误，-100代表网络异常等）                   
                    ShowErrorMsg($"获取最佳网络包大小失败！错误码：{optimalPacketSize}", optimalPacketSize);

                }
            }

            //待跟新代码...
            // ch:设置采集连续模式 | en:Set acquisition continuous mode
            m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

            // ch:注册异常回调函数
            m_MyCamera.MV_CC_RegisterExceptionCallBack_NET(ExceptionCallBack, IntPtr.Zero);


            button9_Click(null, null);      // ch: 获取参数按钮 | en:Get parameters

            SetCtrlWhenOpen();      // ch: 界面打开后的控件状态 | en:Set the control status after opening

        }

        // ch:异常处理回调函数 | en:Exception handling callback function
        private void ExceptionCallBack(uint nMsgType, IntPtr pUser)
        {            
            IsConnect = false;
            ShowErrorMsg();           
        }
        // ch:错误信息显示 | en: Error message display
        private void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0)
            {
                errorMsg = csMessage;
            }
            else
            {
                errorMsg = csMessage + "" + String.Format("{0:X}", nErrorNum);
                //errorMsg = csMessage + " ErrorCode: " + String.Format("0x{0:X8}", nErrorNum);
            }
            switch (nErrorNum)
            {
                case MyCamera.MV_E_HANDLE:
                    errorMsg += " : " + "无效的句柄"; break;
                case MyCamera.MV_E_SUPPORT:
                    errorMsg += " : " + "不支持的功能"; break;
                case MyCamera.MV_E_BUFOVER:
                    errorMsg += " : " + "缓冲区溢出"; break;
                case MyCamera.MV_E_CALLORDER:
                    errorMsg += " : " + "调用顺序错误"; break;
                case MyCamera.MV_E_PARAMETER:
                    errorMsg += " : " + "参数错误"; break;
                case MyCamera.MV_E_RESOURCE:
                    errorMsg += " : " + "资源不足或资源忙"; break;
                case MyCamera.MV_E_NODATA:
                    errorMsg += " : " + "没有数据"; break;
                case MyCamera.MV_E_PRECONDITION:
                    errorMsg += " : " + "运行条件不满足"; break;
                case MyCamera.MV_E_VERSION:
                    errorMsg += " : " + "版本不匹配"; break;
                case MyCamera.MV_E_NOENOUGH_BUF:
                    errorMsg += " : " + "缓冲区大小不足"; break;
                case MyCamera.MV_E_UNKNOW:
                    errorMsg += " : " + "未知错误"; break;
                case MyCamera.MV_E_GC_GENERIC:
                    errorMsg += " : " + "GenICam通用错误"; break;
                case MyCamera.MV_E_GC_ACCESS:
                    errorMsg += " : " + "访问错误"; break;
                case MyCamera.MV_E_ACCESS_DENIED:
                    errorMsg += " : " + "拒绝访问"; break;
                case MyCamera.MV_E_BUSY:
                    errorMsg += " : " + "设备忙"; break;
                default:
                    break;
            }
            MessageBox.Show(errorMsg, "PROMPT", MessageBoxButtons.OK);
        }

        private void ShowErrorMsg()
        {
            throw new NotImplementedException();
        }


        ///<summary>关闭相机</summary>
        private void button4_Click(object sender, EventArgs e)
        {
            if (m_bGrabbing == true)//清除取流标志
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join(); //等待取流线程退出
                //Thread.Sleep(50); //等待取流线程退出
            }
            if (m_pBufForDriver != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_pBufForDriver);   // Marshal.Release(m_BufForDriver);
            }
            if (m_MyCamera != null)
            {
                // ch:关闭设备 | en:Close Device
                m_MyCamera.MV_CC_CloseDevice_NET();
                m_MyCamera.MV_CC_DestroyDevice_NET();
            }

            // 关闭时设置，控件状态设置
            SetCtrlWhenClose();
        }
        #region  IO输出，控制源
        ///<summary>连续模式</summary>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                checkBox1.Enabled = false;  // 关闭外触发使能
                button8.Enabled = false; // 关闭触发一次按钮
            }
        }
        ///<summary>触发模式</summary>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)      // ch:打开触发模式 | en:Open trigger mode
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);

                // ch:触发源选择：0 - line0;
                //                1 - line1;
                //                2 - line2software;
                //                3 - line3;
                //                4 - counter;
                //                7 - software;
                //               -1 - external;
                if (checkBox1.Checked == true)   // 软触发
                {
                    m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                    if (m_bGrabbing)
                    {
                        button8.Enabled = true;  // 打开触发一次按钮
                    }
                }
                else                            // 
                {
                    m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                }
                checkBox1.Enabled = true;       // 打开外触发使能
            }
        }
        #endregion

        ///<summary>开始取流时，控件状态设置</summary>
        private void SetCtrlWhenStartGrab()
        {
            button5.Enabled = false;
            button6.Enabled = true;

            if (radioButton2.Checked && checkBox1.Checked)
            {
                button8.Enabled = true;
            }
            button11.Enabled = true;
        }
        ///<summary>停止取流时，控件状态设置</summary>
        private void SetCtrlWhenStopGrab()
        {
            button5.Enabled = true;
            button6.Enabled = false;
            button8.Enabled = false;
            button11.Enabled = false;
        }
        ///<summary>取图核心线程</summary>
        public void ReceiveThreadProcess()
        {
            MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();    //
            MyCamera.MV_DISPLAY_FRAME_INFO stDisplayInfo = new MyCamera.MV_DISPLAY_FRAME_INFO();
            MyCamera.MV_PIXEL_CONVERT_PARAM stConvertInfo = new MyCamera.MV_PIXEL_CONVERT_PARAM();
        
            int nRet = MyCamera.MV_OK;

            while (m_bGrabbing)
            {

                // ch:获取一帧图像 | en:Get one frame of image
                //MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();  // 用于获取宽度高度参数
                //int nRet = m_MyCamera.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
                nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
                //nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref m_stFrameInfo, m_pBufForDriver, m_nBufSizeForDriver, 1000);

                if (nRet == MyCamera.MV_OK)
                {
                    lock (BufForDriverLock)
                    {
                        if (m_pBufForDriver == IntPtr.Zero || stFrameInfo.stFrameInfo.nFrameLen > m_nBufSizeForDriver)
                        {
                            if (m_pBufForDriver != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(m_pBufForDriver);   // Marshal.Release(m_BufForDriver);
                                m_pBufForDriver = IntPtr.Zero;
                                m_nBufSizeForDriver = 0;
                            }

                            m_pBufForDriver = Marshal.AllocHGlobal((Int32)stFrameInfo.stFrameInfo.nFrameLen);
                            if (m_pBufForDriver == IntPtr.Zero)
                            {
                                // ch:内存申请失败 | en:Memory application failed
                                m_bGrabbing = false;
                                break;
                            }
                            m_nBufSizeForDriver = stFrameInfo.stFrameInfo.nFrameLen;
                        }

                        m_stFrameInfo = stFrameInfo.stFrameInfo;  // 记录图像帧信息
                        CopyMemory(m_pBufForDriver, stFrameInfo.pBufAddr, stFrameInfo.stFrameInfo.nFrameLen);

                        // ch:图像转换格式 | en:Image conversion format
                        stConvertInfo.nWidth = m_stFrameInfo.nWidth;
                        stConvertInfo.nHeight = m_stFrameInfo.nHeight;
                        stConvertInfo.enSrcPixelType = (MyCamera.MvGvspPixelType)m_stFrameInfo.enPixelType;
                        stConvertInfo.pSrcData = stFrameInfo.pBufAddr;
                        stConvertInfo.nSrcDataLen = m_stFrameInfo.nFrameLen;
                        stConvertInfo.pDstBuffer = m_ConvertDstBuf;
                        stConvertInfo.nDstBufferSize = m_nConvertDstBufLen;

                        if (PixelFormat.Format8bppIndexed == m_bitmap.PixelFormat)// 黑白相机
                        {
                            stConvertInfo.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
                            m_MyCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                        }
                        else if (PixelFormat.Format24bppRgb == m_bitmap.PixelFormat)// 彩色相机
                        {
                            stConvertInfo.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                            m_MyCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                        }
                        else
                        {
                            // ch:不支持的像素格式 | en:Unsupported pixel format
                            m_bGrabbing = false;
                            break;
                        }
                       
                        BitmapData bitmapData = m_bitmap.LockBits(new Rectangle(0, 0, m_bitmap.Width, m_bitmap.Height),
                            ImageLockMode.ReadWrite, m_bitmap.PixelFormat);     // 锁定Bitmap数据
                        CopyMemory(bitmapData.Scan0, stConvertInfo.pDstBuffer, (UInt32)(bitmapData.Stride * m_bitmap.Height));
                        m_bitmap.UnlockBits(bitmapData);    // 解锁Bitmap数据
                    }
                    // ch:显示图像 | en:Display image
                    stDisplayInfo.hWnd = displayHandle;
                    stDisplayInfo.pData = stFrameInfo.pBufAddr;
                    stDisplayInfo.nDataLen = stFrameInfo.stFrameInfo.nFrameLen;
                    stDisplayInfo.nWidth = stFrameInfo.stFrameInfo.nWidth;
                    stDisplayInfo.nHeight = stFrameInfo.stFrameInfo.nHeight;
                    stDisplayInfo.enPixelType = (MyCamera.MvGvspPixelType)stFrameInfo.stFrameInfo.enPixelType;

                    m_MyCamera.MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);
                    m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
                }
                else
                {
                    if (radioButton2.Checked)
                    {
                        Thread.Sleep(5);
                    }
                }
            }
        }

        ///<summary> 像素格式判断 </summary>
        private Boolean IsMono(UInt32 nPixelFormat)
        {
            //MyCamera.MvGvspPixelType enPixelType = (MyCamera.MvGvspPixelType)(nPixelFormat & ~CUSTOMER_PIXEL_FORMAT);
            switch (nPixelFormat)
            {
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono16:
                    return true;
                default:
                    return false;
            }
        }

        ///<summary> 取图前的操作步骤 </summary>
        private Int32 NecessaryOperBeforeGrab()
        {
            // ch:取图像宽 | en:Get Iamge Width
            MyCamera.MVCC_INTVALUE_EX stWidth = new MyCamera.MVCC_INTVALUE_EX();
            int nRet = m_MyCamera.MV_CC_GetIntValueEx_NET("Width", ref stWidth);
            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Get Width Info Fail!", nRet);
                return nRet;
            }
            // ch:取图像高 | en:Get Iamge Height
            MyCamera.MVCC_INTVALUE_EX stHeight = new MyCamera.MVCC_INTVALUE_EX();
            nRet = m_MyCamera.MV_CC_GetIntValueEx_NET("Height", ref stHeight);
            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Get Height Info Fail!", nRet);
                return nRet;
            }
            // ch:取像素格式 | en:Get Pixel Format
            MyCamera.MVCC_ENUMVALUE stPixelFormat = new MyCamera.MVCC_ENUMVALUE();
            nRet = m_MyCamera.MV_CC_GetEnumValue_NET("PixelFormat", ref stPixelFormat);
            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Get Pixel Format Fail!", nRet);
                return nRet;
            }
            // ch:根据像素格式，创建Bitmap | en:Create Bitmap according to Pixel Format
            // ch:设置bitmap像素格式，申请相应大小内存 | en:Set Bitmap Pixel Format, alloc memory
            if ((Int32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined == (Int32)stPixelFormat.nCurValue)
            {
                ShowErrorMsg("Unknown Pixel Format!", MyCamera.MV_E_UNKNOW);
                return MyCamera.MV_E_UNKNOW;
            }
            else if (IsMono(stPixelFormat.nCurValue))
            {
                m_bitmapPixelFormat = PixelFormat.Format8bppIndexed;

                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.Release(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }

                // Mono8为单通道
                m_nConvertDstBufLen = (UInt32)(stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf)
                {
                    ShowErrorMsg("Malloc Memory Fail!", MyCamera.MV_E_RESOURCE);
                    return MyCamera.MV_E_RESOURCE;
                }
            }
            else
            {
                m_bitmapPixelFormat = PixelFormat.Format24bppRgb;

                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.FreeHGlobal(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }

                // RGB为三通道
                m_nConvertDstBufLen = (UInt32)(3 * stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf)
                {
                    ShowErrorMsg("Malloc Memory Fail!", MyCamera.MV_E_RESOURCE);
                    return MyCamera.MV_E_RESOURCE;
                }
            }

            // 确保释放保存了旧图像数据的bitmap实例，用新图像宽高等信息new一个新的bitmap实例
            if (null != m_bitmap)
            {
                m_bitmap.Dispose();
                m_bitmap = null;
            }
            m_bitmap = new Bitmap((Int32)stWidth.nCurValue, (Int32)stHeight.nCurValue, m_bitmapPixelFormat);


            // ch:Mono8格式，设置为标准调色板 | en:Set Standard Palette in Mono8 Format
            if (PixelFormat.Format8bppIndexed == m_bitmapPixelFormat)
            {
                ColorPalette palette = m_bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                m_bitmap.Palette = palette;
            }

            return MyCamera.MV_OK;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // ch:开始预览 | en:Start Grab
            int nRet = NecessaryOperBeforeGrab();
            if (MyCamera.MV_OK != nRet)
            {
                return;
            }
            displayHandle = pictureBox1.Handle;  // ch:显示控件句柄 | en:Display control handle         
            m_bGrabbing = true;  // ch:取流标志位 | en:Grab flag bit
            m_stFrameInfo.nFrameLen = 0;    //取流之前先清除帧长度
            m_stFrameInfo.enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;

            m_hReceiveThread = new Thread(ReceiveThreadProcess);
            m_hReceiveThread.Start();

            nRet = m_MyCamera.MV_CC_StartGrabbing_NET();    // ch:开始取流 | en:Start Grab
            pictureBox1.Image = m_bitmap;
            if (MyCamera.MV_OK != nRet)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();
                ShowErrorMsg("Start Grabbing Fail!", nRet);
                return;
            }
            SetCtrlWhenStartGrab();  // ch:开始取流时，控件状态设置
        }

        ///<summary> cbsoftTrigger软触发 </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                if (m_bGrabbing)
                {
                    button8.Enabled = true;
                }
            }
            else
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                button8.Enabled = false;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        ///<summary> 停止采集 </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            //ch:停止预览 | en:Stop Grab
            m_bGrabbing = false;
            m_hReceiveThread.Join(); //取流线程退出
            // ch:停止取流 | en:Stop Grab
            int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();

            // 显示控件置空           

            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Stop Grabbing Fail!", nRet);
                return;
            }
            SetCtrlWhenStopGrab();   // ch:停止取流时，控件状态设置
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // 校验PLC实例是否有效
            if (PlcCommunicator == null)
            {
                ShowErrorMsg("PLC通信实例未初始化", -1);
                return;
            }
            Thread captureThread = new Thread(LoopCaptureByPLC);
            captureThread.IsBackground = true;
            captureThread.Start();
        }
        //plc触发取图线程,自动循环拍照, // 修改LoopCaptureByPLC方法，使用PlcCommunicator属性
        private void LoopCaptureByPLC()
        {
            //int stateValue = 0; // PLC状态值,变为1时触发拍照
           
            while (m_bGrabbing)
            {
                //todo 拍照，PLC触发信号CameraAlive
                if (PlcCommunicator != null && PlcCommunicator.IsConnected && 
                    PlcCommunicator.Read("CameraAlive", out object triggerValue) && 
                   triggerValue is bool isTriggered && isTriggered)
                {
                    // run cmd
                    CaptureImage();

                    // 拍照完成后复位PLC触发信号Cameracomplete
                    PlcCommunicator.Write("Cameracomplete", false);

                    // 短暂延迟避免重复触发
                    Thread.Sleep(100);
                }
                Thread.Sleep(10); //避免死循环
                
            }
        }

        private void CaptureImage()
        {
            try
            {
                //实际拍照逻辑，复用现有取流代码
                m_bGrabbing = true;

                int nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    ShowErrorMsg("Soft Trigger Fail!", nRet);
                    return;
                }
                // 等待图像获取
                MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();
                nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 2000);
                if (nRet != MyCamera.MV_OK)
                {
                    ShowErrorMsg("获取图像失败!", nRet);
                }
                // 处理图像数据
                string imagePath = SaveCapturedImage(stFrameInfo);

                // 释放图像缓冲区
                m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);

                // 保存图像路径用于后续检测
                LastCapturedImagePath = imagePath;
                
            }
            catch (Exception ex)
            {
                ShowErrorMsg("Capture Image Exception: " + ex.Message, -1);
                
            }
        }



        /// <summary>
        /// 方法一：将相机帧数据转换为JPG格式并保存，返回保存路径
        /// </summary>
        /// <param name="frameInfo">相机获取的帧数据</param>
        /// <returns>保存的JPG图像路径</returns>
        private string SaveCapturedImage(MyCamera.MV_FRAME_OUT frameInfo)
        {
            // 1. 创建保存目录（若不存在则创建）
            string saveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapturedImages");
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            // 2. 生成唯一文件名（时间戳+JPG后缀，确保不重复）
            string fileName = $"Capture_{DateTime.Now:yyyyMMddHHmmssfff}.jpg";
            string savePath = Path.Combine(saveDir, fileName);

            //  配置海康SDK保存参数
            MyCamera.MV_SAVE_IMAGE_PARAM stSaveParam = new MyCamera.MV_SAVE_IMAGE_PARAM();
            stSaveParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Jpeg;
            int width = frameInfo.stFrameInfo.nWidth;       // 图宽度
            int height = frameInfo.stFrameInfo.nHeight;     // 图高度
            stSaveParam.enPixelType = frameInfo.stFrameInfo.enPixelType;  // 像素格式（需根据相机SDK定义处理）            
            IntPtr pData = frameInfo.pBufAddr;              // 图像数据缓冲区指针
            UInt32 dataLen = frameInfo.stFrameInfo.nFrameLen;  // 数据长度

            // 3. 调用海康SDK保存图像（核心步骤）
            int nRet = m_MyCamera.MV_CC_SaveImage_NET(ref stSaveParam);
            if (nRet != MyCamera.MV_OK)
            {
                // 保存失败：调用SDK获取错误信息
                MessageBox.Show("图像保存失败");
                return string.Empty;
            }

            // 4. 保存成功，返回路径
            return savePath;
        }

        // 保存拍摄的图像并返回保存路径
        public string LastCapturedImagePath { get; private set; }



        ///<summary> 软触发一次 </summary>
        private void button8_Click(object sender, EventArgs e)
        {
            int nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet)
            {
                ShowErrorMsg("Soft Trigger Fail!", nRet);
                return;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = m_MyCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                textBox1.Text = stParam.fCurValue.ToString("F1");
            }

            nRet = m_MyCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                textBox2.Text = stParam.fCurValue.ToString("F1");
            }

            nRet = m_MyCamera.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                textBox3.Text = stParam.fCurValue.ToString("F1");
            }

            // TODO More Param

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                float.Parse(textBox1.Text);
                float.Parse(textBox2.Text);
                float.Parse(textBox3.Text);
            }
            catch
            {
                ShowErrorMsg("Please enter correct type!", 0);
                return;
            }

            m_MyCamera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);  // 关闭自动曝光
            int nRet = m_MyCamera.MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(textBox1.Text));
            if (nRet != MyCamera.MV_OK)
            {
                ShowErrorMsg("Set Exposure Time Fail!", nRet);
            }

            m_MyCamera.MV_CC_SetEnumValue_NET("GainAuto", 0);  // 关闭自动增益
            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("Gain", float.Parse(textBox2.Text));
            if (nRet != MyCamera.MV_OK)
            {
                ShowErrorMsg("Set Gain Fail!", nRet);
            }

            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("AcquisitionFrameRate", float.Parse(textBox2.Text));  //  设置采集帧率
            if (nRet != MyCamera.MV_OK)
            {
                ShowErrorMsg("Set Frame Rate Fail!", nRet);
            }
        }

        //保存图像
        private void button11_Click(object sender, EventArgs e)
        {
            // 检查是否正在采集
            if (!m_bGrabbing)
            {
                ShowErrorMsg("Not Start Grabbing", 0);
                return;
            }

            // 生成默认文件名（包含宽、高、帧号，与原逻辑保持一致）
            string defaultFileName = $"Image_w{m_stFrameInfo.nWidth}_h{m_stFrameInfo.nHeight}_fn{m_stFrameInfo.nFrameNum}";

            // 初始化文件保存对话框
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                // 设置文件格式过滤器（默认选中JPG）
                saveDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png|TIFF Image|*.tif";
                saveDialog.FilterIndex = 1; // 默认选择第1项（JPG）
                saveDialog.FileName = defaultFileName; // 预设默认文件名
                saveDialog.Title = "Save Image As"; // 对话框标题

                // 显示对话框，用户点击"确定"才继续
                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                // 获取用户选择的文件路径和扩展名
                string savePath = saveDialog.FileName;
                string extension = Path.GetExtension(savePath).ToLower(); // 取扩展名（如.jpg、.bmp）

                // 根据扩展名确定图片类型和参数（质量等）
                (MyCamera.MV_SAVE_IAMGE_TYPE imageType, int? quality) = GetImageParamsByExtension(extension);
               

                // 调用通用保存方法
                SaveImage(imageType, quality, savePath);
            }
        }

        // 根据文件扩展名获取图片类型和质量参数
        private (MyCamera.MV_SAVE_IAMGE_TYPE, int?) GetImageParamsByExtension(string extension)
        {
            // 先统一扩展名大小写（避免 ".JPG" 和 ".jpg" 匹配失败，增强健壮性）
            string lowerExtension = extension?.ToLower() ?? "";

            // 传统 switch 语句：仅处理四种目标格式，无未知格式分支
            switch (lowerExtension)
            {
                case ".jpg":
                    return (MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Jpeg, 80); // JPG 质量80（与原代码一致）
                case ".bmp":
                    return (MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp, null); // BMP 无质量参数
                case ".png":
                    return (MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Png, 8);   // PNG 质量8（与原代码一致）
                case ".tif":
                    return (MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Tif, null); // TIF 无质量参数
                                                                             // 无 default 分支：若传入其他扩展名，后续需提前校验（避免走到这里）
                default:
                    // 若意外传入其他格式，抛出明确异常（便于调试，增强健壮性）
                    throw new ArgumentException(
                        $"不支持的图片格式：{extension}，仅支持 .bmp/.jpg/.png/.tif",
                        nameof(extension)
                    );
            }
        }

        // 通用保存逻辑（提取重复代码）
        private void SaveImage(MyCamera.MV_SAVE_IAMGE_TYPE imageType, int? quality, string savePath)
        {
            MyCamera.MV_SAVE_IMG_TO_FILE_PARAM stSaveFileParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();

            lock (BufForDriverLock) // 加锁保护共享资源
            {
                // 检查帧数据是否有效
                if (m_stFrameInfo.nFrameLen == 0)
                {
                    ShowErrorMsg($"Save {imageType} Fail!", 0);
                    return;
                }

                // 设置公共参数（所有格式通用）
                stSaveFileParam.enImageType = imageType;
                stSaveFileParam.enPixelType = m_stFrameInfo.enPixelType;
                stSaveFileParam.pData = m_pBufForDriver;
                stSaveFileParam.nDataLen = m_stFrameInfo.nFrameLen;
                stSaveFileParam.nHeight = m_stFrameInfo.nHeight;
                stSaveFileParam.nWidth = m_stFrameInfo.nWidth;
                stSaveFileParam.iMethodValue = 2;
                stSaveFileParam.pImagePath = savePath;

                // 设置质量参数（仅JPG和PNG需要）
                if (quality.HasValue)
                    stSaveFileParam.nQuality = (uint)quality.Value;

                // 调用保存接口
                int nRet = m_MyCamera.MV_CC_SaveImageToFile_NET(ref stSaveFileParam);
                if (MyCamera.MV_OK != nRet)
                {
                    ShowErrorMsg($"Save {imageType} Fail!", nRet);
                    return;
                }
            }

            // 保存成功提示
            ShowErrorMsg("Save Succeed!", 0);
        }


    }

  
    
}
