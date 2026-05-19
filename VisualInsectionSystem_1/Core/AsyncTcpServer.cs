using NLog;
using Sharp7;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualInsectionSystem.Core;

namespace VisionInspectionSystem
{
    /// <summary>
    /// 异步TCP服务器类
    /// </summary>
    public class AsyncTcpServer : IDisposable
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private TcpListener _tcpListener;
        private CancellationTokenSource _cts;
        private readonly List<TcpClient> _connectedClients = new List<TcpClient>();
        private readonly object _clientLock = new object();
        private readonly string _messageTerminator = "\n"; // 消息结束符（解决粘包）
        private readonly Encoding _encoding = Encoding.UTF8;

        // 状态属性
        private Thread _listenThread;
        // 状态属性
        public bool IsRunning { get; private set; }
        public int ConnectedClientCount => _connectedClients.Count;

        // 配置参数
        public int Port { get; set; }
        public string IpAddress { get; set; } = "0.0.0.0";
        public int HeartbeatIntervalMs { get; set; } = 5000; // 心跳间隔
        public int ReceiveTimeoutMs { get; set; } = 3000; // 接收超时
        public int MaxClients { get; set; } = 10; // 最大客户端数

        // 事件定义
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ServerErrorEventArgs> ServerErrorOccurred;

        /// <summary>
        /// 服务器启动异步
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                if (IsRunning)
                {
                    LogHelper.Warn("TCP服务已启动，无需重复启动");
                    return;
                }
                // 参数校验
                if (Port < 1 || Port > 65535)
                {
                    OnServerError("StartAsync", -1, "端口号无效（1-65535）");
                    return;
                }

                _cts = new CancellationTokenSource();
                var ip = string.IsNullOrEmpty(IpAddress) ? IPAddress.Any : IPAddress.Parse(IpAddress);
                _tcpListener = new TcpListener(ip, Port);
                _tcpListener.Start();

                IsRunning = true;
                LogHelper.Info($"TCP服务启动成功：{{IpAddress}}:{{Port}}");

