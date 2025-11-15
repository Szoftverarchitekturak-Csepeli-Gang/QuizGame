public interface IGameState
{
    GameStateType Type { get; }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}