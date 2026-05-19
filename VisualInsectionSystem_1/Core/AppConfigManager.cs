using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using NLog;

namespace VisionInspectionSystem
{
    // 配置管理核心类（静态）
    public static class AppConfigManager
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string _configRootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        private static readonly DataContractJsonSerializerSettings _jsonSettings = new DataContractJsonSerializerSettings
        {
            UseSimpleDictionaryFormat = true,
            DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss"),
            IgnoreExtensionDataObject = true
        };

        // 静态构造函数：创建配置目录
        static AppConfigManager()
        {
            try
            {
                if (!Directory.Exists(_configRootDir))
                {
                    Directory.CreateDirectory(_configRootDir);
                    _logger.Info($"配置目录创建成功：{_configRootDir}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "创建配置目录异常");
            }
        }

        // 加载配置（泛型）
        public static T LoadConfig<T>(string configFileName, T defaultConfig = default) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(configFileName))
                {
                    _logger.Error("配置文件名不能为空");
                    return defaultConfig;
                }

                var configPath = Path.Combine(_configRootDir, configFileName);

                // 文件不存在：创建默认配置并保存
                if (!File.Exists(configPath))
                {
                    _logger.Warn($"配置文件不存在，创建默认配置：{configPath}");
                    SaveConfig(configFileName, defaultConfig ?? Activator.CreateInstance<T>());
                    return defaultConfig ?? Activator.CreateInstance<T>();
                }

                // 读取并反序列化配置
                using (var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T), _jsonSettings);
                    var config = serializer.ReadObject(fs) as T;

                    // 参数校验
                    if (config == null)
                    {
                        _logger.Error("配置反序列化失败，使用默认配置");
                        return defaultConfig ?? Activator.CreateInstance<T>();
                    }

                    _logger.Info($"配置加载成功：{configPath}");
                    return config;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"加载配置[{configFileName}]异常");
                return defaultConfig ?? Activator.CreateInstance<T>();
            }
        }

        // 保存配置（泛型）
        public static bool SaveConfig<T>(string configFileName, T config) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(configFileName) || config == null)
                {
                    _logger.Error("配置文件名/配置对象不能为空");
                    return false;
                }

                var configPath = Path.Combine(_configRootDir, configFileName);

                // 序列化并保存
                using (var fs = new FileStream(configPath, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T), _jsonSettings);
                    serializer.WriteObject(fs, config);
                }

                // 格式化JSON（可选：提升可读性）
                FormatJsonFile(configPath);

                _logger.Info($"配置保存成功：{configPath}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"保存配置[{configFileName}]异常");
                return false;
            }
        }

        // 配置项定义
        #region 配置实体
        // 相机配置
        public class CameraConfig
        {
            public string DefaultSerialNumber { get; set; } = "";
            public int DefaultExposureTime { get; set; } = 1000;
            public float DefaultGain { get; set; } = 1.0f;
            public string DefaultPixelFormat { get; set; } = "Mono8";
            public bool DefaultTriggerMode { get; set; } = false;
            public int CaptureTimeoutMs { get; set; } = 5000;
        }

        // MES配置
        public class MESConfig
        {
            public string TcpIp { get; set; } = "0.0.0.0";
            public int TcpPort { get; set; } = 8888;
            public string ImageSaveRootDir { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapturedImages");
            public bool AutoSaveImage { get; set; } = true;
            public string DefaultProductCode { get; set; } = "";
            public int HeartbeatIntervalMs { get; set; } = 5000;
        }

        // VM配置
        public class VMConfig
        {
            public string DefaultProcedureName { get; set; } = "DefaultProcedure";
            public string ResultSeparator { get; set; } = ",";
            public int ExecuteTimeoutMs { get; set; } = 3000;
        }
        #endregion

        // 私有辅助方法：格式化JSON文件
        private static void FormatJsonFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var formattedJson = FormatJson(json);
                File.WriteAllText(filePath, formattedJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "格式化JSON配置文件失败");
            }
        }

        // 格式化JSON字符串
        private static string FormatJson(string json)
        {
            int indentation = 0;
            int quoteCount = 0;
            var sb = new StringBuilder();

            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '"') quoteCount++;

                if (quoteCount % 2 == 0)
                {
                    switch (c)
                    {
                        case '{':
                        case '[':
                            sb.Append(c);
                            indentation++;
                            sb.AppendLine();
                            sb.Append(new string(' ', indentation * 4));
                            break;
                        case '}':
                        case ']':
                            indentation--;
                            sb.AppendLine();
                            sb.Append(new string(' ', indentation * 4));
                            sb.Append(c);
                            break;
                        case ',':
                            sb.Append(c);
                            sb.AppendLine();
                            sb.Append(new string(' ', indentation * 4));
                            break;
                        case ':':
                            sb.Append(c);
                            sb.Append(' ');
                            break;
                        default:
                            if (!char.IsWhiteSpace(c))
                                sb.Append(c);
                            break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}