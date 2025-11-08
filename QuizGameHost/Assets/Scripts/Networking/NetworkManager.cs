using Assets.Scripts.Networking.Data;
using Assets.Scripts.Networking.Http;
using Assets.Scripts.Networking.Websocket;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public IUnityWebSocketClient WebSocketClient { get; private set; }
    public IUnityHttpClient HttpClient { get; private set; }

    [SerializeField] private string _serverUrl = "ws://localhost:8080/ws";
    [SerializeField] private string _apiBaseUrl = "http://localhost:8080";

    private int _roomIndex = 0;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        WebSocketClient = new UnityWebSocketClient(new NativeWebSocketClient());
        HttpClient = new UnityHttpClient(_apiBaseUrl);

        WebSocketClient.On<QuestionDto>("QuestionReceive", QuestionReceiveHandler);
        WebSocketClient.On<string>("ServerMessage", ServerMessageHandler);

        await WebSocketClient.ConnectAsync(_serverUrl);
    }

    private void Update()
    {
        WebSocketClient?.Update();
    }

    private async void OnDestroy()
    {
        await WebSocketClient.DisconnectAsync();
    }

    public async Task CreateRoom()
    {
        var room = new RoomDto
        {
            Id = _roomIndex++,
            Name = $"Room {_roomIndex}"
        };

        var response = await HttpClient.PostAsync<RoomDto, string>($"/rooms", room);

        Debug.Log($"[Network] Room created: {response}");
    }

    public async Task JoinRoom(int roomId)
    {
        var response = await HttpClient.PostAsync<int, string>($"/rooms/{roomId}/join", roomId);
        Debug.Log($"[Network] Joined room: {response}");
    }

    public async Task GetRoomPlayers()
    {
        var players = await HttpClient.GetAsync<string[]>($"/rooms/players");
        Debug.Log($"[Network] Room players: {string.Join(", ", players)}");
    }

    public void QuestionReceiveHandler(QuestionDto question)
    {
        Debug.Log($"[Network] Question received: {question.Question}");
    }

    public void ServerMessageHandler(string message)
    {
        Debug.Log($"[Network] Server message received: {message}");
    }
}
