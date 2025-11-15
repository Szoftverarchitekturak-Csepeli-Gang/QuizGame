using System;
using UnityEngine;


namespace Assets.Scripts.Networking.Data
{
    [Serializable]
    public class QuestionBankDto
    {
        public int id { get; set; }
        public int ownerId { get; set; }
        public string title { get; set; }
        public bool @public { get; set; } // @ escapes C# keyword
    }
}
