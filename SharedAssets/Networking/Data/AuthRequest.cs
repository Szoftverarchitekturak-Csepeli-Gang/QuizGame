using System;
using Newtonsoft.Json;

namespace Assets.SharedAssets.Networking.Data
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