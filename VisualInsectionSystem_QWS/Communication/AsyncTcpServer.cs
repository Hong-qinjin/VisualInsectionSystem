using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisualInsectionSystem.Communication
{
    // AsyncTcpServer.cs增强版TCP服务器
    /// <summary>
    /// 增强版异步TCP服务器，实现IDisposable，提供完善的事件通知和粘包处理
    /// </summary>
    public class AsyncTcpServer : IDisposable
    {
        private readonly int _port;
        private TcpListener _listener;
        private bool _isRunning;
        private bool _disposed;
        private CancellationTokenSource _cts;
        //private readonly ConcurrentDictionary<string, TcpClient> _clients = new();
        private readonly ConcurrentDictionary<string, TcpClient> _clients = new ConcurrentDictionary<string, TcpClient>();
        //private readonly ConcurrentDictionary<string, StringBuilder> _clientBuffers = new ();
        private readonly ConcurrentDictionary<string, StringBuilder> _clientBuffers = new ConcurrentDictionary<string, StringBuilder>();
        //private readonly object _lockObj = new();
        private readonly object _lockObj = new object();

        public event EventHandler<ClientEventArgs> ClientConnected;
        public event EventHandler<ClientEventArgs> ClientDisconnected;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler<ServerErrorEventArgs> ServerError;

        public bool IsRunning => _isRunning;
        public int ConnectedClientsCount => _clients.Count;
        public bool IsHexReceive { get; set; }
        public bool IsHexSend { get; set; }

        public AsyncTcpServer(int port)
        {
            _port = port;
        }

        public async Task StartAsync()
        {
            if (_isRunning) return;
            _isRunning = true;
            _cts = new CancellationTokenSource();

            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                LogHelper.Info($"TCP服务器启动，监听端口：{_port}");

                _ = Task.Run(() => AcceptClientsAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                _isRunning = false;
                OnServerError("StartAsync", ex);
                throw;
            }
        }

        public async Task StopAsync()
        {
            if (!_isRunning) return;
            _isRunning = false;
            _cts?.Cancel();

            try
            {
                _listener?.Stop();
                foreach (var client in _clients.Values)
                {
                    client?.Close();
                    client?.Dispose();
                }
                _clients.Clear();
                _clientBuffers.Clear();
                LogHelper.Info("TCP服务器已停止");
            }
            catch (Exception ex)
            {
                OnServerError("StopAsync", ex);
            }
            await Task.CompletedTask;
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var clientId = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
                    if (_clients.TryAdd(clientId, client))
                    {
                        LogHelper.Info($"客户端连接：{clientId}");
                        OnClientConnected(clientId);
                        _ = HandleClientAsync(client, clientId, token);
                    }
                    else
                    {
                        client.Close();
                    }
                }
                catch (Exception ex) when (!token.IsCancellationRequested)
                {
                    OnServerError("AcceptClientsAsync", ex);
                    await Task.Delay(1000, token);
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, string clientId, CancellationToken token)
        {
            var buffer = new byte[4096];
            var msgBuffer = new StringBuilder();
            _clientBuffers[clientId] = msgBuffer;
            NetworkStream stream = null;

            try
            {
                stream = client.GetStream();
                while (!token.IsCancellationRequested && client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0) break;

                    string receivedText = IsHexReceive
                        ? BitConverter.ToString(buffer, 0, bytesRead).Replace("-", " ")
                        : Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    msgBuffer.Append(receivedText);
                    string fullBuffer = msgBuffer.ToString();
                    int newlineIndex;
                    while ((newlineIndex = fullBuffer.IndexOf('\n')) >= 0)
                    {
                        string message = fullBuffer.Substring(0, newlineIndex).TrimEnd('\r');
                        fullBuffer = fullBuffer.Substring(newlineIndex + 1);
                        msgBuffer.Clear();
                        msgBuffer.Append(fullBuffer);

                        if (!string.IsNullOrEmpty(message))
                        {
                            OnDataReceived(clientId, message);
                            if (message.Trim().Equals("PING", StringComparison.OrdinalIgnoreCase))
                                await SendAsync(clientId, "PONG\n", token);
                        }
                    }
                }
            }
            catch (Exception ex) when (!token.IsCancellationRequested)
            {
                OnServerError($"HandleClientAsync:{clientId}", ex);
            }
            finally
            {
                stream?.Dispose();
                _clientBuffers.TryRemove(clientId, out _);
                RemoveClient(clientId);
            }
        }

        private void RemoveClient(string clientId)
        {
            if (_clients.TryRemove(clientId, out var client))
            {
                client?.Close();
                client?.Dispose();
                LogHelper.Info($"客户端断开：{clientId}");
                OnClientDisconnected(clientId);
            }
        }

        public async Task SendAsync(string clientId, string message, CancellationToken token = default)
        {
            if (!_clients.TryGetValue(clientId, out var client) || !client.Connected) return;
            try
            {
                byte[] data = IsHexSend ? HexStringToByteArray(message) : Encoding.UTF8.GetBytes(message);
                await client.GetStream().WriteAsync(data, 0, data.Length, token);
            }
            catch { RemoveClient(clientId); }
        }

        public async Task BroadcastAsync(string message, CancellationToken token = default)
        {
            byte[] data = IsHexSend ? HexStringToByteArray(message) : Encoding.UTF8.GetBytes(message);
            foreach (var kvp in _clients)
            {
                try { if (kvp.Value.Connected) await kvp.Value.GetStream().WriteAsync(data, 0, data.Length, token); } catch { }
            }
        }

        private byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0) hex += "0";
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void OnClientConnected(string clientId) => ClientConnected?.Invoke(this, new ClientEventArgs(clientId));
        private void OnClientDisconnected(string clientId) => ClientDisconnected?.Invoke(this, new ClientEventArgs(clientId));
        private void OnDataReceived(string clientId, string message) => DataReceived?.Invoke(this, new DataReceivedEventArgs(clientId, message));
        private void OnServerError(string context, Exception ex) => ServerError?.Invoke(this, new ServerErrorEventArgs(context, ex.Message));

        public void Dispose()
        {
            if (_disposed) return;
            StopAsync().Wait(TimeSpan.FromSeconds(3));
            _cts?.Dispose();
            _listener = null;
            _disposed = true;
        }
    }

    public class ClientEventArgs : EventArgs { public string ClientId { get; }
    public ClientEventArgs(string id) => ClientId = id; }
    public class DataReceivedEventArgs : EventArgs { public string ClientId { get; }
    public string Message { get; }
    public DataReceivedEventArgs(string id, string msg) { ClientId = id; Message = msg; } }
    public class ServerErrorEventArgs : EventArgs { public string Context { get; } 
    public string Error { get; } 
    public ServerErrorEventArgs(string ctx, string err) { Context = ctx; Error = err; } }



}