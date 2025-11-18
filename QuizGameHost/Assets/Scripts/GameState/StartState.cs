using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StartState : IGameState
{
    public GameStateType Type => GameStateType.Start;

    bool _init = true; //First time no need to wait 2 seconds for camera movement!

    public void Enter()
    {
        RaycastManager.Instance.ResetSelectedVillage();
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.DisableInputControl();
        BlurManager.Instance.DeactivateBlurEffect();
        GameStateManager.Instance.StartCoroutine(NextStateDelayed());
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
    private IEnumerator NextStateDelayed()
    {
        float delay = _init ? 0f : 2f;
        _init = false;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        GameStateManager.Instance.ChangeState(GameStateType.CheckGameEnded);
    }
}
