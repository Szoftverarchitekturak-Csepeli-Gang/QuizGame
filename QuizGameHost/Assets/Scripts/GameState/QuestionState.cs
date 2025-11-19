using UnityEngine;

public class QuestionState : IGameState
{
    public GameStateType Type => GameStateType.Question;

    private float _timer;
    private float _questionTime = 10.0f;

    private bool _questionArrived;

    public async void Enter()
    {
        _timer = 0.0f;
        _questionArrived = false;
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        RoomManager.Instance.QuestionReceived += HandleQuestionReceived;
        RoomManager.Instance.questionPhaseEnded += Exit;

        await RoomManager.Instance.StartNextRound();

        //Todo: Subsbscibe: Setup network manager websocket handler to connect received data with UI???? 

        //OnQuestionReceived, OnClientAnswerReceived -> Need handlers on the UI side as well
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();

        int correctAnswer = RoomManager.Instance.CurrentQuestion.CorrectAnswerIdx;
        int correctSubmissions = RoomManager.Instance.playerAnswers[correctAnswer];
        int playerCount = RoomManager.Instance.ConnectedPlayers;

        bool success = playerCount == 0 ? false : correctSubmissions / (float)playerCount > 0.75;

        //TODO: victory panel shows even if success was false
        if(success)
            GameDataManager.Instance.RightAnswers++;

        RoomManager.Instance.QuestionReceived -= HandleQuestionReceived;
        RoomManager.Instance.questionPhaseEnded -= Exit;

        _timer = _questionTime;

    }

    public void Update()
    {
        if(_questionArrived)
            _timer += Time.deltaTime;

        GameScreenPresenter.Instance.HandleQuestionTimer(_timer);

        if (_timer > _questionTime)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Fight);
        }
    }

    private void HandleQuestionReceived(Question newQuestion)
    {
        _questionArrived = true;
        GameScreenPresenter.Instance.InitTimer(_questionTime);
        GameScreenPresenter.Instance.ShowQuestionPanel(newQuestion);
        GameScreenPresenter.Instance.SetCurrentQuestionIndex(RoomManager.Instance.RoundCounter, RoomManager.Instance.QuestionCount);
    }

    private void HandleClientAnswerReceived()
    {
        //Call UI update + Save answers???
    }
}
