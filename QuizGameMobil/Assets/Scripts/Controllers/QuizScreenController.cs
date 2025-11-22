using Assets.SharedAssets.Networking.Data;
using System;
using System.Collections;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class QuizScreenController : ScreenController
{
    [SerializeField] private string _question;
    [SerializeField] private string[] _answers = new string[4];
    [SerializeField, HideInInspector] int _maxTime;
    [SerializeField, HideInInspector] float _currentTime;
    bool _timerRunning = false;

    private VisualElement _progressBarColor;

    private bool _answerSubmitted = false;

    void OnEnable()
    {
        NetworkManager.Instance.NewQuestionEvent += QuestionReceivedHandler;

        VisualElement answerButtons = _ui.Q<VisualElement>("AnswerButtons");
        VisualElement[] answers = answerButtons.Children().ToArray();
        for (int i = 0; i < answerButtons.childCount; i++)
        {
            answers[i].dataSourcePath = PropertyPath.FromIndex(i);
        }

        ProgressBar progressBar = _ui.Q<ProgressBar>("TimeLeft");
        _progressBarColor = progressBar.Q<VisualElement>(className: "unity-progress-bar__progress");
        _progressBarColor.AddToClassList("blue-background");
    }

    private void QuestionReceivedHandler(QuestionDto newQuestion)
    {
        _question = newQuestion.Text;

        _answers[0] = newQuestion.OptionA;
        _answers[1] = newQuestion.OptionB;
        _answers[2] = newQuestion.OptionC;
        _answers[3] = newQuestion.OptionD;

        _ui.Q<Button>("Answer1").clicked += () => OnAnswerClicked(1);
        _ui.Q<Button>("Answer2").clicked += () => OnAnswerClicked(2);
        _ui.Q<Button>("Answer3").clicked += () => OnAnswerClicked(3);
        _ui.Q<Button>("Answer4").clicked += () => OnAnswerClicked(4);

        _answerSubmitted = false;

        StartTimer(10);
    }

    public async void OnAnswerClicked(int answerIdx)
    {
        if (_answerSubmitted) return;
        _answerSubmitted = true;
        await NetworkManager.Instance.SendAnswer(answerIdx);
    }

    public void StartTimer(int maxTime)
    {
        _maxTime = maxTime;
        _currentTime = 0;
        _timerRunning = true;
    }

    private void UpdateTimer()
    {
        if (!_timerRunning) return;

        _currentTime += Time.deltaTime;
        if (_currentTime >= _maxTime)
        {
            _timerRunning = false;
            return;
        }

        float p = _currentTime / _maxTime;
        if (p > 0.33f && p <= 0.66f)
        {
            _progressBarColor.RemoveFromClassList("blue-background");
            _progressBarColor.RemoveFromClassList("red-background");
            _progressBarColor.AddToClassList("yellow-background");
        }
        else if (p > 0.66f)
        {
            _progressBarColor.RemoveFromClassList("blue-background");
            _progressBarColor.RemoveFromClassList("yellow-background");
            _progressBarColor.AddToClassList("red-background");
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    public override void ResetUIState()
    {
        _question = "";
        for (int i = 0; i < _answers.Length; i++)
        {
            _answers[i] = "";
        }
        _currentTime = 0;
        _timerRunning = false;
    }
}
