using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Networking.Data
{
    [Serializable]
    public class AuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }
}