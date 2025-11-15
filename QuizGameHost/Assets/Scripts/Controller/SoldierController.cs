using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum Team { Attacker, Defender }

public class SoldierController : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    [SerializeField] private float damagePerHit = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private Team _team;
    private BattleManager _battle;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _returnPoint;
    private float _attackTimer;
    private bool _dead;
    private bool _returningToSpawn;
    private bool _arrivedAtSpawn;

    public Team Team => _team;
    public bool IsDead => _dead;
    public bool ArrivedAtSpawn => _arrivedAtSpawn;

    private int idleAnimType, walkAnimType, runAnimType;

    public void Init(Team team, BattleManager battle)
    {
        _dead = false;
        _returningToSpawn = false;
        _arrivedAtSpawn = false;

    _team = team;
        _battle = battle;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        idleAnimType = UnityEngine.Random.Range(0, 2);
        walkAnimType = UnityEngine.Random.Range(0, 2);
        runAnimType = UnityEngine.Random.Range(0, 2);

        _animator.SetFloat("IdleType", idleAnimType);
        _animator.SetFloat("WalkType", walkAnimType);
        _animator.SetFloat("RunType", runAnimType);
    }

    private void Update()
    {
        if (_dead)
            return;

        if (_battle.IsBattleRunning)
        {
            if (!_battle.IsBattleFinished)
            {
                var target = _battle.GetClosestEnemy(this);

                if (target == null) return;

                var dist = Vector3.Distance(transform.position, target.transform.position);

                if (dist > attackRange)
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(target.transform.position);
                    _animator.SetFloat("MoveType", _agent.velocity.magnitude);
                }
                else
                {
                    _agent.isStopped = true;
                    _attackTimer -= Time.deltaTime;
                    
                    //Todo: Animation end event
                    if (_attackTimer <= 0f)
                    {
                        _attackTimer = attackCooldown;
                        target.TakeDamage(damagePerHit);
                        _animator.SetInteger("AttackType", UnityEngine.Random.Range(0, 3));
                        _animator.SetTrigger("AttackTriggered");
                    }
                }
            }
            else
            {
                if (!_returningToSpawn)
                {
                    _returnPoint = _battle.GetReturnSpawnPoint(this);
                    _returningToSpawn = true;
                    _arrivedAtSpawn = false;
                    _agent.isStopped = false;
                    _agent.SetDestination(_returnPoint.position);
                }
                else
                {
                    if (!_arrivedAtSpawn)
                    {
                        var dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_returnPoint.position.x, 0, _returnPoint.position.z));
                        
                        if (dist > 1f)
                        {
                            _agent.isStopped = false;
                            _animator.SetFloat("MoveType", _agent.velocity.magnitude);
                        }
                        else
                        {
                            _arrivedAtSpawn = true;
                            _agent.isStopped = false;
                            _animator.SetInteger("DanceType", UnityEngine.Random.Range(0, 5));
                            _animator.SetTrigger("DanceTriggered");
                            _battle.OnSoldierArrived(this);
                        }
                    }
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (_dead) 
            return;

        hp -= amount;

        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Soldier Died");
        _dead = true;
        _agent.isStopped = true;
        _animator.SetInteger("DeathType", UnityEngine.Random.Range(0, 3));
        _animator.SetTrigger("DeathTriggered");
        _battle.OnSoldierDied(this);
    }
}
