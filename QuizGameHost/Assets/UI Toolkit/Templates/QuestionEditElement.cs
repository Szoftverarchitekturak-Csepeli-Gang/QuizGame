using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QuestionEditElement : VisualElement
{
    [SerializeField] public string QuestionText;
    [SerializeField] public string AnswerA, AnswerB, AnswerC, AnswerD;
    [SerializeField] private int CorrectAnswerIdx;

    private RadioButtonGroup _correctAnswerGroup;
    private Button _deleteButton;

    public QuestionEditElement()
    {
        dataSource = this;

        CreateQuestionField();
        CreateAnswerFields();
    }

    ~QuestionEditElement()
    {
        _deleteButton.clicked -= OnDeleteButtonClicked;
    }

    private void CreateAnswerFields()
    {
        _correctAnswerGroup = new RadioButtonGroup();
        _correctAnswerGroup.AddToClassList("answer-group");

        BindAnswerField(0, nameof(AnswerA));
        BindAnswerField(1, nameof(AnswerB));
        BindAnswerField(2, nameof(AnswerC));
        BindAnswerField(3, nameof(AnswerD));

        Add(_correctAnswerGroup);

        _correctAnswerGroup.RegisterValueChangedCallback(evt =>
        {
            CorrectAnswerIdx = evt.newValue;
        });
    }

    private void CreateQuestionField()
    {
        VisualElement questionHeader = new();
        questionHeader.AddToClassList("question-header");

        TextField questionTextField = new() { label = "Question" };
        questionTextField.AddToClassList("question-field");
        questionTextField.SetBinding("value", new DataBinding
        {
            dataSourcePath = new PropertyPath(nameof(QuestionText)),
            bindingMode = BindingMode.TwoWay
        });

        questionHeader.Add(questionTextField);

        _deleteButton = new() { text = "Delete" };
        _deleteButton.clicked += OnDeleteButtonClicked;
        questionHeader.Add(_deleteButton);

        Add(questionHeader);
    }

    private void OnDeleteButtonClicked()
    {
        DashboardScreenController.DeleteQuestionEvent.Invoke(this);
    }

    private void BindAnswerField(int i, string propertyName)
    {
        VisualElement answerContainer = new VisualElement();
        answerContainer.AddToClassList("answer-container");

        TextField answerField = new();
        answerField.AddToClassList("answer-field");
        answerField.label = $"{i + 1}";
        answerField.SetBinding("value", new DataBinding
        {
            dataSourcePath = new PropertyPath(propertyName),
            bindingMode = BindingMode.TwoWay
        });

        RadioButton radio = new();

        VisualElement answerRow = new();
        answerRow.AddToClassList("answer-row");

        answerRow.Add(radio);
        answerRow.Add(answerField);

        _correctAnswerGroup.Add(answerRow);
    }

    public void BindQuestion(Question question)
    {
        QuestionText = question.QuestionText;
        AnswerA = question.Answers[0];
        AnswerB = question.Answers[1];
        AnswerC = question.Answers[2];
        AnswerD = question.Answers[3];
        CorrectAnswerIdx = question.CorrectAnswerIdx;

        _correctAnswerGroup.value = CorrectAnswerIdx;
    }

    public Question GetQuestion()
    {
        return new Question(QuestionText, new string[] { AnswerA, AnswerB, AnswerC, AnswerD }, CorrectAnswerIdx);
    }
}

