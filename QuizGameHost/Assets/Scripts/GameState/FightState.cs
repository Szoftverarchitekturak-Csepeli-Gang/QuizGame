using System.Collections;
using UnityEngine;

public class FightState : IGameState
{
    public GameStateType Type => GameStateType.Fight;

    private float _timer;
    public void Enter()
    {
        BattleManager.Instance.OnBattleFinished += HandleBattleFinished;
        BattleManager.Instance.OnBattleEarlyFinished += HandleBattleEarlyFinished;
        RaycastManager.Instance.DisableRaycast(); //To prevent changing current village!
        BlurManager.Instance.DeactivateBlurEffect();
        InputManager.Instance.EnableInputControl();
        CameraManager.Instance.UseVillageCamera(RaycastManager.Instance.CurrentSelectedVillage);
        BattleManager.Instance.StartCoroutine(StartBattleDelayed());
        AudioManager.Instance.PlayFightStartSound();
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
    }

    private IEnumerator StartBattleDelayed()
    {
        yield return new WaitForSeconds(1.5f);

        bool victory = RoomManager.Instance.CheckSuccess(RaycastManager.Instance.CurrentSelectedVillage.GetComponent<VillageController>().SuccessThreshold);
        BattleManager.Instance.StartBattle(RaycastManager.Instance.CurrentSelectedVillage.GetComponent<VillageController>(), victory);
    }

    public void HandleBattleFinished(BattleResult result)
    {
        if (result.attackerWon)
            VillageManager.Instance.VillageConquered(RaycastManager.Instance.CurrentSelectedVillage.GetComponent<VillageController>());

        GameStateManager.Instance.ChangeState(GameStateType.Statistics);
    }

    public void HandleBattleEarlyFinished(BattleResult result)
    {
        var villagePos = RaycastManager.Instance.CurrentSelectedVillage.transform.position + new Vector3(0.0f, 5.0f, 0.0f);

        if (result.attackerWon)
        {
            ParticleManager.Instance.PlayVictoryParticleSystem(villagePos);
            AudioManager.Instance.PlayVictorySound();
        }
        else
        {
            ParticleManager.Instance.PlayDefeatParticleSystem(villagePos);
            AudioManager.Instance.PlayDefeatSound();
        }

        GameScreenPresenter.Instance.ShowBattleEndPanel(result.attackerWon);

    }
}
