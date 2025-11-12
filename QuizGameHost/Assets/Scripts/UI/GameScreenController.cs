using System;
using Assets.Scripts.Networking.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenController : ScreenController
{
    private VisualElement _resultElement;
    [SerializeField, HideInInspector] private string _resultText;

    private QuestionDisplayElement _questionDisplayElement;

    protected override void Awake()
    {
        base.Awake();
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _resultElement = _ui.Q<VisualElement>("ResultElement");
        _questionDisplayElement = _ui.Q<QuestionDisplayElement>();
    }

    void OnEnable()
    {
        _resultText = "DEFEAT";
        GameManager.GameRoundEnded += OnGameRoundEnded;
    }

    void OnDisable()
    {
        GameManager.GameRoundEnded -= OnGameRoundEnded;
    }

    private void OnGameRoundEnded(RoundResult result)
    {
        if (result == RoundResult.VICTORY)
        {
            _resultText = "VICTORY";
            _resultElement.AddToClassList("result-text-victory");
            _resultElement.RemoveFromClassList("result-text-defeat");
        }
        else
        {
            _resultText = "DEFEAT";
            _resultElement.AddToClassList("result-text-defeat");
            _resultElement.RemoveFromClassList("result-text-victory");
        }

        _resultElement.RemoveFromClassList("result-text-start");

        Invoke(nameof(ResetResultText), 2f);
    }

    private void ResetResultText()
    {
        _resultElement.AddToClassList("result-text-start");
    }

    private void ShowResultsStatistics(Question question, float[] percentages)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.LoadPercentages(percentages);
        _questionDisplayElement.RemoveFromClassList("hide");
    }

    public void ShowQuestionDisplay(Question question)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.ResetPercentages();
        _questionDisplayElement.RemoveFromClassList("hide");
    }
}
