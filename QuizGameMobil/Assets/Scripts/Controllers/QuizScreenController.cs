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
    [SerializeField, HideInInspector] int _maxTime, _currentTime;

    void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            _answers[i] = $"answer {i}";
        }

        VisualElement answerButtons = _ui.Q<VisualElement>("AnswerButtons");
        VisualElement[] answers = answerButtons.Children().ToArray();
        for (int i = 0; i < answerButtons.childCount; i++)
        {
            answers[i].dataSourcePath = PropertyPath.FromIndex(i);
        }
    }

    public void StartTimer(int maxTime)
    {
        _maxTime = maxTime;
        _currentTime = 0;

        StartCoroutine(TimerTick(UpdateTimer));
    }

    private void UpdateTimer()
    {
        _currentTime++;
        if (_currentTime < _maxTime)
        {
            StartCoroutine(TimerTick(UpdateTimer));
        }
        else
        {
            // TODO: show next question
        }
    }

    IEnumerator TimerTick(Action action)
    {
        yield return new WaitForSeconds(1);
        action.Invoke();
    }
}
