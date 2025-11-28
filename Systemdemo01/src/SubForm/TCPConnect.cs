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
using Systemdemo01;


namespace VisualInsectionSystem.SubForms
{
    /// <summary>
    /// tcp_connect
    /// </summary>
    public partial class TCPConnect : Form
    {
        private Form1 mainForm;                 //主界面应用update_1117

        // 网络通信相关变量
        private TcpListener tcpServer;          // TCP服务端
        private TcpClient tcpClient;            // TCP客户端

        private TcpClient currentClient;        // 当前连接的客户端
        private string currentClientInfo;

        private UdpClient udpServer;
        private UdpClient udpClient;   

        private NetworkStream stream;
        private Thread listenThread;            // 监听线程
        private Thread receiveThread;           // 接收线程

        private bool isListening = false;
        private bool isConnected = false;
        private bool isSending = false;

        private string currentProtocol = "";    // 当前选择的协议类型
        private Dictionary<string, TcpClient> connectedClients = new Dictionary<string, TcpClient>();   // 存储已连接的客户端
        
        private bool isHexSend = false;     // 是否16进制发送
        private bool isHexReceive = false;
        private int repeatInterval = 1000;      // 默认重复发送间隔1秒      


        /// <summary>
        /// 添加构造函数重载，接收主窗体引用update_1117
        /// </summary>
        /// <param name="form"></param>
        public TCPConnect(Form1 form)
        {
            mainForm = form;    //

            InitializeComponent();
            InitControls();
            GetLocalIPAddress();

            // 协议变化事件
            comboBox1.SelectedIndexChanged += (s, e) =>
            {
                currentProtocol = comboBox1.SelectedItem.ToString();
            };

            // 初始化默认TCP服务器端            
            comboBox1.SelectedItem = "TCP服务端";
            UpdateControlStates();
        }
        
        // 协议选择变化时更新控件状态
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }
        // 更新控件状态
        private void UpdateControlStates()
        {
            //currentProtocol = "TCP服务端";
            string protocol = comboBox1.SelectedItem.ToString();
            bool isTcpServer = protocol == "TCP服务端";
            //TCP服务端模式下,不需要目标IP和端口
            comboBox3.Enabled = !isTcpServer;
            textBox2.Enabled = !isTcpServer;

            if (isTcpServer)
            {
                button1.Text = isListening ? "停止监听" : "开始监听";
            }
            else
            {
                button1.Text = isConnected ? "断开" : "连接";
            }
        }

        private void InitControls()
        {
            // 初始化协议类型下拉框
            comboBox1.Items.AddRange(new string[] { "TCP服务端", "TCP客户端", "UDP服务端", "UDP客户端" });
            comboBox1.SelectedIndex = 0;

            // 获取本机IP地址并显示
            comboBox2.Text = GetLocalIPAddress();

            // 设置默认端口号
            textBox1.Text = "8888";
            textBox2.Text = "8888";
            comboBox3.Text = "127.0.0.1";

            // 设置单选按钮文本
            radioButton1.Text = "16进制接收";
            radioButton2.Text = "16进制发送";

            // 添加右键菜单用于保存消息
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("保存接收信息", null, SaveReceivedMessages);
            listBox1.ContextMenuStrip = contextMenu;
        }

