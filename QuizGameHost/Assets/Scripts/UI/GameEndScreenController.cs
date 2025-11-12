using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEndScreenController : ScreenController
{
    [SerializeField] private string _conqueredVillagesText;
    private Button _exitButton;

    protected override void Awake()
    {
        base.Awake();
        _exitButton = _ui.Q<Button>("ExitButton");
    }

    void OnEnable()
    {
        GameManager.GameEnded += OnGameEnded;
        _exitButton.clicked += OnExitClicked;
    }

    void OnDisable()
    {
        GameManager.GameEnded -= OnGameEnded;
        _exitButton.clicked -= OnExitClicked;
    }

    private void OnExitClicked()
    {
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.DASHBOARD;
    }

    private void OnGameEnded(int rightAnswers, int totalQuestions)
    {
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.FINISH;
        _conqueredVillagesText = $"{rightAnswers} / {totalQuestions}";
    }
}
