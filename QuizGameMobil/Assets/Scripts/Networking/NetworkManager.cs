using Assets.Scripts.Networking.Http;
using Assets.SharedAssets.Networking.Websocket;
using System;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;

public class NetworkManager : MonoBehaviour
{
    public event Action<string> SocketErrorEvent;
    public event Action<int> RoomJoinedEvent;
    public event Action GameStartedEvent;
    public event Action HostDisconnectedEvent;
    public static NetworkManager Instance { get; private set; }
    public IUnitySocketIOClient socketIOClient { get; private set; }

    [SerializeField] private string _serverUrl = "http://localhost:3000";

    private async void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        socketIOClient = new UnitySocketIOClient();

        await socketIOClient.ConnectAsync(_serverUrl);

        socketIOClient.On<string>("error", message => OnSocketError(message));
        socketIOClient.On<int>("joinedRoom", roomId => OnRoomJoined(roomId));
        socketIOClient.On<object>("gameStarted", _ => OnGameStarted());
        socketIOClient.On<object>("hostDisconnected", _ => OnHostDisconnected());
    }
    private async void OnDestroy()
    {
        await socketIOClient.DisconnectAsync();
    }

    public async Task JoinRoom(int roomID)
    {
        try
        {
            await socketIOClient.SendAsync<int>("joinRoom", roomID);
        }
        catch(Exception ex)
        {
            Debug.LogError($"[Network] Failed join room: {ex}");
        }
    }

    public async Task LeaveRoom()
    {
        try
        {
            await socketIOClient.SendAsync<object>("leaveRoom", null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Failed to leave room: {ex}");
        }
    }

    private void OnSocketError(string message)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => SocketErrorEvent?.Invoke(message));
    }

    private void OnRoomJoined(int roomID)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => RoomJoinedEvent?.Invoke(roomID));
    }

    private void OnGameStarted()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => GameStartedEvent?.Invoke());
    }

    private void OnHostDisconnected()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => HostDisconnectedEvent?.Invoke());
    }

}
