using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameStateManager : SingletonBase<GameStateManager>
{
    public GameStateType InitialState = GameStateType.Idle;

    private Dictionary<GameStateType, IGameState> _states;

    private IGameState _currentState;

    public void Awake()
    {
        base.Awake();

        _states = new Dictionary<GameStateType, IGameState>
        {
            { GameStateType.Idle, new IdleState() },
            { GameStateType.VillageSelected, new VillageSelectedState() },
            { GameStateType.VillageConquer, new VillageConquerState() },
            { GameStateType.Question, new QuestionState() },
            { GameStateType.Fight, new FightState() },
            { GameStateType.Statistics, new StatisticsState() },
        };
    }

    private void Start()
    {
        ChangeState(InitialState, true);
    }

    private void Update()
    {
        _currentState.Update();
    }

    public void ChangeState(GameStateType newState)
    {
        ChangeState(newState, false);
    }

    private void ChangeState(GameStateType newState, bool init)
    {
        if (_currentState != null && _currentState.Type == newState)
            return;

        if(!init)
            _currentState.Exit();

        _currentState = _states[newState];
        _currentState.Enter();
    }
}
