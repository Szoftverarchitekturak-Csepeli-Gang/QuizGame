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

        //Just testing, statistics window code here!
        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.ShowQuestionDisplay(new Question());
    }

    public void Exit()
    {
        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.HideQuestionDisplay();
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
