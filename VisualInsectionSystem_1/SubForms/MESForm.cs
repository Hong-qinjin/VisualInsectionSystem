
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VisualInsectionSystem;
using VisualInsectionSystem.Core;

namespace VisualInsectionSystem
{
    public partial class MESForm : Form
    {
        private AsyncTcpServer _tcpServer;
        private readonly string _configPath = Path.Combine(Application.StartupPath, "MESConfig.json");
        private readonly string _rootImagePath = Path.Combine(Application.StartupPath, "CapturedImages");

        private MainForm _mainForm;

        private FileSystemWatcher _imageWatcher;
        private FileSystemWatcher _resultWatcher;
        private string _baseDirectory;
        private string _imageDirectory;
        private string _okDirectory;
        private string _ngDirectory;
        private string _originalDirectory;
        private string _resultDirectory;
        private bool _isServiceRunning;

        public MESForm()
        {
            InitializeComponent();

            // 启动状态连接检查定时器
            timerConnectionCheck.Start();

            // 初始化目录路径
            InitializeDirectories();

            // 加载配置
            LoadConfiguration();

            // 初始化文件监控
            InitializeFileWatchers();

            // 初始化事件订阅
            InitializeEventSubscriptions();
        }
        #region  功能1：MES连接状态显示
        /// <summary>
        /// 更新连接状态UI
        /// </summary>
        /// <param name="status">连接状态：已连接/未连接/异常</param>
        private void UpdateConnectionStatus(string status)
        {
            // 跨线程安全更新UI
            if (lblConnectionStatus.InvokeRequired)
            {
                lblConnectionStatus.Invoke(new Action<string>(UpdateConnectionStatus), status);
                return;
            }

            switch (status.Trim())
            {
                case "已连接":
                    lblConnectionStatus.Text = "已连接";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Green;
                    LogHelper.Info("MES服务连接状态更新：已连接");
                    break;
                case "未连接":
                    lblConnectionStatus.Text = "未连接";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                    LogHelper.Info("MES服务连接状态更新：未连接");
                    break;
                case "异常":
                    lblConnectionStatus.Text = "连接异常";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Orange;
                    LogHelper.Error("MES服务连接状态更新：连接异常");
                    break;
                default:
                    lblConnectionStatus.Text = "未知状态";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Gray;
                    LogHelper.Warn($"MES服务连接状态更新：未知状态-{status}");
                    break;
            }
        }

        /// <summary>
        /// 定时检查连接状态
        /// </summary>
        private void timerConnectionCheck_Tick(object sender, EventArgs e)
        {
            if (_tcpServer == null)
            {
                UpdateConnectionStatus("未连接");
                return;
            }
            try
            {
                // 检查TCP服务器是否处于运行状态
                if (_tcpServer.IsRunning)
                {
                    UpdateConnectionStatus("已连接");
                }
                else
                {
                    UpdateConnectionStatus("未连接");
                }
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus("异常");
                LogHelper.Error($"连接状态检查异常：{ex.Message}", ex);
            }
        }
        #endregion

        #region 功能2：服务器配置输入（IP/端口）
        /// <summary>
        /// 窗体加载时读取配置
        /// </summary>
        private void MESForm_Load(object sender, EventArgs e)
        {
            // 初始化图像存储目录
            InitImageDirectory();
            // 加载配置文件
            LoadMESConfig();
        }

