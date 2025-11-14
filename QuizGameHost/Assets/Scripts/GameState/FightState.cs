using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FightState : IGameState
{
    public GameStateType Type => GameStateType.Fight;

    private float _timer;
    public void Enter()
    {
        _timer = 0.0f;
        RaycastManager.Instance.DisableRaycast();
        BlurManager.Instance.DeactivateBlurEffect();
        InputManager.Instance.EnableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);

        //BattleManager.Instance
        //ParticleSystem
    }

    public void Exit()
    {
        RaycastManager.Instance.EnableRaycast();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 3.0f)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Statistics);
        }
    }

    public void HandleBattleEnd()
    { 
    
    }
}
