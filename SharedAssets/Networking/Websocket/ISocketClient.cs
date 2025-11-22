using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Websocket
{
    public interface ISocketClient
    {
        event Action<string> OnMessageReceived;
        event Action OnOpen;
        event Action OnClose;
        event Action<string> OnError;

        bool IsConnected { get; }

        Task ConnectAsync(string url);
        Task SendAsync(string message);
        Task DisconnectAsync();
        void Update();
    }
}
