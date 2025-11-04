using System;
using UnityEngine;
using Unity.Properties;

public class Question
{
    public string QuestionText { get; set; }
    public string[] Answers { get; set; }
    private int _correctAnswerIdx;
    [CreateProperty]
    public int CorrectAnswerIdx
    {
        get => _correctAnswerIdx;
        set
        {
            if (value >= Answers.Length) return;
            _correctAnswerIdx = value;
        }
    }

    public Question()
    {
        QuestionText = "Question";
        Answers = new string[4] { "a", "b", "c", "d" };
        CorrectAnswerIdx = 1;
    }

    public Question(string questionText, string[] answers, int correctAnswerIdx)
    {
        QuestionText = questionText;
        Answers = answers;
        CorrectAnswerIdx = correctAnswerIdx;
    }
}
