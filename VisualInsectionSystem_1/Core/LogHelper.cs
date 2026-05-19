using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInsectionSystem.Core
{
    public static class LogHelper
    {
        private static readonly string _logDir = Path.Combine(Application.StartupPath, "Logs");
        private static readonly object _lockObj = new object();

        static LogHelper()
        {
            //
            if (!Directory.Exists(_logDir))
            {
                Directory.CreateDirectory(_logDir);
            }
        }
        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warn(string message)
        {
            WriteLog("WARN", message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void Error(string message, Exception ex = null)
        {
            string fullMessage = ex == null ? message : $"{message}\r\n异常详情：{ex.ToString()}";
            WriteLog("ERROR", fullMessage);
        }

        /// <summary>
        /// 写入日志到文件
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志内容</param>
        private static void WriteLog(string level, string message)
        {
            lock (_lockObj) // 线程安全写入
            {
                try
                {
                    string logFileName = $"MESLog_{DateTime.Now:yyyyMMdd}.log";
                    string logPath = Path.Combine(_logDir, logFileName);

                    string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}\r\n";
                    File.AppendAllText(logPath, logContent, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    // 日志写入失败时不抛异常，避免影响主流程
                    Console.WriteLine($"日志写入失败：{ex.Message}");
                }
            }
        }



    }
}
