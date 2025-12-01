using UnityEngine;

public class QuestionState : IGameState
{
    public GameStateType Type => GameStateType.Question;

    private float _timer;
    private float _questionTime = 25.0f;

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

        AudioManager.Instance.PlayQuestionStateSound();

        await RoomManager.Instance.StartNextRound();
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();

        if (RoomManager.Instance.CheckSuccess(RaycastManager.Instance.CurrentSelectedVillage.GetComponent<VillageController>().SuccessThreshold))
            GameDataManager.Instance.RightAnswers++;
            
        RoomManager.Instance.QuestionReceived -= HandleQuestionReceived;
        RoomManager.Instance.questionPhaseEnded -= Exit;

        AudioManager.Instance.StopBackgroundSound();

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
}
