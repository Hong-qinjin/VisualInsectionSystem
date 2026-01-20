using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using VMControls.BaseInterface;
using static iMVS_6000PlatformSDKCS.SyncPlatformSDKCS.ImvsSdkPFSync;


namespace VisualInsectionSystem.SubForms
{
    public partial class TCPConnect : Form
    {
        #region 字段定义（命名）
        private readonly MainForm _mainForm;
        private readonly object _lockObj = new object(); // 线程安全锁
        // 网络通信相关变量
        private TcpListener     _tcpServer;             // TCP服务端
        private TcpClient       _tcpClient;             // TCP客户端
        private UdpClient       _udpServer;
        private UdpClient       _udpClient;
        private IPEndPoint      _udpRemoteEndPoint;     // UDP服务端保存的远端端点

        // 线程与取消令牌（替代Abort()）
        private Thread _listenThread;            // 监听线程
        private Thread _receiveThread;           // 接收线程        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        //
        private bool _isListening = false;
        private bool _isConnected = false;
        private bool _isHexSend = false;     // 是否16进制发送
        private bool _isHexReceive = false;        
        // 新增：RadioButton自定义选中状态（用于实现取消选中功能，解耦原生Checked属性）
        private bool _isRadioHexReceiveChecked = false;     // radioButton1（16进制接收）
        private bool _isRadioHexSendChecked = false;        // radioButton2（16进制发送）
        // 
        private string _currentProtocol = "";    // 当前选择的协议类型
        private readonly Dictionary<string, TcpClient> _connectedTcpClients = new Dictionary<string, TcpClient>();
        private TcpClient _activeTcpClient;       // 已连接的客户端     
        #endregion

        #region 构造函数与初始化
        // 构造函数重载
        public TCPConnect(MainForm form)
        {
            _mainForm = form;    //
            InitializeComponent();
            InitControls();
            BindEvents();           
            UpdateControlStates();
        }

        // 协议选择变化时更新控件状态
        private void InitControls()
        {
            // 初始化协议类型下拉框
            comboBox1.Items.AddRange(new string[] { "TCP服务端", "TCP客户端", "UDP服务端", "UDP客户端" });
            comboBox1.SelectedIndex = 0;

            // 本机IP初始化  并显示
            var localIp = GetLocalIPAddress();
            comboBox2.Items.AddRange(GetAllLocalIPs().ToArray());
            comboBox2.Text = localIp;

            // 设置默认端口号
            textBox1.Text = "8888";         //本机
            textBox2.Text = "6666";         //目标
            comboBox3.Text = "127.0.0.1";   //默认目标IP

            // 设置单选按钮文本
            radioButton1.Text = "16进制接收";
            radioButton2.Text = "16进制发送";        

            // 添加右键菜单用于保存消息
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("保存接收信息", null, SaveReceivedMessages);
            listBox1.ContextMenuStrip = contextMenu;
        }

        // 绑定事件（统一管理）
        private void BindEvents()
        {
            comboBox1.SelectedIndexChanged += (s, e) =>
            {
                _currentProtocol = comboBox1.SelectedItem.ToString();
                UpdateControlStates();
            };
            radioButton1.CheckedChanged += (s, e) => _isHexReceive = radioButton1.Checked;
            radioButton2.CheckedChanged += (s, e) => _isHexSend = radioButton2.Checked;
            FormClosing += OnFormClosing;
        }
        #endregion
        #region 控件管理
        private void TCPConnect_Load(object sender, EventArgs e)
        {            
        }

