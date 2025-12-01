using Assets.SharedAssets.Networking.Data;
using Assets.SharedAssets.Networking.Http;
using Assets.SharedAssets.Networking.Mappers;
using Assets.SharedAssets.Networking.Websocket;
using Newtonsoft.Json.Linq;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RoomManager : SingletonBase<RoomManager>
{
    public event Action RoomCreated;
    public event Action<Question> QuestionReceived;
    public event Action<int> AnswerReceived;
    public event Action UserCountChanged;
    public event Action questionPhaseEnded;
    public int RoomID { get; private set; }
    public int ConnectedPlayers { get; private set; }
    public Question CurrentQuestion { get; private set; }

    public int RoundCounter { get; private set; }
    public int QuestionCount { get; private set; }
    public int[] playerAnswers { get; private set; }


    private void Start()
    {
        RoundCounter = 0;
        QuestionCount = 0;
        playerAnswers = new int[4];

        NetworkManager.Instance.OnRoomCreated += HandleRoomCreated;
        NetworkManager.Instance.OnPlayerJoined += ClientJoinedHandler;
        NetworkManager.Instance.OnPlayerDisconnected += ClientDisconnectedHandler;
        NetworkManager.Instance.OnQuestionArrived += QuestionReceiveHandler;
        NetworkManager.Instance.OnAnswerReceived += AnswerReceivedHandler;
    }

    public float[] GetAnswerPercentages()
    {
        float[] answerPercentages = playerAnswers
        .Select(count => 
        ConnectedPlayers > 0
        ? (count / (float)ConnectedPlayers)
        : 0f)
        .ToArray();

        return answerPercentages;
    }

    public async Task CreateRoom(int questionBankID)
    {
        RoundCounter = 0;
        ConnectedPlayers = 0;
        UserCountChanged?.Invoke();
        await NetworkManager.Instance.CreateRoom(questionBankID);
        var response = await NetworkManager.Instance.GetQuestionsFromBank(questionBankID);
        if (response.IsSuccess) QuestionCount = response.Data.Count;
    }

    public async Task StartGame()
    {
        await NetworkManager.Instance.StartGame();
    }

    public async void FinishGame()
    {
        await NetworkManager.Instance.FinishGame();
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
        Array.Clear(playerAnswers, 0, playerAnswers.Length);
        RoundCounter++;
        Question newQuestion = QuestionMapper.ToModel(question);
        CurrentQuestion = newQuestion;
        UnityMainThreadDispatcher.Instance().Enqueue(() => QuestionReceived?.Invoke(newQuestion));
    }

    private void AnswerReceivedHandler(int answer)
    {
        playerAnswers[answer]++;
        UnityMainThreadDispatcher.Instance().Enqueue(() => AnswerReceived?.Invoke(answer));

        if(playerAnswers.Sum() >= ConnectedPlayers)
            UnityMainThreadDispatcher.Instance().Enqueue(() => questionPhaseEnded?.Invoke());
    }

    private void ClientDisconnectedHandler()
    {
        ConnectedPlayers--;
        UserCountChanged?.Invoke();
    }

    public bool CheckSuccess(float threshold)
    {
        int correctAnswer = CurrentQuestion.CorrectAnswerIdx;
        int correctSubmissions = playerAnswers[correctAnswer];
        int playerCount = ConnectedPlayers;

        return playerCount == 0 ? false : correctSubmissions / (float)playerCount > threshold;
    }
}