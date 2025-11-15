using System;
using System.Collections;
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
        BattleManager.Instance.OnBattleFinished += HandleBattleFinished;
        RaycastManager.Instance.DisableRaycast(); //To prevent changing current village!
        BlurManager.Instance.DeactivateBlurEffect();
        InputManager.Instance.EnableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        BattleManager.Instance.StartCoroutine(StartBattleDelayed());
    }

    public void Exit()
    {
        BattleManager.Instance.OnBattleFinished -= HandleBattleFinished;
        RaycastManager.Instance.EnableRaycast(); 
    }

    public void Update()
    {
        /*
        //Just for testing
        _timer += Time.deltaTime;
        if (_timer > 3.0f)
        {
            GameStateManager.Instance.ChangeState(GameStateType.Statistics);
        }
        */


    }

    private IEnumerator StartBattleDelayed()
    {
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.StartBattle(RaycastManager.Instance.CurrentSelectedVillage.GetComponent<VillageController>());
    }

    public void HandleBattleFinished(BattleResult result)
    {
        //ParticleSystem
        //....

        GameStateManager.Instance.ChangeState(GameStateType.Statistics);
    }
}
