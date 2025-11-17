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
        NetworkManager.Instance.OnRoomCreated += SetRoomId;
        NetworkManager.Instance.OnPlayerJoined += ClientJoinedHandler;
        NetworkManager.Instance.OnPlayerDisconnected += ClientDisconnectedHandler;

    }

    void OnDisable()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnRoomCreated -= SetRoomId;
            NetworkManager.Instance.OnPlayerJoined -= ClientJoinedHandler;
            NetworkManager.Instance.OnPlayerDisconnected -= ClientDisconnectedHandler;
        }
    }

    private void OnStartGameButtonClicked()
    {
        // TODO: Notify server to start the game
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.GAME;
    }

    private void ClientJoinedHandler()
    {
        _connectedUsers++;
    }

    private void ClientDisconnectedHandler()
    {
        _connectedUsers--;
    }

    private void SetRoomId(int roomId)
    {
        _roomID = roomId;
    }
}
