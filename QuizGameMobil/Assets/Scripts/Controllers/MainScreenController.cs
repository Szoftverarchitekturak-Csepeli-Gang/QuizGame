using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScreenController : ScreenController
{
    private Button _joinButton;
    private Label _inputErrorLabel;

    [SerializeField, HideInInspector] private int _roomID;
    [SerializeField, HideInInspector] private string _roomPassword;
    [SerializeField, HideInInspector] private string _errorMessage = "Error message";

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");

        _joinButton = _ui.Q<Button>("JoinButton");
        _joinButton.clicked += OnJoinButtonClick;

        _inputErrorLabel = _ui.Q<Label>("InputError");
        _inputErrorLabel.style.visibility = Visibility.Hidden;
        NetworkManager.Instance.RoomJoinedEvent += HandleRoomJoined;
        NetworkManager.Instance.SocketErrorEvent += HandleSocketError;
    }

    void OnDisable()
    {
        _joinButton.clicked -= OnJoinButtonClick;
        if(NetworkManager.Instance != null)
        {
            NetworkManager.Instance.RoomJoinedEvent -= HandleRoomJoined;
            NetworkManager.Instance.SocketErrorEvent -= HandleSocketError;
        }
    }

    private async void OnJoinButtonClick()
    {
        await NetworkManager.Instance.JoinRoom(_roomID);
        Debug.Log($"id: {_roomID}, pass: {_roomPassword}");
    }

    private void HandleRoomJoined(int roomId)
    {
        Debug.Log($"Joined room {roomId}");
        _inputErrorLabel.style.visibility = Visibility.Hidden;
        ScreenManagerMobil.Instance.CurrentScreen = AppScreen.WAITROOM;
    }

    private void HandleSocketError(string message)
    {
        Debug.Log($"Socket error: {message}");
        _errorMessage = message;
        _inputErrorLabel.style.visibility = Visibility.Visible;
    }

    public override void ResetUIState()
    {
        _roomID = 0;
        _roomPassword = "";
    }
}