        // 窗体关闭时确保断开连接
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _cts.Cancel();
            if (_isListening) StopServer();
            if (_isConnected) DisconnectFromServer();
        }
        // 更新控件状态
        private void UpdateControlStates()
        {            
            if (string.IsNullOrEmpty(_currentProtocol)) return;             
            bool isServerMode = _currentProtocol == "TCP服务端" || _currentProtocol == "UDP服务端";
            bool isClientMode = _currentProtocol == "TCP客户端" || _currentProtocol == "UDP客户端";

            // 服务端：启用本机IP/端口，禁用目标IP/端口
            comboBox2.Enabled = isServerMode;
            textBox1.Enabled = isServerMode;
            // 客户端：启用目标IP/端口，禁用本机IP/端口
            comboBox3.Enabled = isClientMode;
            textBox2.Enabled = isClientMode;

            // 按钮文本更新
            if (isServerMode)
            {
                button1.Text = _isListening ? "停止监听" : "开始监听";
            }
            else if (isClientMode)
            {
                button1.Text = _isConnected ? "断开" : "连接";
            }

            //button1.Text = isServerMode
            //    ? (_isListening ? "停止监听" : "开始监听")
            //    : (_isConnected ? "断开" : "连接");
        }
        #endregion

        #region 核心操作（监听/连接/断开）
        // 监听/连接按钮点击
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!((Button)sender).Enabled) return;
            ((Button)sender).Enabled = false;
            try
            {
                _currentProtocol = comboBox1.SelectedItem.ToString();    // 获取当前选择的协议类型
                // 服务端模式（TCP/UDP）：监听/停止监听
                if (_currentProtocol == "TCP服务端" || _currentProtocol == "UDP服务端")
                {
                    if (!_isListening)
                    {
                        StartServer();
                    }
                    else
                    {
                        StopServer();
                    }
                }
                // 客户端模式（TCP/UDP）：连接/断开
                else if (_currentProtocol == "TCP客户端" || _currentProtocol == "UDP客户端")
                {
                    if (!_isConnected)
                    {
                        ConnectToServer();
                    }
                    else
                    {
                        DisconnectFromServer();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ((Button)sender).Enabled = true;
            }
        }        

        // 启动服务端(TCP/UDP)
        private void StartServer()
        {
            if (!ValidatePort(textBox1.Text, out int localPort)) //本地端口
            {
                MessageBox.Show("本机端口无效（需1-65535）", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            localPort = int.Parse(textBox1.Text);
            try
            {
                _cts = new CancellationTokenSource();
                if (_currentProtocol=="TCP服务端")
                {
                    //获取到的本机IP，作为服务器
                    if (!ValidateIPAddress(comboBox2.Text))
                    {
                        MessageBox.Show("本机IP地址无效", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    IPAddress localAddr = IPAddress.Parse(comboBox2.Text);
                    _tcpServer = new TcpListener(localAddr, localPort);  // 创建TCP服务端
                    _tcpServer.Start();
                    _isListening = true;
                    AddMessage($"TCP服务端已启动，监听 {localAddr}:{localPort}，等待连接...");

                    // 启动监听线程                   
                    _listenThread = new Thread(() => ListenForTcpClients(_cts.Token))
                    {
                        IsBackground = true
                    };
                    _listenThread.Start();
                }
                else if (_currentProtocol == "UDP服务端")
                {
                    _udpServer = new UdpClient(localPort);
                    _isListening = true;
                    AddMessage($"UDP服务端已启动，监听端口 {localPort}");

                    // 启动UDP监听线程
                    _listenThread = new Thread(() => ListenForUdpData(_cts.Token))
                    {
                        IsBackground = true
                    };
                    _listenThread.Start();
                }
                UpdateControlStates();
            }                 
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务端失败: {ex.Message}");
                _isListening = false;
                UpdateControlStates();
            }
        }

        // 停止服务端（TCP/UDP）,add:当外部客户端断开时
        private void StopServer()
        {
            _cts.Cancel();
            _isListening = false;
            _isConnected = false;
            try
            {
                //清理TCP
                if( _tcpServer != null )
                {
                    _tcpServer.Stop();
                    lock(_lockObj)
                    {
                        foreach (var client in _connectedTcpClients.Values) client.Close();
                        _connectedTcpClients.Clear();
                        _activeTcpClient = null;
                    }
                }
                //清理udp
                if (_udpServer != null)
                {
                    _udpServer.Close();
                    _udpRemoteEndPoint = null;
                }                           
                //
                if (_listenThread != null && _listenThread.IsAlive)
                {
                    //_listenThread.Abort();
                    _listenThread.Join(1000);
                }
                //
                if(_receiveThread != null && _receiveThread.IsAlive)
                {
                  //  _receiveThread.Abort();
                    _receiveThread.Join(1000);
                }
                AddMessage($"{_currentProtocol}已停止");
                UpdateControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止TCP服务端错误: {ex.Message}");
            }
        }

        // 连接到服务端（作为TCP/UDP客户端）
        private void ConnectToServer()
        {
            int remotePort; //  远程port    
            IPAddress remoteIP = IPAddress.Parse(comboBox3.Text);
            // 验证目标IP和端口
            if (!ValidateIPAddress(comboBox3.Text) || !ValidatePort(textBox2.Text, out remotePort))
            {
                MessageBox.Show("目标IP/端口无效", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _cts = new CancellationTokenSource();
                
                if (_currentProtocol == "TCP客户端")
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(remoteIP, remotePort);
                    _isConnected = true;
                    // TCP客户端模式下，将自身的_tcpClient赋值给_activeTcpClient
                    _activeTcpClient = _tcpClient;
                    AddMessage($"TCP客户端已连接到 {remoteIP}:{remotePort}");

                    // 启动TCP接收线程
                    _receiveThread = new Thread(() => ReceiveTcpData(_cts.Token))
                    {
                        IsBackground = true
                    };
                    _receiveThread.Start();
                }
                else if(_currentProtocol =="UDP客户端")
                {
                    _udpClient = new UdpClient();
                    _udpClient.Connect(remoteIP, remotePort);
                    _isConnected = true;
                    AddMessage($"UDP客户端已连接到 {remoteIP}:{remotePort}");

                    // 启动UDP接收线程
                    _receiveThread = new Thread(() => ReceiveUdpData(_cts.Token))
                    {
                        IsBackground = true
                    };
                    _receiveThread.Start();
                }
                UpdateControlStates();                
            }
            catch (Exception ex)
            {
                AddMessage($"连接失败: {ex.Message}");
                _isConnected = false;
                UpdateControlStates();
            }
        }

        // 断开连接（作为TCP/UDP客户端）
        private void DisconnectFromServer()
        {
            _cts.Cancel();
            _isConnected = false;
            _isListening = false;
            try
            {
                if (_currentProtocol == "TCP客户端")
                {
                    _tcpClient?.Close();
                    _activeTcpClient = null;
                }
                else if (_currentProtocol == "UDP客户端")
                {
                    _udpClient?.Close();
                }
                // 线程终止
                if (_receiveThread != null && _receiveThread.IsAlive)
                {
                    _receiveThread.Join(1000);
                }

                AddMessage($"{_currentProtocol}已断开连接");
                UpdateControlStates();
            }
            catch (Exception ex)
            {
                AddMessage($"断开连接异常: {ex.Message}");
            }
        }

        #region 数据监听与接收（持续监听保障）

        // TCP服务端监听客户端连接（持续监听，TCP客户端异常不终止）
        private void ListenForTcpClients(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _isListening)
            {
                /*
                //client断开时，释放之前连接的client信号，持续监听新的client连接
                */
                try
                {
                    if (_tcpServer.Pending()) // 检查是否有挂起的连接
                    {
                        //等待客户端连接
                        TcpClient client = _tcpServer.AcceptTcpClient();
                        // 成功获取到client消息（远程端点信息，IP和port）
                        IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                        // 转换格式化显示
                        string clientIP = clientEndPoint.Address.ToString();
                        string clientPort = clientEndPoint.Port.ToString();
                        string currentClientInfo = $"{clientEndPoint.AddressFamily}:{clientEndPoint.Port}";
                        string clientKey = GetClientKey(client);
                        //
                        lock (_lockObj)
                        {
                            _connectedTcpClients[clientKey] = client;
                            _activeTcpClient = client;
                        }                        
                        Invoke(new Action(() =>
                        {
                            AddMessage($"客户端已连接: {clientKey}");
                            _isConnected = true;

                            // 启动/重启接收线程
                            if (_receiveThread == null || !_receiveThread.IsAlive)
                            {
                                _receiveThread = new Thread(() => ReceiveTcpData(token))
                                {
                                    IsBackground = true
                                };
                                _receiveThread.Start();
                            }
                        }));
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    if (_isListening && !token.IsCancellationRequested)
                    {                       
                        Invoke(new Action(() => AddMessage($"TCP监听异常: {ex.Message}，继续监听...")));
                        Thread.Sleep(1000); // 异常后延迟，避免高频报错
                    }
                }
            }
        }
        
        // UDP服务端监听数据,add 持续监听
        private void ListenForUdpData(CancellationToken token)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (!token.IsCancellationRequested && _isListening)
            {
                try
                {
                    if (_udpServer.Available > 0)
                    {
                        byte[] data = _udpServer.Receive(ref remoteEndPoint);
                        _udpRemoteEndPoint = remoteEndPoint; //保存远程端点通道
                        string dataStr = GetDisplayText(data);
                        Invoke(new Action(() =>
                        {
                            AddMessage($"从 {remoteEndPoint.Address}:{remoteEndPoint.Port} 收到UDP数据: {dataStr}");
                            ProcessCommand(Encoding.UTF8.GetString(data).Trim(), remoteEndPoint); // UDP指令处理
                        }));
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    if (_isListening && !token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => AddMessage($"UDP监听错误: {ex.Message}，继续监听···")));
                        Thread.Sleep(1000);
                    }
                }

            }
        }

        // 接收TCP数据(客户端和服务端通用）_add Process
        private void ReceiveTcpData(CancellationToken token)
        {
            TcpClient targetClient = _currentProtocol == "TCP客户端" ? _tcpClient : _activeTcpClient;

            // 循环
            while (!token.IsCancellationRequested && _isConnected && _activeTcpClient?.Connected == true)
            {
                try
                {
                    NetworkStream stream = _activeTcpClient.GetStream();
                    if (!stream.DataAvailable)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        //byte[] data = new byte[bytesRead];      //转换接收数据
                        //Array.Copy(buffer, data, bytesRead);
                        byte[] data = buffer.Take(bytesRead).ToArray();
                        // 获取发送数据的客户端信息                        
                        IPEndPoint clientEndPoint = (IPEndPoint)_activeTcpClient.Client.RemoteEndPoint;
                        string clientInfo = $"{clientEndPoint.Address}:{clientEndPoint.Port}";

                        string clientKey = GetClientKey(_activeTcpClient);                                              
                        string receivedStr = GetDisplayText(data);

                        //显示接收的数据在listBox1内，
                        Invoke(new Action(() =>
                        {
                            if(_currentProtocol=="TCP客户端")
                            {
                                AddMessage($"从 {clientKey} 收到server数据: {receivedStr}");
                            }

                            // 处理接收指令函数
                            if (_currentProtocol == "TCP服务端")                                
                            {
                                AddMessage($"从 {clientKey} 收到client数据: {receivedStr}");
                                ProcessCommand(Encoding.UTF8.GetString(data).Trim(), clientEndPoint);
                            }
                        }));
                    }
                    else
                    {
                        // 如果客户端断开连接                        
                        Invoke(new Action(() =>
                        {
                            if(_currentProtocol == "TCP客户端")
                            {
                                AddMessage($"客户端 {GetClientKey(_activeTcpClient)} 已断开");
                                _isConnected = false;
                                _tcpClient.Close();
                                UpdateControlStates();
                            }
                            else
                            {
                                AddMessage($"客户端 {GetClientKey(_activeTcpClient)} 已断开");
                                CleanupClientConnection(_activeTcpClient);
                                SwitchToNextClient();
                            }
                        }));
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (_isConnected && !token.IsCancellationRequested)
                    {                        
                        Invoke(new Action(() =>
                        {
                            if(_currentProtocol == "TCP客户端")
                            {
                                AddMessage($"TCP客户端接收异常: {ex.Message}");
                                _isConnected = false;
                                _tcpClient.Close();
                                UpdateControlStates();
                            }
                            else
                            {
                                AddMessage($"TCP接收异常: {ex.Message}");
                                CleanupClientConnection(_activeTcpClient);
                                SwitchToNextClient();
                            }
                        }));
                        break;
                    }
                }
            }
        }

        // 接收UDP数据(客户端）
        private void ReceiveUdpData(CancellationToken token)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (!token.IsCancellationRequested && _isConnected)
            {
                try
                {
                    if(_udpClient.Available > 0)
                    {
                        byte[] data = _udpClient.Receive(ref remoteEndPoint);
                        string receivedStr = GetDisplayText(data);
                        Invoke(new Action(() =>
                        {
                            AddMessage($"从 {remoteEndPoint.Address}:{remoteEndPoint.Port} 收到UDP数据: {receivedStr}");
                        }));
                        Thread.Sleep(100);
                    }                    
                }
                catch (Exception ex)
                {
                    if (_isConnected && !token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => AddMessage($"UDP接收错误: {ex.Message}")));
                        break;
                    }
                }
            }
        }

        #endregion

        // 处理指令函数(作为服务端）
        private void ProcessCommand(string command, IPEndPoint clientInfo)
        {
            try
            {
                string result = "";

                //在主线程执行Form1的对应操作
                _mainForm.Invoke(new Action(() =>
                {
                    switch (command)
                    {
                        case "A":
                            AddMessage($"收到指令(receive command) ‘{command}’，执行流程一(ex 1)");
                            result = _mainForm.ExecuteProcedure("Flow1");
                            break;
                        case "B":
                            AddMessage($"收到指令(receive command) ‘{command}’，执行流程二(ex 2)");
                            result = _mainForm.ExecuteProcedure("Flow2");
                            break;
                        case "C":
                            AddMessage($"收到指令(receive command) ‘{command}’，执行流程三(ex 3)");
                            result = _mainForm.ExecuteProcedure("Flow3");
                            break;
                        case "START_CONTINUOUS":
                            AddMessage($"收到指令(receive command) ‘{command}’，启动连续执行(contious run)");
                            _mainForm.buttonContinuExecute_Click(this, EventArgs.Empty);
                            result = "连续执行已启动(Continous Run has started!)";
                            break;
                        case "STOP_CONTINUOUS":
                            AddMessage($"收到指令(receive command) '{command}', 停止连续执行...");
                            _mainForm.buttonStopExecute_Click(this, EventArgs.Empty);
                            result = "连续执行已停止(Continous Run has stopped!)";
                            break;
                        case "CAPTURE_IMAGE":
                            AddMessage($" 收到指令(receive command) '{command}', 触发拍照");
                            if (_mainForm.HKCameraInstance != null)
                            {
                                _mainForm.HKCameraInstance.TriggerCapture();
                                result = $"拍照已执行";
                            }
                            else
                            { result = $"相机帧异常"; }
                            break;

                        default:
                            AddMessage($"未知指令(uknown command):{command}");
                            // todo：显示指令
                            result = $"未知指令(uknown command): {command}";
                            break;
                    }
                }));
                //// 回复指令执行结果
                SendResultToClient(result, clientInfo);                                
            }
            catch (Exception ex) 
            {
                Invoke(new Action(() =>
                {
                    AddMessage($"处理指令错误: {ex.Message}");
                }));
                SendResultToClient($"处理指令错误: {ex.Message}", clientInfo);               
            }
        }

        // 发送数据（适配TCP/UDP）
        private void SendData()
        {            
            try
            {
                byte[] data = GetSendDataBytes(textBox.Text);
                if (data.Length == 0)
                {
                    AddMessage("发送数据为空");
                    return;
                }                          

                // 发送数据到当前连接到的客户端,防止中文乱码
                if (_currentProtocol.Contains("TCP"))
                {
                    //if (_activeTcpClient?.Connected == true)
                    if (_currentProtocol=="TCP客户端")
                    {
                        if(_tcpClient?.Connected==true)
                        {
                            NetworkStream stream = _tcpClient.GetStream();
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                            AddMessage($"TCP客户端发送到 {comboBox3.Text}:{textBox2.Text}: {GetDisplayText(data)}");
                        }
                        else
                        {
                            AddMessage("Client端已断开连接，无法发送数据");
                            _isConnected = false;
                            UpdateControlStates();
                        }                          
                    }
                    else  //TCP server
                    {
                        if( _activeTcpClient?.Connected==true)
                        {
                            NetworkStream stream = _activeTcpClient.GetStream();
                            stream.Write(data, 0, data.Length);
                            stream.Flush();
                            AddMessage($"发送到 {_activeTcpClient}: {GetDisplayText(data)}");
                        }
                        else
                        {
                            AddMessage("Server端已断开连接，无法发送数据");
                            _isConnected = false;
                            SwitchToNextClient();
                        }
                    }                 
                }
                else if(_currentProtocol.Contains("UDP"))
                {
                    if (_currentProtocol == "UDP客户端" && _udpClient != null)
                    {
                        _udpClient.Send(data, data.Length);
                        AddMessage($"发送UDP数据到 {comboBox3.Text}:{textBox2.Text}: {GetDisplayText(data)}");
                    }
                    else if (_currentProtocol == "UDP服务端" && _udpServer != null && _udpRemoteEndPoint != null)
                    {
                        _udpServer.Send(data, data.Length, _udpRemoteEndPoint);
                        AddMessage($"发送UDP数据到 {_udpRemoteEndPoint.Address}:{_udpRemoteEndPoint.Port}: {GetDisplayText(data)}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送失败: {ex.Message}");
            }
        }

        // 发送结果到客户端
        private void SendResultToClient(string result, IPEndPoint clientEP)
        {
            try
            {
                // 发送数据到当前连接到的客户端
                if (string.IsNullOrEmpty(result)) return;
                byte[] data = Encoding.UTF8.GetBytes(result);
                if (_currentProtocol == "TCP服务端" && _activeTcpClient?.Connected == true)
                {
                    NetworkStream stream = _activeTcpClient.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    Invoke(new Action(() =>
                    {
                        AddMessage($"回复TCP客户端 {clientEP}: {result}");
                    }));
                }
                else if (_currentProtocol == "UDP服务端" && _udpServer != null && _udpRemoteEndPoint != null)
                {
                    _udpServer.Send(data, data.Length, _udpRemoteEndPoint);
                    Invoke(new Action(() => AddMessage($"回复UDP客户端 {clientEP}: {result}")));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    AddMessage($"回复结果失败: {ex.Message}");
                }));
            }
        }
        #endregion
        
        // 清理断开连接的客户端缓存，方便客户端重新连接
        private void CleanupClientConnection(TcpClient client)
        {
            if (client == null) return;

            lock (_lockObj)
            {
                string clientInfo = GetClientKey(client);
                if (_connectedTcpClients.ContainsKey(clientInfo))
                {
                    _connectedTcpClients.Remove(clientInfo);
                }
            }
            try
            {
                client.Close();
            }
            catch (Exception ex) { }           
        }

        // 切换到下一个可用的TCP客户端       
        private void SwitchToNextClient()
        {
            try
            {
                lock (_lockObj)
                {
                    _activeTcpClient = _connectedTcpClients.Values.FirstOrDefault(c => c.Connected);
                }
                Invoke(new Action(() =>
                {
                    if (_activeTcpClient != null)
                    {
                        AddMessage($"切换到客户端: {GetClientKey(_activeTcpClient)}");
                        _isConnected = true;
                        // 重启接收线程
                        if (_receiveThread == null || !_receiveThread.IsAlive)
                        {
                            _receiveThread = new Thread(() => ReceiveTcpData(_cts.Token))
                            {
                                IsBackground = true
                            };
                            _receiveThread.Start();
                        }
                    }
                    else
                    {
                        AddMessage("无可用客户端，等待新连接...");
                        _isConnected = false;
                    }
                    UpdateControlStates();
                }));
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage($"连接到客户端错误: {ex.Message}")));
            }
        }

        // 清空消息按钮点击事件
        private void Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
        // 发送按钮点击事件
        private void Button3_Click(object sender, EventArgs e)
        {          
            try
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("请输入发送数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!_isListening && !_isConnected)
                {
                    MessageBox.Show("请先建立连接/启动监听", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SendData();
            }
            catch (Exception ex)
            {
                // 显示错误信息
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        #region  辅助函数
        //添加消息到列表
        private void AddMessage(string message)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action<string>(AddMessage), message);
                return;
            }

            string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
            listBox1.Items.Add($"[{timeStamp}] {message}");
            listBox1.TopIndex = listBox1.Items.Count - 1; // 滚动到最新消息
        }

        // 获取本机IP地址
        private string GetLocalIPAddress()
        {
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)  //ip4
                {
                    comboBox2.Items.Add(ip.ToString());
                    return ip.ToString();
                }
                //if (ip.AddressFamily == AddressFamily.InterNetworkV6)  //ip6
                //{
                //    comboBox2.Items.Add(ip.ToString());

                //}
                //IPAddress localAddr = IPAddress.Any;    // 监听所有网络接口
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
            return "127.0.0.1";
        }

        // 获取所有本机IPv4
        private List<string> GetAllLocalIPs()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .ToList();
        }

        // 验证IP地址
        private bool ValidateIPAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

        // 验证端口号
        private bool ValidatePort(string portText, out int port)
        {
            if (int.TryParse(portText, out port))
            {
                return port >= 1 && port <= 65535;
            }
            return false;
        }

        // 获取客户端信息
        private string GetClientInfo(TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null && client.Client.RemoteEndPoint != null)
                {
                    IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    return $"{endPoint.Address}:{endPoint.Port}";
                }
                return "未知客户端";
            }
            catch (Exception)
            {
                return "未知客户端";
            }
        }
        // 获取客户端标识（IP:Port）
        private string GetClientKey(TcpClient client)
        {
            if (client?.Client?.RemoteEndPoint == null) return "未知客户端";
            IPEndPoint ep = (IPEndPoint)client.Client.RemoteEndPoint;
            return $"{ep.Address}:{ep.Port}";
        }
        #endregion   


        // 转换发送数据为字节数组（支持16进制/字符串）
        private byte[] GetSendDataBytes(string input)
        {
            if (string.IsNullOrEmpty(input)) return Array.Empty<byte>();

            return _isHexSend
                ? HexStringToByteArray(input.Replace(" ", ""))
                : Encoding.UTF8.GetBytes(input);
        }
        // 16进制字符串转字节数组
        private byte[] HexStringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                hex += "0"; //补位
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        // 字节数组转显示文本（支持16进制/字符串）
        private string GetDisplayText(byte[] data)
        {
            if (data.Length == 0) return "";

            return _isHexReceive
                ? BitConverter.ToString(data).Replace("-", " ")
                : Encoding.UTF8.GetString(data);
        }

        // 保存接收的消息到文件
        private void SaveReceivedMessages(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("无消息可保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                sfd.FileName = $"消息记录_{DateTime.Now:yyyyMMddHHmmss}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllLines(sfd.FileName, listBox1.Items.Cast<string>());
                        MessageBox.Show("消息保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 16进制接收单选按钮事件
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _isHexReceive = radioButton1.Checked;
        }

        // 16进制发送单选按钮事件
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _isHexSend = radioButton2.Checked;
        }
    }
}