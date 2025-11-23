using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager : SingletonBase<BattleManager>
{
    public event Action<bool> OnFightFinished;
    public event Action<BattleResult> OnBattleFinished;
    public event Action<BattleResult> OnBattleEarlyFinished;
    private readonly List<SoldierController> _attackers = new();
    private readonly List<SoldierController> _defenders = new();

    private int _returnAssigned = 0;
    private bool _battleRunning;
    private bool _battleFinished;
    private VillageController _currentVillage;

    private bool _enableBattleFinishTimer = false;
    private float _battleFinishTimer = 0f; //This is a safe timer, in case soldiers get stuck and battle never ends
    private float _maxBattleFinishTime = 20f;
    private void Update()
    {
        if (_enableBattleFinishTimer && _battleFinished)
        { 
            _battleFinishTimer += Time.deltaTime;

            if (_battleFinishTimer > _maxBattleFinishTime)
            { 
                Debug.Log("Battle End Timer Finished");
                CheckBattleEnd(true);
                DisableBattleEndTimer();
            }
        }
    }

    private void CleanupPreviousBattle()
    {
        foreach (var attacker in _attackers)
            if (attacker != null)
                Destroy(attacker.gameObject);

        foreach (var defender in _defenders)
            if (defender != null)
                Destroy(defender.gameObject);
    }

    private void SpawnSoldiers(Team team, VillageController village, bool boostDamage)
    {
        var count = team == Team.Attacker ? village.Config.attackerCount : village.Config.defenderCount;
        var spawnPointParent = team == Team.Attacker ? village.AttackerSpawnPointParent : village.DefenderSpawnPointParent;
        var prefab = team == Team.Attacker ? village.Config.attackerPrefab : village.Config.defenderPrefab;

        for (int i = 0; i < count; i++)
        {
            var spawnIndex = i % spawnPointParent.transform.childCount;
            var spawnPoint = spawnPointParent.transform.GetChild(spawnIndex);
            var go = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            go.transform.parent = village.CharacterContainer.transform;

            var soldier = go.GetComponent<SoldierController>();
            soldier.Init(team, this);
            soldier.SetDamagePerHit(10f * (boostDamage ? 1.5f : 1.0f));
            soldier.SetTeamMarkerMaterial(team == Team.Attacker);

            if (team == Team.Attacker)
                _attackers.Add(soldier);
            else
                _defenders.Add(soldier);
        }
    }

    public void StartBattle(VillageController village, bool victory)
    {
        CleanupPreviousBattle();

        SpawnSoldiers(Team.Attacker, village, victory);
        SpawnSoldiers(Team.Defender, village, !victory);  

        _currentVillage = village;
        _battleRunning = true;
        _battleFinished = false;
    }

    public bool IsBattleRunning => _battleRunning;
    public bool IsBattleFinished => _battleFinished;

    public SoldierController GetClosestEnemy(SoldierController self)
    {
        var soldiers = self.Team == Team.Attacker ? _defenders : _attackers;

        SoldierController best = null;
        float bestDist = float.MaxValue;

        foreach (var soldier in soldiers)
        {
            if (soldier == null || soldier.IsDead)
                continue;

            float dist = Vector3.Distance(self.transform.position, soldier.transform.position);

            if (dist < bestDist)
            {
                bestDist = dist;
                best = soldier;
            }
        }

        return best;
    }

    public Transform GetReturnSpawnPoint(SoldierController self)
    {
        var returnPointCount = _currentVillage.DefenderSpawnPointParent.transform.childCount;
        var returnPointIndex = _returnAssigned++;
        _returnAssigned = _returnAssigned % returnPointCount;

        return _currentVillage.DefenderSpawnPointParent.transform.GetChild(returnPointIndex);
    }

    public void OnSoldierDied(SoldierController soldier)
    {
        CheckBattleFinished();
    }

    public void OnSoldierArrived(SoldierController soldier)
    {
        CheckBattleEnd(false);
    }

    private void CheckBattleFinished()
    {
        if (_battleFinished) 
            return;

        int attackersAlive = _attackers.Count(a => a != null && !a.IsDead);
        int defendersAlive = _defenders.Count(d => d != null && !d.IsDead);

        if (attackersAlive > 0 && defendersAlive > 0)
            return;

        _battleFinished = true;

        OnFightFinished?.Invoke(AttackerWin());

        //Start timer, soldiers "might" get stuck, and BattleEnd will never be called
        //Tested soldiers seems like they never get stuck, but just in case
        EnableBattleEndTimer();
    }

    private bool AttackerWin()
    {
        int attackersAlive = _attackers.Count(a => a != null && !a.IsDead);
        int defendersAlive = _defenders.Count(d => d != null && !d.IsDead);
        return attackersAlive >= defendersAlive;
    }

    private void CheckBattleEnd(bool timerFinished)
    {
        int attackersAlive = _attackers.Count(a => a != null && !a.IsDead);
        int attackersArrived = _attackers.Count(a => a != null && !a.IsDead && a.ArrivedAtSpawn);
        bool allAttackerArrived = attackersArrived == attackersAlive;

        int defendersAlive = _defenders.Count(d => d != null && !d.IsDead);
        int defendersArrived = _defenders.Count(a => a != null && !a.IsDead && a.ArrivedAtSpawn);
        bool allDefenderArrived = defendersArrived == defendersAlive;

        bool attackerWin = AttackerWin();

        if (timerFinished || ((attackerWin && allAttackerArrived) || (!attackerWin && allDefenderArrived)))
        {
            _battleRunning = false;
            DisableBattleEndTimer();
            StartCoroutine(BattleEndSequence(attackerWin, attackersAlive, defendersAlive));
        }
    }

    private IEnumerator BattleEndSequence(bool attackerWin, int attackersAlive, int defendersAlive)
    {
        var result = new BattleResult
        {
            attackerWon = attackerWin,
            attackersAlive = attackersAlive,
            defendersAlive = defendersAlive
        };

        OnBattleEarlyFinished?.Invoke(result);

        yield return new WaitForSeconds(8f);

        CleanupPreviousBattle();
        OnBattleFinished?.Invoke(result);
    }

    public Vector3 GetRandomPointOnNavMesh(float radiusStart, float radiusEnd, VillageController village)
    {
        int sampleCounter = 0;
        bool success = false;
        Vector3 center = village.Ground.transform.position;
        Vector3 result = Vector3.zero;

        while (!success && sampleCounter < 100)
        {
            float radius = UnityEngine.Random.Range(radiusStart, radiusEnd);
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;

            Vector3 randomPos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 15.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                success = true;
            }

            sampleCounter++;
        }

        return result;
    }

    public void EnableBattleEndTimer()
    {
        _enableBattleFinishTimer = true;
        _battleFinishTimer = 0f;
        Debug.Log("Battle End Timer Enabled");
    }

    private void DisableBattleEndTimer()
    {
        _enableBattleFinishTimer = false;
        _battleFinishTimer = 0f;
        Debug.Log("Battle End Timer Disabled");
    }
}

