using Newtonsoft.Json.Linq;
using System;

namespace Assets.Scripts.Networking.Data
{
    [Serializable]
    public class SocketMessage
    {
        public string Type { get; set; }
        public JToken Data { get; set; }
    }
}