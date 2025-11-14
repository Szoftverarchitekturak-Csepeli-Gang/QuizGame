using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IdleState : IGameState
{
    public GameStateType Type => GameStateType.Idle;

    public void Enter()
    {
        RaycastManager.Instance.OnVillageSelectChanged += NextState;
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.EnableInputControl();
        BlurManager.Instance.DeactivateBlurEffect();

        //_ctx.UI.HideAll();
    }

    public void Exit()
    {
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

