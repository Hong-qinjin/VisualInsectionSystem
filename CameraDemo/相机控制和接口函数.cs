using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
namespace MvCameraControl
{
    public class MyCamera
    {
        public MyCamera()
        {
            handle = IntPtr.Zero;
        }
        ~MyCamera()
        {
            handle = IntPtr.Zero;
        }
        IntPtr handle;
        #region ch:委托声明 | en:delegate
        public delegate void cbOutputdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);
        public delegate void cbOutputExdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser);
        public delegate void cbXmlUpdatedelegate(MV_XML_InterfaceType enType, IntPtr pstFeature, ref MV_XML_NODES_LIST pstNodesList, IntPtr pUser);
        public delegate void cbExceptiondelegate(UInt32 nMsgType, IntPtr pUser);
        public delegate void cbEventdelegate(UInt32 nUserDefinedId, IntPtr pUser);
        public delegate void cbEventdelegateEx(ref MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser);
        public delegate void cbStreamException(MV_CC_STREAM_EXCEPTION_TYPE enExceptionType, IntPtr pUser);
        #endregion
        public static Int32 MV_CC_Initialize_NET()
        {
            return MV_CC_Initialize();
        }
        public static Int32 MV_CC_Finalize_NET()
        {
            return MV_CC_Finalize();
        }
        #region 采集卡接口
        public static Int32 MV_CC_EnumInterfaces_NET(UInt32 nTLayerType, ref MV_INTERFACE_INFO_LIST pInterfaceInfoList)
        {
            return MV_CC_EnumInterfaces(nTLayerType, ref pInterfaceInfoList);
        }
        public Int32 MV_CC_CreateInterface_NET(ref MV_INTERFACE_INFO pInterfaceInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyInterface(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateInterface(ref handle, ref pInterfaceInfo);
        }
        public Int32 MV_CC_CreateInterfaceByID_NET(String pInterfaceID)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyInterface(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateInterfaceByID(ref handle, pInterfaceID);
        }
        public Int32 MV_CC_OpenInterface_NET(String strConfigFile)
        {
            return MV_CC_OpenInterface(handle, strConfigFile);
        }
        public Int32 MV_CC_CloseInterface_NET()
        {
            return MV_CC_CloseInterface(handle);
        }
        public Int32 MV_CC_DestroyInterface_NET()
        {
            Int32 nRet = MV_CC_DestroyInterface(handle);
            handle = IntPtr.Zero;
            return nRet;
        }
        #endregion
        #region ch:相机的控制和取流接口 | en:Camera control and streaming
        public IntPtr GetCameraHandle()
        {
            return handle;
        }
        public static UInt32 MV_CC_GetSDKVersion_NET()
        {
            return MV_CC_GetSDKVersion();
        }
        public static Int32 MV_CC_EnumerateTls_NET()
        {
            return MV_CC_EnumerateTls();
        }
        public static Int32 MV_CC_EnumDevices_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevices(nTLayerType, ref stDevList);
        }
        public static Int32 MV_CC_EnumDevicesEx_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName)
        {
            return MV_CC_EnumDevicesEx(nTLayerType, ref stDevList, pManufacturerName);
        }
        public static Int32 MV_CC_EnumDevicesEx2_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName, MV_SORT_METHOD enSortMethod)
        {
            return MV_CC_EnumDevicesEx2(nTLayerType, ref stDevList, pManufacturerName, enSortMethod);
        }
        public static Boolean MV_CC_IsDeviceAccessible_NET(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode)
        {
            Byte bRet = MV_CC_IsDeviceAccessible(ref stDevInfo, nAccessMode);
            if (bRet != 0)
            {
                return true;
            }
            return false;
        }
        public static Int32 MV_CC_SetSDKLogPath_NET(String pSDKLogPath)
        {
            return MV_CC_SetSDKLogPath(pSDKLogPath);
        }
        public Int32 MV_CC_CreateDevice_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandle(ref handle, ref stDevInfo);
        }
        public Int32 MV_CC_CreateDeviceWithoutLog_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandleWithoutLog(ref handle, ref stDevInfo);
        }
        public Int32 MV_CC_DestroyDevice_NET()
        {
            Int32 nRet = MV_CC_DestroyHandle(handle);
            handle = IntPtr.Zero;
            return nRet;
        }
        public Int32 MV_CC_OpenDevice_NET()
        {
            return MV_CC_OpenDevice(handle, 1, 0);
        }
        public Int32 MV_CC_OpenDevice_NET(UInt32 nAccessMode, UInt16 nSwitchoverKey)
        {
            return MV_CC_OpenDevice(handle, nAccessMode, nSwitchoverKey);
        }
        public Int32 MV_CC_CloseDevice_NET()
        {
            return MV_CC_CloseDevice(handle);
        }
        public Boolean MV_CC_IsDeviceConnected_NET()
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }
            Byte bRet = MV_CC_IsDeviceConnected(handle);
            if (bRet != 0)
            {
                return true;
            }
            return false;
        }
        public Int32 MV_CC_RegisterImageCallBackEx_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackEx(handle, cbOutput, pUser);
        }
        public Int32 MV_CC_RegisterImageCallBackForRGB_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForRGB(handle, cbOutput, pUser);
        }
        public Int32 MV_CC_RegisterImageCallBackForBGR_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForBGR(handle, cbOutput, pUser);
        }
        public Int32 MV_CC_StartGrabbing_NET()
        {
            return MV_CC_StartGrabbing(handle);
        }
        public Int32 MV_CC_StopGrabbing_NET()
        {
            return MV_CC_StopGrabbing(handle);
        }
        public Int32 MV_CC_GetImageForRGB_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForRGB(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }
        public Int32 MV_CC_GetImageForBGR_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForBGR(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }
        public Int32 MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT pFrame, Int32 nMsec)
        {
            return MV_CC_GetImageBuffer(handle, ref pFrame, nMsec);
        }
        public Int32 MV_CC_FreeImageBuffer_NET(ref MV_FRAME_OUT pFrame)
        {
            return MV_CC_FreeImageBuffer(handle, ref pFrame);
        }
        public Int32 MV_CC_GetOneFrameTimeout_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetOneFrameTimeout(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }
        public Int32 MV_CC_ClearImageBuffer_NET()
        {
            return MV_CC_ClearImageBuffer(handle);
        }
        public Int32 MV_CC_GetValidImageNum_NET(ref UInt32 pnValidImageNum)
        {
            return MV_CC_GetValidImageNum(handle, ref pnValidImageNum);
        }
        public Int32 MV_CC_DisplayOneFrame_NET(ref MV_DISPLAY_FRAME_INFO pDisplayInfo)
        {
            return MV_CC_DisplayOneFrame(handle, ref pDisplayInfo);
        }
        public Int32 MV_CC_DisplayOneFrameEx_NET(IntPtr pDisplayHandle, ref MV_DISPLAY_FRAME_INFO_EX pDisplayInfoEx)
        {
            if (IntPtr.Zero == pDisplayHandle)
            {
                return MV_E_PARAMETER;
            }
            return MV_CC_DisplayOneFrameEx(handle, pDisplayHandle, ref pDisplayInfoEx);
        }
        public Int32 MV_CC_SetImageNodeNum_NET(UInt32 nNum)
        {
            return MV_CC_SetImageNodeNum(handle, nNum);
        }
        public Int32 MV_CC_SetGrabStrategy_NET(MV_GRAB_STRATEGY enGrabStrategy)
        {
            return MV_CC_SetGrabStrategy(handle, enGrabStrategy);
        }
        public Int32 MV_CC_SetOutputQueueSize_NET(UInt32 nOutputQueueSize)
        {
            return MV_CC_SetOutputQueueSize(handle, nOutputQueueSize);
        }
        public Int32 MV_CC_GetDeviceInfo_NET(ref MV_CC_DEVICE_INFO pstDevInfo)
        {
            return MV_CC_GetDeviceInfo(handle, ref pstDevInfo);
        }
        public Int32 MV_CC_GetAllMatchInfo_NET(ref MV_ALL_MATCH_INFO pstInfo)
        {
            return MV_CC_GetAllMatchInfo(handle, ref pstInfo);
        }
        #endregion
        #region ch:相机属性万能配置接口&读写寄存器接口 | en: Camera attribute nodes set and obtained universal interface
        public Int32 MV_CC_GetIntValueEx_NET(String strKey, ref MVCC_INTVALUE_EX pstValue)
        {
            return MV_CC_GetIntValueEx(handle, strKey, ref pstValue);
        }
        public Int32 MV_CC_SetIntValueEx_NET(String strKey, Int64 nValue)
        {
            return MV_CC_SetIntValueEx(handle, strKey, nValue);
        }
        public Int32 MV_CC_GetEnumValue_NET(String strKey, ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetEnumValue(handle, strKey, ref pstValue);
        }
        public Int32 MV_CC_SetEnumValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetEnumValue(handle, strKey, nValue);
        }
        public Int32 MV_CC_GetEnumEntrySymbolic_NET(String strKey, ref MVCC_ENUMENTRY pstEnumEntry)
        {
            return MV_CC_GetEnumEntrySymbolic(handle, strKey, ref pstEnumEntry);
        }
        public Int32 MV_CC_SetEnumValueByString_NET(String strKey, String sValue)
        {
            return MV_CC_SetEnumValueByString(handle, strKey, sValue);
        }
        public Int32 MV_CC_GetFloatValue_NET(String strKey, ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFloatValue(handle, strKey, ref pstValue);
        }
        public Int32 MV_CC_SetFloatValue_NET(String strKey, Single fValue)
        {
            return MV_CC_SetFloatValue(handle, strKey, fValue);
        }
        public Int32 MV_CC_GetBoolValue_NET(String strKey, ref Boolean pbValue)
        {
            return MV_CC_GetBoolValue(handle, strKey, ref pbValue);
        }
        public Int32 MV_CC_SetBoolValue_NET(String strKey, Boolean bValue)
        {
            return MV_CC_SetBoolValue(handle, strKey, bValue);
        }
        public Int32 MV_CC_GetStringValue_NET(String strKey, ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetStringValue(handle, strKey, ref pstValue);
        }
        public Int32 MV_CC_SetStringValue_NET(String strKey, String strValue)
        {
            return MV_CC_SetStringValue(handle, strKey, strValue);
        }
        public Int32 MV_CC_SetCommandValue_NET(String strKey)
        {
            return MV_CC_SetCommandValue(handle, strKey);
        }
        public Int32 MV_CC_ReadMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_ReadMemory(handle, pBuffer, nAddress, nLength);
        }
        public Int32 MV_CC_WriteMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_WriteMemory(handle, pBuffer, nAddress, nLength);
        }
        public Int32 MV_CC_InvalidateNodes_NET()
        {
            return MV_CC_InvalidateNodes(handle);
        }
        public Int32 MV_XML_GetGenICamXML_NET(IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen)
        {
            return MV_XML_GetGenICamXML(handle, pData, nDataSize, ref pnDataLen);
        }
        public Int32 MV_XML_GetNodeAccessMode_NET(String pstrName, ref MV_XML_AccessMode pAccessMode)
        {
            return MV_XML_GetNodeAccessMode(handle, pstrName, ref pAccessMode);
        }
        public Int32 MV_XML_GetNodeInterfaceType_NET(String pstrName, ref MV_XML_InterfaceType pInterfaceType)
        {
            return MV_XML_GetNodeInterfaceType(handle, pstrName, ref pInterfaceType);
        }
        public Int32 MV_CC_FeatureSave_NET(String pFileName)
        {
            return MV_CC_FeatureSave(handle, pFileName);
        }
        public Int32 MV_CC_FeatureLoad_NET(String pFileName)
        {
            return MV_CC_FeatureLoad(handle, pFileName);
        }
        public Int32 MV_CC_FileAccessRead_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessRead(handle, ref pstFileAccess);
        }
        public Int32 MV_CC_FileAccessReadEx_NET(ref MV_CC_FILE_ACCESS_EX pstFileAccessEx)
        {
            return MV_CC_FileAccessReadEx(handle, ref pstFileAccessEx);
        }
        public Int32 MV_CC_FileAccessWrite_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessWrite(handle, ref pstFileAccess);
        }
        public Int32 MV_CC_FileAccessWriteEx_NET(ref MV_CC_FILE_ACCESS_EX pstFileAccessEx)
        {
            return MV_CC_FileAccessWriteEx(handle, ref pstFileAccessEx);
        }
        public Int32 MV_CC_GetFileAccessProgress_NET(ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress)
        {
            return MV_CC_GetFileAccessProgress(handle, ref pstFileAccessProgress);
        }
        #endregion
        #region ch: 设备升级 | en: Device upgrade interface
        public Int32 MV_CC_LocalUpgrade_NET(String pFilePathName)
        {
            return MV_CC_LocalUpgrade(handle, pFilePathName);
        }
        public Int32 MV_CC_GetUpgradeProcess_NET(ref UInt32 pnProcess)
        {
            return MV_CC_GetUpgradeProcess(handle, ref pnProcess);
        }
        #endregion
        #region ch: 注册异常回调和事件接口 | en: Enrol abnormal callbacks and event interface
        public Int32 MV_CC_RegisterExceptionCallBack_NET(cbExceptiondelegate cbException, IntPtr pUser)
        {
            return MV_CC_RegisterExceptionCallBack(handle, cbException, pUser);
        }
        public Int32 MV_CC_RegisterAllEventCallBack_NET(cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterAllEventCallBack(handle, cbEvent, pUser);
        }
        public Int32 MV_CC_RegisterEventCallBackEx_NET(String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBackEx(handle, pEventName, cbEvent, pUser);
        }
        #endregion
        #region ch: 仅GigE设备支持的接口 | en: Only support GigE device interface
        public static Int32 MV_GIGE_SetEnumDevTimeout_NET(UInt32 nMilTimeout)
        {
            return MV_GIGE_SetEnumDevTimeout(nMilTimeout);
        }
        public Int32 MV_GIGE_ForceIpEx_NET(UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay)
        {
            return MV_GIGE_ForceIpEx(handle, nIP, nSubNetMask, nDefaultGateWay);
        }
        public Int32 MV_GIGE_SetIpConfig_NET(UInt32 nType)
        {
            return MV_GIGE_SetIpConfig(handle, nType);
        }
        public Int32 MV_GIGE_SetNetTransMode_NET(UInt32 nType)
        {
            return MV_GIGE_SetNetTransMode(handle, nType);
        }
        public Int32 MV_GIGE_GetNetTransInfo_NET(ref MV_NETTRANS_INFO pstInfo)
        {
            return MV_GIGE_GetNetTransInfo(handle, ref pstInfo);
        }
        public static Int32 MV_GIGE_SetDiscoveryMode_NET(UInt32 nMode)
        {
            return MV_GIGE_SetDiscoveryMode(nMode);
        }
        public Int32 MV_GIGE_SetGvspTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvspTimeout(handle, nMillisec);
        }
        public Int32 MV_GIGE_GetGvspTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvspTimeout(handle, ref pMillisec);
        }
        public Int32 MV_GIGE_SetGvcpTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvcpTimeout(handle, nMillisec);
        }
        public Int32 MV_GIGE_GetGvcpTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvcpTimeout(handle, ref pMillisec);
        }
        public Int32 MV_GIGE_SetRetryGvcpTimes_NET(UInt32 nRetryGvcpTimes)
        {
            return MV_GIGE_SetRetryGvcpTimes(handle, nRetryGvcpTimes);
        }
        public Int32 MV_GIGE_GetRetryGvcpTimes_NET(ref UInt32 pRetryGvcpTimes)
        {
            return MV_GIGE_GetRetryGvcpTimes(handle, ref pRetryGvcpTimes);
        }
        public Int32 MV_CC_GetOptimalPacketSize_NET()
        {
            return MV_CC_GetOptimalPacketSize(handle);
        }
        public Int32 MV_GIGE_SetResend_NET(UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout)
        {
            return MV_GIGE_SetResend(handle, bEnable, nMaxResendPercent, nResendTimeout);
        }
        public Int32 MV_GIGE_SetResendMaxRetryTimes_NET(UInt32 nRetryTimes)
        {
            return MV_GIGE_SetResendMaxRetryTimes(handle, nRetryTimes);
        }
        public Int32 MV_GIGE_GetResendMaxRetryTimes_NET(ref UInt32 pnRetryTimes)
        {
            return MV_GIGE_GetResendMaxRetryTimes(handle, ref pnRetryTimes);
        }
        public Int32 MV_GIGE_SetResendTimeInterval_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetResendTimeInterval(handle, nMillisec);
        }
        public Int32 MV_GIGE_GetResendTimeInterval_NET(ref UInt32 pnMillisec)
        {
            return MV_GIGE_GetResendTimeInterval(handle, ref pnMillisec);
        }
        public Int32 MV_GIGE_SetTransmissionType_NET(ref MV_CC_TRANSMISSION_TYPE pstTransmissionType)
        {
            return MV_GIGE_SetTransmissionType(handle, ref pstTransmissionType);
        }
        public Int32 MV_GIGE_IssueActionCommand_NET(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults)
        {
            return MV_GIGE_IssueActionCommand(ref pstActionCmdInfo, ref pstActionCmdResults);
        }
        public static Int32 MV_GIGE_GetMulticastStatus_NET(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus)
        {
            return MV_GIGE_GetMulticastStatus(ref pstDevInfo, ref pStatus);
        }
        #endregion
        #region ch: 仅CameraLink 设备支持的接口 | en: Only support camlink device interface
        public Int32 MV_CAML_SetDeviceBauderate_NET(UInt32 nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }
        public Int32 MV_CAML_GetDeviceBauderate_NET(ref UInt32 pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }
        public Int32 MV_CAML_GetSupportBauderates_NET(ref UInt32 pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }
        public Int32 MV_CAML_SetGenCPTimeOut_NET(UInt32 nMillisec)
        {
            return MV_CAML_SetGenCPTimeOut(handle, nMillisec);
        }
        #endregion
        #region ch: 仅U3V设备支持的接口 | en: Only support U3V device interface
        public Int32 MV_USB_SetTransferSize_NET(UInt32 nTransferSize)
        {
            return MV_USB_SetTransferSize(handle, nTransferSize);
        }
        public Int32 MV_USB_GetTransferSize_NET(ref UInt32 pTransferSize)
        {
            return MV_USB_GetTransferSize(handle, ref pTransferSize);
        }
        public Int32 MV_USB_SetTransferWays_NET(UInt32 nTransferWays)
        {
            return MV_USB_SetTransferWays(handle, nTransferWays);
        }
        public Int32 MV_USB_GetTransferWays_NET(ref UInt32 pTransferWays)
        {
            return MV_USB_GetTransferWays(handle, ref pTransferWays);
        }
        public Int32 MV_USB_RegisterStreamExceptionCallBack_NET(cbStreamException cbException, IntPtr pUser)
        {
            return MV_USB_RegisterStreamExceptionCallBack(handle, cbException, pUser);
        }
        public Int32 MV_USB_SetEventNodeNum_NET(UInt32 nEventNodeNum)
        {
            return MV_USB_SetEventNodeNum(handle, nEventNodeNum);
        }
        public Int32 MV_USB_SetSyncTimeOut_NET(UInt32 nMills)
        {
            return MV_USB_SetSyncTimeOut(handle, nMills);
        }
        public Int32 MV_USB_GetSyncTimeOut_NET(ref UInt32 pnMills)
        {
            return MV_USB_GetSyncTimeOut(handle, ref pnMills);
        }
        #endregion
        #region ch: GenTL相关接口 | en: GenTL related interface
        public static Int32 MV_CC_EnumInterfacesByGenTL_NET(ref MV_GENTL_IF_INFO_LIST stIFInfoList, String pGenTLPath)
        {
            return MV_CC_EnumInterfacesByGenTL(ref stIFInfoList, pGenTLPath);
        }
        public static Int32 MV_CC_UnloadGenTLLibrary_NET(String strGenTLPath)
        {
            return MV_CC_UnloadGenTLLibrary(strGenTLPath);
        }
        public static Int32 MV_CC_EnumDevicesByGenTL_NET(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevicesByGenTL(ref stIFInfo, ref stDevList);
        }
        public Int32 MV_CC_CreateDeviceByGenTL_NET(ref MV_GENTL_DEV_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandleByGenTL(ref handle, ref stDevInfo);
        }
        #endregion
        #region ch: 图像保存、格式转换等相关接口 | en: Related image save and format convert interface
        public Int32 MV_CC_SaveImageEx3_NET(ref MV_SAVE_IMAGE_PARAM_EX3 stSaveParam)
        {
            return MV_CC_SaveImageEx3(handle, ref stSaveParam);
        }
        public Int32 MV_CC_SaveImageToFileEx_NET(ref MV_SAVE_IMG_TO_FILE_PARAM_EX pstSaveFileParam)
        {
            return MV_CC_SaveImageToFileEx(handle, ref pstSaveFileParam);
        }
        public Int32 MV_CC_SavePointCloudData_NET(ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam)
        {
            return MV_CC_SavePointCloudData(handle, ref pstPointDataParam);
        }
        public Int32 MV_CC_RotateImage_NET(ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam)
        {
            return MV_CC_RotateImage(handle, ref pstRotateParam);
        }
        public Int32 MV_CC_FlipImage_NET(ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam)
        {
            return MV_CC_FlipImage(handle, ref pstFlipParam);
        }
        public Int32 MV_CC_ConvertPixelTypeEx_NET(ref MV_CC_PIXEL_CONVERT_PARAM_EX pstCvtParam)
        {
            return MV_CC_ConvertPixelTypeEx(handle, ref pstCvtParam);
        }
        public Int32 MV_CC_SetBayerCvtQuality_NET(UInt32 BayerCvtQuality)
        {
            return MV_CC_SetBayerCvtQuality(handle, BayerCvtQuality);
        }
        public Int32 MV_CC_SetBayerFilterEnable_NET(Boolean bFilterEnable)
        {
            return MV_CC_SetBayerFilterEnable(handle, bFilterEnable);
        }
        public Int32 MV_CC_SetBayerGammaValue_NET(Single fBayerGammaValue)
        {
            return MV_CC_SetBayerGammaValue(handle, fBayerGammaValue);
        }
        public Int32 MV_CC_SetGammaValue_NET(MvGvspPixelType enPixelType, Single fGammaValue)
        {
            return MV_CC_SetGammaValue(handle, enPixelType, fGammaValue);
        }
        public Int32 MV_CC_SetBayerGammaParam_NET(ref MV_CC_GAMMA_PARAM pstGammaParam)
        {
            return MV_CC_SetBayerGammaParam(handle, ref pstGammaParam);
        }
        public Int32 MV_CC_SetBayerCCMParam_NET(ref MV_CC_CCM_PARAM pstCCMParam)
        {
            return MV_CC_SetBayerCCMParam(handle, ref pstCCMParam);
        }
        public Int32 MV_CC_SetBayerCCMParamEx_NET(ref MV_CC_CCM_PARAM_EX pstCCMParam)
        {
            return MV_CC_SetBayerCCMParamEx(handle, ref pstCCMParam);
        }
        public Int32 MV_CC_ImageContrast_NET(ref MV_CC_CONTRAST_PARAM pstContrastParam)
        {
            return MV_CC_ImageContrast(handle, ref pstContrastParam);
        }
        public Int32 MV_CC_HB_Decode_NET(ref MV_CC_HB_DECODE_PARAM pstDecodeParam)
        {
            return MV_CC_HB_Decode(handle, ref pstDecodeParam);
        }
        public Int32 MV_CC_DrawRect_NET(ref MVCC_RECT_INFO pstRectInfo)
        {
            return MV_CC_DrawRect(handle, ref pstRectInfo);
        }
        public Int32 MV_CC_DrawCircle_NET(ref MVCC_CIRCLE_INFO pstCircleInfo)
        {
            return MV_CC_DrawCircle(handle, ref pstCircleInfo);
        }
        public Int32 MV_CC_DrawLines_NET(ref MVCC_LINES_INFO pstLinesInfo)
        {
            return MV_CC_DrawLines(handle, ref pstLinesInfo);
        }
        public Int32 MV_CC_StartRecord_NET(ref MV_CC_RECORD_PARAM pstRecordParam)
        {
            return MV_CC_StartRecord(handle, ref pstRecordParam);
        }
        public Int32 MV_CC_InputOneFrame_NET(ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo)
        {
            return MV_CC_InputOneFrame(handle, ref pstInputFrameInfo);
        }
        public Int32 MV_CC_StopRecord_NET()
        {
            return MV_CC_StopRecord(handle);
        }
        public Int32 MV_CC_OpenParamsGUI_NET()
        {
            return MV_CC_OpenParamsGUI(handle);
        }
        public Int32 MV_CC_ReconstructImage_NET(ref MV_RECONSTRUCT_IMAGE_PARAM pstReconstructParam)
        {
            return MV_CC_ReconstructImage(handle, ref pstReconstructParam);
        }
        #endregion
        #region ch: 内部使用的公共功能接口 | en: Inner Interface
        public static object ByteToStruct(Byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        public static bool IsTextUTF8(byte[] inputStream)
        {
            int encodingBytesCount = 0;
            bool allTextsAreASCIIChars = true;
            for (int i = 0; i < inputStream.Length; i++)
            {
                byte current = inputStream[i];
                if ((current & 0x80) == 0x80)
                {
                    allTextsAreASCIIChars = false;
                }
                if (encodingBytesCount == 0)
                {
                    if ((current & 0x80) == 0)
                    {
                        continue;
                    }
                    if ((current & 0xC0) == 0xC0)
                    {
                        encodingBytesCount = 1;
                        current <<= 2;
                        while ((current & 0x80) == 0x80)
                        {
                            current <<= 1;
                            encodingBytesCount++;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if ((current & 0xC0) == 0x80)
                    {
                        encodingBytesCount--;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (encodingBytesCount != 0)
            {
                return false;
            }
            return !allTextsAreASCIIChars;
        }
        public static void WriteErrorMsg(string csMessage, int nErrorNum)
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
            Debug.WriteLine(errorMsg);
        }
        #endregion
        #region ch: 弃用的接口 | en: Abandoned interface
        public Int32 MV_CC_SaveImageEx2_NET(ref MV_SAVE_IMAGE_PARAM_EX2 stSaveParam)
        {
            return MV_CC_SaveImageEx2(handle, ref stSaveParam);
        }
        public Int32 MV_CC_SaveImageToFile_NET(ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam)
        {
            return MV_CC_SaveImageToFile(handle, ref pstSaveFileParam);
        }
        public Int32 MV_CC_ConvertPixelType_NET(ref MV_PIXEL_CONVERT_PARAM pstCvtParam)
        {
            return MV_CC_ConvertPixelType(handle, ref pstCvtParam);
        }
        public Int32 MV_CC_GetImageInfo_NET(ref MV_IMAGE_BASIC_INFO pstInfo)
        {
            return MV_CC_GetImageInfo(handle, ref pstInfo);
        }
        public IntPtr MV_CC_GetTlProxy_NET()
        {
            return MV_CC_GetTlProxy(handle);
        }
        public Int32 MV_XML_GetRootNode_NET(ref MV_XML_NODE_FEATURE pstNode)
        {
            return MV_XML_GetRootNode(handle, ref pstNode);
        }
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, pstNodesList);
        }
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, ref pstNodesList);
        }
        public Int32 MV_XML_GetNodeFeature_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature)
        {
            return MV_XML_GetNodeFeature(handle, ref pstNode, pstFeature);
        }
        public Int32 MV_XML_UpdateNodeFeature_NET(MV_XML_InterfaceType enType, IntPtr pstFeature)
        {
            return MV_XML_UpdateNodeFeature(handle, enType, pstFeature);
        }
        public Int32 MV_XML_RegisterUpdateCallBack_NET(cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser)
        {
            return MV_XML_RegisterUpdateCallBack(handle, cbXmlUpdate, pUser);
        }
        public Int32 MV_CC_BayerNoiseEstimate_NET(ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_BayerNoiseEstimate(handle, ref pstNoiseEstimateParam);
        }
        public Int32 MV_CC_BayerSpatialDenoise_NET(ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_BayerSpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }
        public Int32 MV_CC_Display_NET(IntPtr hWnd)
        {
            return MV_CC_Display(handle, hWnd);
        }
        public Int32 MV_CC_GetOneFrame_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo)
        {
            return MV_CC_GetOneFrame(handle, pData, nDataSize, ref pFrameInfo);
        }
        public Int32 MV_CC_GetOneFrameEx_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo)
        {
            return MV_CC_GetOneFrameEx(handle, pData, nDataSize, ref pFrameInfo);
        }
        public Int32 MV_CC_SaveImage_NET(ref MV_SAVE_IMAGE_PARAM stSaveParam)
        {
            return MV_CC_SaveImage(ref stSaveParam);
        }
        public Int32 MV_GIGE_ForceIp_NET(UInt32 nIP)
        {
            return MV_GIGE_ForceIp(handle, nIP);
        }
        public Int32 MV_CC_RegisterEventCallBack_NET(cbEventdelegate cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBack(handle, cbEvent, pUser);
        }
        public Int32 MV_CC_GetIntValue_NET(String strKey, ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetIntValue(handle, strKey, ref pstValue);
        }
        public Int32 MV_CC_SetIntValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetIntValue(handle, strKey, nValue);
        }
        public Int32 MV_CC_SetBayerCLUTParam_NET(ref MV_CC_CLUT_PARAM pstCLUTParam)
        {
            return MV_CC_SetBayerCLUTParam(handle, ref pstCLUTParam);
        }
        public Int32 MV_CC_ImageSharpen_NET(ref MV_CC_SHARPEN_PARAM pstSharpenParam)
        {
            return MV_CC_ImageSharpen(handle, ref pstSharpenParam);
        }
        public Int32 MV_CC_ColorCorrect_NET(ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam)
        {
            return MV_CC_ColorCorrect(handle, ref pstColorCorrectParam);
        }
        public Int32 MV_CC_NoiseEstimate_NET(ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_NoiseEstimate(handle, ref pstNoiseEstimateParam);
        }
        public Int32 MV_CC_SpatialDenoise_NET(ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_SpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }
        public Int32 MV_CC_LSCCalib_NET(ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam)
        {
            return MV_CC_LSCCalib(handle, ref pstLSCCalibParam);
        }
        public Int32 MV_CC_LSCCorrect_NET(ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam)
        {
            return MV_CC_LSCCorrect(handle, ref pstLSCCorrectParam);
        }
        public Int32 MV_CC_GetWidth_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetWidth(handle, ref pstValue);
        }
        public Int32 MV_CC_SetWidth_NET(UInt32 nValue)
        {
            return MV_CC_SetWidth(handle, nValue);
        }
        public Int32 MV_CC_GetHeight_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeight(handle, ref pstValue);
        }
        public Int32 MV_CC_SetHeight_NET(UInt32 nValue)
        {
            return MV_CC_SetHeight(handle, nValue);
        }
        public Int32 MV_CC_GetAOIoffsetX_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetX(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAOIoffsetX_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetX(handle, nValue);
        }
        public Int32 MV_CC_GetAOIoffsetY_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetY(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAOIoffsetY_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetY(handle, nValue);
        }
        public Int32 MV_CC_GetAutoExposureTimeLower_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeLower(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAutoExposureTimeLower_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeLower(handle, nValue);
        }
        public Int32 MV_CC_GetAutoExposureTimeUpper_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeUpper(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAutoExposureTimeUpper_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeUpper(handle, nValue);
        }
        public Int32 MV_CC_GetBrightness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBrightness(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBrightness_NET(UInt32 nValue)
        {
            return MV_CC_SetBrightness(handle, nValue);
        }
        public Int32 MV_CC_GetFrameRate_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFrameRate(handle, ref pstValue);
        }
        public Int32 MV_CC_SetFrameRate_NET(Single fValue)
        {
            return MV_CC_SetFrameRate(handle, fValue);
        }
        public Int32 MV_CC_GetGain_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGain(handle, ref pstValue);
        }
        public Int32 MV_CC_SetGain_NET(Single fValue)
        {
            return MV_CC_SetGain(handle, fValue);
        }
        public Int32 MV_CC_GetExposureTime_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetExposureTime(handle, ref pstValue);
        }
        public Int32 MV_CC_SetExposureTime_NET(Single fValue)
        {
            return MV_CC_SetExposureTime(handle, fValue);
        }
        public Int32 MV_CC_GetPixelFormat_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetPixelFormat(handle, ref pstValue);
        }
        public Int32 MV_CC_SetPixelFormat_NET(UInt32 nValue)
        {
            return MV_CC_SetPixelFormat(handle, nValue);
        }
        public Int32 MV_CC_GetAcquisitionMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetAcquisitionMode(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAcquisitionMode_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionMode(handle, nValue);
        }
        public Int32 MV_CC_GetGainMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGainMode(handle, ref pstValue);
        }
        public Int32 MV_CC_SetGainMode_NET(UInt32 nValue)
        {
            return MV_CC_SetGainMode(handle, nValue);
        }
        public Int32 MV_CC_GetExposureAutoMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetExposureAutoMode(handle, ref pstValue);
        }
        public Int32 MV_CC_SetExposureAutoMode_NET(UInt32 nValue)
        {
            return MV_CC_SetExposureAutoMode(handle, nValue);
        }
        public Int32 MV_CC_GetTriggerMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerMode(handle, ref pstValue);
        }
        public Int32 MV_CC_SetTriggerMode_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerMode(handle, nValue);
        }
        public Int32 MV_CC_GetTriggerDelay_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetTriggerDelay(handle, ref pstValue);
        }
        public Int32 MV_CC_SetTriggerDelay_NET(Single fValue)
        {
            return MV_CC_SetTriggerDelay(handle, fValue);
        }
        public Int32 MV_CC_GetTriggerSource_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerSource(handle, ref pstValue);
        }
        public Int32 MV_CC_SetTriggerSource_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerSource(handle, nValue);
        }
        public Int32 MV_CC_TriggerSoftwareExecute_NET()
        {
            return MV_CC_TriggerSoftwareExecute(handle);
        }
        public Int32 MV_CC_GetGammaSelector_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGammaSelector(handle, ref pstValue);
        }
        public Int32 MV_CC_SetGammaSelector_NET(UInt32 nValue)
        {
            return MV_CC_SetGammaSelector(handle, nValue);
        }
        public Int32 MV_CC_GetGamma_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGamma(handle, ref pstValue);
        }
        public Int32 MV_CC_SetGamma_NET(Single fValue)
        {
            return MV_CC_SetGamma(handle, fValue);
        }
        public Int32 MV_CC_GetSharpness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSharpness(handle, ref pstValue);
        }
        public Int32 MV_CC_SetSharpness_NET(UInt32 nValue)
        {
            return MV_CC_SetSharpness(handle, nValue);
        }
        public Int32 MV_CC_GetHue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHue(handle, ref pstValue);
        }
        public Int32 MV_CC_SetHue_NET(UInt32 nValue)
        {
            return MV_CC_SetHue(handle, nValue);
        }
        public Int32 MV_CC_GetSaturation_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSaturation(handle, ref pstValue);
        }
        public Int32 MV_CC_SetSaturation_NET(UInt32 nValue)
        {
            return MV_CC_SetSaturation(handle, nValue);
        }
        public Int32 MV_CC_GetBalanceWhiteAuto_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetBalanceWhiteAuto(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBalanceWhiteAuto_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceWhiteAuto(handle, nValue);
        }
        public Int32 MV_CC_GetBalanceRatioRed_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioRed(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBalanceRatioRed_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioRed(handle, nValue);
        }
        public Int32 MV_CC_GetBalanceRatioGreen_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioGreen(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBalanceRatioGreen_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioGreen(handle, nValue);
        }
        public Int32 MV_CC_GetBalanceRatioBlue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioBlue(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBalanceRatioBlue_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioBlue(handle, nValue);
        }
        public Int32 MV_CC_GetDeviceUserID_NET(ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetDeviceUserID(handle, ref pstValue);
        }
        public Int32 MV_CC_SetDeviceUserID_NET(string chValue)
        {
            return MV_CC_SetDeviceUserID(handle, chValue);
        }
        public Int32 MV_CC_GetBurstFrameCount_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBurstFrameCount(handle, ref pstValue);
        }
        public Int32 MV_CC_SetBurstFrameCount_NET(UInt32 nValue)
        {
            return MV_CC_SetBurstFrameCount(handle, nValue);
        }
        public Int32 MV_CC_GetAcquisitionLineRate_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAcquisitionLineRate(handle, ref pstValue);
        }
        public Int32 MV_CC_SetAcquisitionLineRate_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionLineRate(handle, nValue);
        }
        public Int32 MV_CC_GetHeartBeatTimeout_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeartBeatTimeout(handle, ref pstValue);
        }
        public Int32 MV_CC_SetHeartBeatTimeout_NET(UInt32 nValue)
        {
            return MV_CC_SetHeartBeatTimeout(handle, nValue);
        }
        public Int32 MV_GIGE_GetGevSCPSPacketSize_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPSPacketSize(handle, ref pstValue);
        }
        public Int32 MV_GIGE_SetGevSCPSPacketSize_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPSPacketSize(handle, nValue);
        }
        public Int32 MV_GIGE_GetGevSCPD_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPD(handle, ref pstValue);
        }
        public Int32 MV_GIGE_SetGevSCPD_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPD(handle, nValue);
        }
        public Int32 MV_GIGE_GetGevSCDA_NET(ref UInt32 pnIP)
        {
            return MV_GIGE_GetGevSCDA(handle, ref pnIP);
        }
        public Int32 MV_GIGE_SetGevSCDA_NET(UInt32 nIP)
        {
            return MV_GIGE_SetGevSCDA(handle, nIP);
        }
        public Int32 MV_GIGE_GetGevSCSP_NET(ref UInt32 pnPort)
        {
            return MV_GIGE_GetGevSCSP(handle, ref pnPort);
        }
        public Int32 MV_GIGE_SetGevSCSP_NET(UInt32 nPort)
        {
            return MV_GIGE_SetGevSCSP(handle, nPort);
        }
        public Int32 MV_CC_RegisterImageCallBack_NET(cbOutputdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBack(handle, cbOutput, pUser);
        }
        #endregion
        #region ch: 参数定义 | en: Paramera Define
        #region ch采集卡类型 |en Interface type
        public const Int32 MV_GIGE_INTERFACE = unchecked((Int32)0x00000001);
        public const Int32 MV_CAMERALINK_INTERFACE = unchecked((Int32)0x00000004);
        public const Int32 MV_CXP_INTERFACE = unchecked((Int32)0x00000008);
        public const Int32 MV_XOF_INTERFACE = unchecked((Int32)0x00000010);
        #endregion
        #region 设备类型定义
        public const Int32 MV_UNKNOW_DEVICE = unchecked((Int32)0x00000000);
        public const Int32 MV_GIGE_DEVICE = unchecked((Int32)0x00000001);
        public const Int32 MV_1394_DEVICE = unchecked((Int32)0x00000002);
        public const Int32 MV_USB_DEVICE = unchecked((Int32)0x00000004);
        public const Int32 MV_CAMERALINK_DEVICE = unchecked((Int32)0x00000008);
        public const Int32 MV_VIR_GIGE_DEVICE = unchecked((Int32)0x00000010);
        public const Int32 MV_VIR_USB_DEVICE = unchecked((Int32)0x00000020);
        public const Int32 MV_GENTL_GIGE_DEVICE = unchecked((Int32)0x00000040);
        public const Int32 MV_GENTL_CAMERALINK_DEVICE = unchecked((Int32)0x00000080);
        public const Int32 MV_GENTL_CXP_DEVICE = unchecked((Int32)0x00000100);
        public const Int32 MV_GENTL_XOF_DEVICE = unchecked((Int32)0x00000200);
        public const Int32 INFO_MAX_BUFFER_SIZE = 64;
        public const Int32 MV_MAX_DEVICE_NUM = 256;
        public const Int32 MV_MAX_GENTL_IF_NUM = 256;
        public const Int32 MV_MAX_GENTL_DEV_NUM = 256;
        public const Int32 MV_MAX_XML_DISC_STRLEN_C = 512;
        public const Int32 MV_MAX_XML_NODE_STRLEN_C = 64;
        public const Int32 MV_MAX_XML_NODE_NUM_C = 128;
        public const Int32 MV_MAX_XML_SYMBOLIC_NUM = 64;
        public const Int32 MV_MAX_XML_STRVALUE_STRLEN_C = 64;
        public const Int32 MV_MAX_XML_PARENTS_NUM = 8;
        public const Int32 MV_MAX_XML_SYMBOLIC_STRLEN_C = 64;
        public const Int32 MV_EXCEPTION_DEV_DISCONNECT = 0x00008001;
        public const Int32 MV_EXCEPTION_VERSION_CHECK = 0x00008002;
        public const Int32 MAX_EVENT_NAME_SIZE = 128;
        public const Int32 MV_MAX_SYMBOLIC_LEN = 64;
        public const Int32 MV_MAX_SPLIT_NUM = 8;
        public const Int32 MV_MAX_INTERFACE_NUM = 64;
        public const Int32 MV_IP_CFG_STATIC = 0x05000000;
        public const Int32 MV_IP_CFG_DHCP = 0x06000000;
        public const Int32 MV_IP_CFG_LLA = 0x04000000;
        public const Int32 MV_CAML_BAUDRATE_9600 = 0x00000001;
        public const Int32 MV_CAML_BAUDRATE_19200 = 0x00000002;
        public const Int32 MV_CAML_BAUDRATE_38400 = 0x00000004;
        public const Int32 MV_CAML_BAUDRATE_57600 = 0x00000008;
        public const Int32 MV_CAML_BAUDRATE_115200 = 0x00000010;
        public const Int32 MV_CAML_BAUDRATE_230400 = 0x00000020;
        public const Int32 MV_CAML_BAUDRATE_460800 = 0x00000040;
        public const Int32 MV_CAML_BAUDRATE_921600 = 0x00000080;
        public const Int32 MV_CAML_BAUDRATE_AUTOMAX = 0x40000000;
        public const Int32 MV_MATCH_TYPE_NET_DETECT = 0x00000001;
        public const Int32 MV_MATCH_TYPE_USB_DETECT = 0x00000002;
        public const Int32 MV_ACCESS_Exclusive = 1;
        public const Int32 MV_ACCESS_ExclusiveWithSwitch = 2;
        public const Int32 MV_ACCESS_Control = 3;
        public const Int32 MV_ACCESS_ControlWithSwitch = 4;
        public const Int32 MV_ACCESS_ControlSwitchEnable = 5;
        public const Int32 MV_ACCESS_ControlSwitchEnableWithKey = 6;
        public const Int32 MV_ACCESS_Monitor = 7;
        #endregion
        #region 相机参数结构体定义
        public struct MV_INTERFACE_INFO_LIST
        {
            public UInt32 nInterfaceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_INTERFACE_NUM)]
            public IntPtr[] pInterfaceInfo;
        };
        public struct MV_INTERFACE_INFO
        {
            public UInt32 nTLayerType;
            public UInt32 nPCIEInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chManufacturer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public UInt32[] nReserved;
        };
        public enum MV_SORT_METHOD
        {
            SORTMETHOD_SERIALNUMBER = 0,
            SORTMETHOD_USERID = 1,
            SORTMETHOD_CURRENTIP_ASC = 2,
            SORTMETHOD_CURRENTIP_DESC = 3,
        };
        public struct MV_GIGE_DEVICE_INFO
        {
            public UInt32 nIpCfgOption;
            public UInt32 nIpCfgCurrent;
            public UInt32 nCurrentIp;
            public UInt32 nCurrentSubNetMask;
            public UInt32 nDefultGateWay;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chUserDefinedName;
            public UInt32 nNetExport;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };
        public struct MV_GIGE_DEVICE_INFO_EX
        {
            public UInt32 nIpCfgOption;
            public UInt32 nIpCfgCurrent;
            public UInt32 nCurrentIp;
            public UInt32 nCurrentSubNetMask;
            public UInt32 nDefultGateWay;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Byte[] chUserDefinedName;
            public UInt32 nNetExport;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };
        public struct MV_USB3_DEVICE_INFO
        {
            public Byte CrtlInEndPoint;
            public Byte CrtlOutEndPoint;
            public Byte StreamEndPoint;
            public Byte EventEndPoint;
            public UInt16 idVendor;
            public UInt16 idProduct;
            public UInt32 nDeviceNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;
            public UInt32 nbcdUSB;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };
        public struct MV_USB3_DEVICE_INFO_EX
        {
            public Byte CrtlInEndPoint;
            public Byte CrtlOutEndPoint;
            public Byte StreamEndPoint;
            public Byte EventEndPoint;
            public UInt16 idVendor;
            public UInt16 idProduct;
            public UInt32 nDeviceNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            public UInt32 nbcdUSB;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };
        public struct MV_CamL_DEV_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chPortID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
            public UInt32[] nReserved;
        };
        public struct MV_CML_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                           
        };
        public struct MV_CXP_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                           
        };
        public struct MV_XOF_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                           
        };
        [StructLayout(LayoutKind.Sequential)]    
        public struct MV_CC_DEVICE_INFO
        {
            public UInt16 nMajorVer;
            public UInt16 nMinorVer;
            public UInt32 nMacAddrHigh;
            public UInt32 nMacAddrLow;
            public UInt32 nTLayerType;
            public UInt32 nDevTypeInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
            [StructLayout(LayoutKind.Explicit, Size = 540)]
            public struct SPECIAL_INFO
            {
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 216)]
                public Byte[] stGigEInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 536)]
                public Byte[] stCamLInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stUsb3VInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stCMLInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stCXPInfo;
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stXoFInfo;
            };
            public SPECIAL_INFO SpecialInfo;
            public MV_CC_DEVICE_INFO(UInt32 nAnyNum)
            {
                nMajorVer = 0;
                nMinorVer = 0;
                nMacAddrHigh = 0;
                nMacAddrLow = 0;
                nTLayerType = 0;
                nDevTypeInfo = 0;
                nReserved = new uint[3];
                SpecialInfo.stGigEInfo = new byte[216];
                SpecialInfo.stCamLInfo = new byte[536];
                SpecialInfo.stUsb3VInfo = new byte[540];
                SpecialInfo.stCMLInfo = new byte[540];
                SpecialInfo.stCXPInfo = new byte[540];
                SpecialInfo.stXoFInfo = new byte[540];
            }
        };
        public struct MV_CC_DEVICE_INFO_LIST
        {
            public UInt32 nDeviceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_DEVICE_NUM)]
            public IntPtr[] pDeviceInfo;
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct MV_GENTL_IF_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chTLType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDisplayName;
            public UInt32 nCtiIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };
        public struct MV_GENTL_IF_INFO_LIST
        {
            public UInt32 nInterfaceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_IF_NUM)]
            public IntPtr[] pIFInfo;
        };
        public struct MV_GENTL_DEV_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chTLType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            public UInt32 nCtiIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };
        public struct MV_GENTL_DEV_INFO_EX
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chTLType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDisplayName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            public UInt32 nCtiIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };
        public struct MV_GENTL_DEV_INFO_LIST
        {
            public UInt32 nDeviceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_DEV_NUM)]
            public IntPtr[] pDeviceInfo;
        };
        public struct MV_NETTRANS_INFO
        {
            public Int64 nReviceDataSize;
            public Int32 nThrowFrameCount;
            public UInt32 nNetRecvFrameCount;
            public Int64 nRequestResendPacketCount;
            public Int64 nResendPacketCount;
        };
        public struct MV_FRAME_OUT_INFO
        {
            public UInt16 nWidth;
            public UInt16 nHeight;
            public MvGvspPixelType enPixelType;
            public UInt32 nFrameNum;
            public UInt32 nDevTimeStampHigh;
            public UInt32 nDevTimeStampLow;
            public UInt32 nReserved0;
            public Int64 nHostTimeStamp;
            public UInt32 nFrameLen;
            public UInt32 nLostPacket;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        };
        public struct MV_CHUNK_DATA_CONTENT
        {
            public IntPtr pChunkData;
            public UInt32 nChunkID;
            public UInt32 nChunkLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };
        public struct MV_FRAME_OUT_INFO_EX
        {
            public UInt16 nWidth;
            public UInt16 nHeight;
            public MvGvspPixelType enPixelType;
            public UInt32 nFrameNum;
            public UInt32 nDevTimeStampHigh;
            public UInt32 nDevTimeStampLow;
            public UInt32 nReserved0;
            public Int64 nHostTimeStamp;
            public UInt32 nFrameLen;
            public UInt32 nSecondCount;
            public UInt32 nCycleCount;
            public UInt32 nCycleOffset;
            public Single fGain;
            public Single fExposureTime;
            public UInt32 nAverageBrightness;
            public UInt32 nRed;
            public UInt32 nGreen;
            public UInt32 nBlue;
            public UInt32 nFrameCounter;
            public UInt32 nTriggerIndex;
            public UInt32 nInput;
            public UInt32 nOutput;
            public UInt16 nOffsetX;
            public UInt16 nOffsetY;
            public UInt16 nChunkWidth;
            public UInt16 nChunkHeight;
            public UInt32 nLostPacket;
            public UInt32 nUnparsedChunkNum;
            [StructLayout(LayoutKind.Explicit)]
            public struct UNPARSED_CHUNK_LIST
            {
                [FieldOffset(0)]
                public IntPtr pUnparsedChunkContent;
                [FieldOffset(0)]
                public Int64 nAligning;
            }
            public UNPARSED_CHUNK_LIST UnparsedChunkList;
            public UInt32 nExtendWidth;
            public UInt32 nExtendHeight;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public UInt32[] nReserved;
        };
        public struct MV_FRAME_OUT
        {
            public IntPtr pBufAddr;
            public MV_FRAME_OUT_INFO_EX stFrameInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        };
        public enum MV_GRAB_STRATEGY
        {
            MV_GrabStrategy_OneByOne = 0,
            MV_GrabStrategy_LatestImagesOnly = 1,
            MV_GrabStrategy_LatestImages = 2,
            MV_GrabStrategy_UpcomingImage = 3,
        };
        public struct MV_DISPLAY_FRAME_INFO
        {
            public IntPtr hWnd;
            public IntPtr pData;
            public UInt32 nDataLen;
            public UInt16 nWidth;
            public UInt16 nHeight;
            public MvGvspPixelType enPixelType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };
        public struct MV_DISPLAY_FRAME_INFO_EX
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pData;
            public UInt32 nDataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };
        public enum MV_SAVE_POINT_CLOUD_FILE_TYPE
        {
            MV_PointCloudFile_Undefined = 0,
            MV_PointCloudFile_PLY = 1,
            MV_PointCloudFile_CSV = 2,
            MV_PointCloudFile_OBJ = 3,
        };
        public struct MV_SAVE_POINT_CLOUD_PARAM
        {
            public UInt32 nLinePntNum;
            public UInt32 nLineNum;
            public MvGvspPixelType enSrcPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public MV_SAVE_POINT_CLOUD_FILE_TYPE enPointCloudFileType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_SAVE_IAMGE_TYPE
        {
            MV_Image_Undefined = 0,
            MV_Image_Bmp = 1,
            MV_Image_Jpeg = 2,
            MV_Image_Png = 3,
            MV_Image_Tif = 4,
        };
        public struct MV_SAVE_IMAGE_PARAM
        {
            public IntPtr pData;
            public UInt32 nDataLen;
            public MvGvspPixelType enPixelType;
            public UInt16 nWidth;
            public UInt16 nHeight;
            public IntPtr pImageBuffer;
            public UInt32 nImageLen;
            public UInt32 nBufferSize;
            public MV_SAVE_IAMGE_TYPE enImageType;
        };
        public struct MV_SAVE_IMAGE_PARAM_EX2
        {
            public IntPtr pData;
            public UInt32 nDataLen;
            public MvGvspPixelType enPixelType;
            public UInt16 nWidth;
            public UInt16 nHeight;
            public IntPtr pImageBuffer;
            public UInt32 nImageLen;
            public UInt32 nBufferSize;
            public MV_SAVE_IAMGE_TYPE enImageType;
            public UInt32 nJpgQuality;
            public UInt32 iMethodValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };
        public struct MV_SAVE_IMAGE_PARAM_EX3
        {
            public IntPtr pData;
            public UInt32 nDataLen;
            public MvGvspPixelType enPixelType;
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pImageBuffer;
            public UInt32 nImageLen;
            public UInt32 nBufferSize;
            public MV_SAVE_IAMGE_TYPE enImageType;
            public UInt32 nJpgQuality;
            public UInt32 iMethodValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        }
        public struct MV_SAVE_IMG_TO_FILE_PARAM
        {
            public MvGvspPixelType enPixelType;
            public IntPtr pData;
            public UInt32 nDataLen;
            public UInt16 nWidth;
            public UInt16 nHeight;
            public MV_SAVE_IAMGE_TYPE enImageType;
            public UInt32 nQuality;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string pImagePath;
            public UInt32 iMethodValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_SAVE_IMG_TO_FILE_PARAM_EX
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pData;
            public UInt32 nDataLen;
            public MV_SAVE_IAMGE_TYPE enImageType;
            public string pImagePath;
            public UInt32 nQuality;
            public UInt32 iMethodValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_IMG_ROTATION_ANGLE
        {
            MV_IMAGE_ROTATE_90 = 1,
            MV_IMAGE_ROTATE_180 = 2,
            MV_IMAGE_ROTATE_270 = 3,
        }
        public struct MV_CC_ROTATE_IMAGE_PARAM
        {
            public MvGvspPixelType enPixelType;
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufLen;
            public UInt32 nDstBufSize;
            public MV_IMG_ROTATION_ANGLE enRotationAngle;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_IMG_FLIP_TYPE
        {
            MV_FLIP_VERTICAL = 1,
            MV_FLIP_HORIZONTAL = 2,
        }
        public struct MV_CC_FLIP_IMAGE_PARAM
        {
            public MvGvspPixelType enPixelType;
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufLen;
            public UInt32 nDstBufSize;
            public MV_IMG_FLIP_TYPE enFlipType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_PIXEL_CONVERT_PARAM
        {
            public UInt16 nWidth;
            public UInt16 nHeight;
            public MvGvspPixelType enSrcPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public MvGvspPixelType enDstPixelType;
            public IntPtr pDstBuffer;
            public UInt32 nDstLen;
            public UInt32 nDstBufferSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nRes;
        }
        public struct MV_CC_PIXEL_CONVERT_PARAM_EX
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enSrcPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public MvGvspPixelType enDstPixelType;
            public IntPtr pDstBuffer;
            public UInt32 nDstLen;
            public UInt32 nDstBufferSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nRes;
        }
        public enum MV_CC_GAMMA_TYPE
        {
            MV_CC_GAMMA_TYPE_NONE = 0,
            MV_CC_GAMMA_TYPE_VALUE = 1,
            MV_CC_GAMMA_TYPE_USER_CURVE = 2,
            MV_CC_GAMMA_TYPE_LRGB2SRGB = 3,
            MV_CC_GAMMA_TYPE_SRGB2LRGB = 4,
        }
        public struct MV_CC_GAMMA_PARAM
        {
            public MV_CC_GAMMA_TYPE enGammaType;
            public Single fGammaValue;
            public IntPtr pGammaCurveBuf;
            public UInt32 nGammaCurveBufLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_CCM_PARAM
        {
            public Boolean bCCMEnable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_CCM_PARAM_EX
        {
            public Boolean bCCMEnable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;
            public UInt32 nCCMScale;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_CLUT_PARAM
        {
            public Boolean bCLUTEnable;
            public UInt32 nCLUTScale;
            public UInt32 nCLUTSize;
            public IntPtr pCLUTBuf;
            public UInt32 nCLUTBufLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_CONTRAST_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public MvGvspPixelType enPixelType;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public UInt32 nContrastFactor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_SHARPEN_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public MvGvspPixelType enPixelType;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public UInt32 nSharpenAmount;
            public UInt32 nSharpenRadius;
            public UInt32 nSharpenThreshold;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_COLOR_CORRECT_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public MvGvspPixelType enPixelType;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public UInt32 nImageBit;
            public MV_CC_GAMMA_PARAM stGammaParam;
            public MV_CC_CCM_PARAM_EX stCCMParam;
            public MV_CC_CLUT_PARAM stCLUTParam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_RECT_I
        {
            public UInt32 nX;
            public UInt32 nY;
            public UInt32 nWidth;
            public UInt32 nHeight;
        };
        public struct MV_CC_NOISE_ESTIMATE_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public IntPtr pstROIRect;
            public UInt32 nROINum;
            public UInt32 nNoiseThreshold;
            public IntPtr pNoiseProfile;
            public UInt32 nNoiseProfileSize;
            public UInt32 nNoiseProfileLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_SPATIAL_DENOISE_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public IntPtr pNoiseProfile;
            public UInt32 nNoiseProfileLen;
            public UInt32 nBayerDenoiseStrength;
            public UInt32 nBayerSharpenStrength;
            public UInt32 nBayerNoiseCorrect;
            public UInt32 nNoiseCorrectLum;
            public UInt32 nNoiseCorrectChrom;
            public UInt32 nStrengthLum;
            public UInt32 nStrengthChrom;
            public UInt32 nStrengthSharpen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_LSC_CALIB_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public IntPtr pCalibBuf;
            public UInt32 nCalibBufSize;
            public UInt32 nCalibBufLen;
            public UInt32 nSecNumW;
            public UInt32 nSecNumH;
            public UInt32 nPadCoef;
            public UInt32 nCalibMethod;
            public UInt32 nTargetGray;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_LSC_CORRECT_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcBuf;
            public UInt32 nSrcBufLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public IntPtr pCalibBuf;
            public UInt32 nCalibBufLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_CC_BAYER_NOISE_FEATURE_TYPE
        {
            MV_CC_BAYER_NOISE_FEATURE_TYPE_INVALID = 0,
            MV_CC_BAYER_NOISE_FEATURE_TYPE_PROFILE = 1,
            MV_CC_BAYER_NOISE_FEATURE_TYPE_LEVEL = 2,
            MV_CC_BAYER_NOISE_FEATURE_TYPE_DEFAULT = 2,
        };
        public struct MV_CC_BAYER_NOISE_PROFILE_INFO
        {
            public UInt32 nVersion;
            public MV_CC_BAYER_NOISE_FEATURE_TYPE enNoiseFeatureType;
            public MvGvspPixelType enPixelType;
            public Int32 nNoiseLevel;
            public UInt32 nCurvePointNum;
            public IntPtr nNoiseCurve;
            public IntPtr nLumCurve;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_BAYER_NOISE_ESTIMATE_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public UInt32 nNoiseThreshold;
            public IntPtr pCurveBuf;
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;
            public UInt32 nThreadNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_BAYER_SPATIAL_DENOISE_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;
            public UInt32 nDenoiseStrength;
            public UInt32 nSharpenStrength;
            public UInt32 nNoiseCorrect;
            public UInt32 nThreadNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_FRAME_SPEC_INFO
        {
            public UInt32 nSecondCount;
            public UInt32 nCycleCount;
            public UInt32 nCycleOffset;
            public Single fGain;
            public Single fExposureTime;
            public UInt32 nAverageBrightness;
            public UInt32 nRed;
            public UInt32 nGreen;
            public UInt32 nBlue;
            public UInt32 nFrameCounter;
            public UInt32 nTriggerIndex;
            public UInt32 nInput;
            public UInt32 nOutput;
            public UInt16 nOffsetX;
            public UInt16 nOffsetY;
            public UInt16 nFrameWidth;
            public UInt16 nFrameHeight;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nRes;
        };
        public struct MV_CC_HB_DECODE_PARAM
        {
            public IntPtr pSrcBuf;
            public UInt32 nSrcLen;
            public UInt32 nWidth;
            public UInt32 nHeight;
            public IntPtr pDstBuf;
            public UInt32 nDstBufSize;
            public UInt32 nDstBufLen;
            public MvGvspPixelType enDstPixelType;
            public MV_CC_FRAME_SPEC_INFO stFrameSpecInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_RECORD_FORMAT_TYPE
        {
            MV_FormatType_Undefined = 0,
            MV_FormatType_AVI = 1,
        }
        public struct MV_CC_RECORD_PARAM
        {
            public MvGvspPixelType enPixelType;
            public UInt16 nWidth;
            public UInt16 nHeight;
            public Single fFrameRate;
            public UInt32 nBitRate;
            public MV_RECORD_FORMAT_TYPE enRecordFmtType;
            public String strFilePath;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public struct MV_CC_INPUT_FRAME_INFO
        {
            public IntPtr pData;
            public UInt32 nDataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };
        public enum MV_CAM_ACQUISITION_MODE
        {
            MV_ACQ_MODE_SINGLE = 0,
            MV_ACQ_MODE_MUTLI = 1,
            MV_ACQ_MODE_CONTINUOUS = 2,
        };
        public enum MV_CAM_GAIN_MODE
        {
            MV_GAIN_MODE_OFF = 0,
            MV_GAIN_MODE_ONCE = 1,
            MV_GAIN_MODE_CONTINUOUS = 2,
        };
        public enum MV_CAM_EXPOSURE_MODE
        {
            MV_EXPOSURE_MODE_TIMED = 0,
            MV_EXPOSURE_MODE_TRIGGER_WIDTH = 1,
        };
        public enum MV_CAM_EXPOSURE_AUTO_MODE
        {
            MV_EXPOSURE_AUTO_MODE_OFF = 0,
            MV_EXPOSURE_AUTO_MODE_ONCE = 1,
            MV_EXPOSURE_AUTO_MODE_CONTINUOUS = 2,
        };
        public enum MV_CAM_TRIGGER_MODE
        {
            MV_TRIGGER_MODE_OFF = 0,
            MV_TRIGGER_MODE_ON = 1,
        };
        public enum MV_CAM_GAMMA_SELECTOR
        {
            MV_GAMMA_SELECTOR_USER = 1,
            MV_GAMMA_SELECTOR_SRGB = 2,
        };
        public enum MV_CAM_BALANCEWHITE_AUTO
        {
            MV_BALANCEWHITE_AUTO_OFF = 0,
            MV_BALANCEWHITE_AUTO_ONCE = 2,
            MV_BALANCEWHITE_AUTO_CONTINUOUS = 1,
        }
        public enum MV_CAM_TRIGGER_SOURCE
        {
            MV_TRIGGER_SOURCE_LINE0 = 0,
            MV_TRIGGER_SOURCE_LINE1 = 1,
            MV_TRIGGER_SOURCE_LINE2 = 2,
            MV_TRIGGER_SOURCE_LINE3 = 3,
            MV_TRIGGER_SOURCE_COUNTER0 = 4,
            MV_TRIGGER_SOURCE_SOFTWARE = 7,
            MV_TRIGGER_SOURCE_FrequencyConverter = 8,
        }
        public struct MV_ALL_MATCH_INFO
        {
            public UInt32 nType;
            public IntPtr pInfo;
            public UInt32 nInfoSize;
        }
        public struct MV_MATCH_INFO_NET_DETECT
        {
            public Int64 nReviceDataSize;
            public Int64 nLostPacketCount;
            public UInt32 nLostFrameCount;
            public UInt32 nNetRecvFrameCount;
            public Int64 nRequestResendPacketCount;
            public Int64 nResendPacketCount;
        }
        public struct MV_MATCH_INFO_USB_DETECT
        {
            public Int64 nReviceDataSize;
            public UInt32 nRevicedFrameCount;
            public UInt32 nErrorFrameCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        }
        public struct MV_IMAGE_BASIC_INFO
        {
            public UInt16 nWidthValue;
            public UInt16 nWidthMin;
            public UInt32 nWidthMax;
            public UInt32 nWidthInc;
            public UInt32 nHeightValue;
            public UInt32 nHeightMin;
            public UInt32 nHeightMax;
            public UInt32 nHeightInc;
            public Single fFrameRateValue;
            public Single fFrameRateMin;
            public Single fFrameRateMax;
            public UInt32 enPixelType;
            public UInt32 nSupportedPixelFmtNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] enPixelList;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }
        public enum MV_XML_Visibility
        {
            V_Beginner = 0,
            V_Expert = 1,
            V_Guru = 2,
            V_Invisible = 3,
            V_Undefined = 99
        }
        public struct MV_EVENT_OUT_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_EVENT_NAME_SIZE)]
            public string EventName;
            public UInt16 nEventID;
            public UInt16 nStreamChannel;
            public UInt32 nBlockIdHigh;
            public UInt32 nBlockIdLow;
            public UInt32 nTimestampHigh;
            public UInt32 nTimestampLow;
            public IntPtr pEventData;
            public UInt32 nEventDataSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }
        public struct MV_CC_FILE_ACCESS
        {
            public String pUserFileName;
            public String pDevFileName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }
        public struct MV_CC_FILE_ACCESS_EX
        {
            public IntPtr pUserFileBuf;
            public UInt32 nFileBufSize;
            public UInt32 nFileBufLen;
            public String pDevFileName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }
        public struct MV_CC_FILE_ACCESS_PROGRESS
        {
            public Int64 nCompleted;
            public Int64 nTotal;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }
        public enum MV_GIGE_TRANSMISSION_TYPE
        {
            MV_GIGE_TRANSTYPE_UNICAST = 0x0,
            MV_GIGE_TRANSTYPE_MULTICAST = 0x1,
            MV_GIGE_TRANSTYPE_LIMITEDBROADCAST = 0x2,
            MV_GIGE_TRANSTYPE_SUBNETBROADCAST = 0x3,
            MV_GIGE_TRANSTYPE_CAMERADEFINED = 0x4,
            MV_GIGE_TRANSTYPE_UNICAST_DEFINED_PORT = 0x5,
            MV_GIGE_TRANSTYPE_UNICAST_WITHOUT_RECV = 0x00010000,
            MV_GIGE_TRANSTYPE_MULTICAST_WITHOUT_RECV = 0x00010001,
        }
        public struct MV_CC_TRANSMISSION_TYPE
        {
            public MV_GIGE_TRANSMISSION_TYPE enTransmissionType;
            public UInt32 nDestIp;
            public UInt16 nDestPort;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }
        public struct MV_ACTION_CMD_INFO
        {
            public UInt32 nDeviceKey;
            public UInt32 nGroupKey;
            public UInt32 nGroupMask;
            public UInt32 bActionTimeEnable;
            public Int64 nActionTime;
            public String pBroadcastAddress;
            public UInt32 nTimeOut;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }
        public struct MV_ACTION_CMD_RESULT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String strDeviceAddress;
            public Int32 nStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_ACTION_CMD_RESULT_LIST
        {
            public UInt32 nNumResults;
            public IntPtr pResults;
        }
        public enum MV_XML_InterfaceType
        {
            IFT_IValue,
            IFT_IBase,
            IFT_IInteger,
            IFT_IBoolean,
            IFT_ICommand,
            IFT_IFloat,
            IFT_IString,
            IFT_IRegister,
            IFT_ICategory,
            IFT_IEnumeration,
            IFT_IEnumEntry,
            IFT_IPort,
        }
        public struct MV_XML_NODE_FEATURE
        {
            public MV_XML_InterfaceType enType;
            public MV_XML_Visibility enVisivility;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_NODES_LIST
        {
            public UInt32 nNodeNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_NODE_NUM_C)]
            public MV_XML_NODE_FEATURE[] stNodes;
        }
        public struct MVCC_INTVALUE
        {
            public UInt32 nCurValue;
            public UInt32 nMax;
            public UInt32 nMin;
            public UInt32 nInc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_INTVALUE_EX
        {
            public Int64 nCurValue;
            public Int64 nMax;
            public Int64 nMin;
            public Int64 nInc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }
        public struct MVCC_FLOATVALUE
        {
            public Single fCurValue;
            public Single fMax;
            public Single fMin;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_ENUMVALUE
        {
            public UInt32 nCurValue;
            public UInt32 nSupportedNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] nSupportValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_STRINGVALUE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string chCurValue;
            public Int64 nMaxLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        }
        public enum MV_XML_AccessMode
        {
            AM_NI,
            AM_NA,
            AM_WO,
            AM_RO,
            AM_RW,
            AM_Undefined,
            AM_CycleDetect
        }
        public struct MV_XML_FEATURE_Integer
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            public Int64 nValue;
            public Int64 nMinValue;
            public Int64 nMaxValue;
            public Int64 nIncrement;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Boolean
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            public bool bValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Command
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Float
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            public Double dfValue;
            public Double dfMinValue;
            public Double dfMaxValue;
            public Double dfIncrement;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_String
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_STRVALUE_STRLEN_C)]
            public string strValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Register
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            public Int64 nAddrValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Category
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_Visibility enVisivility;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_EnumEntry
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public Int32 bIsImplemented;
            public Int32 nParentsNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_PARENTS_NUM)]
            public MV_XML_NODE_FEATURE[] stParentsList;
            public MV_XML_Visibility enVisivility;
            public Int64 nValue;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct StrSymbolic
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string str;
        }
        public struct MV_XML_FEATURE_Enumeration
        {
            public MV_XML_Visibility enVisivility;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public Int32 nSymbolicNum;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string strCurrentSymbolic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public StrSymbolic[] strSymbolic;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            public Int64 nValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MV_XML_FEATURE_Port
        {
            public MV_XML_Visibility enVisivility;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;
            public MV_XML_AccessMode enAccessMode;
            public Int32 bIsLocked;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_COLORF
        {
            public float fR;
            public float fG;
            public float fB;
            public float fAlpha;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_POINTF
        {
            public float fX;
            public float fY;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_RECT_INFO
        {
            public float fTop;
            public float fBottom;
            public float fLeft;
            public float fRight;
            public MVCC_COLORF stColor;
            public UInt32 nLineWidth;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_CIRCLE_INFO
        {
            public MVCC_POINTF stCenterPoint;
            public float fR1;
            public float fR2;
            public MVCC_COLORF stColor;
            public UInt32 nLineWidth;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_LINES_INFO
        {
            public MVCC_POINTF stStartPoint;
            public MVCC_POINTF stEndPoint;
            public MVCC_COLORF stColor;
            public UInt32 nLineWidth;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public struct MVCC_ENUMENTRY
        {
            public UInt32 nValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_SYMBOLIC_LEN)]
            public Byte[] chSymbolic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        public enum MV_CC_STREAM_EXCEPTION_TYPE
        {
            MV_CC_STREAM_EXCEPTION_ABNORMAL_IMAGE = 0x4001,
            MV_CC_STREAM_EXCEPTION_LIST_OVERFLOW = 0x4002,
            MV_CC_STREAM_EXCEPTION_LIST_EMPTY = 0x4003,
            MV_CC_STREAM_EXCEPTION_RECONNECTION = 0x4004,
            MV_CC_STREAM_EXCEPTION_DISCONNECTED = 0x4005,
            MV_CC_STREAM_EXCEPTION_DEVICE = 0x4006,
        }
        public struct MV_OUTPUT_IMAGE_INFO
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pBuf;
            public UInt32 nBufLen;
            public UInt32 nBufSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }
        public enum MV_IMAGE_RECONSTRUCTION_METHOD
        {
            MV_SPLIT_BY_LINE = 1,
        }
        public struct MV_RECONSTRUCT_IMAGE_PARAM
        {
            public UInt32 nWidth;
            public UInt32 nHeight;
            public MvGvspPixelType enPixelType;
            public IntPtr pSrcData;
            public UInt32 nSrcDataLen;
            public UInt32 nExposureNum;
            public MV_IMAGE_RECONSTRUCTION_METHOD enReconstructMethod;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_SPLIT_NUM)]
            public MV_OUTPUT_IMAGE_INFO[] stDstBufList;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        #endregion
        #region 像素格式定义
        public enum MvGvspPixelType
        {
            PixelType_Gvsp_Undefined = -1,
            PixelType_Gvsp_Mono1p = 0x01010037,
            PixelType_Gvsp_Mono2p = 0x01020038,
            PixelType_Gvsp_Mono4p = 0x01040039,
            PixelType_Gvsp_Mono8 = 0x01080001,
            PixelType_Gvsp_Mono8_Signed = 0x01080002,
            PixelType_Gvsp_Mono10 = 0x01100003,
            PixelType_Gvsp_Mono10_Packed = 0x010c0004,
            PixelType_Gvsp_Mono12 = 0x01100005,
            PixelType_Gvsp_Mono12_Packed = 0x010c0006,
            PixelType_Gvsp_Mono14 = 0x01100025,
            PixelType_Gvsp_Mono16 = 0x01100007,
            PixelType_Gvsp_BayerGR8 = 0x01080008,
            PixelType_Gvsp_BayerRG8 = 0x01080009,
            PixelType_Gvsp_BayerGB8 = 0x0108000a,
            PixelType_Gvsp_BayerBG8 = 0x0108000b,
            PixelType_Gvsp_BayerRBGG8 = 0x01080046,
            PixelType_Gvsp_BayerGR10 = 0x0110000c,
            PixelType_Gvsp_BayerRG10 = 0x0110000d,
            PixelType_Gvsp_BayerGB10 = 0x0110000e,
            PixelType_Gvsp_BayerBG10 = 0x0110000f,
            PixelType_Gvsp_BayerGR12 = 0x01100010,
            PixelType_Gvsp_BayerRG12 = 0x01100011,
            PixelType_Gvsp_BayerGB12 = 0x01100012,
            PixelType_Gvsp_BayerBG12 = 0x01100013,
            PixelType_Gvsp_BayerGR10_Packed = 0x010c0026,
            PixelType_Gvsp_BayerRG10_Packed = 0x010c0027,
            PixelType_Gvsp_BayerGB10_Packed = 0x010c0028,
            PixelType_Gvsp_BayerBG10_Packed = 0x010c0029,
            PixelType_Gvsp_BayerGR12_Packed = 0x010c002a,
            PixelType_Gvsp_BayerRG12_Packed = 0x010c002b,
            PixelType_Gvsp_BayerGB12_Packed = 0x010c002c,
            PixelType_Gvsp_BayerBG12_Packed = 0x010c002d,
            PixelType_Gvsp_BayerGR16 = 0x0110002e,
            PixelType_Gvsp_BayerRG16 = 0x0110002f,
            PixelType_Gvsp_BayerGB16 = 0x01100030,
            PixelType_Gvsp_BayerBG16 = 0x01100031,
            PixelType_Gvsp_RGB8_Packed = 0x02180014,
            PixelType_Gvsp_BGR8_Packed = 0x02180015,
            PixelType_Gvsp_RGBA8_Packed = 0x02200016,
            PixelType_Gvsp_BGRA8_Packed = 0x02200017,
            PixelType_Gvsp_RGB10_Packed = 0x02300018,
            PixelType_Gvsp_BGR10_Packed = 0x02300019,
            PixelType_Gvsp_RGB12_Packed = 0x0230001a,
            PixelType_Gvsp_BGR12_Packed = 0x0230001b,
            PixelType_Gvsp_RGB16_Packed = 0x02300033,
            PixelType_Gvsp_BGR16_Packed = 0x0230004b,
            PixelType_Gvsp_RGBA16_Packed = 0x02400040,
            PixelType_Gvsp_BGRA16_Packed = 0x02400051,
            PixelType_Gvsp_RGB10V1_Packed = 0x0220001c,
            PixelType_Gvsp_RGB10V2_Packed = 0x0220001d,
            PixelType_Gvsp_RGB12V1_Packed = 0x02240034,
            PixelType_Gvsp_RGB565_Packed = 0x02100035,
            PixelType_Gvsp_BGR565_Packed = 0x02100036,
            PixelType_Gvsp_YUV411_Packed = 0x020c001e,
            PixelType_Gvsp_YUV422_Packed = 0x0210001f,
            PixelType_Gvsp_YUV422_YUYV_Packed = 0x02100032,
            PixelType_Gvsp_YUV444_Packed = 0x02180020,
            PixelType_Gvsp_YCBCR8_CBYCR = 0x0218003a,
            PixelType_Gvsp_YCBCR422_8 = 0x0210003b,
            PixelType_Gvsp_YCBCR422_8_CBYCRY = 0x02100043,
            PixelType_Gvsp_YCBCR411_8_CBYYCRYY = 0x020c003c,
            PixelType_Gvsp_YCBCR601_8_CBYCR = 0x0218003d,
            PixelType_Gvsp_YCBCR601_422_8 = 0x0210003e,
            PixelType_Gvsp_YCBCR601_422_8_CBYCRY = 0x02100044,
            PixelType_Gvsp_YCBCR601_411_8_CBYYCRYY = 0x020c003f,
            PixelType_Gvsp_YCBCR709_8_CBYCR = 0x02180040,
            PixelType_Gvsp_YCBCR709_422_8 = 0x02100041,
            PixelType_Gvsp_YCBCR709_422_8_CBYCRY = 0x02100045,
            PixelType_Gvsp_YCBCR709_411_8_CBYYCRYY = 0x020c0042,
            PixelType_Gvsp_YUV420SP_NV12 = 0X020c8001,
            PixelType_Gvsp_YUV420SP_NV21 = 0X020c8002,
            PixelType_Gvsp_RGB8_Planar = 0x02180021,
            PixelType_Gvsp_RGB10_Planar = 0x02300022,
            PixelType_Gvsp_RGB12_Planar = 0x02300023,
            PixelType_Gvsp_RGB16_Planar = 0x02300024,
            PixelType_Gvsp_Jpeg = unchecked((Int32)0x80180001),
            PixelType_Gvsp_Coord3D_ABC32f = 0x026000C0,
            PixelType_Gvsp_Coord3D_ABC32f_Planar = 0x026000C1,
            PixelType_Gvsp_Coord3D_AC32f = 0x024000C2, 
            PixelType_Gvsp_COORD3D_DEPTH_PLUS_MASK = unchecked((Int32)0x821c0001),
            PixelType_Gvsp_Coord3D_ABC32 = unchecked((Int32)0x82603001),
            PixelType_Gvsp_Coord3D_AB32f = unchecked((Int32)0x82403002),
            PixelType_Gvsp_Coord3D_AB32 = unchecked((Int32)0x82403003),
            PixelType_Gvsp_Coord3D_AC32f_64 = unchecked((Int32)0x024000C2),
            PixelType_Gvsp_Coord3D_AC32f_Planar = 0x024000C3,
            PixelType_Gvsp_Coord3D_AC32 = unchecked((Int32)0x82403004),
            PixelType_Gvsp_Coord3D_A32f = 0x012000BD,
            PixelType_Gvsp_Coord3D_A32 = unchecked((Int32)0x81203005),
            PixelType_Gvsp_Coord3D_C32f = 0x012000BF,
            PixelType_Gvsp_Coord3D_C32 = unchecked((Int32)0x81203006),
            PixelType_Gvsp_Coord3D_ABC16 = 0x023000b9,
            PixelType_Gvsp_Coord3D_C16 = 0x011000b8,
            PixelType_Gvsp_Float32 = unchecked((Int32)0x81200001),
            PixelType_Gvsp_HB_Mono8 = unchecked((Int32)0x81080001),
            PixelType_Gvsp_HB_Mono10 = unchecked((Int32)0x81100003),
            PixelType_Gvsp_HB_Mono10_Packed = unchecked((Int32)0x810c0004),
            PixelType_Gvsp_HB_Mono12 = unchecked((Int32)0x81100005),
            PixelType_Gvsp_HB_Mono12_Packed = unchecked((Int32)0x810c0006),
            PixelType_Gvsp_HB_Mono16 = unchecked((Int32)0x81100007),
            PixelType_Gvsp_HB_BayerGR8 = unchecked((Int32)0x81080008),
            PixelType_Gvsp_HB_BayerRG8 = unchecked((Int32)0x81080009),
            PixelType_Gvsp_HB_BayerGB8 = unchecked((Int32)0x8108000a),
            PixelType_Gvsp_HB_BayerBG8 = unchecked((Int32)0x8108000b),
            PixelType_Gvsp_HB_BayerRBGG8 = unchecked((Int32)0x81080046),
            PixelType_Gvsp_HB_BayerGR10 = unchecked((Int32)0x8110000c),
            PixelType_Gvsp_HB_BayerRG10 = unchecked((Int32)0x8110000d),
            PixelType_Gvsp_HB_BayerGB10 = unchecked((Int32)0x8110000e),
            PixelType_Gvsp_HB_BayerBG10 = unchecked((Int32)0x8110000f),
            PixelType_Gvsp_HB_BayerGR12 = unchecked((Int32)0x81100010),
            PixelType_Gvsp_HB_BayerRG12 = unchecked((Int32)0x81100011),
            PixelType_Gvsp_HB_BayerGB12 = unchecked((Int32)0x81100012),
            PixelType_Gvsp_HB_BayerBG12 = unchecked((Int32)0x81100013),
            PixelType_Gvsp_HB_BayerGR10_Packed = unchecked((Int32)0x810c0026),
            PixelType_Gvsp_HB_BayerRG10_Packed = unchecked((Int32)0x810c0027),
            PixelType_Gvsp_HB_BayerGB10_Packed = unchecked((Int32)0x810c0028),
            PixelType_Gvsp_HB_BayerBG10_Packed = unchecked((Int32)0x810c0029),
            PixelType_Gvsp_HB_BayerGR12_Packed = unchecked((Int32)0x810c002a),
            PixelType_Gvsp_HB_BayerRG12_Packed = unchecked((Int32)0x810c002b),
            PixelType_Gvsp_HB_BayerGB12_Packed = unchecked((Int32)0x810c002c),
            PixelType_Gvsp_HB_BayerBG12_Packed = unchecked((Int32)0x810c002d),
            PixelType_Gvsp_HB_YUV422_Packed = unchecked((Int32)0x8210001f),
            PixelType_Gvsp_HB_YUV422_YUYV_Packed = unchecked((Int32)0x82100032),
            PixelType_Gvsp_HB_RGB8_Packed = unchecked((Int32)0x82180014),
            PixelType_Gvsp_HB_BGR8_Packed = unchecked((Int32)0x82180015),
            PixelType_Gvsp_HB_RGBA8_Packed = unchecked((Int32)0x82200016),
            PixelType_Gvsp_HB_BGRA8_Packed = unchecked((Int32)0x82200017),
            PixelType_Gvsp_HB_RGB16_Packed = unchecked((Int32)0x82300033),
            PixelType_Gvsp_HB_BGR16_Packed = unchecked((Int32)0x8230004B),
            PixelType_Gvsp_HB_RGBA16_Packed = unchecked((Int32)0x82400064),
            PixelType_Gvsp_HB_BGRA16_Packed = unchecked((Int32)0x82400051),
        }
        #endregion
        #region 设备错误码定义
        public const Int32 MV_OK = unchecked((Int32)0x00000000);
        public const Int32 MV_E_HANDLE = unchecked((Int32)0x80000000);
        public const Int32 MV_E_SUPPORT = unchecked((Int32)0x80000001);
        public const Int32 MV_E_BUFOVER = unchecked((Int32)0x80000002);
        public const Int32 MV_E_CALLORDER = unchecked((Int32)0x80000003);
        public const Int32 MV_E_PARAMETER = unchecked((Int32)0x80000004);
        public const Int32 MV_E_RESOURCE = unchecked((Int32)0x80000006);
        public const Int32 MV_E_NODATA = unchecked((Int32)0x80000007);
        public const Int32 MV_E_PRECONDITION = unchecked((Int32)0x80000008);
        public const Int32 MV_E_VERSION = unchecked((Int32)0x80000009);
        public const Int32 MV_E_NOENOUGH_BUF = unchecked((Int32)0x8000000A);
        public const Int32 MV_E_ABNORMAL_IMAGE = unchecked((Int32)0x8000000B);
        public const Int32 MV_E_LOAD_LIBRARY = unchecked((Int32)0x8000000C);
        public const Int32 MV_E_NOOUTBUF = unchecked((Int32)0x8000000D);
        public const Int32 MV_E_ENCRYPT = unchecked((Int32)0x8000000E);
        public const Int32 MV_E_OPENFILE = unchecked((Int32)0x8000000F);
        public const Int32 MV_E_UNKNOW = unchecked((Int32)0x800000FF);
        public const Int32 MV_E_GC_GENERIC = unchecked((Int32)0x80000100);
        public const Int32 MV_E_GC_ARGUMENT = unchecked((Int32)0x80000101);
        public const Int32 MV_E_GC_RANGE = unchecked((Int32)0x80000102);
        public const Int32 MV_E_GC_PROPERTY = unchecked((Int32)0x80000103);
        public const Int32 MV_E_GC_RUNTIME = unchecked((Int32)0x80000104);
        public const Int32 MV_E_GC_LOGICAL = unchecked((Int32)0x80000105);
        public const Int32 MV_E_GC_ACCESS = unchecked((Int32)0x80000106);
        public const Int32 MV_E_GC_TIMEOUT = unchecked((Int32)0x80000107);
        public const Int32 MV_E_GC_DYNAMICCAST = unchecked((Int32)0x80000108);
        public const Int32 MV_E_GC_UNKNOW = unchecked((Int32)0x800001FF);
        public const Int32 MV_E_NOT_IMPLEMENTED = unchecked((Int32)0x80000200);
        public const Int32 MV_E_INVALID_ADDRESS = unchecked((Int32)0x80000201);
        public const Int32 MV_E_WRITE_PROTECT = unchecked((Int32)0x80000202);
        public const Int32 MV_E_ACCESS_DENIED = unchecked((Int32)0x80000203);
        public const Int32 MV_E_BUSY = unchecked((Int32)0x80000204);
        public const Int32 MV_E_PACKET = unchecked((Int32)0x80000205);
        public const Int32 MV_E_NETER = unchecked((Int32)0x80000206);
        public const Int32 MV_E_IP_CONFLICT = unchecked((Int32)0x80000221);
        public const Int32 MV_E_USB_READ = unchecked((Int32)0x80000300);
        public const Int32 MV_E_USB_WRITE = unchecked((Int32)0x80000301);
        public const Int32 MV_E_USB_DEVICE = unchecked((Int32)0x80000302);
        public const Int32 MV_E_USB_GENICAM = unchecked((Int32)0x80000303);
        public const Int32 MV_E_USB_BANDWIDTH = unchecked((Int32)0x80000304);
        public const Int32 MV_E_USB_DRIVER = unchecked((Int32)0x80000305);
        public const Int32 MV_E_USB_UNKNOW = unchecked((Int32)0x800003FF);
        public const Int32 MV_E_UPG_FILE_MISMATCH = unchecked((Int32)0x80000400);
        public const Int32 MV_E_UPG_LANGUSGE_MISMATCH = unchecked((Int32)0x80000401);
        public const Int32 MV_E_UPG_CONFLICT = unchecked((Int32)0x80000402);
        public const Int32 MV_E_UPG_INNER_ERR = unchecked((Int32)0x80000403);
        public const Int32 MV_E_UPG_UNKNOW = unchecked((Int32)0x800004FF);
        #endregion
        #region 来自ISP算法库的错误码
        public const Int32 MV_ALG_OK = unchecked((Int32)0x00000000);
        public const Int32 MV_ALG_ERR = unchecked((Int32)0x10000000);
        public const Int32 MV_ALG_E_ABILITY_ARG = unchecked((Int32)0x10000001);
        public const Int32 MV_ALG_E_MEM_NULL = unchecked((Int32)0x10000002);
        public const Int32 MV_ALG_E_MEM_ALIGN = unchecked((Int32)0x10000003);
        public const Int32 MV_ALG_E_MEM_LACK = unchecked((Int32)0x10000004);
        public const Int32 MV_ALG_E_MEM_SIZE_ALIGN = unchecked((Int32)0x10000005);
        public const Int32 MV_ALG_E_MEM_ADDR_ALIGN = unchecked((Int32)0x10000006);
        public const Int32 MV_ALG_E_IMG_FORMAT = unchecked((Int32)0x10000007);
        public const Int32 MV_ALG_E_IMG_SIZE = unchecked((Int32)0x10000008);
        public const Int32 MV_ALG_E_IMG_STEP = unchecked((Int32)0x10000009);
        public const Int32 MV_ALG_E_IMG_DATA_NULL = unchecked((Int32)0x1000000A);
        public const Int32 MV_ALG_E_CFG_TYPE = unchecked((Int32)0x1000000B);
        public const Int32 MV_ALG_E_CFG_SIZE = unchecked((Int32)0x1000000C);
        public const Int32 MV_ALG_E_PRC_TYPE = unchecked((Int32)0x1000000D);
        public const Int32 MV_ALG_E_PRC_SIZE = unchecked((Int32)0x1000000E);
        public const Int32 MV_ALG_E_FUNC_TYPE = unchecked((Int32)0x1000000F);
        public const Int32 MV_ALG_E_FUNC_SIZE = unchecked((Int32)0x10000010);
        public const Int32 MV_ALG_E_PARAM_INDEX = unchecked((Int32)0x10000011);
        public const Int32 MV_ALG_E_PARAM_VALUE = unchecked((Int32)0x10000012);
        public const Int32 MV_ALG_E_PARAM_NUM = unchecked((Int32)0x10000013);
        public const Int32 MV_ALG_E_NULL_PTR = unchecked((Int32)0x10000014);
        public const Int32 MV_ALG_E_OVER_MAX_MEM = unchecked((Int32)0x10000015);
        public const Int32 MV_ALG_E_CALL_BACK = unchecked((Int32)0x10000016);
        public const Int32 MV_ALG_E_ENCRYPT = unchecked((Int32)0x10000017);
        public const Int32 MV_ALG_E_EXPIRE = unchecked((Int32)0x10000018);
        public const Int32 MV_ALG_E_BAD_ARG = unchecked((Int32)0x10000019);
        public const Int32 MV_ALG_E_DATA_SIZE = unchecked((Int32)0x1000001A);
        public const Int32 MV_ALG_E_STEP = unchecked((Int32)0x1000001B);
        public const Int32 MV_ALG_E_CPUID = unchecked((Int32)0x1000001C);
        public const Int32 MV_ALG_WARNING = unchecked((Int32)0x1000001D);
        public const Int32 MV_ALG_E_TIME_OUT = unchecked((Int32)0x1000001E);
        public const Int32 MV_ALG_E_LIB_VERSION = unchecked((Int32)0x1000001F);
        public const Int32 MV_ALG_E_MODEL_VERSION = unchecked((Int32)0x10000020);
        public const Int32 MV_ALG_E_GPU_MEM_ALLOC = unchecked((Int32)0x10000021);
        public const Int32 MV_ALG_E_FILE_NON_EXIST = unchecked((Int32)0x10000022);
        public const Int32 MV_ALG_E_NONE_STRING = unchecked((Int32)0x10000023);
        public const Int32 MV_ALG_E_IMAGE_CODEC = unchecked((Int32)0x10000024);
        public const Int32 MV_ALG_E_FILE_OPEN = unchecked((Int32)0x10000025);
        public const Int32 MV_ALG_E_FILE_READ = unchecked((Int32)0x10000026);
        public const Int32 MV_ALG_E_FILE_WRITE = unchecked((Int32)0x10000027);
        public const Int32 MV_ALG_E_FILE_READ_SIZE = unchecked((Int32)0x10000028);
        public const Int32 MV_ALG_E_FILE_TYPE = unchecked((Int32)0x10000029);
        public const Int32 MV_ALG_E_MODEL_TYPE = unchecked((Int32)0x1000002A);
        public const Int32 MV_ALG_E_MALLOC_MEM = unchecked((Int32)0x1000002B);
        public const Int32 MV_ALG_E_BIND_CORE_FAILED = unchecked((Int32)0x1000002C);
        public const Int32 MV_ALG_E_DENOISE_NE_IMG_FORMAT = unchecked((Int32)0x10402001);
        public const Int32 MV_ALG_E_DENOISE_NE_FEATURE_TYPE = unchecked((Int32)0x10402002);
        public const Int32 MV_ALG_E_DENOISE_NE_PROFILE_NUM = unchecked((Int32)0x10402003);
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_NUM = unchecked((Int32)0x10402004);
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_VAL = unchecked((Int32)0x10402005);
        public const Int32 MV_ALG_E_DENOISE_NE_BIN_NUM = unchecked((Int32)0x10402006);
        public const Int32 MV_ALG_E_DENOISE_NE_INIT_GAIN = unchecked((Int32)0x10402007);
        public const Int32 MV_ALG_E_DENOISE_NE_NOT_INIT = unchecked((Int32)0x10402008);
        public const Int32 MV_ALG_E_DENOISE_COLOR_MODE = unchecked((Int32)0x10402009);
        public const Int32 MV_ALG_E_DENOISE_ROI_NUM = unchecked((Int32)0x1040200a);
        public const Int32 MV_ALG_E_DENOISE_ROI_ORI_PT = unchecked((Int32)0x1040200b);
        public const Int32 MV_ALG_E_DENOISE_ROI_SIZE = unchecked((Int32)0x1040200c);
        public const Int32 MV_ALG_E_DENOISE_GAIN_NOT_EXIST = unchecked((Int32)0x1040200d);
        public const Int32 MV_ALG_E_DENOISE_GAIN_BEYOND_RANGE = unchecked((Int32)0x1040200e);
        public const Int32 MV_ALG_E_DENOISE_NP_BUF_SIZE = unchecked((Int32)0x1040200f);
        #endregion
        #endregion
        #region ch: 从C/C++接口库导出的函数 | en: C/C++ Load Function
        [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern void OutputDebugString(string message);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumInterfaces")]
        private static extern Int32 MV_CC_EnumInterfaces(UInt32 nTLayerType, ref MV_INTERFACE_INFO_LIST pInterfaceInfoList);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateInterface")]
        private static extern Int32 MV_CC_CreateInterface(ref IntPtr handle, ref MV_INTERFACE_INFO pInterfaceInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateInterfaceByID")]
        private static extern Int32 MV_CC_CreateInterfaceByID(ref IntPtr handle, String pInterfaceID);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenInterface")]
        private static extern Int32 MV_CC_OpenInterface(IntPtr handle, String pConfigFile);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CloseInterface")]
        private static extern Int32 MV_CC_CloseInterface(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DestroyInterface")]
        private static extern Int32 MV_CC_DestroyInterface(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Initialize")]
        private static extern Int32 MV_CC_Initialize();
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Finalize")]
        private static extern Int32 MV_CC_Finalize();
        #region 相机的基本指令和操作函数
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSDKVersion")]
        private static extern UInt32 MV_CC_GetSDKVersion();
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumerateTls")]
        private static extern Int32 MV_CC_EnumerateTls();
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevices")]
        private static extern Int32 MV_CC_EnumDevices(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesEx")]
        private static extern Int32 MV_CC_EnumDevicesEx(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, String pManufacturerName);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesEx2")]
        private static extern Int32 MV_CC_EnumDevicesEx2(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, String pManufacturerName, MV_SORT_METHOD enSortMethod);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceAccessible")]
        private static extern Byte MV_CC_IsDeviceAccessible(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSDKLogPath")]
        private static extern Int32 MV_CC_SetSDKLogPath(String pSDKLogPath);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandle")]
        private static extern Int32 MV_CC_CreateHandle(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleWithoutLog")]
        private static extern Int32 MV_CC_CreateHandleWithoutLog(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DestroyHandle")]
        private static extern Int32 MV_CC_DestroyHandle(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenDevice")]
        private static extern Int32 MV_CC_OpenDevice(IntPtr handle, UInt32 nAccessMode, UInt16 nSwitchoverKey);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CloseDevice")]
        private static extern Int32 MV_CC_CloseDevice(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceConnected")]
        private static extern Byte MV_CC_IsDeviceConnected(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackEx")]
        private static extern Int32 MV_CC_RegisterImageCallBackEx(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForRGB")]
        private static extern Int32 MV_CC_RegisterImageCallBackForRGB(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForBGR")]
        private static extern Int32 MV_CC_RegisterImageCallBackForBGR(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartGrabbing")]
        private static extern Int32 MV_CC_StartGrabbing(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopGrabbing")]
        private static extern Int32 MV_CC_StopGrabbing(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForRGB")]
        private static extern Int32 MV_CC_GetImageForRGB(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForBGR")]
        private static extern Int32 MV_CC_GetImageForBGR(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageBuffer")]
        private static extern Int32 MV_CC_GetImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame, Int32 nMsec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FreeImageBuffer")]
        private static extern Int32 MV_CC_FreeImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameTimeout")]
        private static extern Int32 MV_CC_GetOneFrameTimeout(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ClearImageBuffer")]
        private static extern Int32 MV_CC_ClearImageBuffer(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetValidImageNum")]
        private static extern Int32 MV_CC_GetValidImageNum(IntPtr handle, ref UInt32 pnValidImageNum);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DisplayOneFrame")]
        private static extern Int32 MV_CC_DisplayOneFrame(IntPtr handle, ref MV_DISPLAY_FRAME_INFO pDisplayInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DisplayOneFrameEx")]
        private static extern Int32 MV_CC_DisplayOneFrameEx(IntPtr handle, IntPtr hWnd, ref MV_DISPLAY_FRAME_INFO_EX pDisplayInfoEx);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetImageNodeNum")]
        private static extern Int32 MV_CC_SetImageNodeNum(IntPtr handle, UInt32 nNum);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGrabStrategy")]
        private static extern Int32 MV_CC_SetGrabStrategy(IntPtr handle, MV_GRAB_STRATEGY enGrabStrategy);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetOutputQueueSize")]
        private static extern Int32 MV_CC_SetOutputQueueSize(IntPtr handle, UInt32 nOutputQueueSize);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceInfo")]
        private static extern Int32 MV_CC_GetDeviceInfo(IntPtr handle, ref MV_CC_DEVICE_INFO pstDevInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAllMatchInfo")]
        private static extern Int32 MV_CC_GetAllMatchInfo(IntPtr handle, ref MV_ALL_MATCH_INFO pstInfo);
        #endregion
        #region 设置和获取相机参数的万能接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValueEx")]
        private static extern Int32 MV_CC_GetIntValueEx(IntPtr handle, String strValue, ref MVCC_INTVALUE_EX pIntValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValueEx")]
        private static extern Int32 MV_CC_SetIntValueEx(IntPtr handle, String strValue, Int64 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetEnumValue")]
        private static extern Int32 MV_CC_GetEnumValue(IntPtr handle, String strValue, ref MVCC_ENUMVALUE pEnumValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValue")]
        private static extern Int32 MV_CC_SetEnumValue(IntPtr handle, String strValue, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetEnumEntrySymbolic")]
        private static extern Int32 MV_CC_GetEnumEntrySymbolic(IntPtr handle, string strKey, ref MVCC_ENUMENTRY pstEnumEntry);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValueByString")]
        private static extern Int32 MV_CC_SetEnumValueByString(IntPtr handle, String strValue, String sValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFloatValue")]
        private static extern Int32 MV_CC_GetFloatValue(IntPtr handle, String strValue, ref MVCC_FLOATVALUE pFloatValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFloatValue")]
        private static extern Int32 MV_CC_SetFloatValue(IntPtr handle, String strValue, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBoolValue")]
        private static extern Int32 MV_CC_GetBoolValue(IntPtr handle, String strValue, ref Boolean pBoolValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBoolValue")]
        private static extern Int32 MV_CC_SetBoolValue(IntPtr handle, String strValue, Boolean bValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetStringValue")]
        private static extern Int32 MV_CC_GetStringValue(IntPtr handle, String strKey, ref MVCC_STRINGVALUE pStringValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetStringValue")]
        private static extern Int32 MV_CC_SetStringValue(IntPtr handle, String strKey, String sValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetCommandValue")]
        private static extern Int32 MV_CC_SetCommandValue(IntPtr handle, String strValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InvalidateNodes")]
        private static extern Int32 MV_CC_InvalidateNodes(IntPtr handle);
        #endregion
        #region 设备升级 和 寄存器读写 和异常、事件回调
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LocalUpgrade")]
        private static extern Int32 MV_CC_LocalUpgrade(IntPtr handle, String pFilePathName);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetUpgradeProcess")]
        private static extern Int32 MV_CC_GetUpgradeProcess(IntPtr handle, ref UInt32 pnProcess);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ReadMemory")]
        private static extern Int32 MV_CC_ReadMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_WriteMemory")]
        private static extern Int32 MV_CC_WriteMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterExceptionCallBack")]
        private static extern Int32 MV_CC_RegisterExceptionCallBack(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBack")]
        private static extern Int32 MV_CC_RegisterEventCallBack(IntPtr handle, cbEventdelegate cbEvent, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterAllEventCallBack")]
        private static extern Int32 MV_CC_RegisterAllEventCallBack(IntPtr handle, cbEventdelegateEx cbEvent, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBackEx")]
        private static extern Int32 MV_CC_RegisterEventCallBackEx(IntPtr handle, String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser);
        #endregion
        #region GigEVision 设备独有的接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetEnumDevTimeout")]
        private static extern Int32 MV_GIGE_SetEnumDevTimeout(UInt32 nMilTimeout);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIpEx")]
        private static extern Int32 MV_GIGE_ForceIpEx(IntPtr handle, UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetIpConfig")]
        private static extern Int32 MV_GIGE_SetIpConfig(IntPtr handle, UInt32 nType);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetNetTransMode")]
        private static extern Int32 MV_GIGE_SetNetTransMode(IntPtr handle, UInt32 nType);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetNetTransInfo")]
        private static extern Int32 MV_GIGE_GetNetTransInfo(IntPtr handle, ref MV_NETTRANS_INFO pstInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetDiscoveryMode")]
        private static extern Int32 MV_GIGE_SetDiscoveryMode(UInt32 nMode);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvspTimeout")]
        private static extern Int32 MV_GIGE_SetGvspTimeout(IntPtr handle, UInt32 nMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvspTimeout")]
        private static extern Int32 MV_GIGE_GetGvspTimeout(IntPtr handle, ref UInt32 pMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvcpTimeout")]
        private static extern Int32 MV_GIGE_SetGvcpTimeout(IntPtr handle, UInt32 nMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvcpTimeout")]
        private static extern Int32 MV_GIGE_GetGvcpTimeout(IntPtr handle, ref UInt32 pMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_SetRetryGvcpTimes(IntPtr handle, UInt32 nRetryGvcpTimes);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_GetRetryGvcpTimes(IntPtr handle, ref UInt32 pRetryGvcpTimes);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOptimalPacketSize")]
        private static extern Int32 MV_CC_GetOptimalPacketSize(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResend")]
        private static extern Int32 MV_GIGE_SetResend(IntPtr handle, UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_SetResendMaxRetryTimes(IntPtr handle, UInt32 nRetryTimes);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_GetResendMaxRetryTimes(IntPtr handle, ref UInt32 pnRetryTimes);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendTimeInterval")]
        private static extern Int32 MV_GIGE_SetResendTimeInterval(IntPtr handle, UInt32 nMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendTimeInterval")]
        private static extern Int32 MV_GIGE_GetResendTimeInterval(IntPtr handle, ref UInt32 pnMillisec);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetTransmissionType")]
        private static extern Int32 MV_GIGE_SetTransmissionType(IntPtr handle, ref MV_CC_TRANSMISSION_TYPE pstTransmissionType);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_IssueActionCommand")]
        private static extern Int32 MV_GIGE_IssueActionCommand(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetMulticastStatus")]
        private static extern Int32 MV_GIGE_GetMulticastStatus(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus);
        #endregion
        #region CameraLink独有的接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetDeviceBauderate")]
        private static extern Int32 MV_CAML_SetDeviceBaudrate(IntPtr handle, UInt32 nBaudrate);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetDeviceBauderate")]
        private static extern Int32 MV_CAML_GetDeviceBaudrate(IntPtr handle, ref UInt32 pnCurrentBaudrate);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetSupportBauderates")]
        private static extern Int32 MV_CAML_GetSupportBaudrates(IntPtr handle, ref UInt32 pnBaudrateAblity);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetGenCPTimeOut")]
        private static extern Int32 MV_CAML_SetGenCPTimeOut(IntPtr handle, UInt32 nMillisec);
        #endregion
        #region U3V 设备独有的接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferSize")]
        private static extern Int32 MV_USB_SetTransferSize(IntPtr handle, UInt32 nTransferSize);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferSize")]
        private static extern Int32 MV_USB_GetTransferSize(IntPtr handle, ref UInt32 pTransferSize);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferWays")]
        private static extern Int32 MV_USB_SetTransferWays(IntPtr handle, UInt32 nTransferWays);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferWays")]
        private static extern Int32 MV_USB_GetTransferWays(IntPtr handle, ref UInt32 pTransferWays);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_RegisterStreamExceptionCallBack")]
        private static extern Int32 MV_USB_RegisterStreamExceptionCallBack(IntPtr handle, cbStreamException cbException, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetEventNodeNum")]
        private static extern Int32 MV_USB_SetEventNodeNum(IntPtr handle, UInt32 nEventNodeNum);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetSyncTimeOut")]
        private static extern Int32 MV_USB_SetSyncTimeOut(IntPtr handle, UInt32 nMills);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetSyncTimeOut")]
        private static extern Int32 MV_USB_GetSyncTimeOut(IntPtr handle, ref UInt32 pnMills);
        #endregion
        #region GenTL相关接口，其它接口可以复用（部分接口不支持）
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumInterfacesByGenTL")]
        private static extern Int32 MV_CC_EnumInterfacesByGenTL(ref MV_GENTL_IF_INFO_LIST pstIFInfoList, String sGenTLPath);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesByGenTL")]
        private static extern Int32 MV_CC_EnumDevicesByGenTL(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST pstDevList);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_UnloadGenTLLibrary")]
        private static extern Int32 MV_CC_UnloadGenTLLibrary(String strGenTLPath);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleByGenTL")]
        private static extern Int32 MV_CC_CreateHandleByGenTL(ref IntPtr handle, ref MV_GENTL_DEV_INFO stDevInfo);
        #endregion
        #region XML解析树的生成
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetGenICamXML")]
        private static extern Int32 MV_XML_GetGenICamXML(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeAccessMode")]
        private static extern Int32 MV_XML_GetNodeAccessMode(IntPtr handle, String pstrName, ref MV_XML_AccessMode pAccessMode);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeInterfaceType")]
        private static extern Int32 MV_XML_GetNodeInterfaceType(IntPtr handle, String pstrName, ref MV_XML_InterfaceType pInterfaceType);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetRootNode")]
        private static extern Int32 MV_XML_GetRootNode(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeFeature")]
        private static extern Int32 MV_XML_GetNodeFeature(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_UpdateNodeFeature")]
        private static extern Int32 MV_XML_UpdateNodeFeature(IntPtr handle, MV_XML_InterfaceType enType, IntPtr pstFeature);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_RegisterUpdateCallBack")]
        private static extern Int32 MV_XML_RegisterUpdateCallBack(IntPtr handle, cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser);
        #endregion
        #region 附加接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageEx3")]
        private static extern Int32 MV_CC_SaveImageEx3(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX3 stSaveParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageToFileEx")]
        private static extern Int32 MV_CC_SaveImageToFileEx(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM_EX pstSaveFileParamEx);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SavePointCloudData")]
        private static extern Int32 MV_CC_SavePointCloudData(IntPtr handle, ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RotateImage")]
        private static extern Int32 MV_CC_RotateImage(IntPtr handle, ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FlipImage")]
        private static extern Int32 MV_CC_FlipImage(IntPtr handle, ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ConvertPixelTypeEx")]
        private static extern Int32 MV_CC_ConvertPixelTypeEx(IntPtr handle, ref MV_CC_PIXEL_CONVERT_PARAM_EX pstCvtParamEx);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGammaValue")]
        private static extern Int32 MV_CC_SetGammaValue(IntPtr handle, MvGvspPixelType enSrcPixelType, Single fGammaValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCvtQuality")]
        private static extern Int32 MV_CC_SetBayerCvtQuality(IntPtr handle, UInt32 BayerCvtQuality);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerFilterEnable")]
        private static extern Int32 MV_CC_SetBayerFilterEnable(IntPtr handle, Boolean bFilterEnable);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaParam")]
        private static extern Int32 MV_CC_SetBayerGammaParam(IntPtr handle, ref MV_CC_GAMMA_PARAM pstGammaParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParam")]
        private static extern Int32 MV_CC_SetBayerCCMParam(IntPtr handle, ref MV_CC_CCM_PARAM pstCCMParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParamEx")]
        private static extern Int32 MV_CC_SetBayerCCMParamEx(IntPtr handle, ref MV_CC_CCM_PARAM_EX pstCCMParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageContrast")]
        private static extern Int32 MV_CC_ImageContrast(IntPtr handle, ref MV_CC_CONTRAST_PARAM pstContrastParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_HB_Decode")]
        private static extern Int32 MV_CC_HB_Decode(IntPtr handle, ref MV_CC_HB_DECODE_PARAM pstDecodeParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawRect")]
        private static extern Int32 MV_CC_DrawRect(IntPtr handle, ref MVCC_RECT_INFO pRectInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawCircle")]
        private static extern Int32 MV_CC_DrawCircle(IntPtr handle, ref MVCC_CIRCLE_INFO pCircleInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawLines")]
        private static extern Int32 MV_CC_DrawLines(IntPtr handle, ref MVCC_LINES_INFO pLinesInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureSave")]
        private static extern Int32 MV_CC_FeatureSave(IntPtr handle, String pFileName);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureLoad")]
        private static extern Int32 MV_CC_FeatureLoad(IntPtr handle, String pFileName);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessRead")]
        private static extern Int32 MV_CC_FileAccessRead(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessReadEx")]
        private static extern Int32 MV_CC_FileAccessReadEx(IntPtr handle, ref MV_CC_FILE_ACCESS_EX pstFileAccessEx);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessWrite")]
        private static extern Int32 MV_CC_FileAccessWrite(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessWriteEx")]
        private static extern Int32 MV_CC_FileAccessWriteEx(IntPtr handle, ref MV_CC_FILE_ACCESS_EX pstFileAccessEx);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFileAccessProgress")]
        private static extern Int32 MV_CC_GetFileAccessProgress(IntPtr handle, ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartRecord")]
        private static extern Int32 MV_CC_StartRecord(IntPtr handle, ref MV_CC_RECORD_PARAM pstRecordParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InputOneFrame")]
        private static extern Int32 MV_CC_InputOneFrame(IntPtr handle, ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopRecord")]
        private static extern Int32 MV_CC_StopRecord(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenParamsGUI")]
        private static extern Int32 MV_CC_OpenParamsGUI(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ReconstructImage")]
        private static extern Int32 MV_CC_ReconstructImage(IntPtr handle, ref MV_RECONSTRUCT_IMAGE_PARAM pstReconstructParam);
        #endregion
        #region 弃用的接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageEx2")]
        private static extern Int32 MV_CC_SaveImageEx2(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX2 stSaveParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageToFile")]
        private static extern Int32 MV_CC_SaveImageToFile(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ConvertPixelType")]
        private static extern Int32 MV_CC_ConvertPixelType(IntPtr handle, ref MV_PIXEL_CONVERT_PARAM pstCvtParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrame")]
        private static extern Int32 MV_CC_GetOneFrame(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameEx")]
        private static extern Int32 MV_CC_GetOneFrameEx(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBack")]
        private static extern Int32 MV_CC_RegisterImageCallBack(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImage")]
        private static extern Int32 MV_CC_SaveImage(ref MV_SAVE_IMAGE_PARAM stSaveParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIp")]
        private static extern Int32 MV_GIGE_ForceIp(IntPtr handle, UInt32 nIP);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerNoiseEstimate")]
        private static extern Int32 MV_CC_BayerNoiseEstimate(IntPtr handle, ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerSpatialDenoise")]
        private static extern Int32 MV_CC_BayerSpatialDenoise(IntPtr handle, ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Display")]
        private static extern Int32 MV_CC_Display(IntPtr handle, IntPtr hWnd);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageInfo")]
        private static extern Int32 MV_CC_GetImageInfo(IntPtr handle, ref MV_IMAGE_BASIC_INFO pstInfo);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_GetGevSCPSPacketSize(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_SetGevSCPSPacketSize(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPD")]
        private static extern Int32 MV_GIGE_GetGevSCPD(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPD")]
        private static extern Int32 MV_GIGE_SetGevSCPD(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCDA")]
        private static extern Int32 MV_GIGE_GetGevSCDA(IntPtr handle, ref UInt32 pnIP);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCDA")]
        private static extern Int32 MV_GIGE_SetGevSCDA(IntPtr handle, UInt32 nIP);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCSP")]
        private static extern Int32 MV_GIGE_GetGevSCSP(IntPtr handle, ref UInt32 pnPort);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCSP")]
        private static extern Int32 MV_GIGE_SetGevSCSP(IntPtr handle, UInt32 nPort);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCLUTParam")]
        private static extern Int32 MV_CC_SetBayerCLUTParam(IntPtr handle, ref MV_CC_CLUT_PARAM pstCLUTParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageSharpen")]
        private static extern Int32 MV_CC_ImageSharpen(IntPtr handle, ref MV_CC_SHARPEN_PARAM pstSharpenParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ColorCorrect")]
        private static extern Int32 MV_CC_ColorCorrect(IntPtr handle, ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_NoiseEstimate")]
        private static extern Int32 MV_CC_NoiseEstimate(IntPtr handle, ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SpatialDenoise")]
        private static extern Int32 MV_CC_SpatialDenoise(IntPtr handle, ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCalib")]
        private static extern Int32 MV_CC_LSCCalib(IntPtr handle, ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCorrect")]
        private static extern Int32 MV_CC_LSCCorrect(IntPtr handle, ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTlProxy")]
        private static extern IntPtr MV_CC_GetTlProxy(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_WriteLog")]
        private static extern Int32 MV_CC_WriteLog(string strLog);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValue")]
        private static extern Int32 MV_CC_GetIntValue(IntPtr handle, String strValue, ref MVCC_INTVALUE pIntValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValue")]
        private static extern Int32 MV_CC_SetIntValue(IntPtr handle, String strValue, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetWidth")]
        private static extern Int32 MV_CC_GetWidth(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetWidth")]
        private static extern Int32 MV_CC_SetWidth(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeight")]
        private static extern Int32 MV_CC_GetHeight(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeight")]
        private static extern Int32 MV_CC_SetHeight(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetX")]
        private static extern Int32 MV_CC_GetAOIoffsetX(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetX")]
        private static extern Int32 MV_CC_SetAOIoffsetX(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetY")]
        private static extern Int32 MV_CC_GetAOIoffsetY(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetY")]
        private static extern Int32 MV_CC_SetAOIoffsetY(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_GetAutoExposureTimeLower(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_SetAutoExposureTimeLower(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_GetAutoExposureTimeUpper(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_SetAutoExposureTimeUpper(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBrightness")]
        private static extern Int32 MV_CC_GetBrightness(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBrightness")]
        private static extern Int32 MV_CC_SetBrightness(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFrameRate")]
        private static extern Int32 MV_CC_GetFrameRate(IntPtr handle, ref MVCC_FLOATVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFrameRate")]
        private static extern Int32 MV_CC_SetFrameRate(IntPtr handle, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGain")]
        private static extern Int32 MV_CC_GetGain(IntPtr handle, ref MVCC_FLOATVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGain")]
        private static extern Int32 MV_CC_SetGain(IntPtr handle, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureTime")]
        private static extern Int32 MV_CC_GetExposureTime(IntPtr handle, ref MVCC_FLOATVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureTime")]
        private static extern Int32 MV_CC_SetExposureTime(IntPtr handle, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetPixelFormat")]
        private static extern Int32 MV_CC_GetPixelFormat(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetPixelFormat")]
        private static extern Int32 MV_CC_SetPixelFormat(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionMode")]
        private static extern Int32 MV_CC_GetAcquisitionMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionMode")]
        private static extern Int32 MV_CC_SetAcquisitionMode(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGainMode")]
        private static extern Int32 MV_CC_GetGainMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGainMode")]
        private static extern Int32 MV_CC_SetGainMode(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureAutoMode")]
        private static extern Int32 MV_CC_GetExposureAutoMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureAutoMode")]
        private static extern Int32 MV_CC_SetExposureAutoMode(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerMode")]
        private static extern Int32 MV_CC_GetTriggerMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerMode")]
        private static extern Int32 MV_CC_SetTriggerMode(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerDelay")]
        private static extern Int32 MV_CC_GetTriggerDelay(IntPtr handle, ref MVCC_FLOATVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerDelay")]
        private static extern Int32 MV_CC_SetTriggerDelay(IntPtr handle, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerSource")]
        private static extern Int32 MV_CC_GetTriggerSource(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerSource")]
        private static extern Int32 MV_CC_SetTriggerSource(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_TriggerSoftwareExecute")]
        private static extern Int32 MV_CC_TriggerSoftwareExecute(IntPtr handle);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGammaSelector")]
        private static extern Int32 MV_CC_GetGammaSelector(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGammaSelector")]
        private static extern Int32 MV_CC_SetGammaSelector(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGamma")]
        private static extern Int32 MV_CC_GetGamma(IntPtr handle, ref MVCC_FLOATVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGamma")]
        private static extern Int32 MV_CC_SetGamma(IntPtr handle, Single fValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSharpness")]
        private static extern Int32 MV_CC_GetSharpness(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSharpness")]
        private static extern Int32 MV_CC_SetSharpness(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHue")]
        private static extern Int32 MV_CC_GetHue(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHue")]
        private static extern Int32 MV_CC_SetHue(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSaturation")]
        private static extern Int32 MV_CC_GetSaturation(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSaturation")]
        private static extern Int32 MV_CC_SetSaturation(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_GetBalanceWhiteAuto(IntPtr handle, ref MVCC_ENUMVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_SetBalanceWhiteAuto(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioRed")]
        private static extern Int32 MV_CC_GetBalanceRatioRed(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioRed")]
        private static extern Int32 MV_CC_SetBalanceRatioRed(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioGreen")]
        private static extern Int32 MV_CC_GetBalanceRatioGreen(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioGreen")]
        private static extern Int32 MV_CC_SetBalanceRatioGreen(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioBlue")]
        private static extern Int32 MV_CC_GetBalanceRatioBlue(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioBlue")]
        private static extern Int32 MV_CC_SetBalanceRatioBlue(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceUserID")]
        private static extern Int32 MV_CC_GetDeviceUserID(IntPtr handle, ref MVCC_STRINGVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetDeviceUserID")]
        private static extern Int32 MV_CC_SetDeviceUserID(IntPtr handle, string chValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBurstFrameCount")]
        private static extern Int32 MV_CC_GetBurstFrameCount(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBurstFrameCount")]
        private static extern Int32 MV_CC_SetBurstFrameCount(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionLineRate")]
        private static extern Int32 MV_CC_GetAcquisitionLineRate(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionLineRate")]
        private static extern Int32 MV_CC_SetAcquisitionLineRate(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeartBeatTimeout")]
        private static extern Int32 MV_CC_GetHeartBeatTimeout(IntPtr handle, ref MVCC_INTVALUE pstValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeartBeatTimeout")]
        private static extern Int32 MV_CC_SetHeartBeatTimeout(IntPtr handle, UInt32 nValue);
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaValue")]
        private static extern Int32 MV_CC_SetBayerGammaValue(IntPtr handle, Single fBayerGammaValue);
        internal class MV_PIXEL_FORMAT_MONO8
        {
        }
        #endregion
        #endregion
    }
}
