using System;
using Assets.SharedAssets.Networking.Data;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QuestionDisplayElement : VisualElement
{
    [SerializeField, CreateProperty] private string _questionText;
    [SerializeField, CreateProperty] private string _currentQuestionIndexText;
    [SerializeField, CreateProperty] private string _answerA, _answerB, _answerC, _answerD;
    [SerializeField, CreateProperty] private float _answerACompletion, _answerBCompletion, _answerCCompletion, _answerDCompletion;

    private Button _nextButton;
    private Action _onNext;
    private readonly UnityEvent<int> CorrectAnswerIdx = new();

    public QuestionDisplayElement()
    {
        dataSource = this;
        VisualElement questionHeader = new();
        questionHeader.AddToClassList("question-header");
        Add(questionHeader);

        CreateLabel(questionHeader, "question-label", nameof(_questionText));
        CreateLabel(questionHeader, "question-index-label", nameof(_currentQuestionIndexText));

        CreateNextButton(questionHeader);
        CreateAnswerElements();

        DebugValues();
    }

    public void SetOnNext(Action onNext)
    {
        _onNext = onNext;
    }

    private void CreateLabel(VisualElement parent, string className, string bindingPath)
    {
        Label label = new();
        label.SetBinding("text", new DataBinding
        {
            dataSourcePath = new PropertyPath(bindingPath),
            bindingMode = BindingMode.ToTarget
        });
        label.AddToClassList(className);
        parent.Add(label);
    }

    private void CreateNextButton(VisualElement questionHeader)
    {
        _nextButton = new();
        _nextButton.AddToClassList("icon-button");
        _nextButton.AddToClassList("red-button");
        Image nextImage = new();
        _nextButton.Add(nextImage);
        questionHeader.Add(_nextButton);

        _nextButton.clicked += () =>
        {
            _onNext?.Invoke();
            AddToClassList("hide");
            Reset();
        };
    }

    private void CreateAnswerElements()
    {
        VisualElement answersContainer = new();
        answersContainer.AddToClassList("answers-container");
        Add(answersContainer);

        VisualElement answerRow = new();
        answerRow.AddToClassList("answer-row");
        answersContainer.Add(answerRow);

        CreateAnswerElement(answerRow, nameof(_answerA), nameof(_answerACompletion), 0);
        CreateAnswerElement(answerRow, nameof(_answerB), nameof(_answerBCompletion), 1);

        VisualElement answerRow2 = new();
        answerRow2.AddToClassList("answer-row");
        answersContainer.Add(answerRow2);

        CreateAnswerElement(answerRow2, nameof(_answerC), nameof(_answerCCompletion), 2);
        CreateAnswerElement(answerRow2, nameof(_answerD), nameof(_answerDCompletion), 3);
    }

    private void CreateAnswerElement(VisualElement answerRow, string labelPath, string completionPath, int answerIdx)
    {
        AnswerElement answerElement = new(labelPath, completionPath, answerIdx, CorrectAnswerIdx);
        answerElement.dataSource = this;
        answerRow.Add(answerElement);
    }

    public void SetCurrentQuestionIndex(int currentIndex, int totalQuestions)
    {
        _currentQuestionIndexText = $"{currentIndex} / {totalQuestions}";
    }

    private void DebugValues()
    {
        _questionText = "What is the capital of France?";
        _answerA = "Berlin";
        _answerB = "Madrid";
        _answerC = "Paris";
        _answerD = "Rome";

        _answerACompletion = 0.1f;
        _answerBCompletion = 0.2f;
        _answerCCompletion = 0.6f;
        _answerDCompletion = 0.1f;

        SetCurrentQuestionIndex(1, 10);
    }

    public void LoadQuestion(Question question)
    {
        _questionText = question.QuestionText;
        _answerA = question.Answers[0];
        _answerB = question.Answers[1];
        _answerC = question.Answers[2];
        _answerD = question.Answers[3];

        // -1 if no correct answer yet -> we are in question state
        if (question.CorrectAnswerIdx < 0) _nextButton.AddToClassList("hide");
        else _nextButton.RemoveFromClassList("hide");

        CorrectAnswerIdx.Invoke(question.CorrectAnswerIdx);
    }

    public void LoadPercentages(float[] percentages)
    {
        _answerACompletion = percentages[0];
        _answerBCompletion = percentages[1];
        _answerCCompletion = percentages[2];
        _answerDCompletion = percentages[3];
    }

    public void ResetPercentages()
    {
        LoadPercentages(new float[] { 0f, 0f, 0f, 0f });
    }

    public void Reset()
    {
        ResetPercentages();
        _questionText = _answerA = _answerB = _answerC = _answerD = _currentQuestionIndexText = "";
    }
}
