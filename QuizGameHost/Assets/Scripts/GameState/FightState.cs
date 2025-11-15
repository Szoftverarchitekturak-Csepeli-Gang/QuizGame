using System.Collections;
using UnityEngine;

public class FightState : IGameState
{
    public GameStateType Type => GameStateType.Fight;

    private float _timer;
    public void Enter()
    {
        _timer = 0.0f;
        BattleManager.Instance.OnBattleFinished += HandleBattleFinished;
        BattleManager.Instance.OnBattleEarlyFinished += HandleBattleEarlyFinished;

        RaycastManager.Instance.DisableRaycast(); //To prevent changing current village!
        BlurManager.Instance.DeactivateBlurEffect();
        InputManager.Instance.EnableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        BattleManager.Instance.StartCoroutine(StartBattleDelayed());
    }

    public void Exit()
    {
        BattleManager.Instance.OnBattleFinished -= HandleBattleFinished;
        BattleManager.Instance.OnBattleEarlyFinished -= HandleBattleEarlyFinished;
        GameScreenPresenter.Instance.HideBattleEndPanel();
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
        GameStateManager.Instance.ChangeState(GameStateType.Statistics);
    }

    public void HandleBattleEarlyFinished(BattleResult result)
    {
        var villagePos = RaycastManager.Instance.CurrentSelectedVillage.transform.position + new Vector3(0.0f, 5.0f, 0.0f);

        if (result.attackerWon)
            ParticleManager.Instance.PlayVictoryParticleSystem(villagePos);
        else
            ParticleManager.Instance.PlayDefeatParticleSystem(villagePos);

        GameScreenPresenter.Instance.ShowBattleEndPanel(result.attackerWon);
    }
}
