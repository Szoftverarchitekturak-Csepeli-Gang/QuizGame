using Assets.SharedAssets.Networking.Websocket;
using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.SharedAssets.Networking.Websocket
{
    public class UnitySocketIOClient : IUnitySocketIOClient
    {
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<string> OnError;

        private string _url;
        private SocketIOUnity _socket;
        public bool IsConnected => _socket?.Connected ?? false;

        public async Task ConnectAsync(string url)
        {
            _url = url;
            _socket = new SocketIOUnity(_url, new SocketIOOptions
            {
                ConnectionTimeout = TimeSpan.FromHours(1),
                Reconnection = true,
                ReconnectionAttempts = int.MaxValue,
                ReconnectionDelay = TimeSpan.FromSeconds(2).Seconds,
            });

            _socket.OnConnected +=  (sender, e) =>
            {
                Debug.Log($"[Socket.IO] Connected to server: {url}");
                OnConnected?.Invoke();
            };

            _socket.OnDisconnected += (sender, e) =>
            {
                Debug.Log($"[Socket.IO] Disconnected from server: {e}");
                OnDisconnected?.Invoke();
            };

            _socket.OnError += (sender, error) =>
            {
                Debug.Log($"[Socket.IO] Error: {error}");
                OnError?.Invoke(error);
            };

            await _socket.ConnectAsync();
        }

        public async Task DisconnectAsync()
        {
            if (IsConnected) await _socket.DisconnectAsync();
        }

        public async Task SendAsync<T>(string eventName, T data)
        {
            if (!IsConnected)
                return;

            await _socket.EmitAsync(eventName, data);
        }

        public void On<T>(string eventName, Action<T> handler)
        {
            _socket.On(eventName, response =>
            {
                if (response.Count == 0)
                {
                    if (typeof(T) == typeof(string))
                        handler?.Invoke((T)(object)"");
                    else if (typeof(T).IsClass)
                        handler?.Invoke(default);
                    return;
                }

                try
                {
                    var data = response.GetValue<T>(0);
                    handler?.Invoke(data);
                }
                catch (Exception ex)
                {
                    Debug.Log($"[Socket.IO] Type exception '{eventName}' → {typeof(T).Name}: {ex.Message}");
                }
            });
        }
    }
}
