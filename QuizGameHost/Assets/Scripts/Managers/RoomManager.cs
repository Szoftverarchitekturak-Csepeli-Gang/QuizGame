using Assets.Scripts.Networking.Data;
using Assets.Scripts.Networking.Http;
using Assets.SharedAssets.Networking.Http;
using Assets.SharedAssets.Networking.Mappers;
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
    public Question CurrentQuestion { get; private set; }

    public int RoundCounter { get; private set; }
    public int QuestionCount { get; private set; }


    private void Start()
    {
        RoundCounter = 0;
        QuestionCount = 0;
        NetworkManager.Instance.OnRoomCreated += HandleRoomCreated;
        NetworkManager.Instance.OnPlayerJoined += ClientJoinedHandler;
        NetworkManager.Instance.OnPlayerDisconnected += ClientDisconnectedHandler;
        NetworkManager.Instance.OnQuestionArrived += QuestionReceiveHandler;
    }

    public async Task CreateRoom(int questionBankID)
    {
        RoundCounter = 0;
        await NetworkManager.Instance.CreateRoom(questionBankID);
        var response = await NetworkManager.Instance.GetQuestionsFromBank(questionBankID);
        if (response.IsSuccess) QuestionCount = response.Data.Count;
    }

    public async Task StartGame()
    {
        await NetworkManager.Instance.StartGame();
    }

    public async Task StartNextRound()
    {
        await NetworkManager.Instance.StartNextRound(RoundCounter);
    }

    public async Task LeaveRoom()
    {
        await NetworkManager.Instance.LeaveRoom();
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

    private void QuestionReceiveHandler(QuestionDto question)
    {
        RoundCounter++;
        Question newQuestion = QuestionMapper.ToModel(question);
        CurrentQuestion = newQuestion;
        UnityMainThreadDispatcher.Instance().Enqueue(() => QuestionReceived?.Invoke(newQuestion));
    }

    private void ClientDisconnectedHandler()
    {
        ConnectedPlayers--;
        UserCountChanged?.Invoke();
    }
}