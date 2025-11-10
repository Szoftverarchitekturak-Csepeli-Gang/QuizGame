using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    [SerializeField, HideInInspector] private int _roomID;
    [SerializeField, HideInInspector] private int _connectedUsers;

    public int RoomID { set => _roomID = value; }
    public int ConnectedUsers { set => _connectedUsers = value; }

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _ui.Q<Button>("StartGameBtn").clicked += OnStartGameButtonClicked;
    }

    private void OnStartGameButtonClicked()
    {
        // TODO: Notify server to start the game
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.GAME;
    }
}
