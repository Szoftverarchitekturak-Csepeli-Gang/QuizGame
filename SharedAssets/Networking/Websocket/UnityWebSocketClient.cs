using Assets.Scripts.Networking.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Networking.Websocket
{
    public class UnityWebSocketClient : IUnityWebSocketClient
    {
        private readonly ISocketClient _socketClient;
        private readonly Dictionary<string, Delegate> _handlers = new();

        public UnityWebSocketClient(ISocketClient socketClient)
        {
            _socketClient = socketClient;
            _socketClient.OnMessageReceived += HandleMessage;
        }

        public async Task ConnectAsync(string url)
        {
            await _socketClient.ConnectAsync(url);
        }

        public async Task DisconnectAsync()
        {
            await _socketClient.DisconnectAsync();
        }

        public async Task SendAsync<T>(string type, T data)
        {
            var msg = new SocketMessage
            {
                Type = type,
                Data = JToken.FromObject(data) // ← Most JToken-t csinálunk belőle
            };

            var json = JsonConvert.SerializeObject(msg);
            await _socketClient.SendAsync(json);
        }

        public void On<T>(string messageType, Action<T> handler)
        {
            _handlers[messageType] = handler;
        }

        private void HandleMessage(string json)
        {
            SocketMessage msg;

            try
            {
                msg = JsonConvert.DeserializeObject<SocketMessage>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Invalid WebSocket JSON: {e.Message}");
                return;
            }

            if (msg == null || string.IsNullOrEmpty(msg.Type))
            {
                Debug.LogWarning("Received empty or invalid WebSocket message.");
                return;
            }

            if (_handlers.TryGetValue(msg.Type, out var handler))
            {
                var handlerType = handler.GetType();
                var genericType = handlerType.GenericTypeArguments[0];

                try
                {
                    object typedData;

                    try
                    {
                        typedData = msg.Data.ToObject(genericType);
                        Debug.Log($"[WebSocket] Deserialized '{msg.Type}' as {genericType.Name}");
                    }
                    catch
                    {
                        try
                        {
                            typedData = Convert.ChangeType(msg.Data, genericType);
                            Debug.Log($"[WebSocket] Converted '{msg.Type}' to {genericType.Name}");
                        }
                        catch
                        {
                            typedData = msg.Data?.ToString();
                            Debug.Log($"[WebSocket] Fallback raw data for '{msg.Type}': {typedData}");
                        }
                    }

                    handler.DynamicInvoke(typedData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[WebSocket] Failed to process '{msg.Type}': {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"[WebSocket] Unhandled message type: {msg.Type}");
            }
        }

        public void Update()
        {
            _socketClient.Update();
        }
    }
}