using UnityEngine.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using Assets.SharedAssets.Networking.Http;
using Assets.SharedAssets.Networking.Data;
using Assets.SharedAssets.Networking.Websocket;
using Assets.SharedAssets.Networking.Mappers;
using Assets.SharedAssets.Networking.Validators;

public class NetworkManager : SingletonBase<NetworkManager>
{
    public event Action<int> OnRoomCreated;
    public event Action<int> OnAnswerReceived;
    public event Action<string> OnSocketError;
    public event Action<QuestionDto> OnQuestionArrived;
    public event Action OnPlayerJoined;
    public event Action OnPlayerDisconnected;
    public event Action OnLoggedIn;

    public int UserID { get; private set; }
    public string JWT { get; private set; }
    public string Username { get; private set; }

    public IUnitySocketIOClient socketIOClient { get; private set; }
    public IUnityHttpClient HttpClient { get; private set; }

    [SerializeField] private string _apiBaseUrl = "http://localhost:8080";

    private async void Awake()
    {
        base.Awake();

        UserID = -1;
        JWT = "";

        HttpClient = new UnityHttpClient(_apiBaseUrl);
    }

    private async void StartSocket()
    {
        socketIOClient = new UnitySocketIOClient();
        await socketIOClient.ConnectAsync(_apiBaseUrl);

        socketIOClient.On<int>("roomCreated", roomId => OnRoomCreated?.Invoke(roomId));
        socketIOClient.On<object>("clientConnected", _ => OnPlayerJoined?.Invoke());
        socketIOClient.On<object>("clientDisconnected", _ => OnPlayerDisconnected?.Invoke());
        socketIOClient.On<QuestionDto>("newQuestion", (question) => OnQuestionArrived?.Invoke(question));
        socketIOClient.On<string>("error", message => SocketErrorHandler(message));
        socketIOClient.On<int>("answerReceived", answer => OnAnswerReceived?.Invoke(answer - 1));
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
        catch (Exception ex)
        {
            Debug.Log($"[Network] Failed to create room: {ex}");
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
            Debug.Log($"[Network] Failed to leave room: {ex}");
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
            Debug.Log($"[Network] Failed to start game: {ex}");
        }
    }

    public async Task FinishGame()
    {
        try
        {
            await socketIOClient.SendAsync<object>("gameFinished", null);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Failed to finish game: {ex}");
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
            Debug.Log($"[Network] Failed to start game: {ex}");
        }
    }

    public async Task<Response<List<QuestionBank>>> GetQuestionBanks(string search, int ownerId)
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

            var questionBankDtos = await HttpClient.GetAsync<List<QuestionBankDto>>(endpoint, JWT);
            var questionBanks = QuestionBankMapper.ToModelList(questionBankDtos);

