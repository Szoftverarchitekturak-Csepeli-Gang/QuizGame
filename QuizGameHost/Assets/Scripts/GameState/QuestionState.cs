using Assets.Scripts.Networking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class QuestionState : IGameState
{
    public GameStateType Type => GameStateType.Question;

    private float _timer;

    public void Enter()
    {
        _timer = 0.0f;
        BlurManager.Instance.ActivateBlurEffect();
        InputManager.Instance.DisableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);

        //Todo: Networking request question here?
        //Todo: Subsbscibe: Setup network manager websocket handler to connect received data with UI???? 
        //OnQuestionReceived, OnClientAnswerReceived -> Need handlers on the UI side as well

        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.ShowQuestionDisplay(new Question());
    }

    public void Exit()
    {
        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.HideQuestionDisplay();

        //Todo: Network handler unsubscribe
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 5.0f)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Fight);
        }
    }

    private void HandleClientAnswerReceived()
    {
        //Call UI update + Save answers???
    }
}
