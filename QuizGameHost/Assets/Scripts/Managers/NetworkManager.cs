using Assets.Scripts.Networking.Data;
using Assets.Scripts.Networking.Http;
using Assets.Scripts.Networking.Websocket;
using Assets.SharedAssets.Networking.Http;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.Searcher;
using SocketIOClient;
using Assets.SharedAssets.Networking.Websocket;
using PimDeWitte.UnityMainThreadDispatcher;

public class NetworkManager : SingletonBase<NetworkManager>
{
    public event Action<int> OnRoomCreated;
    public event Action<int> OnAnswerReceived;
    public event Action<string> OnSocketError;
    public event Action<QuestionDto> OnQuestionArrived;
    public event Action OnPlayerJoined;
    public event Action OnPlayerDisconnected;
    public IUnitySocketIOClient socketIOClient { get; private set; }
    public IUnityHttpClient HttpClient { get; private set; }

    [SerializeField] private string _apiBaseUrl = "http://localhost:8080";

    private async void Awake()
    {
        base.Awake();

        HttpClient = new UnityHttpClient(_apiBaseUrl);
        socketIOClient = new UnitySocketIOClient();
        await socketIOClient.ConnectAsync(_apiBaseUrl);

        socketIOClient.On<int>("roomCreated", roomId => OnRoomCreated?.Invoke(roomId));
        socketIOClient.On<object>("clientConnected", _ => OnPlayerJoined?.Invoke());
        socketIOClient.On<object>("clientDisconnected", _ => OnPlayerDisconnected?.Invoke());
        socketIOClient.On<QuestionDto>("newQuestion", (question) => OnQuestionArrived?.Invoke(question));
        socketIOClient.On<string>("error", message => SocketErrorHandler(message));
        socketIOClient.On<int>("answerReceived", answer => OnAnswerReceived?.Invoke(answer-1));
    }
    private async void OnDestroy()
    {
        await socketIOClient.DisconnectAsync();
    }

    public async Task CreateRoom(int questionBankId)
    {
        try
        {
            await socketIOClient.SendAsync<int>("createRoom", questionBankId);
        }
        catch(Exception ex)
        {
            Debug.LogError($"[Network] Failed to create room: {ex}");
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

    public async Task StartGame()
    {
        try
        {
            await socketIOClient.SendAsync<object>("gameStarted", null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Failed to start game: {ex}");
        }
    }

    public async Task StartNextRound(int questionIndex)
    {
        try
        {
            await socketIOClient.SendAsync<int>("nextRoundStarted", questionIndex);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Failed to start game: {ex}");
        }
    }

    public async Task<Response<List<QuestionBankDto>>> GetQuestionBanks(string search, int ownerId)
    {
        try
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (ownerId != -1)
                query["ownerId"] = ownerId.ToString();
            else
                query["title"] = search;

            var endpoint = "/questionbanks";
            endpoint += $"?{query}";

            var questionBanks = await HttpClient.GetAsync<List<QuestionBankDto>>(endpoint);

            return Response<List<QuestionBankDto>>.Success(questionBanks);
        }
        catch(Exception ex)
        {
            return Response<List<QuestionBankDto>>.Failure(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteQuestionBank(int questionBankId)
    {
        try
        {
            await HttpClient.DeleteAsync($"/questionbanks/{questionBankId}");
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Delete failed: {ex.Message}");
            return Response<bool>.Failure(ex.Message);
        }

    }

    public async Task<QuestionBankDto> GetQuestionBankWithId(int id)
    {
        var questionBank = await HttpClient.GetAsync<QuestionBankDto>($"/questionbanks/{id}");
        return questionBank;
    }

    public async Task<QuestionBankDto> CreateQuestionBank(int owner_id, string title, bool isPublic)
    {
        if(owner_id < 0)
        {
            Debug.LogError($"Invalid data: owner id cannot be negative!");
            return null;
        }
        else if(title.Length < 1)
        {
            Debug.LogError($"Invalid data: title cannot be empty!");
            return null;
        }

            var questionBank = new QuestionBankDto
            {
                ownerId = owner_id,
                title = title,
                @public = isPublic
            };

        try
        {
            var response = await HttpClient.PostAsync<QuestionBankDto, QuestionBankDto>($"/questionbanks", questionBank);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Create failed: {ex.Message}");
            return null;
        }
    }

    public async Task<QuestionBankDto> UpdateQuestionBank(int id, int owner_id, string title, bool isPublic)
    {
        if (owner_id < 0)
        {
            Debug.LogError($"Invalid data: owner id cannot be negative!");
            return null;
        }
        else if (title.Length < 1)
        {
            Debug.LogError($"Invalid data: title cannot be empty!");
            return null;
        }

        var questionBank = new QuestionBankDto
        {
            id = id,
            ownerId = owner_id,
            title = title,
            @public = isPublic
        };

        try
        {
            var response = await HttpClient.PutAsync<QuestionBankDto, QuestionBankDto>($"/questionbanks/{id}", questionBank);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Update failed: {ex.Message}");
            return null;
        }
    }

    public async Task<Response<List<QuestionDto>>> GetQuestionsFromBank(int bankId)
    {
        try
        {
            var questions = await HttpClient.GetAsync<List<QuestionDto>>($"/questionbanks/{bankId}/questions");
            return Response<List<QuestionDto>>.Success(questions);
        }
        catch (Exception ex)
        {
            return Response<List<QuestionDto>>.Failure(ex.Message);
        }
    }

    private void SocketErrorHandler(string errorMessage)
    {
        Debug.Log("[Socket] Error: " + errorMessage);
        OnSocketError?.Invoke(errorMessage);
    }
}
