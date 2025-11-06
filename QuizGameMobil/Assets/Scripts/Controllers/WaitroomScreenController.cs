using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    private int _connectedPlayers;
    private LocalizedString _connectedPlayersText;

    public int ConnectedPlayers
    {
        set
        {
            _connectedPlayers = value;
            IntVariable playerNum = _connectedPlayersText["playerNum"] as IntVariable;
            playerNum.Value = _connectedPlayers;
        }
        get => _connectedPlayers;
    }

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        _connectedPlayersText = _ui.Q<Label>("ConnectedPlayers").GetBinding("text") as LocalizedString;
    }
}
