using Assets.Scripts.Networking.Http;
using Assets.SharedAssets.Networking.Websocket;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomManager : SingletonBase<RoomManager>
{
    public event Action             RoomCreated;
    public event Action<Question>   QuestionReceived;
    public event Action<int>        AnswerReceived;
    public event Action             UserCountChanged;
    public int RoomID { get; private set; }
    public int ConnectedPlayers { get; private set; }
    public Question CurrentQuestionPlayers { get; private set; }
    private int _roundCounter = 0;

    private void Start()
    {
        NetworkManager.Instance.OnRoomCreated += HandleRoomCreated;
        NetworkManager.Instance.OnPlayerJoined += ClientJoinedHandler;
        NetworkManager.Instance.OnPlayerDisconnected += ClientDisconnectedHandler;
    }

    public async Task CreateRoom(int questionBankID)
    {
        await NetworkManager.Instance.CreateRoom(questionBankID);
    }

    public async Task StartGame()
    {
        await NetworkManager.Instance.StartGame();
    }

    public void HandleRoomCreated(int roomId)
    {
        RoomID = roomId;
        UnityMainThreadDispatcher.Instance().Enqueue(() => RoomCreated?.Invoke());
    }

    private void ClientJoinedHandler()
    {
        ConnectedPlayers++;
        UserCountChanged?.Invoke();
    }

    private void ClientDisconnectedHandler()
    {
        ConnectedPlayers--;
        UserCountChanged?.Invoke();
    }
}