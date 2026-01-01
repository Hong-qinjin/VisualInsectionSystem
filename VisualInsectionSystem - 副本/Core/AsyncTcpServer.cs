using FrontendUI.Design.Controls;
using Sharp7;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


public class AsyncTcpServer
{
    private readonly int port;
    private TcpListener listener;
    private bool isRunning;     /*server 运行标识*/
    private readonly SynchronizationContext syncContext;  // 用于UI线程同步


    /// <summary>
    /// 线程安全字典管理客户端连接（键：客户端标识（IP:端口），值：TcpClient）
    /// </summary>
    private readonly ConcurrentDictionary<string, TcpClient> clients
        = new ConcurrentDictionary<string, TcpClient>();

    /// <summary>
    /// 心跳检测配置
    /// </summary>
    private readonly TimeSpan heartbeatInterval = TimeSpan.FromSeconds(5);
    private readonly TimeSpan timeoutThreshold = TimeSpan.FromSeconds(5);

    /// <summary>
    /// 编码处理16HEX,接收和发送
    /// </summary>
    public bool IsHexReceive { get; set; } = false;
    public bool IsHexSend { get; set; } = false;

    // 事件定义（供外部UI订阅）
    public event Action<string> OnLog;                      // 日志事件（自动切换到UI线程）
    public event Action<string, string> OnMessageReceived;  // 消息接收事件（客户端ID, 消息）
    public event Action<string> OnClientConnected;          // 客户端连接事件（客户端ID）
    public event Action<string> OnClientDisconnected;       // 客户端断开事件（客户端ID）

    public AsyncTcpServer(int port)
    {
        this.port = port;
        this.syncContext = syncContext ?? SynchronizationContext.Current;
    }