            return Response<List<QuestionBank>>.Success(questionBanks);
        }
        catch (Exception ex)
        {
            return Response<List<QuestionBank>>.Failure(ex.Message.Split(":")[1]);
        }
    }

    public async Task<Response<bool>> DeleteQuestionBank(int questionBankId)
    {
        try
        {
            await HttpClient.DeleteAsync($"/questionbanks/{questionBankId}", JWT);
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Delete failed: {ex.Message}");
            return Response<bool>.Failure(ex.Message.Split(":")[1]);
        }

    }

    public async Task<QuestionBankDto> GetQuestionBankWithId(int id)
    {
        var questionBank = await HttpClient.GetAsync<QuestionBankDto>($"/questionbanks/{id}", JWT);
        return questionBank;
    }

    public async Task<Response<QuestionBankDto>> CreateQuestionBank(int owner_id, string title, List<Question> questions, bool isPublic = false)
    {
        var questionBankDto = new QuestionBankDto
        {
            OwnerId = owner_id,
            Title = title,
            Questions = QuestionMapper.ToDtoList(questions),
            IsPublic = isPublic
        };

        var errors = ValidateQuestionBankDto(questionBankDto);

        if (errors.Count > 0)
        {
            return Response<QuestionBankDto>.Failure(errors[0]);
        }

        try
        {
            var response = await HttpClient.PostAsync<QuestionBankDto, QuestionBankDto>($"/questionbanks", questionBankDto, JWT);
            return Response<QuestionBankDto>.Success(response);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Create failed: {ex.Message}");
            return Response<QuestionBankDto>.Failure(ex.Message.Split(":")[1]);
        }
    }

    public async Task<Response<QuestionBankDto>> UpdateQuestionBank(int id, int owner_id, string title, List<Question> questions, bool isPublic)
    {
        var questionBankDto = new QuestionBankDto
        {
            Id = id,
            OwnerId = owner_id,
            Title = title,
            Questions = QuestionMapper.ToDtoList(questions),
            IsPublic = isPublic
        };

        var errors = ValidateQuestionBankDto(questionBankDto);

        if (errors.Count > 0)
        {
            return Response<QuestionBankDto>.Failure(errors[0]);
        }

        try
        {
            var response = await HttpClient.PutAsync<QuestionBankDto, QuestionBankDto>($"/questionbanks/{id}", questionBankDto, JWT);
            return Response<QuestionBankDto>.Success(response);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Update failed: {ex.Message}");
            return Response<QuestionBankDto>.Failure(ex.Message.Split(":")[1]);
        }
    }

    public async Task<Response<List<Question>>> GetQuestionsFromBank(int bankId)
    {
        try
        {
            var questionDtos = await HttpClient.GetAsync<List<QuestionDto>>($"/questionbanks/{bankId}/questions", JWT);
            var questions = QuestionMapper.ToModelList(questionDtos);
            return Response<List<Question>>.Success(questions);
        }
        catch (Exception ex)
        {
            return Response<List<Question>>.Failure(ex.Message.Split(":")[1]);
        }
    }

    public async Task<Response<AuthResponse>> RegisterUser(string username, string password)
    {
        var request = new AuthRequest
        {
            Username = username,
            Password = password,
        };

        try
        {
            var response = await HttpClient.PostAsync<AuthRequest, AuthResponse>($"/register", request);
            UserID = response.User.Id;
            Username = username;
            JWT = response.Token;
            StartSocket();
            OnLoggedIn?.Invoke();
            return Response<AuthResponse>.Success(response);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Create failed: {ex.Message}");
            return Response<AuthResponse>.Failure(ex.Message.Split(":")[1]);
        }
    }

    public async Task<Response<AuthResponse>> Login(string username, string password)
    {
        var request = new AuthRequest
        {
            Username = username,
            Password = password,
        };

        try
        {
            var response = await HttpClient.PostAsync<AuthRequest, AuthResponse>($"/login", request);
            UserID = response.User.Id;
            Username = username;
            JWT = response.Token;
            StartSocket();
            OnLoggedIn?.Invoke();
            return Response<AuthResponse>.Success(response);
        }
        catch (Exception ex)
        {
            Debug.Log($"[Network] Login failed: {ex.Message}");
            return Response<AuthResponse>.Failure(ex.Message.Split(":")[1]);
        }


    }

    public async Task Logout()
    {
        UserID = -1;
        JWT = "";
        await socketIOClient.DisconnectAsync();
    }

    private List<string> ValidateQuestionBankDto(QuestionBankDto questionBankDto)
    {
        var errors = QuestionBankDtoValidator.Validate(questionBankDto);

        if (errors.Count > 0)
        {
            Debug.Log(string.Join("\n", errors));
        }

        foreach (var question in questionBankDto.Questions)
        {
            errors = QuestionDtoValidator.Validate(question);

            if (errors.Count > 0)
            {
                Debug.Log(string.Join("\n", errors));
            }
        }

        return errors;
    }

    private void SocketErrorHandler(string errorMessage)
    {
        Debug.Log("[Socket] Error: " + errorMessage);
        OnSocketError?.Invoke(errorMessage);
    }
}
