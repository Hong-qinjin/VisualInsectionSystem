using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Systemdemo01
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初始化应用程序目录
            InitializeApplicationPaths();
            
            // 设置异常处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// 初始化应用程序路径
        /// </summary>
        private static void InitializeApplicationPaths()
        {
            try
            {
                // 获取应用程序基目录
                string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
                
                // 确保必要的目录存在
                string[] requiredDirs = { "images", "logs", "config" };
                foreach (string dir in requiredDirs)
                {
                    string fullPath = Path.Combine(appBasePath, dir);
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                }

                // 初始化日志系统
                InitializeLogging(Path.Combine(appBasePath, "logs"));
                
                // 加载语言配置
                LoadLanguageSettings(Path.Combine(appBasePath, "LanguageSet.cfg"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用程序初始化失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        private static void InitializeLogging(string logPath)
        {
            // 这里可以集成 NLog、log4net 等日志框架
            string logFile = Path.Combine(logPath, $"app_{DateTime.Now:yyyyMMdd}.log");
            // 日志初始化代码...
        }

        /// <summary>
        /// 加载语言设置
        /// </summary>
        private static void LoadLanguageSettings(string configPath)
        {
            if (File.Exists(configPath))
            {
                // 加载 LanguageSet.cfg 配置
                // 实现语言配置加载逻辑
            }
        }

        /// <summary>
        /// 全局异常处理
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            string errorMessage = $"未处理的异常: {ex?.Message}\n{ex?.StackTrace}";
            
            // 记录到日志文件
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "error.log");
            File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {errorMessage}\n");
            
            MessageBox.Show("应用程序发生未处理的异常，请查看日志文件。", "错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}