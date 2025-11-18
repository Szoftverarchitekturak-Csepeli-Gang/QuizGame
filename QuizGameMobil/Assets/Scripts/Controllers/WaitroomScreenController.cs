using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    private int _connectedPlayers;
    private LocalizedString _connectedPlayersText;

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        _connectedPlayersText = _ui.Q<Label>("ConnectedPlayers").GetBinding("text") as LocalizedString;
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
}
