using System;
using System.IO;
using System.Threading;

namespace VisualInsectionSystem
{
    public static class LogHelper
    {
        private static readonly string _logDirectory;
        private static readonly object _lockObj = new object();

        static LogHelper()
        {
            // 初始化日志目录
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// <summary>
        /// 写入信息日志
        /// </summary>
        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        public static void Error(string message, Exception ex = null)
        {
            var logMessage = ex != null
                ? $"{message}\n异常信息: {ex.Message}\n堆栈跟踪: {ex.StackTrace}"
                : message;
            WriteLog("ERROR", logMessage);
        }

        /// <summary>
        /// 写入调试日志
        /// </summary>
        public static void Debug(string message)
        {
#if DEBUG
            WriteLog("DEBUG", message);
#endif
        }

        /// <summary>
        /// 实际写入日志的方法
        /// </summary>
        private static void WriteLog(string level, string message)
        {
            lock (_lockObj)
            {
                try
                {
                    var fileName = Path.Combine(_logDirectory, $"Log_{DateTime.Now:yyyyMMdd}.txt");
                    var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] [{Thread.CurrentThread.ManagedThreadId}] {message}\n";

                    File.AppendAllText(fileName, logLine);
                }
                catch (Exception ex)
                {
                    // 日志写入失败时，无法再记录日志，只能输出到控制台
                    Console.WriteLine($"日志写入失败: {ex.Message}");
                }
            }
        }
    }
}