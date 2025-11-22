using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Networking.Data
{
    [Serializable]
    public class AuthRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}