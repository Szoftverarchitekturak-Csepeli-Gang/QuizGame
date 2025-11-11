using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenController : ScreenController
{
    private VisualElement _resultElement;
    [SerializeField] private string _resultText;

    protected override void Awake()
    {
        base.Awake();
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _resultElement = _ui.Q<VisualElement>("ResultElement");
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
        // show the results statistics of the answers
        ShowResultsStatistics();
    }

    private void ShowResultsStatistics()
    {
        // TODO: results screen with player's answers and the right answer
        throw new NotImplementedException();
    }
}
