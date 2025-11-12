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

    public static event Action<int> CorrectAnswerIdx;
    public static void RaiseCorrectAnswerIdx(int idx) => CorrectAnswerIdx?.Invoke(idx);

    public static event Action<int, int> GameEnded;
    public static void RaiseGameEnded(int rightAnswers, int totalQuestions) => GameEnded?.Invoke(rightAnswers, totalQuestions);

}
