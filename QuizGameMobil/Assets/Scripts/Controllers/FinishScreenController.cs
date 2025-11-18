using System;
using UnityEngine;
using UnityEngine.UIElements;

public class FinishScreenController : ScreenController
{
    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        _ui.Q<Button>("ReturnButton").clicked += OnReturnButtonClicked;
    }

    private void OnReturnButtonClicked()
    {
        // TODO: leave room
        ScreenManagerMobil.Instance.CurrentScreen = AppScreen.MAIN;
    }
}
