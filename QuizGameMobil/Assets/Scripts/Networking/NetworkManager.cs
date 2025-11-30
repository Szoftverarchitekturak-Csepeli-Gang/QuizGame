using Assets.SharedAssets.Networking.Http;
using Assets.SharedAssets.Networking.Websocket;
using Assets.SharedAssets.Networking.Data;
using System;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using SocketIOClient;

public class NetworkManager : SingletonBase<NetworkManager>
{
    public event Action<string> SocketErrorEvent;
    public event Action<int> RoomJoinedEvent;
    public event Action<QuestionDto> NewQuestionEvent;
    public event Action<string, string> AnswerSentEvent;
    public event Action GameStartedEvent;
    public event Action GameEndedEvent;
    public event Action HostDisconnectedEvent;
    public IUnitySocketIOClient socketIOClient { get; private set; }

    [SerializeField] private string _serverUrl = "http://localhost:3000";

    private async void Awake()
    {
        base.Awake();
        socketIOClient = new UnitySocketIOClient();

        await socketIOClient.ConnectAsync(_serverUrl);

        socketIOClient.On<string>("error", message => OnSocketError(message));
        socketIOClient.On<int>("joinedRoom", roomId => OnRoomJoined(roomId));
        socketIOClient.On<object>("gameStarted", _ => OnGameStarted());
        socketIOClient.On<object>("hostDisconnected", _ => OnHostDisconnected());
        socketIOClient.On<object>("gameEnded", _ => OnGameEnded());
        socketIOClient.On<QuestionDto>("newQuestion", (newQuestion) => OnQuestionReceived(newQuestion));
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
        catch (Exception ex)
        {
            ScreenManagerBase.Instance.DisplayErrorMessage("Failed to join room");
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
            ScreenManagerBase.Instance.DisplayErrorMessage("Failed to leave room");
            Debug.LogError($"[Network] Failed to leave room: {ex}");
        }
    }

    public async Task Logout()
    {
        await LeaveRoom();
    }

    public async Task SendAnswer(int answer)
    {
        try
        {
            await socketIOClient.SendAsync<int>("submitAnswer", answer);
        }
        catch (Exception ex)
        {
            ScreenManagerBase.Instance.DisplayErrorMessage("Failed to submit answer");
            Debug.LogError($"[Network] Failed to leave room: {ex}");
        }
    }

    private void OnSocketError(string message)
    {
        Debug.Log("[Socket] Error: " + message);
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

    private void OnGameEnded()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => GameEndedEvent?.Invoke());
    }

    private void OnHostDisconnected()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => HostDisconnectedEvent?.Invoke());
    }

    private void OnQuestionReceived(QuestionDto question)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => NewQuestionEvent?.Invoke(question));
    }

    public void RaiseAnswerSent(string question, string answer)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => AnswerSentEvent?.Invoke(question, answer));
    }

}