        /// <summary>
        /// 加载MES配置文件
        /// </summary>
        private void LoadMESConfig()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    string configJson = File.ReadAllText(_configPath, Encoding.UTF8);
                    var config = JsonConvert.DeserializeObject<MESConfig>(configJson);
                    if (config != null)
                    {
                        txtServerIP.Text = config.ServerIP;
                        txtServerPort.Text = config.ServerPort.ToString();
                        LogHelper.Info("MES配置文件加载成功");
                    }
                }
                else
                {
                    // 配置文件不存在则保存默认值
                    SaveMESConfig(txtServerIP.Text, int.Parse(txtServerPort.Text));
                    LogHelper.Info("MES配置文件不存在，已创建默认配置");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"加载MES配置异常：{ex.Message}", ex);
                MessageBox.Show("配置文件加载失败，将使用默认值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 保存MES配置文件
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">端口号</param>
        private void SaveMESConfig(string ip, int port)
        {
            try
            {
                var config = new MESConfig
                {
                    ServerIP = ip,
                    ServerPort = port
                };
                string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configPath, configJson, Encoding.UTF8);
                LogHelper.Info($"MES配置保存成功：IP={ip}, Port={port}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"保存MES配置异常：{ex.Message}", ex);
                MessageBox.Show("配置保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// IP输入框文本变更事件（实时保存配置）
        /// </summary>
        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {
            if (ValidateIP(txtServerIP.Text))
            {
                SaveConfigWithValidation();
            }
        }

        /// <summary>
        /// 端口输入框文本变更事件（实时保存配置）
        /// </summary>
        private void txtServerPort_TextChanged(object sender, EventArgs e)
        {
            if (ValidatePort(txtServerPort.Text))
            {
                SaveConfigWithValidation();
            }
        }

        /// <summary>
        /// 验证并保存配置
        /// </summary>
        private void SaveConfigWithValidation()
        {
            string ip = txtServerIP.Text.Trim();
            string portStr = txtServerPort.Text.Trim();

            if (ValidateIP(ip) && ValidatePort(portStr))
            {
                int port = int.Parse(portStr);
                SaveMESConfig(ip, port);
            }
        }

        /// <summary>
        /// 验证IPv4地址格式
        /// </summary>
        /// <param name="ip">待验证IP</param>
        /// <returns>是否有效</returns>
        private bool ValidateIP(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("IP地址不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // IPv4正则表达式
            string ipPattern = @"^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$";
            if (!Regex.IsMatch(ip, ipPattern))
            {
                MessageBox.Show("IP地址格式错误，请输入合法的IPv4地址", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证端口号
        /// </summary>
        /// <param name="portStr">待验证端口字符串</param>
        /// <returns>是否有效</returns>
        private bool ValidatePort(string portStr)
        {
            if (string.IsNullOrWhiteSpace(portStr))
            {
                MessageBox.Show("端口号不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("端口号必须为数字", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (port < 1 || port > 65535)
            {
                MessageBox.Show("端口号必须在1-65535之间", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion

        #region 功能3：MES服务启停控制
        /// <summary>
        /// 启动MES服务按钮点击事件
        /// </summary>
        private void btnStartMES_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证配置
                if (!ValidateIP(txtServerIP.Text) || !ValidatePort(txtServerPort.Text))
                {
                    return;
                }

                string ip = txtServerIP.Text.Trim();
                int port = int.Parse(txtServerPort.Text.Trim());

                // 创建TCP服务器实例
                _tcpServer = new AsyncTcpServer(ip, port);
                // 绑定服务器事件
                _tcpServer.ServerStarted += TcpServer_ServerStarted;
                _tcpServer.ServerStopped += TcpServer_ServerStopped;
                _tcpServer.ServerError += TcpServer_ServerError;

                // 启动服务器
                _tcpServer.StartServer();

                LogHelper.Info($"MES服务启动请求已发送：IP={ip}, Port={port}");
            }
            catch (SocketException ex)
            {
                LogHelper.Error($"启动MES服务失败：端口被占用或权限不足，{ex.Message}", ex);
                MessageBox.Show("启动失败：端口被占用或权限不足", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateConnectionStatus("异常");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"启动MES服务异常：{ex.Message}", ex);
                MessageBox.Show($"启动失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateConnectionStatus("异常");
            }
        }

        /// <summary>
        /// 停止MES服务按钮点击事件
        /// </summary>
        private void btnStopMES_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tcpServer != null && _tcpServer.IsRunning)
                {
                    _tcpServer.StopServer();
                    LogHelper.Info("MES服务停止请求已发送");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"停止MES服务异常：{ex.Message}", ex);
                MessageBox.Show($"停止失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateConnectionStatus("异常");
            }
        }

        /// <summary>
        /// 服务器启动成功事件
        /// </summary>
        private void TcpServer_ServerStarted(object sender, EventArgs e)
        {
            if (btnStartMES.InvokeRequired)
            {
                btnStartMES.Invoke(new Action(() => TcpServer_ServerStarted(sender, e)));
                return;
            }

            btnStartMES.Enabled = false;
            btnStopMES.Enabled = true;
            UpdateConnectionStatus("已连接");
            LogHelper.Info("MES服务启动成功");
        }

        /// <summary>
        /// 服务器停止成功事件
        /// </summary>
        private void TcpServer_ServerStopped(object sender, EventArgs e)
        {
            if (btnStopMES.InvokeRequired)
            {
                btnStopMES.Invoke(new Action(() => TcpServer_ServerStopped(sender, e)));
                return;
            }

            btnStartMES.Enabled = true;
            btnStopMES.Enabled = false;
            UpdateConnectionStatus("未连接");
            LogHelper.Info("MES服务停止成功");
        }

        /// <summary>
        /// 服务器异常事件
        /// </summary>
        private void TcpServer_ServerError(object sender, string errorMsg)
        {
            UpdateConnectionStatus("异常");
            LogHelper.Error($"MES服务异常：{errorMsg}");
            MessageBox.Show($"服务异常：{errorMsg}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region 功能4：Vision Master执行结果显示
        /// <summary>
        /// 解析Vision Master原始结果
        /// </summary>
        /// <param name="rawResult">原始结果字符串</param>
        /// <returns>结构化结果对象</returns>
        public VMResult ParseVMResult(string rawResult)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rawResult))
                {
                    throw new ArgumentNullException(nameof(rawResult), "Vision Master原始结果为空");
                }

                string[] resultParts = rawResult.Split(',');
                if (resultParts.Length < 4)
                {
                    throw new FormatException("Vision Master结果格式错误，无法解析");
                }

                // 解析结果字段
                string detectResult = resultParts[0].Split(':')[1].Trim();
                float x = float.Parse(resultParts[1].Split(':')[1].Trim());
                float y = float.Parse(resultParts[2].Split(':')[1].Trim());
                float r = float.Parse(resultParts[3].Split(':')[1].Trim());

                // 构建结构化结果（产品代码需根据实际业务补充）
                var vmResult = new VMResult
                {
                    DetectResult = detectResult,
                    X = x,
                    Y = y,
                    R = r,
                    ProductCode = "PROD001", // 示例产品代码，需替换为实际获取逻辑
                    ExecuteTime = DateTime.Now
                };

                LogHelper.Info($"Vision Master结果解析成功：{detectResult}, X={x}, Y={y}, R={r}");
                return vmResult;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"解析Vision Master结果异常：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 更新Vision Master执行结果到UI
        /// </summary>
        /// <param name="vmResult">结构化结果对象</param>
        public void UpdateVMResultToUI(VMResult vmResult)
        {
            if (txtExecutionResult.InvokeRequired)
            {
                txtExecutionResult.Invoke(new Action<VMResult>(UpdateVMResultToUI), vmResult);
                return;
            }

            // 格式化结果文本
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"【执行时间】：{vmResult.ExecuteTime:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"【检测结果】：{vmResult.DetectResult}");
            sb.AppendLine($"【坐标信息】：X={vmResult.X:F1}，Y={vmResult.Y:F1}，R={vmResult.R:F1}°");
            sb.AppendLine("------------------------------");

            // 追加到文本框
            txtExecutionResult.AppendText(sb.ToString());
            // 滚动到最新内容
            txtExecutionResult.SelectionStart = txtExecutionResult.Text.Length;
            txtExecutionResult.ScrollToCaret();

            LogHelper.Info("Vision Master结果已更新到UI");

            // 解析结果后触发图像保存（功能5）
            SaveCameraImage(vmResult);
        }
        #endregion

        #region 功能5：工业相机图像帧存储

        private void InitializeDirectories()
        {
            _imageDirectory = Path.Combine(_baseDirectory, "CapturedImages");
            _okDirectory = Path.Combine(_imageDirectory, "OK");
            _ngDirectory = Path.Combine(_imageDirectory, "NG");
            _originalDirectory = Path.Combine(_imageDirectory, "Original");
            _resultDirectory = Path.Combine(_baseDirectory, "ExecutionResults");

            // 创建目录
            CreateDirectoryIfNotExist(_imageDirectory);
            CreateDirectoryIfNotExist(_okDirectory);
            CreateDirectoryIfNotExist(_ngDirectory);
            CreateDirectoryIfNotExist(_originalDirectory);
            CreateDirectoryIfNotExist(_resultDirectory);
        }

        private void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LogHelper.Info($"创建目录: {path}");
            }
        }

        private void InitializeFileWatchers()
        {
            try
            {
                // 图片文件监控
                _imageWatcher = new FileSystemWatcher(_imageDirectory);
                _imageWatcher.Filter = "*.jpg";
                _imageWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
                _imageWatcher.Created += ImageWatcher_Created;
                _imageWatcher.EnableRaisingEvents = true;

                // 结果文件监控
                _resultWatcher = new FileSystemWatcher(_resultDirectory);
                _resultWatcher.Filter = "*.json";
                _resultWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
                _resultWatcher.Created += ResultWatcher_Created;
                _resultWatcher.EnableRaisingEvents = true;

                LogHelper.Info("文件监控器初始化成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"初始化文件监控器失败: {ex.Message}");
            }
        }

        private void InitializeEventSubscriptions()
        {
            // 订阅MainForm的执行完成事件
            if (_mainForm != null)
            {
                _mainForm.ProcedureExecuted += MainForm_ProcedureExecuted;
                _mainForm.ImageCaptured += MainForm_ImageCaptured;
            }
        }

        private void MainForm_ProcedureExecuted(object sender, ProcedureExecutedEventArgs e)
        {
            UpdateExecutionResultDisplay(e.ProcedureName, e.ResultData, e.IsSuccess);
        }

        private void MainForm_ImageCaptured(object sender, ImageCapturedEventArgs e)
        {
            UpdateImageStorageDisplay(e.FileName, e.DetectionResult);
        }

        private void LoadConfiguration()
        {
            try
            {
                string configPath = Path.Combine(_baseDirectory, "Config", "MESConfig.json");
                if (File.Exists(configPath))
                {
                    var config = JsonConvert.DeserializeObject<MESConfig>(File.ReadAllText(configPath));
                    if (config != null)
                    {
                        txtServerIP.Text = config.ServerIP;
                        txtServerPort.Text = config.ServerPort.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"加载MES配置失败: {ex.Message}");
                // 使用默认值
                txtServerIP.Text = "0.0.0.0";
                txtServerPort.Text = "5000";
            }
        }

        private void SaveConfiguration()
        {
            try
            {
                var config = new MESConfig
                {
                    ServerIP = txtServerIP.Text.Trim(),
                    ServerPort = int.TryParse(txtServerPort.Text.Trim(), out int port) ? port : 5000
                };

                string configDir = Path.Combine(_baseDirectory, "Config");
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                string configPath = Path.Combine(configDir, "MESConfig.json");
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                LogHelper.Info("MES配置已保存");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"保存MES配置失败: {ex.Message}");
            }
        }
        #endregion
    }
}