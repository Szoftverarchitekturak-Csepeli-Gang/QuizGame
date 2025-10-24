using System;
using UnityEngine;

public class WaitroomScreenController : ScreenController
{
    private int _connectedPlayers;
    [SerializeField, HideInInspector] private string _connectedPlayersText;

    public int ConnectedPlayers
    {
        set
        {
            _connectedPlayers = value;
            _connectedPlayersText = $"Connected players: {_connectedPlayers}";
        }
        get => _connectedPlayers;
    }

    void OnEnable()
    {
        ConnectedPlayers = 1;
    }
}
