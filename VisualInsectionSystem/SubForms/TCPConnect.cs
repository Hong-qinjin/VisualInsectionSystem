using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;


namespace VisualInsectionSystem.SubForms
{
    public partial class TCPConnect : Form
    {
        // 网络通信相关变量
        private TcpListener tcpServer;          // TCP服务端
        private TcpClient tcpClient;            // TCP客户端
        private UdpClient udpServer;
        private UdpClient udpClient;
        private Thread listenThread;
        private bool isConnected = false;
        private bool isSending = false;
        private string currentProtocol = "";    // 当前选择的协议类型

        // 配置相关变量
        private bool isHexSend = false;
        private bool isHexReceive = false;
        private int repeatInterval = 1000;      // 默认重复发送间隔1秒

        public TCPConnect()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            // 初始化协议类型下拉框
            comboBox1.Items.AddRange(new string[] { "TCP服务端", "TCP客户端", "UDP服务端", "UDP客户端" });
            comboBox1.SelectedIndex = 0;

            // 获取本机IP地址并显示
            textBox1.Text = GetLocalIPAddress();

            // 设置默认端口号
            textBox2.Text = "8888";
            textBox3.Text = "8888";
            textBox4.Text = "127.0.0.1";

            // 设置单选按钮文本
            radioButton1.Text = "16进制接收";
            radioButton2.Text = "16进制发送";

            // 绑定按钮事件
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            // 添加右键菜单用于保存消息
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("保存接收信息", null, SaveReceivedMessages);
            listBox1.ContextMenuStrip = contextMenu;
        }

        // 获取本机IP地址
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        // 连接/断开按钮点击事件
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                if (Connect())
                {
                    button1.Text = "断开";
                    isConnected = true;
                    AddMessage("连接成功");
                }
            }
            else
            {
                Disconnect();
                button1.Text = "连接";
                isConnected = false;
                AddMessage("已断开连接");
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
            if (!isConnected)
            {
                MessageBox.Show("请先建立连接");
                return;
            }

            // 如果没有在重复发送，则立即发送一次
            if (!isSending)
            {
                SendData();
            }
            else
            {
                // 停止重复发送
                isSending = false;
                button3.Text = "发送";
            }
        }

        // 连接网络
        private bool Connect()
        {
            currentProtocol = comboBox1.SelectedItem.ToString();

            // 验证IP和端口
            if (!ValidateIPAddress(textBox1.Text))
            {
                MessageBox.Show("本机IP地址无效");
                return false;
            }

            if (!ValidatePort(textBox2.Text, out int localPort))
            {
                MessageBox.Show("本地端口无效");
                return false;
            }

            try
            {
                switch (currentProtocol)
                {
                    case "TCP服务端":
                        tcpServer = new TcpListener(IPAddress.Parse(textBox1.Text), localPort);
                        tcpServer.Start();
                        listenThread = new Thread(TcpServerListen);
                        listenThread.IsBackground = true;
                        listenThread.Start();
                        AddMessage("TCP服务端已启动，等待连接...");
                        return true;

                    case "TCP客户端":
                        if (!ValidateIPAddress(textBox4.Text))
                        {
                            MessageBox.Show("目标IP地址无效");
                            return false;
                        }

                        if (!ValidatePort(textBox3.Text, out int remotePort))
                        {
                            MessageBox.Show("目标端口无效");
                            return false;
                        }

                        tcpClient = new TcpClient();
                        tcpClient.Connect(textBox4.Text, remotePort);
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
                        if (!ValidateIPAddress(textBox4.Text))
                        {
                            MessageBox.Show("目标IP地址无效");
                            return false;
                        }

                        if (!ValidatePort(textBox3.Text, out remotePort))
                        {
                            MessageBox.Show("目标端口无效");
                            return false;
                        }

                        udpClient = new UdpClient(localPort);
                        listenThread = new Thread(() => UdpClientReceive(IPAddress.Parse(textBox4.Text), remotePort));
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

        // 发送数据
        private void SendData()
        {
            if (string.IsNullOrEmpty(textBox5.Text) && !isSending)
            {
                MessageBox.Show("请输入要发送的数据");
                return;
            }

            try
            {
                byte[] data;

                // 根据设置转换发送数据
                if (isHexSend)
                {
                    // 16进制发送
                    string hexString = textBox5.Text.Replace(" ", "");
                    data = HexStringToByteArray(hexString);
                }
                else
                {
                    // 字符串发送
                    data = Encoding.UTF8.GetBytes(textBox5.Text);
                }

                if (data.Length == 0)
                {
                    AddMessage("发送数据为空");
                    return;
                }

                // 根据协议类型发送数据
                switch (currentProtocol)
                {
                    case "TCP服务端":
                        if (tcpClient != null && tcpClient.Connected)
                        {
                            NetworkStream stream = tcpClient.GetStream();
                            stream.Write(data, 0, data.Length);
                            AddMessage($"已发送: {GetDisplayText(data)}");
                        }
                        else
                        {
                            AddMessage("未连接到客户端，无法发送数据");
                        }
                        break;

                    case "TCP客户端":
                        if (tcpClient != null && tcpClient.Connected)
                        {
                            NetworkStream stream = tcpClient.GetStream();
                            stream.Write(data, 0, data.Length);
                            AddMessage($"已发送: {GetDisplayText(data)}");
                        }
                        break;

                    case "UDP服务端":
                        if (udpServer != null && !string.IsNullOrEmpty(textBox4.Text) && ValidatePort(textBox3.Text, out int port))
                        {
                            udpServer.Send(data, data.Length, textBox4.Text, port);
                            AddMessage($"已发送: {GetDisplayText(data)}");
                        }
                        break;

                    case "UDP客户端":
                        if (udpClient != null && !string.IsNullOrEmpty(textBox4.Text) && ValidatePort(textBox3.Text, out port))
                        {
                            udpClient.Send(data, data.Length, textBox4.Text, port);
                            AddMessage($"已发送: {GetDisplayText(data)}");
                        }
                        break;
                }

                // 如果需要重复发送，启动定时器
                if (isSending)
                {
                    Task.Delay(repeatInterval).ContinueWith(t =>
                    {
                        if (isSending && isConnected)
                        {
                            SendData();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (Exception ex)
            {
                AddMessage($"发送失败: {ex.Message}");
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
                    textBox4.Text = remoteEndPoint.Address.ToString();
                    textBox3.Text = remoteEndPoint.Port.ToString();

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

        // 添加消息到列表
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

        // 窗体关闭时确保断开连接
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




