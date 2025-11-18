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
        RoomManager.Instance.UserCountChanged += SetConnectedUsers;
        RoomManager.Instance.RoomCreated += SetRoomId;

    }

    void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.UserCountChanged -= SetConnectedUsers;
            RoomManager.Instance.RoomCreated -= SetRoomId;
        }
    }

    private async void OnStartGameButtonClicked()
    {
        // TODO: Notify server to start the game
        await RoomManager.Instance.StartGame();
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.GAME;
    }

    private void SetConnectedUsers()
    {
        _connectedUsers = RoomManager.Instance.ConnectedPlayers;
    }

    private void SetRoomId()
    {
        _roomID = RoomManager.Instance.RoomID;
    }
}
