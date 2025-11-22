using UnityEngine;
using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    [SerializeField, HideInInspector] private int _roomID;
    [SerializeField, HideInInspector] private int _connectedUsers;

    public int RoomID { set => _roomID = value; }
    public int ConnectedUsers { set => _connectedUsers = value; }

    public override void ResetUIState()
    {
        RoomID = 0;
        ConnectedUsers = 0;
    }

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _ui.Q<Button>("StartGameBtn").clicked += OnStartGameButtonClicked;
        RoomManager.Instance.UserCountChanged += SetConnectedUsers;
        RoomManager.Instance.RoomCreated += SetRoomId;

        VisibilityChanged += HandleVisibilityChanged;
    }

    void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.UserCountChanged -= SetConnectedUsers;
            RoomManager.Instance.RoomCreated -= SetRoomId;
        }
        
        VisibilityChanged -= HandleVisibilityChanged;
    }

    private async void OnStartGameButtonClicked()
    {
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

    private void HandleVisibilityChanged(bool visible)
    {
        var audioListener = GameObject.FindGameObjectWithTag("UICamera").GetComponent<AudioListener>();
        audioListener.enabled = visible;

        if (visible)
            AudioManager.Instance.PlayWaitingRoomBackgroundSound();
        else
            AudioManager.Instance.StopBackgroundSound();
    }
}