                // 启动客户端监听循环
                await Task.Run(async () =>
                {
                    while (!_cts.Token.IsCancellationRequested && IsRunning)
                    {
                        try
                        {
                            // 限制最大客户端数
                            if (ConnectedClientCount >= MaxClients)
                            {
                                await Task.Delay(100, _cts.Token);
                                continue;
                            }

                            // 异步接受客户端连接
                            var client = await _tcpListener.AcceptTcpClientAsync();
                            lock (_clientLock)
                            {
                                _connectedClients.Add(client);
                            }

                            // 配置客户端
                            client.ReceiveTimeout = ReceiveTimeoutMs;
                            client.SendTimeout = ReceiveTimeoutMs;

                            var clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                            _logger.Info($"客户端连接成功：{clientEndPoint.Address}:{clientEndPoint.Port}");

                            // 触发连接事件
                            ClientConnected?.Invoke(this, new ClientConnectedEventArgs
                            {
                                ClientIp = clientEndPoint.Address.ToString(),
                                ClientPort = clientEndPoint.Port,
                                ConnectTime = DateTime.Now
                            });

                            // 启动客户端处理任务
                            _ = HandleClientAsync(client, _cts.Token);
                            // 启动心跳检测
                            _ = HeartbeatClientAsync(client, _cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            // 服务停止时的取消异常，无需处理
                        }
                        catch (Exception ex)
                        {
                            OnServerError("StartAsync_AcceptLoop", -2, ex.Message);
                        }
                    }
                }, _cts.Token);
            }
            catch (Exception ex)
            {
                IsRunning = false;
                _logger.Error(ex, "启动TCP服务异常");
                OnServerError("StartAsync", -1, ex.Message);
            }
        }

        // 停止服务（异步）
        public async Task StopAsync()
        {
            try
            {
                if (!IsRunning)
                {
                    _logger.Warn("TCP服务未启动，无需停止");
                    return;
                }

                IsRunning = false;
                _cts?.Cancel();

                // 关闭所有客户端连接
                lock (_clientLock)
                {
                    foreach (var client in _connectedClients)
                    {
                        try
                        {
                            if (client.Connected)
                            {
                                client.GetStream().Close();
                            }
                            client.Close();
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn(ex, $"关闭客户端连接异常：{(IPEndPoint)client.Client.RemoteEndPoint}");
                        }
                    }
                    _connectedClients.Clear();
                }

                // 停止监听
                _tcpListener?.Stop();
                _cts?.Dispose();

                await Task.Delay(100); // 等待资源释放
                _logger.Info("TCP服务已停止");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "停止TCP服务异常");
                OnServerError("StopAsync", -1, ex.Message);
            }
        }

        // 发送数据到指定客户端
        public async Task<bool> SendDataAsync(string clientIp, int clientPort, string data)
        {
            return await SendDataAsync(clientIp, clientPort, _encoding.GetBytes(data));
        }

        // 发送数据到指定客户端（字节数组）
        public async Task<bool> SendDataAsync(string clientIp, int clientPort, byte[] data)
        {
            try
            {
                if (!IsRunning || data == null || data.Length == 0)
                    return false;

                TcpClient targetClient = null;
                lock (_clientLock)
                {
                    targetClient = _connectedClients.Find(c =>
                    {
                        var ep = (IPEndPoint)c.Client.RemoteEndPoint;
                        return ep.Address.ToString() == clientIp && ep.Port == clientPort;
                    });
                }

                if (targetClient == null || !targetClient.Connected)
                {
                    OnServerError("SendDataAsync", -3, $"客户端未连接：{clientIp}:{clientPort}");
                    return false;
                }

                // 发送数据（添加结束符）
                var sendData = _encoding.GetBytes(_encoding.GetString(data) + _messageTerminator);
                await targetClient.GetStream().WriteAsync(sendData, 0, sendData.Length);
                _logger.Debug($"发送数据到客户端[{clientIp}:{clientPort}]：{_encoding.GetString(data)}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "发送数据异常");
                OnServerError("SendDataAsync", -1, ex.Message);
                return false;
            }
        }

        // 广播数据到所有客户端
        public async Task BroadcastDataAsync(string data)
        {
            await BroadcastDataAsync(_encoding.GetBytes(data));
        }

        // 广播数据到所有客户端（字节数组）
        public async Task BroadcastDataAsync(byte[] data)
        {
            try
            {
                if (!IsRunning || data == null || data.Length == 0)
                    return;

                var clients = new List<TcpClient>();
                lock (_clientLock)
                {
                    clients.AddRange(_connectedClients);
                }

                foreach (var client in clients)
                {
                    if (!client.Connected) continue;

                    var ep = (IPEndPoint)client.Client.RemoteEndPoint;
                    await SendDataAsync(ep.Address.ToString(), ep.Port, data);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "广播数据异常");
                OnServerError("BroadcastDataAsync", -1, ex.Message);
            }
        }

        // 处理客户端通信
        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            var clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            var clientIp = clientEndPoint.Address.ToString();
            var clientPort = clientEndPoint.Port;
            var buffer = new byte[4096];
            var receiveBuffer = new StringBuilder();

            try
            {
                using (var stream = client.GetStream())
                {
                    while (!token.IsCancellationRequested && client.Connected)
                    {
                        // 异步读取数据
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (bytesRead == 0)
                        {
                            _logger.Info($"客户端断开连接（无数据）：{clientIp}:{clientPort}");
                            break;
                        }

                        // 解码数据并拼接缓冲区
                        var data = _encoding.GetString(buffer, 0, bytesRead);
                        receiveBuffer.Append(data);

                        // 处理粘包：按结束符分割完整消息
                        var rawData = receiveBuffer.ToString();
                        var messageEndIndex = rawData.IndexOf(_messageTerminator);
                        while (messageEndIndex >= 0)
                        {
                            // 提取完整消息
                            var completeMessage = rawData.Substring(0, messageEndIndex).Trim();
                            rawData = rawData.Substring(messageEndIndex + _messageTerminator.Length);
                            receiveBuffer.Clear();
                            receiveBuffer.Append(rawData);

                            // 触发数据接收事件
                            DataReceived?.Invoke(this, new DataReceivedEventArgs
                            {
                                ClientIp = clientIp,
                                ClientPort = clientPort,
                                Data = completeMessage,
                                ReceiveTime = DateTime.Now
                            });

                            _logger.Debug($"接收客户端数据：{clientIp}:{clientPort} -> {completeMessage}");

                            // 检查下一个完整消息
                            messageEndIndex = rawData.IndexOf(_messageTerminator);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 取消操作，正常退出
            }
            catch (Exception ex)
            {
                OnServerError("HandleClientAsync", -4, $"客户端[{clientIp}:{clientPort}]通信异常：{ex.Message}");
            }
            finally
            {
                // 移除并关闭客户端
                lock (_clientLock)
                {
                    _connectedClients.Remove(client);
                }

                try
                {
                    client.Close();
                }
                catch { }

                // 触发断开事件
                ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs
                {
                    ClientIp = clientIp,
                    ClientPort = clientPort,
                    DisconnectTime = DateTime.Now,
                    Reason = "正常断开/异常断开"
                });

                _logger.Info($"客户端已断开：{clientIp}:{clientPort}，当前连接数：{ConnectedClientCount}");
            }
        }

        // 客户端心跳检测
        private async Task HeartbeatClientAsync(TcpClient client, CancellationToken token)
        {
            var clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            var clientIp = clientEndPoint.Address.ToString();
            var clientPort = clientEndPoint.Port;

            try
            {
                while (!token.IsCancellationRequested && client.Connected)
                {
                    await Task.Delay(HeartbeatIntervalMs, token);

                    // 发送心跳包（空数据/特定标识）
                    try
                    {
                        var heartbeatData = _encoding.GetBytes($"HEARTBEAT|{DateTime.Now:yyyyMMddHHmmss}{_messageTerminator}");
                        await client.GetStream().WriteAsync(heartbeatData, 0, heartbeatData.Length, token);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn($"客户端[{clientIp}:{clientPort}]心跳发送失败：{ex.Message}");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                _logger.Error(ex, $"客户端[{clientIp}:{clientPort}]心跳检测异常");
            }
        }

        // 资源释放
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 停止服务
                if (IsRunning)
                {
                    _ = StopAsync().WaitAsync(TimeSpan.FromSeconds(5));
                }

                // 释放托管资源
                _cts?.Dispose();
                _tcpListener?.Stop();

                // 清理客户端
                lock (_clientLock)
                {
                    foreach (var client in _connectedClients)
                    {
                        try
                        {
                            client.Close();
                        }
                        catch { }
                    }
                    _connectedClients.Clear();
                }
            }
        }

        // 触发服务错误事件
        private void OnServerError(string methodName, int errorCode, string errorMsg)
        {
            _logger.Error($"TCP服务错误[{methodName}]：{errorCode} - {errorMsg}");
            ServerErrorOccurred?.Invoke(this, new ServerErrorEventArgs
            {
                MethodName = methodName,
                ErrorCode = errorCode,
                ErrorMsg = errorMsg,
                ErrorTime = DateTime.Now
            });
        }

        // 自定义事件参数
        public class ClientConnectedEventArgs : EventArgs
        {
            public string ClientIp { get; set; }
            public int ClientPort { get; set; }
            public DateTime ConnectTime { get; set; }
        }

        public class ClientDisconnectedEventArgs : EventArgs
        {
            public string ClientIp { get; set; }
            public int ClientPort { get; set; }
            public DateTime DisconnectTime { get; set; }
            public string Reason { get; set; }
        }

        public class DataReceivedEventArgs : EventArgs
        {
            public string ClientIp { get; set; }
            public int ClientPort { get; set; }
            public string Data { get; set; }
            public DateTime ReceiveTime { get; set; }
        }

        public class ServerErrorEventArgs : EventArgs
        {
            public string MethodName { get; set; }
            public int ErrorCode { get; set; }
            public string ErrorMsg { get; set; }
            public DateTime ErrorTime { get; set; }
        }

    }
}



