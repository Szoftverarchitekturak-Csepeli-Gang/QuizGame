using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEndedState : IGameState
{
    public GameStateType Type => GameStateType.GameEnded;

    public void Enter()
    {
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        RoomManager.Instance.FinishGame();

        GameEndScreenPresenter.Instance.ShowGameEndScreen(
            GameDataManager.Instance.RightAnswers,
            RoomManager.Instance.QuestionCount,
            GameDataManager.Instance.ConqueredVillages,
            GameDataManager.Instance.TotalVillages
        );
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
