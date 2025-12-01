using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Websocket
{
    public interface IUnitySocketIOClient
    {
        event Action OnConnected;
        event Action OnDisconnected;
        event Action<string> OnError;
        bool IsConnected { get; }
        Task ConnectAsync(string url);
        Task DisconnectAsync();
        Task SendAsync<T>(string eventName, T data, bool useAuth = false);

        void On<T>(string eventName, Action<T> handler);
    }
}
