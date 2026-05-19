using NLog;
using System;
using System.IO;
using System.Threading;

namespace VisualInsectionSystem
{
    /// <summary>
    /// 统一日志接口记录，使用Nlog
    /// </summary>
    
    public static class LogHelper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger(); 


        /// <summary>
        /// 写入信息日志
        /// </summary>
        public static void Info(string message)
        {
            Logger.Info(message);
        }
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }
        public static void Error(string message,Exception ex=null)
        {
            Logger.Error(ex, message);
        }
        public static void Fatal(string message, Exception ex = null)
        {
            Logger.Fatal(ex, message);
        }
    }
}