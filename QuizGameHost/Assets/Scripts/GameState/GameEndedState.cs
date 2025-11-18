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

        GameEndScreenPresenter.Instance.ShowGameEndScreen(
            GameDataManager.Instance.RightAnswers,
            GameDataManager.Instance.TotalQuestions,
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
