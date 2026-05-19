// 文件: Configuration/AppConfigManager.cs
using System;
using System.IO;
using Newtonsoft.Json;

namespace VisualInsectionSystem.Configuration
{
    public static class AppConfigManager
    {
        private static readonly string ConfigDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

        static AppConfigManager() { if (!Directory.Exists(ConfigDir)) Directory.CreateDirectory(ConfigDir); }

        public static T Load<T>(string fileName) where T : new()
        {
            string path = Path.Combine(ConfigDir, fileName);
            try
            {
                if (File.Exists(path))
                {
                    var result = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                    if(result == null)
                    {
                        result = new T();
                    }
                    return new T();
                }                    
                var config = new T();
                Save(fileName, config);
                return config;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"加载配置失败: {fileName}", ex);
                return new T(); 
            }
        }

        public static void Save<T>(string fileName, T config)
        {
            string path = Path.Combine(ConfigDir, fileName);
            try 
            { 
                File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented)); 
            }
            catch (Exception ex) 
            { 
                LogHelper.Error($"保存配置失败: {fileName}", ex); 
            }
        }
    }

    public class MESConfig 
    { 
        public MESConfig()
        {
            ServerIP = "0.0.0.0";
            ServerPort = 5000;
        }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; } 
    }
    public class CameraConfig 
    { 
        public CameraConfig()
        {
            DefaultSerialNumber = "";
            DefaultExposureTime = 10000f;
            DefaultGain = 1.0f;
        }
        public string DefaultSerialNumber { get; set; }
        public float DefaultExposureTime { get; set; }
        public float DefaultGain { get; set; }
    }
}