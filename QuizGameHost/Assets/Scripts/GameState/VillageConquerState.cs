using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //Todo: Networking send conquer request???
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