    //启动服务器
    public async Task StartAsync()
    {
        if (isRunning)
        {
            Log("服务器已在运行中");
            return;
        }
        isRunning = true;

        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Log($"TCP server 已启动，监听端口：{port}");

            // 启动客户端接收循环和心跳检测（使用Task.Run避免阻塞）
            _ = AcceptClientLoop();
            _ = HeartbeatCheckLoop();
        }
        catch (Exception ex)
        {
            Log($"启动server fail：{ex.Message}");
            isRunning = false;
            throw;      // 允许外部捕获启动异常
        }
        await Task.CompletedTask;  // match async signature,date:2025-11-28
    }

    // 停止TCP服务器
    public async Task StopASync()
    {
        if (isRunning)
        {
            Log("server is not running ");
            return;
        }
        isRunning = false;
        try
        {
            listener?.Stop();
            // 关闭所有客户端连接
            foreach (var kvp in clients.ToArray())  // 使用KeyValuePair变量
            {
                string clientId = kvp.Key;         // 访问Key
                TcpClient client = kvp.Value;      // 访问Value
                try
                {
                    client.Close(); // 关闭客户端连接
                    Log($"已强制关闭客户端：{clientId}");
                }
                catch (Exception ex)
                {
                    Log($"关闭客户端 {clientId} 失败：{ex.Message}");
                }
            }
            clients.Clear();
            Log("TCP服务端已停止");
        }
        catch (Exception ex)
        {
            Log($"停止服务器异常：{ex.Message}");
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// 循环接收客户端连接
    /// </summary>
    private async Task AcceptClientLoop()
    {
        while (isRunning)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                string clientId = client.Client.RemoteEndPoint.ToString();

                // 添加客户到字典
                if (clients.TryAdd(clientId, client))
                {
                    Log($"client connecting:{clientId}");
                    syncContext.Post(_ => OnClientConnected?.Invoke(clientId), null);

                    // 启动接收消息的循环
                    _ = ReceiveMessageLoop(client, clientId);
                    //_ = Task.Run(() => ReceiveMessageLoop(client, clientId));
                }
                else
                {
                    Log($"客户端添加失败：{clientId}（可能已存在）");
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                if (isRunning)  // 服务器运行中才记录错误
                {
                    Log($"接收连接异常：{ex.Message}");
                    await Task.Delay(1000);  // 避免占用CPU
                }                
            }
        }
    }

    /// <summary>
    /// 循环接收客户端的消息
    /// </summary>
    private async Task ReceiveMessageLoop(TcpClient client, string clientId)
    {
        NetworkStream stream = null;
        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[4096];

            while (client.Connected && isRunning)
            {
                // 异步读取数据（非阻塞）
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    Log($"客户端断开连接：{clientId}");
                    break;
                }

                // 处理接收的数据
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                byte[] receivedData = new byte[bytesRead];
                Array.Copy(buffer, receivedData, bytesRead);

                string message = IsHexReceive
                    ? ByteArrayToHexString(receivedData)
                    : Encoding.UTF8.GetString(receivedData);

                // 在UI线程触发消息接收事件
                syncContext.Post(_ => OnMessageReceived?.Invoke(clientId, message), null);
                Log($"收到 {clientId} 的消息：{message}");

                // 处理心跳回应(如果是PING则回复PONG）                
                if (message.Trim().Equals("PING", StringComparison.OrdinalIgnoreCase))
                {
                    await SendToClientAsync(clientId, "PONG").ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            Log($"接收 {clientId} 消息异常：{ex.Message}");
        }
        finally
        {
            stream?.Dispose();      // 释放
            RemoveClient(clientId); // 清理
        }
    }

    /// <summary>
    /// 心跳检测，自动移除死链接
    /// </summary>
    /// <returns></returns>
    private async Task HeartbeatCheckLoop()
    {
        while (isRunning)
        {
            try
            {
                await Task.Delay(heartbeatInterval).ConfigureAwait(false);

                //bianli
                foreach (var kvp in clients.ToArray())  // 使用KeyValuePair变量
                {
                    string clientId = kvp.Key;
                    TcpClient client = kvp.Value;
                    if (!IsClientConnected(client))
                    {
                        Log($"客户端心跳检测失败（已断开）：{clientId}");
                        RemoveClient(clientId);
                        continue;
                    }

                    // 主动发送心跳包
                    await SendToClientAsync(clientId, "PING").ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Log($"心跳检测异常：{ex.Message}");
            }
        }
    }

    /// <summary>
    /// 向连接的client发送消息
    /// </summary>
    public async Task SendToClientAsync(string key, string msg)
    {
        if (clients.TryGetValue(key, out var client) || !client.Connected)
        {
            Log($"发送失败：客户端 {key} 未连接");
            return;
        }
        try
        {
            // 根据配置转换发送数据（字符串转字节或16进制解析）
            byte[] data = IsHexSend
                ? HexStringToByteArray(msg)
                : Encoding.UTF8.GetBytes(msg);

            await client.GetStream().WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            Log($"发送到 {key} 的消息：{msg}");
        }
        catch (Exception ex)
        {
            Log($"发送到 {key} 失败：{ex.Message}");
            RemoveClient(key);  // 发送失败时移除客户端
        }
    }

    /// <summary>
    /// 广播：向全部客户端发送
    /// <param name="msg"></param>
    public async Task BroadcastAsync(string msg)
    {
        var clientList = clients.ToArray();
        if (clientList.Length == 0)
        {
            Log("广播失败：无连接的客户端");
            return;
        }

        //foreach (var (clientId, client) in clientList)
        foreach (var kvp in clients.ToArray())  // 使用KeyValuePair变量
        {
            string clientId = kvp.Key;
            TcpClient client = kvp.Value;
            if (!IsClientConnected(client))
            {
                Log($"客户端心跳检测失败（已断开）：{clientId}");
                RemoveClient(clientId);
                continue;
                
            }
            await SendToClientAsync(clientId, "PING").ConfigureAwait(false);
        }
        
    }

    /// <summary>
    /// 清理客户端并清理资源
    /// </summary>
    private void RemoveClient(string clientId)
    {
        if (clients.TryRemove(clientId, out var client))
        {
            try
            {
                client.Close(); // 确保关闭客户端
            }
            catch (Exception ex)
            {
                Log($"关闭客户端 {clientId} 异常：{ex.Message}");
            }
            finally
            {
                client.Dispose(); // 释放资源
            }

            Log($"客户端已移除：{clientId}");
            // 在UI线程触发断开事件
            syncContext.Post(_ => OnClientDisconnected?.Invoke(clientId), null);
        }
    }

    /// <summary>
    /// 检查客户端是否处于连接状态
    /// </summary>
    private bool IsClientConnected(TcpClient client)
    {
        try
        {
            var socket = client.Client;
            // 检测套接字状态（Poll方法用于非阻塞检测）
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }
        catch
        { return false; }
    }

    /// <summary>
    /// 字节数组转16进制字符串
    /// </summary>
    private string ByteArrayToHexString(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.AppendFormat("{0:X2} ", b);
        }
        return sb.ToString().Trim();
    }

    /// <summary>
    /// 16进制字符串转字节数组
    /// </summary>
    private byte[] HexStringToByteArray(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            return Array.Empty<byte>();

        // 移除空格并补全偶数长度
        hex = hex.Replace(" ", "");
        if (hex.Length % 2 != 0)
            hex += "0";

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            if (byte.TryParse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b))
            {
                bytes[i / 2] = b;
            }
            else
            {
                throw new ArgumentException($"无效的16进制字符：{hex.Substring(i, 2)}");
            }
        }
        return bytes;
    }

    /// <summary>
    /// 日志输出（自动切换到UI线程）
    /// </summary>
    private void Log(string text)
    {
        string log = $"[{DateTime.Now:HH:mm:ss.fff}] {text}";
        // 通过同步上下文在UI线程触发事件
        syncContext.Post(_ => OnLog?.Invoke(log), null);
    } 
}

