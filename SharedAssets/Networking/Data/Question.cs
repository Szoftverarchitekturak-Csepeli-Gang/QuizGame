using Unity.Properties;

public class Question
{
    public int Id { get; set; }
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
        Id = 0;
        QuestionText = "";
        Answers = new string[4] { "", "", "", "" };
        CorrectAnswerIdx = -1;
    }

    public Question(string questionText, string[] answers) : this()
    {
        Id = 0;
        QuestionText = questionText;
        Answers = answers;
    }

    public Question(int id, string questionText, string[] answers, int correctAnswerIdx) : this(questionText, answers)
    {
        Id = id;
        CorrectAnswerIdx = correctAnswerIdx;
    }
}
