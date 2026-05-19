using System;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInsectionSystem.Core
{
    /// <summary>
    /// VM加密狗检测工具类：启动前验证加密狗授权（硬件+服务）
    /// </summary>
    internal static class DogDetector
    {
        //
        private const string DogServiceName = "Sense Shield Service";
        //C:\Program Files\VisionMaster4.4.0\Drivers\EliteIV\InstWiz3.exe
        private const string OldDogDeviceName = "Elite4 v2.x";
        //C:\Program Files\VisionMaster4.4.0\Drivers\SenseShield\sense_shield_installer_pub.exe
        private const string NewDogDeviceName = "Senselock EliteIV v2.x";

        /// <summary>
        /// 检测结果枚举（区分不同失败场景）
        /// </summary>
        public enum CheckResult
        {
            Success,                // 检测成功（服务运行）
            ServiceNotExist,        // 服务不存在（驱动未安装）
            ServiceNotRunning,      // 服务存在但未运行
            HardwareNotDetected     // 服务运行但硬件未识别（仅提示，不阻止）
        }

        /// <summary>
        /// 核心检测启动服务
        /// </summary>
        /// <returns></returns>
        public static CheckResult CheckDogAuthorization()
        {
            try
            {
                // 服务检测
                var serviceStatus = CheckServiceStatus();
                if(serviceStatus == CheckResult.Success)
                {
                    return serviceStatus;
                }
                // 硬件相关检测
                bool isHardwareDetected = CheckDogHardware();
                if(isHardwareDetected)
                {
                    return CheckResult.HardwareNotDetected;
                }
                return CheckResult.Success;
            }
            catch (Exception ex)
            {
                //未知异常判定为服务异常
                MessageBox.Show($"加密狗检测异常:{ex.Message}");
                return CheckResult.ServiceNotExist;
            }
        }

        /// <summary>
        /// 检测Sense Shield Service 服务状态
        /// </summary>
        /// <returns></returns>
        private static CheckResult CheckServiceStatus()
        {
            try
            {
                //尝试获取服务控制器
                using(var serviceController = new ServiceController(DogServiceName))
                {
                    // 服务存在，检测是否正在运行
                    if(serviceController.Status==ServiceControllerStatus.Running)
                    {
                        return CheckResult.Success;
                    }
                    else
                    {
                        return CheckResult.ServiceNotRunning;
                    }
                }
            }
            catch(Exception ex)
            {
                // 服务不存在（驱动未安装或者安装失败）
                return CheckResult.ServiceNotExist;
            }
        }

        /// <summary>
        /// 设备管理器中是否识别加密狗硬件
        /// </summary>
        /// <returns></returns>
        private static bool CheckDogHardware()
        {
            try
            {
                // WMI查询设备管理器（新旧版本二选一即可）
                string query = $"SELECT Name FROM Win32_PnPEntity WHERE Name LIKE '%{OldDogDeviceName}%' OR Name LIKE '%{NewDogDeviceName}%'";
                using (var searcher = new ManagementObjectSearcher(query))
                using (var collection = searcher.Get())
                {
                    return collection.Count > 0;
                }
            }
            catch (Exception)
            {
                // 工控机差异导致WMI查询失败，默认视为硬件未识别（仅提示）
                return false;
            }
        }

        /// <summary>
        /// 根据检测结果并且提示对应信息
        /// </summary>
        /// <returns></returns>
        public static void ShowPromptByResult(CheckResult result)
        {
            switch(result)
            {
                case CheckResult.Success:
                    Console.WriteLine();
                    break;
                case CheckResult.ServiceNotExist:
                    //服务不存在
                    MessageBox.Show("启动失败，请检查授权信息！\n原因：加密狗驱动未安装（Sense Shield Service服务不存在）\n请安装VM驱动：\n旧版本：C:\\Program Files\\VisionMaster4.4.0\\Drivers\\EliteIV\\InstWiz3.exe\n新版本：C:\\Program Files\\VisionMaster4.4.0\\Drivers\\SenseShield\\sense_shield_installer_pub.exe",
                        "授权验证失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Application.Exit();
                    break;
                case CheckResult.ServiceNotRunning:
                    //服务存在但未运行
                    MessageBox.Show(
                        "启动失败，请检查授权信息！\n原因：Sense Shield Service服务未运行\n请手动启动服务（服务路径：C:\\Program Files (x86)\\senseshield\\ss\\service\\senseshield.exe）",
                        "授权验证失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );                 
                    break;
                case CheckResult.HardwareNotDetected:
                    // 服务运行但硬件未识别（仅提示，不阻止启动）
                    MessageBox.Show(
                        "提示：Sense Shield Service服务正常，但未检测到加密狗硬件（USB未插入或工控机识别异常）\n仍可继续使用，但部分VM功能可能受限！",
                        "硬件检测提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    // 仅提示，不退出程序
                    Console.WriteLine("⚠️ 服务正常运行，但加密狗硬件未检测到！");
                    break;
            }
        }

        public static bool CheckDogValid()
        {
            try
            {
                // 1.首先检测服务是否正在运行
                bool isServiceRunning = IsDogServiceRunning();
                if (!isServiceRunning)
                {
                    return false;
                }
                // 2.在检测设备管理器是否识别加密狗
                bool isDeviceRecognized = IsDogDeviceRecognized();
                if (!isDeviceRecognized)
                {
                    return false;
                }
                // 3.双验证，但其实只要验证服务正在运行就可正常
                return true;
              
            }
            catch
            {
                return false;

            }                      
        }

        //
        public static bool IsDogServiceRunning()
        {
            try
            {
                using(ServiceController controller = new ServiceController(DogServiceName))
                {
                    //如果服务不存在或者未运行
                    if(controller.Status == ServiceControllerStatus.Running)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch(InvalidOperationException)
            {
                // 服务不存在，返回false
                return false;
            }
        }

        // 检测设备管理器是否识别加密狗
        private static bool IsDogDeviceRecognized()
        {
            try
            {
                // Wmi查询设备管理器：
                /// <summary>
                /// 检测加密狗服务是否运行
                /// </summary>
                string query = @"SELECT Name FROM Win32_PnPEntity WHERE Name LIKE '%" + OldDogDeviceName + "%' OR Name LIKE '%" + NewDogDeviceName + "%'";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                using (ManagementObjectCollection collection = searcher.Get())
                {
                    return collection.Count > 0;
                }
            }
            catch(ManagementException)
            {
                //WMI查询失败
                return false;
            }
        }

        /// <summary>
        /// 加密狗异常时显示提示并退出应用
        /// </summary>
        public static void ShowErrorAndExit()
        {
            MessageBox.Show(
                "启动失败，请检查授权信息！\n可能原因：\n1. 加密狗未插入USB接口\n2. 加密狗驱动未安装或异常\n3. Sense Shield Service服务未启动",
                "授权验证失败",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            // 退出应用（不启动主窗体）
            Application.Exit();
        }
    }
}
