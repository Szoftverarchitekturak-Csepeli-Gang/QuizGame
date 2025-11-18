using UnityEngine;

public class GameDataManager : SingletonBase<GameDataManager>
{
    public int RightAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public int ConqueredVillages { get; set; }
    public int TotalVillages { get; set; }

    private void Awake()
    {
        base.Awake();

        RightAnswers = 0;
        TotalQuestions = 15; //Todo change this to bank size
        ConqueredVillages = 0;
        TotalVillages = GameObject.FindGameObjectsWithTag("Village").Length;
    }
}
