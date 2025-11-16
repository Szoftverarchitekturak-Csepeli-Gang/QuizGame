public class VillageSelectedState : IGameState
{
    public GameStateType Type => GameStateType.VillageSelected;

    public void Enter()
    {
        var village = RaycastManager.Instance.CurrentSelectedVillage;
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.DisableInputControl();
        BlurManager.Instance.ActivateBlurEffect();
        GameScreenPresenter.Instance.ShowVillagePanel(village, HandleConquer, HandleExit);
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideVillagePanel();
    }

    public void Update()
    {
        return;
    }

    private void HandleConquer()
    {
        GameStateManager.Instance.ChangeState(GameStateType.VillageConquer);
    }

    private void HandleExit()
    {
        GameStateManager.Instance.ChangeState(GameStateType.Idle);
    }
}
