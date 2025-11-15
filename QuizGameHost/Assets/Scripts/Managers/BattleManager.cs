using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.STP;

public class BattleManager : SingletonBase<BattleManager>
{
    public event Action<BattleResult> OnBattleFinished;
    private readonly List<SoldierController> _attackers = new();
    private readonly List<SoldierController> _defenders = new();

    private bool _battleRunning;
    private VillageController _currentVillage;

    private void CleanupPreviousBattle()
    {
        foreach (var attacker in _attackers)
            if (attacker != null)
                Destroy(attacker.gameObject);

        foreach (var defender in _defenders)
            if (defender != null)
                Destroy(defender.gameObject);
    }

    private void SpawnSoldiers(Team team, VillageController village)
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

            if(team == Team.Attacker)
                _attackers.Add(soldier);
            else
                _defenders.Add(soldier);
        }
    }

    public void StartBattle(VillageController village)
    {
        CleanupPreviousBattle();

        SpawnSoldiers(Team.Attacker, village);
        SpawnSoldiers(Team.Defender, village);  

        _currentVillage = village;
        _battleRunning = true;
    }

    public bool IsBattleRunning => _battleRunning;

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

    public void OnSoldierDied(SoldierController soldier)
    {
        CheckBattleEnd();
    }

    private void CheckBattleEnd()
    {
        if (!_battleRunning) 
            return;

        int attackersAlive = _attackers.Count(a => a != null && !a.IsDead);
        int defendersAlive = _defenders.Count(d => d != null && !d.IsDead);

        if (attackersAlive > 0 && defendersAlive > 0)
            return;

        _battleRunning = false;

        StartCoroutine(BattleEndSequence(attackersAlive, defendersAlive));
    }

    private IEnumerator BattleEndSequence(int attackersAlive, int defendersAlive)
    {
        yield return new WaitForSeconds(2f);
        /*
        var config = _currentVillage.Config;

        bool attackerWin;
        // Példa logika: ha legalább X támadó él → támadó win, különben védő win
        if (attackersAlive >= config.minAttackersToSurviveForWin)
            attackerWin = true;
        else
            attackerWin = false;

        // összegyűjtjük a még élő katonákat
        var allSoldiers = _attackers.Concat(_defenders)
            .Where(s => s != null && !s.IsDead)
            .ToList();

        // középre futás
        var centerPoint = _currentVillage.transform.position; // vagy egy empty "DancePoint"
        foreach (var s in allSoldiers)
        {
            s.GoToCelebrate(centerPoint);
        }

        // kis várakozás, hogy odaérjenek
        yield return new WaitForSeconds(2f);

        // anim triggerei
        foreach (var s in allSoldiers)
        {
            if (attackerWin && s.Team == Team.Attacker ||
                !attackerWin && s.Team == Team.Defender)
            {
                s.PlayDanceAnimation();
            }
            else
            {
                s.PlayLaughAnimation();
            }
        }

        // még pár másodperc
        yield return new WaitForSeconds(2f);

        // particle
        var particlePrefab = attackerWin ? config.winParticlePrefab : config.loseParticlePrefab;
        if (particlePrefab != null)
        {
            Instantiate(particlePrefab, centerPoint, Quaternion.identity);
        }

        // eredmény visszajelzése
        var result = new BattleResult
        {
            attackerWon = attackerWin,
            attackersAlive = attackersAlive,
            defendersAlive = defendersAlive
        };

        OnBattleFinished?.Invoke(result);
        */
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
}

