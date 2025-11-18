using UnityEngine;

public class CheckGameEndedState : IGameState
{
    public GameStateType Type => GameStateType.CheckGameEnded;

    public void Enter()
    {
        RaycastManager.Instance.ResetSelectedVillage();
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.DisableInputControl();
        BlurManager.Instance.DeactivateBlurEffect();

        bool gameEnded = false;
        gameEnded |= VillageManager.Instance.AllVillageConquered;
        gameEnded |= (RoomManager.Instance.RoundCounter >= RoomManager.Instance.QuestionCount);

        if (gameEnded)
            GameStateManager.Instance.ChangeState(GameStateType.GameEnded);
        else
            GameStateManager.Instance.ChangeState(GameStateType.Idle);
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
