using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public void ShowStatisticsPanel(Question question)
    {
        //Todo: Bind statistics panel
        _screen?.ShowQuestionDisplay(question);
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
