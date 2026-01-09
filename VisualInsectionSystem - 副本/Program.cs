using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInsectionSystem
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 全局异常处理
            Application.ThreadException += (s, e) =>
            {
                HandleException(e.Exception, "Application Thread Exception");
            };

            // 未处理异常
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                HandleException(e.ExceptionObject as Exception, "Unhandled Exception");
            };


            Application.Run(new MainForm());
        }

        //
        private static void HandleException(Exception ex, string exceptionType)
        {        
            try
            {
                string errorMessage = $"异常类型: {exceptionType}\n" +
                                    $"错误消息: {ex.Message}\n" +
                                    $"堆栈跟踪: {ex.StackTrace}\n" +
                                    $"时间: {DateTime.Now}";

                // 记录到日志文件
                File.AppendAllText("error.log", $"{DateTime.Now}: {errorMessage}\n");

                // 显示友好错误提示
                DialogResult dialogResult = MessageBox.Show("发生未知错误，请检查日志文件。", "系统错误",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                // 如果连错误日志都写不成功，就直接弹出消息框
                MessageBox.Show("系统严重错误，无法处理，请重新启动应用","系统错误",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
