using System;
using Newtonsoft.Json;

namespace Assets.SharedAssets.Networking.Data
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