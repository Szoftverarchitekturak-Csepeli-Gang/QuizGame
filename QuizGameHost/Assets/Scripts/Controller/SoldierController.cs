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
    private float _attackTimer;
    private bool _dead;

    public Team Team => _team;
    public bool IsDead => _dead;

    private int idleAnimType, walkAnimType, runAnimType;

    public void Init(Team team, BattleManager battle)
    {
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
        if (_dead || !_battle.IsBattleRunning)
            return;

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

            if (_attackTimer <= 0f)
            {
                _attackTimer = attackCooldown;
                target.TakeDamage(damagePerHit);
                _animator.SetInteger("AttackType", UnityEngine.Random.Range(0, 3));
                _animator.SetTrigger("AttackTriggered");
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
