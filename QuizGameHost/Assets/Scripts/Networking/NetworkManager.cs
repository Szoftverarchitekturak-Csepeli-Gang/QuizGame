using Assets.Scripts.Networking.Data;
using Assets.Scripts.Networking.Http;
using Assets.Scripts.Networking.Websocket;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEditor.Localization.Plugins.XLIFF.V12;

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

    public async Task<List<QuestionBankDto>> GetAllQuestionBanks()
    {
        var questionBanks = await HttpClient.GetAsync<List<QuestionBankDto>>($"/questionbanks");
        return questionBanks;
    }

    public async Task<List<QuestionBankDto>> GetUserQuestionBanks(int user_id)
    {
        var questionBanks = await HttpClient.GetAsync<List<QuestionBankDto>>($"/questionbanks/user/{user_id}");
        return questionBanks;
    }

    public async Task<QuestionBankDto> GetQuestionBankWithId(int id)
    {
        var questionBank = await HttpClient.GetAsync<QuestionBankDto>($"/questionbanks/{id}");
        return questionBank;
    }

    public async Task<bool> DeleteQuestionBank(int questionBankId)
    {
        if (questionBankId < 0)
        {
            Debug.LogError($"Invalid data: id cannot be negative!");
            return false;
        }

        try
        {
            await HttpClient.DeleteAsync($"/questionbanks/{questionBankId}");
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError($"[Network] Delete failed: {ex.Message}");
            return false;
        }
        
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
                owner_user_id = owner_id,
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
            owner_user_id = owner_id,
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

    public async Task<List<QuestionBankDto>> GetFilteredQuestionBanks(string title_filter)
    {
        var questionBanks = await HttpClient.GetAsync<List<QuestionBankDto>>($"/questionbanks/filter/{title_filter}");
        return questionBanks;
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
