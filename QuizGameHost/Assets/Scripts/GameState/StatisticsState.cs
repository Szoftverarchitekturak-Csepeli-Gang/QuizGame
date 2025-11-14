using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StatisticsState : IGameState
{
    public GameStateType Type => GameStateType.Statistics;

    private float _timer;

    public void Enter()
    {
        _timer = 0.0f;
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);

        //Show UI
        //Set continue button action

        GameScreenPresenter.Instance.ShowQuestionPanel(new Question());
    }

    public void Exit()
    {
        GameScreenPresenter.Instance.HideQuestionPanel();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 5.0f)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Idle);
        }
    }

    private void Continue()
    {
        //Village conquered or not??? Set property
        //Raycast village to null!
    }
}
