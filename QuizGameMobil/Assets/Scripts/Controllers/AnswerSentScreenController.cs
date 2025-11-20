using System;
using Assets.Scripts.Networking.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class AnswerSentScreenController : ScreenController
{
    [SerializeField, HideInInspector] string _question;
    [SerializeField, HideInInspector] string _answer;

    VisualElement _screenData;

    void OnEnable()
    {
        NetworkManager.Instance.AnswerSentEvent += LoadScreenData;
        NetworkManager.Instance.NewQuestionEvent += OnNewQuestion;

        _screenData = _ui.Q<VisualElement>("ScreenData");
    }

    private void OnNewQuestion(QuestionDto dto)
    {
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.QUIZ;
    }

    public void LoadScreenData(string question, string answer)
    {
        ShowScreenData();

        _question = question;
        _answer = answer;

        Invoke(nameof(HideScreenData), 10);
    }

    private void HideScreenData()
    {
        _screenData.RemoveFromClassList("screen-data-init");
        _screenData.AddToClassList("no-opacity");
    }

    private void ShowScreenData()
    {
        _screenData.RemoveFromClassList("no-opacity");
        _screenData.AddToClassList("screen-data-init");
    }

    public override void ResetUIState()
    {
        _question = _answer = "";
    }
}
