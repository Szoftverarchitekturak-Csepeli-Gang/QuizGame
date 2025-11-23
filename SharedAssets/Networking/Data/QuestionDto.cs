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
        public int id { get; set; }
        public int questionBankId { get; set; }
        public string text { get; set; }
        public string optionA { get; set; }
        public string optionB { get; set; }
        public string optionC { get; set; }
        public string optionD { get; set; }
        public int correctAnswer { get; set; }
    }
}
