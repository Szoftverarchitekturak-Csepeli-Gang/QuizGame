using System;
using UnityEngine;

public enum RoundResult
{
    VICTORY, DEFEAT
}

public class GameManager : SingletonBase<GameManager>
{
    public static event Action<RoundResult> GameRoundEnded;
    public static void RaiseGameRoundEnded(RoundResult result) => GameRoundEnded?.Invoke(result);

}
