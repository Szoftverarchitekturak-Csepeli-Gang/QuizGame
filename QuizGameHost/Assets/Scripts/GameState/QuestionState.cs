using UnityEngine;

public class QuestionState : IGameState
{
    public GameStateType Type => GameStateType.Question;

    private float _timer;
    private float _questionTime = 5.0f;

    public void Enter()
    {
        _timer = 0.0f;
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);

        //Todo: Networking request question here?
        //Todo: Subsbscibe: Setup network manager websocket handler to connect received data with UI???? 
        //OnQuestionReceived, OnClientAnswerReceived -> Need handlers on the UI side as well

        GameScreenPresenter.Instance.InitTimer(_questionTime);
        GameScreenPresenter.Instance.ShowQuestionPanel(new Question("Ingredient of cheese", new string[] { "moon", "milk", "flour", "rock" }));

        // TODO: get question index from server
        GameScreenPresenter.Instance.SetCurrentQuestionIndex(2, 10);
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();

        //Todo: Success/Fuilure???

        bool success = true;

        if(success)
            GameDataManager.Instance.RightAnswers++;

        //Todo: Network handler unsubscribe
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        GameScreenPresenter.Instance.HandleQuestionTimer(_timer);

        if (_timer > _questionTime)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Fight);
        }
    }

    private void HandleClientAnswerReceived()
    {
        //Call UI update + Save answers???
    }
}
