using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Websocket
{
    public interface IUnityWebSocketClient
    {
        Task ConnectAsync(string url);
        Task DisconnectAsync();
        Task SendAsync<T>(string type, T data);
        void On<T>(string messageType, Action<T> handler);
        void Update();
    }
}
