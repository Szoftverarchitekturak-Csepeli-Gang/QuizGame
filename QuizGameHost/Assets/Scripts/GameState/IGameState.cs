using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameState
{
    GameStateType Type { get; }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}