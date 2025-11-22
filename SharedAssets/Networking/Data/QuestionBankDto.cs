using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.SharedAssets.Networking.Data
{
    [Serializable]
    public class QuestionBankDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("ownerId")]
        public int OwnerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("questions")]
        public List<QuestionDto> Questions { get; set; }
        [JsonProperty("public")]
        public bool IsPublic { get; set; }
    }
}
