using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.Common;
using VisualInsectionSystem.Communication;
using VisualInsectionSystem.Configuration;

namespace VisualInsectionSystem.SubForms
{
    public partial class MESForm : Form
    {
        private MainForm _mainForm;
        private AsyncTcpServer _tcpServer;        
        private string _currentProductCode = "UNKNOWN";
        private MESConfig _config;
        private string _okPath, _ngPath, _origPath, baseDirRes;
        public MESForm()
        {            
            InitializeComponent();
            InitializePath();
            LoadConfig();
            _mainForm = new MainForm();
            _mainForm.InspectionCompleted += OnInspectionCompleted;
            this.FormClosing += (s, e) => SaveConfig();
        }

        private void InitializePath()
        {
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapturedImages");
            _origPath = Path.Combine(baseDir, "Original");
            _okPath = Path.Combine(baseDir, "OK");
            _ngPath = Path.Combine(baseDir, "NG");
            Directory.CreateDirectory(_origPath); 
            Directory.CreateDirectory(_okPath); 
            Directory.CreateDirectory(_ngPath);

            string baseDirRes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Results");
            Directory.CreateDirectory(baseDirRes);
        }
        private void LoadConfig()
        {
            _config = new MESConfig();
            txtServerIP.Text = _config.ServerIP;
            txtServerPort.Text = _config.ServerPort.ToString();

        }

        private void SaveConfig()
        {
            _config.ServerIP = txtServerIP.Text;
            if (int.TryParse(txtServerPort.Text, out int port)) _config.ServerPort = port;
            AppConfigManager.Save("MESConfig.json", _config);
        }

                private async void btnStartMES_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtServerPort.Text, out int port) || port < 1 || port > 65535) { MessageBox.Show("无效端口"); return; }
            try
            {
                _tcpServer = new AsyncTcpServer(port);
                _tcpServer.ClientConnected += (s, ev) => this.InvokeIfRequired(() => UpdateStatus(true, _tcpServer.ConnectedClientsCount));
                _tcpServer.ClientDisconnected += (s, ev) => this.InvokeIfRequired(() => UpdateStatus(_tcpServer.ConnectedClientsCount > 0, _tcpServer.ConnectedClientsCount));
                _tcpServer.DataReceived += (s, ev) => this.InvokeIfRequired(() =>
                {
                    if (ev.Message.StartsWith("PRODUCT:")) _currentProductCode = ev.Message.Substring(8).Trim();
                });
                await _tcpServer.StartAsync();
                //btnStartMES.Enabled = false;
                //btnStopMES.Enabled = true;
                UpdateStatus(true, 0);
                LogHelper.Info($"MES服务已启动，端口:{port}");
            }
            catch (Exception ex) { MessageBox.Show($"启动失败: {ex.Message}"); }
        }

        private void InvokeIfRequired(Action value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(value);
            }
            else
            {
                value();
                throw new NotImplementedException();
            }            
        }

        private async void btnStopMES_Click(object sender, EventArgs e)
        {
            if (_tcpServer != null) { await _tcpServer.StopAsync(); _tcpServer.Dispose(); _tcpServer = null; }
            btnStartMES.Enabled = true;
            btnStopMES.Enabled = false;
            UpdateStatus(false, 0);
        }

        private void UpdateStatus(bool connected, int count)
        {
            lblConnectionStatus.Text = connected ? $"已连接 ({count})" : "未连接";
            lblConnectionStatus.ForeColor = connected ? Color.Green : Color.Red;
        }

        private void OnInspectionCompleted(object sender, MainForm.InspectionCompletedEventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                // 显示结果
                string line = $"[{e.Result.ExecuteTime:HH:mm:ss.fff}] {e.Result.DetectResult.ToUpper()} | X={e.Result.X:F2} Y={e.Result.Y:F2} R={e.Result.R:F2}\r\n";
                
                txtExecutionResult.AppendText(line);
                                
                Task.Run(() =>
                {
                    // 保存图像
                    SaveImage(e.Result, e.TempImagePath);
                    // 保存JSON
                    SaveResJson(e.Result);
                });
                
            });
        }

        private void SaveImage(VMResult result, string tempPath)
        {
            try
            {
                if (string.IsNullOrEmpty(tempPath) || !File.Exists(tempPath)) return;
                string targetDir = result.DetectResult == "ok" ? _okPath : (result.DetectResult == "ng" ? _ngPath : _origPath);
                string productCode = !string.IsNullOrWhiteSpace(_currentProductCode) ? _currentProductCode : "UNKNOWN";
                string fileName = $"{productCode}_{DateTime.Now:yyyyMMddHHmmssfff}_CAM01_{result.DetectResult.ToUpper()}.jpg";
                string destPath = Path.Combine(targetDir, fileName);
                File.Copy(tempPath, destPath, true);
                this.InvokeIfRequired(() => lblImageStorage.Text = $"最新图像: {fileName}");
                LogHelper.Info($"图像已保存: {destPath}");
            }
            catch (Exception ex) { LogHelper.Error("保存图像失败", ex); }
        }

        private void SaveResJson(VMResult result)
        {
            try
            {

                // 构建JSON文件名
                string productCode = !string.IsNullOrWhiteSpace(_currentProductCode) ? _currentProductCode : "UNKNOWN001";
                string fileName = $"{productCode}_{DateTime.Now:yyyyMMddHHmmssfff}_CAM01_{result.DetectResult.ToUpper()}.json";
                string jsonPath = Path.Combine(baseDirRes, fileName);

                //序列化HJSON
                var json = new
                {
                    DetectResult = result.DetectResult,
                    X = result.X,
                    Y = result.Y,
                    R = result.R,
                    productCode = productCode,
                    ExcuteTime = result.ExecuteTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonPath, jsonString, Encoding.UTF8);

                LogHelper.Info($"结果JSON已保存：{jsonPath}");
            }
            catch(Exception ex)
            {
                LogHelper.Error("保存结果JSON失败", ex);
            }
        }
    }
}