        // 获取本机所有IP地址
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
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0;
                }
            }
            return "127.0.0.1";
        }

        // 监听/停止按钮"客户端连接"
        private void Button1_Click(object sender, EventArgs e)
        {
            currentProtocol = comboBox1.SelectedItem.ToString();    // 获取当前选择的协议类型

            if (currentProtocol == "TCP服务端")
            {
                if (!isListening)
                {
                    StartTcpServer();
                }
                else
                {
                    StopTcpServer();
                }
            }

        }
        //启动TCP服务端
        private void StartTcpServer()
        {          
            try
            {
                //获取到的本机IP和port，作为服务器
                if(!ValidateIPAddress(comboBox2.Text))
                {
                    MessageBox.Show("本地IP地址无效");
                }
                if(!ValidatePort(textBox1.Text, out int port))
                {
                    MessageBox.Show("端口号无效");
                    return;
                }

                //IPAddress localAddr = IPAddress.Any;    // 监听所有网络接口
                IPAddress localAddr = IPAddress.Parse(comboBox2.Text);  // 本地IP地址
                port = int.Parse(textBox1.Text);    // 本地端口号
                tcpServer = new TcpListener(localAddr, port);  // 创建TCP服务端
                
                tcpServer.Start();
                isListening = true;
                button1.Text = "停止监听";
                AddMessage($"TCP服务端已启动，监听 {localAddr}:{port}，等待连接...");

                //启动监听线程
                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动TCP服务端失败: {ex.Message}");
            }
        }
        //停止客户端,add:当外部客户端断开时
        private void StopTcpServer()
        {
            isListening = false;
            isConnected = false;
            try
            {
                if (tcpServer != null)
                {
                    tcpServer.Stop();
                }

                //关闭所有已连接客户端
                foreach (var client in connectedClients.Values)
                {
                    client.Close();
                }
                connectedClients.Clear();
                currentClient = null;
                currentClientInfo = "";
                //
                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Abort();
                }
                //
                if(receiveThread != null && receiveThread.IsAlive)
                {
                    receiveThread.Abort();
                }
                button1.Text = "开始监听";
                AddMessage("TCP服务端已停止");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止TCP服务端错误: {ex.Message}");
            }
        }
        //监听client连接,add 持续监听
        private void ListenForClients()
        {
            try
            {
                while (isListening)
                {
                    //等待客户端连接
                    TcpClient client = tcpServer.AcceptTcpClient();                    
                    
                    //成功获取到client消息（远程端点信息，IP和port）
                    IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

                    //转换格式化显示
                    //string clientIP = clientEndPoint.Address.ToString();
                    //string clientPort = clientEndPoint.Port.ToString();
                    string clientEndPointInfoStr = $"{clientEndPoint.AddressFamily}:{clientEndPoint.Port}";
                    clientEndPointInfoStr.ToString();

                    //保存已连接客户端
                    lock (connectedClients)
                    {
                        connectedClients[clientEndPointInfoStr] = client;
                        currentClient = client;
                        currentClientInfo += clientEndPointInfoStr;
                    }                   

                    // 更新UI显示
                    Invoke(new Action(() =>
                    {
                        AddMessage($"客户端已连接: {clientEndPointInfoStr}");
                        // 只将连接的客户端设为活动连接客户端
                        if (tcpClient == null || !tcpClient.Connected)
                        {
                            tcpClient = client;
                            stream = tcpClient.GetStream();
                            isConnected = true;

                            // 启动接收线程
                            if (receiveThread == null || !receiveThread.IsAlive)
                            {
                                receiveThread = new Thread(ReceiveData);
                                receiveThread.IsBackground = true;
                                receiveThread.Start();
                            }
                        }
                    }));
                    ////client断开时，释放之前连接的client信号，持续监听新的client连接
                }
            }
            catch (Exception ex)
            {
                if (isListening)
                {
                    Invoke(new Action(() => AddMessage($"监听错误: {ex.Message}")));
                    // 发生错误后，重新启动监听
                    if(isListening)
                    {
                        Invoke(new Action(() => AddMessage("尝试重新启动监听...")));
                        Thread.Sleep(3000);
                        ListenForClients();     //递归
                    }                    
                }
            }
        }

        //接收客户端数据_add Process
        private void ReceiveData()
        {
            try
            {
                // 循环
                while (isConnected && tcpClient != null && tcpClient.Connected)
                {
                    NetworkStream clientStream = tcpClient.GetStream();
                    byte[] buffer = new byte[4096];
                    int bytesRead = clientStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        //转换接收数据
                        byte[] data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);

                        // 获取发送数据的客户端信息
                        IPEndPoint clientEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
                        string clientInfo = $"{clientEndPoint.Address}:{clientEndPoint.Port}";

                        // data转换为字符串_update1117
                        string receivedStr = Encoding.UTF8.GetString(data).Trim();

                        //显示接收的数据在listBox1内，
                        Invoke(new Action(() =>
                        {
                            AddMessage($"从 {clientInfo} 收到: {GetDisplayText(data)}");
                        }));

                        //处理接收指令函数
                        ProcessCommand(receivedStr, clientEndPoint);
                    }
                    else if (bytesRead == 0)
                    {
                        // 如果客户端断开连接
                        string clientInfo = GetClientInfo(tcpClient);
                        Invoke(new Action(() => AddMessage($"客户端 {clientInfo} 已断开连接!")));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (isConnected && tcpClient != null)
                {
                    string clientInfo = GetClientInfo(tcpClient);
                    Invoke(new Action(() => AddMessage($"与 {clientInfo} 通信错误: {ex.Message}")));
                }
            }
            finally
            {
                // 清理连接的缓存
                CleanupClientConnection(tcpClient);

                // 重新等待新的客户端连接
                ConnectToNextClient();                              
            }           
        }

        // 处理指令函数
        private void ProcessCommand(string command, IPEndPoint clientInfo)
        {
            try
            {
                string result = "";

                //在主线程执行Form1的对应操作
                mainForm.Invoke(new Action(() =>
                {
                    switch (command)
                    {
                        case "A":
                            AddMessage($"收到指令 ‘{command}’，执行流程一");
                            result = mainForm.ExecuteProcedure("Flow1");
                            break;
                        case "B":
                            AddMessage($"收到指令 ‘{command}’，执行流程二");
                            result = mainForm.ExecuteProcedure("Flow2");
                            break;
                        case "C":
                            AddMessage($"收到指令 ‘{command}’，执行流程三");
                            result = mainForm.ExecuteProcedure("Flow3");
                            break;
                        case "START_CONTINUOUS":
                            AddMessage($"收到指令 ‘{command}’，启动连续执行");
                            mainForm.button4_Click(this, EventArgs.Empty);
                            result = "连续执行已启动";
                        break;
                        case "STOP_CONTINUOUS":
                            AddMessage($"收到指令 '{command}', 停止连续执行...");
                            mainForm.button5_Click(this, EventArgs.Empty);
                            result = "连续执行已停止";
                            break;

                        default:
                            AddMessage($"未知指令: {command}");
                            result = $"未知指令: {command}";
                            break;
                    }
                }));

                //
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

        // 发送结果到客户端
        private void SendResultToClient(string result, IPEndPoint clientInfo)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(result);

                // 发送数据到当前连接到的客户端
                if (tcpClient != null && tcpClient.Connected)
                {
                    NetworkStream clientStream = tcpClient.GetStream();
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();

                    Invoke(new Action(() =>
                    {
                        AddMessage($"发送结果到 {clientInfo}: {result}");
                    }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        AddMessage($"发送结果到 {clientInfo}: {result}");
                    }));
                }
            }
            catch(Exception ex)
            {
                Invoke(new Action(() =>
                {
                    AddMessage($"发送结果失败: {ex.Message}");
                }));
            }
        }

        // 发送数据
        private void SendData()
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("请输入要发送的数据");
                return;
            }
            //发送textBox5内数据给连接成功的tcpClient
            try
            {
                byte[] data;
                //根据设置转换发送数据
                if (isHexSend)
                {
                    //16进制发送
                    string hexString = textBox5.Text.Replace(" ", "");
                    data = HexStringToByteArray(hexString);
                }
                else
                {
                    //字符串发送
                    data = Encoding.UTF8.GetBytes(textBox5.Text);
                }
                if (data.Length == 0)
                {
                    AddMessage("发送数据为空");
                    return;
                }

                // 发送数据到当前连接到的客户端,防止中文乱码
                if (tcpClient != null && tcpClient.Connected)
                {
                    NetworkStream clientStream = tcpClient.GetStream();
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();

                    string clientInfo = GetClientInfo(tcpClient);
                    AddMessage($"发送到 {clientInfo}: {GetDisplayText(data)}");
                }
                else
                {
                    AddMessage("客户端已断开连接，无法发送数据");
                    isConnected = false;
                    ConnectToNextClient();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送失败: {ex.Message}");
            }
        }

        // 获取客户端信息字符串
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

        // 清理断开连接的客户端缓存，方便客户端重新连接
        private void CleanupClientConnection(TcpClient client)
        {
            try
            {
                if (client != null)
                {
                    string clientInfo = GetClientInfo(client);
                    lock (connectedClients)
                    {
                        if (connectedClients.ContainsKey(clientInfo))
                        {
                            connectedClients.Remove(clientInfo);
                        }
                    }
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => AddMessage($"清理客户端连接错误: {ex.Message}")));
            }
        }

        // 重新等待连接，确保持续监听新的连接
        private void ConnectToNextClient()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lock (connectedClients)
                    {
                        if (connectedClients.Count > 0)
                        {
                            // 获取第一个客户端
                            var firstClient = connectedClients.First();
                            tcpClient = firstClient.Value;

                            //add if_1120
                            if (tcpClient != null && tcpClient.Connected)
                            {
                                stream = tcpClient.GetStream();     //client断开，异常
                                isConnected = true;
                                AddMessage($"已切换客户端: {firstClient.Key}");

                                // 启动接收线程
                                if (receiveThread == null || !receiveThread.IsAlive)
                                {
                                    receiveThread = new Thread(ReceiveData);
                                    receiveThread.IsBackground = true;
                                    receiveThread.Start();
                                }
                            }
                            else
                            {
                                //连接断开时
                                connectedClients.Remove(firstClient.Key);
                                tcpClient = null;
                                stream = null;
                                isConnected = false;

                                AddMessage("等待新连接的客户端...");
                            }
                        }
                        else
                        {
                            //无客户端时
                            tcpClient = null;
                            stream = null;
                            isConnected = false;

                            AddMessage("等待新连接的客户端...");
                            // 如果没有客户端连接但需要持续监听，并重启
                            if (isListening && (listenThread == null || !listenThread.IsAlive))
                            {
                                listenThread = new Thread(ListenForClients);
                                listenThread.IsBackground = true;
                                listenThread.Start();
                            }
                        }
                    }
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
            if (currentProtocol != "TCP服务端")
            {
                MessageBox.Show("仅TCP服务端模式下可发送数据");
                return;
            }
            if (!isListening)
            {
                MessageBox.Show("请先启动TCP服务端");
                return;
            }
            if (!isConnected || tcpClient == null || !tcpClient.Connected)
            {
                MessageBox.Show("请先建立连接,无法发送数据");
                return;
            }

            string message = textBox5.Text;
            if (string.IsNullOrEmpty(message))
                return;

            SendData();
        }
        
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

        // 16进制字符串转字节数组
        private byte[] HexStringToByteArray(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return new byte[0];

            if (hex.Length % 2 != 0)
            {
                hex += "0";
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        // 字节数组转16进制字符串
        private string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:X2} ", b);
            }
            return sb.ToString().Trim();
        }

        // 根据设置获取显示文本
        private string GetDisplayText(byte[] data)
        {
            if (isHexReceive)
            {
                return ByteArrayToHexString(data);
            }
            else
            {
                try
                {
                    return Encoding.UTF8.GetString(data);
                }
                catch
                {
                    return ByteArrayToHexString(data);
                }
            }
        }

        // 保存接收的消息到文件
        private void SaveReceivedMessages(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("没有消息可保存");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            saveFileDialog.Title = "保存接收的消息";
            saveFileDialog.FileName = $"消息记录_{DateTime.Now:yyyyMMddHHmmss}.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (var item in listBox1.Items)
                        {
                            sw.WriteLine(item.ToString());
                        }
                    }
                    MessageBox.Show("消息已成功保存");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存失败: {ex.Message}");
                }
            }
        }

        // 连接网络
        private bool Connect()
        {
            currentProtocol = comboBox1.SelectedItem.ToString();
            int localPort = 8888;
            try
            {
                switch (currentProtocol)
                {
                    case "TCP服务端":
                        tcpServer = new TcpListener(IPAddress.Parse(comboBox2.Text), localPort);
                        tcpServer.Start();
                        listenThread = new Thread(TcpServerListen);
                        listenThread.IsBackground = true;
                        listenThread.Start();
                        AddMessage("TCP服务端已启动，等待连接...");
                        return true;

                    case "TCP客户端":
                        if (!ValidateIPAddress(comboBox3.Text))
                        {
                            MessageBox.Show("目标IP地址无效");
                            return false;
                        }

                        if (!ValidatePort(textBox2.Text, out int remotePort))
                        {
                            MessageBox.Show("目标端口无效");
                            return false;
                        }

                        tcpClient = new TcpClient();
                        tcpClient.Connect(textBox2.Text, remotePort);
                        listenThread = new Thread(TcpClientReceive);
                        listenThread.IsBackground = true;
                        listenThread.Start();
                        AddMessage("TCP客户端已连接到服务器");
                        return true;

                    case "UDP服务端":
                        udpServer = new UdpClient(localPort);
                        listenThread = new Thread(UdpServerReceive);
                        listenThread.IsBackground = true;
                        listenThread.Start();
                        AddMessage("UDP服务端已启动");
                        return true;

                    case "UDP客户端":
                        if (!ValidateIPAddress(comboBox2.Text))
                        {
                            MessageBox.Show("目标IP地址无效");
                            return false;
                        }

                        if (!ValidatePort(textBox2.Text, out remotePort))
                        {
                            MessageBox.Show("目标端口无效");
                            return false;
                        }

                        udpClient = new UdpClient(localPort);
                        listenThread = new Thread(() => UdpClientReceive(IPAddress.Parse(textBox2.Text), remotePort));
                        listenThread.IsBackground = true;
                        listenThread.Start();
                        AddMessage("UDP客户端已启动");
                        return true;

                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                AddMessage($"连接失败: {ex.Message}");
                return false;
            }
        }

        // 断开连接
        private void Disconnect()
        {
            isSending = false;
            button3.Text = "发送";

            try
            {
                switch (currentProtocol)
                {
                    case "TCP服务端":
                        if (tcpServer != null)
                        {
                            tcpServer.Stop();
                        }
                        break;

                    case "TCP客户端":
                        if (tcpClient != null)
                        {
                            tcpClient.Close();
                        }
                        break;

                    case "UDP服务端":
                        if (udpServer != null)
                        {
                            udpServer.Close();
                        }
                        break;

                    case "UDP客户端":
                        if (udpClient != null)
                        {
                            udpClient.Close();
                        }
                        break;
                }

                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Abort();
                }
            }
            catch (Exception ex)
            {
                AddMessage($"断开连接错误: {ex.Message}");
            }
        }       

        // TCP服务端监听
        private void TcpServerListen()
        {
            try
            {
                while (isConnected)
                {
                    tcpClient = tcpServer.AcceptTcpClient();
                    AddMessage("客户端已连接");

                    // 开始接收数据
                    TcpClientReceive();
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    AddMessage($"TCP服务端错误: {ex.Message}");
                }
            }
        }

        // TCP客户端接收数据
        private void TcpClientReceive()
        {
            try
            {
                if (tcpClient == null) return;

                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while (isConnected && tcpClient.Connected)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        byte[] data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);
                        AddMessage($"收到: {GetDisplayText(data)}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    AddMessage($"TCP接收错误: {ex.Message}");
                }
            }
        }

        // UDP服务端接收数据
        private void UdpServerReceive()
        {
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                while (isConnected)
                {
                    byte[] data = udpServer.Receive(ref remoteEndPoint);
                    // 更新目标IP和端口为发送方的信息
                    textBox2.Text = remoteEndPoint.Address.ToString();
                    textBox1.Text = remoteEndPoint.Port.ToString();

                    AddMessage($"收到来自 {remoteEndPoint}: {GetDisplayText(data)}");
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    AddMessage($"UDP服务端错误: {ex.Message}");
                }
            }
        }

        // UDP客户端接收数据
        private void UdpClientReceive(IPAddress remoteIp, int remotePort)
        {
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(remoteIp, remotePort);

                while (isConnected)
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    AddMessage($"收到来自 {remoteEndPoint}: {GetDisplayText(data)}");
                }
            }
            catch (Exception ex)
            {
                if (isConnected)
                {
                    AddMessage($"UDP客户端错误: {ex.Message}");
                }
            }
        }

        // 16进制接收单选按钮事件
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            isHexReceive = radioButton1.Checked;
        }

        // 16进制发送单选按钮事件
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            isHexSend = radioButton2.Checked;
        }

        private void TCPConnect_Load(object sender, EventArgs e)
        {
            // 绑定单选按钮事件
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
        }

        /// <summary>
        /// 窗体关闭时确保断开连接
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isConnected)
            {
                Disconnect();
            }
            base.OnFormClosing(e);
        }
    }
}




