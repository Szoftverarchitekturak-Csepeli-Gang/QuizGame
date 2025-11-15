using UnityEngine;

public class QuestionState : IGameState
{
    public GameStateType Type => GameStateType.Question;

    private float _timer;

    public void Enter()
    {
        _timer = 0.0f;
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);

        //Todo: Networking request question here?
        //Todo: Subsbscibe: Setup network manager websocket handler to connect received data with UI???? 
        //OnQuestionReceived, OnClientAnswerReceived -> Need handlers on the UI side as well

        GameScreenPresenter.Instance.ShowQuestionPanel(new Question());
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();

        //Todo: Network handler unsubscribe
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 5.0f)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Fight);
        }
    }

    private void HandleClientAnswerReceived()
    {
        //Call UI update + Save answers???
    }
}
