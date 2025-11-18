using System;
using UnityEngine;


public class GameEndScreenPresenter : SingletonBase<GameEndScreenPresenter>, IPresenter<GameEndScreenController>
{
    private GameEndScreenController _screen;
    public void AttachScreen(GameEndScreenController screen)
    {
        _screen = screen;
    }

    public void DetachScreen()
    {
        _screen = null;
    }

    public void ShowGameEndScreen(int rightAnswers, int totalQuestions, int conqueredVillages, int totalVillages)
    {   
        _screen?.OnGameEnded(rightAnswers, totalQuestions, conqueredVillages, totalVillages);
    }
}
