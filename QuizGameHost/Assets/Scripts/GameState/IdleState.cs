using UnityEngine;

public class IdleState : IGameState
{
    public GameStateType Type => GameStateType.Idle;

    public void Enter()
    {
        RaycastManager.Instance.ResetSelectedVillage();
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.EnableInputControl();
        BlurManager.Instance.DeactivateBlurEffect();
        AudioManager.Instance.PlayGameBackgroundSound();

        RaycastManager.Instance.OnVillageSelectChanged += NextState;
    }

    public void Exit()
    {
        AudioManager.Instance.StopBackgroundSound();
        RaycastManager.Instance.OnVillageSelectChanged -= NextState;
    }

    private void NextState(GameObject village)
    {
        if(village == null) 
            return;

        GameStateManager.Instance.ChangeState(GameStateType.VillageSelected);
    }

    public void Update()
    {
        return;
    }
}

