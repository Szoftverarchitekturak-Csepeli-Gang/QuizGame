using NativeWebSocket;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Websocket
{
    public class NativeWebSocketClient : ISocketClient
    {
        private WebSocket _socket;

        public event Action<string> OnMessageReceived;
        public event Action OnOpen;
        public event Action OnClose;
        public event Action<string> OnError;

        public bool IsConnected { get => _socket != null && _socket.State == WebSocketState.Open; }

        public async Task ConnectAsync(string url)
        {
            _socket = new WebSocket(url);

            _socket.OnOpen += () => OnOpen?.Invoke();
            _socket.OnClose += (code) => OnClose?.Invoke();
            _socket.OnError += (err) => OnError?.Invoke(err);
            _socket.OnMessage += (bytes) =>
            {
                var message = Encoding.UTF8.GetString(bytes);
                OnMessageReceived?.Invoke(message);
            };

            await _socket.Connect();
        }

        public async Task SendAsync(string message)
        {
            if (_socket.State == WebSocketState.Open)
                await _socket.SendText(message);
        }

        public async Task DisconnectAsync()
        {
            if (_socket != null)
                await _socket.Close();
        }

        public void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
            _socket.DispatchMessageQueue();
            #endif
        }
    }
}