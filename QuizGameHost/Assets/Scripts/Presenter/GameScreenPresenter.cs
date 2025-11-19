using System;
using System.Linq;
using UnityEngine;


public class GameScreenPresenter : SingletonBase<GameScreenPresenter>, IPresenter<GameScreenController>
{
    private GameScreenController _screen;
    public void AttachScreen(GameScreenController screen)
    {
        _screen = screen;
    }

    public void DetachScreen()
    {
        _screen = null;
    }

    public void ShowVillagePanel(GameObject village, Action onConquer, Action onExit)
    {
        _screen?.ShowVillagePanel(village, onConquer, onExit);
    }

    public void HideVillagePanel()
    {
        _screen?.HideVillagePanel();
    }

    public void ShowQuestionPanel(Question question)
    {
        _screen?.ShowQuestionDisplay(question);
    }

    public void HideQuestionPanel()
    {
        _screen?.HideQuestionDisplay();
    }

    public void SetCurrentQuestionIndex(int currentIndex, int totalQuestions)
    {
        _screen?.SetCurrentQuestionIndex(currentIndex, totalQuestions);
    }

    public void HandleQuestionTimer(float timer)
    {
        _screen?.HandleQuestionTimer(timer);
    }

    public void InitTimer(float max)
    {
        _screen?.InitTimer(max);
    }

    public void ShowStatisticsPanel(Question question, Action onNext)
    {
        _screen?.ShowResultsStatistics(question, RoomManager.Instance.GetAnswerPercentages(), onNext);
    }

    public void HideStatisticsPanel()
    {
        _screen?.HideQuestionDisplay();
    }

    public void ShowBattleEndPanel(bool victory)
    {
        _screen?.OnGameRoundEnded(victory ? RoundResult.VICTORY : RoundResult.FAILURE);
    }

    public void HideBattleEndPanel()
    {
    }
}
