using System;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        ScreenManagerMobil.Instance.ShowScreen(AppScreen.MAIN);
        NetworkManager.Instance.GameStartedEvent += HandleGameStarted;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.GameStartedEvent -= HandleGameStarted;
    }

    private void HandleGameStarted()
    {
        ScreenManagerMobil.Instance.CurrentScreen = AppScreen.QUIZ;
    }

    public override void ResetUIState()
    {

    }
}
