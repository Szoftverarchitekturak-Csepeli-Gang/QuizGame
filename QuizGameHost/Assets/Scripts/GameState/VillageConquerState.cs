using UnityEngine;

public class VillageConquerState : IGameState
{
    public GameStateType Type => GameStateType.VillageConquer;

    private float _timer;

    public void Enter()
    {
        _timer = 0.0f;
        BlurManager.Instance.DeactivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        AudioManager.Instance.PlayCameraFlySound();
    }

    public void Exit()
    {
        return;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 3.0f) 
        {
            GameStateManager.Instance.ChangeState(GameStateType.Question);
        }
    }
}
