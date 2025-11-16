using System;
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

    public void HandleQuestionTimer(float timer)
    {
        _screen?.HandleQuestionTimer(timer);
    }

    public void InitTimer(float max)
    {
        _screen?.InitTimer(max);
    }

    public void ShowStatisticsPanel(Question question)
    {
        //Todo: Bind statistics panel
        _screen?.ShowResultsStatistics(question, new float[] { 0.5f, 0.1f, 0.2f, 0.2f });
    }

    public void HideStatisticsPanel()
    {
        //Todo: Bind statistics panel
        _screen?.HideQuestionDisplay();
    }

    public void ShowBattleEndPanel(bool victory)
    {
        _screen?.OnGameRoundEnded(victory ? RoundResult.VICTORY : RoundResult.FAILURE);
    }

    public void HideBattleEndPanel()
    {
        //_screen?.ResetResultText();
    }
}
