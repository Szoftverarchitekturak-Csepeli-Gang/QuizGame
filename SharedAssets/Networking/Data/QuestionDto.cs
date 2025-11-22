using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.SharedAssets.Networking.Data
{
    [Serializable]
    public class QuestionDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("optionA")]
        public string OptionA { get; set; }
        [JsonProperty("optionB")]
        public string OptionB { get; set; }
        [JsonProperty("optionC")]
        public string OptionC { get; set; }
        [JsonProperty("optionD")]
        public string OptionD { get; set; }
        [JsonProperty("correctAnswer")]
        public int CorrectAnswer { get; set; }
    }
}
