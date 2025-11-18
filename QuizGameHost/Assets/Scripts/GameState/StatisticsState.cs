using UnityEngine;

public class StatisticsState : IGameState
{
    public GameStateType Type => GameStateType.Statistics;

    public void Enter()
    {
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        GameScreenPresenter.Instance.ShowStatisticsPanel(new Question("Ingredient of cheese", new string[] { "moon", "milk", "flour", "rock" }, 1), OnNext);
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();
        //VillageManager.Instance.SetAllVillageToConqueredTest();
    }

    public void Update()
    {
    }

    private void OnNext()
    {
        GameStateManager.Instance.ChangeState(GameStateType.Start);
    }
}
