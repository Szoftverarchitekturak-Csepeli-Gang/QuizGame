using UnityEngine;

/// <summary>
/// Singleton class to swap between screens of the application.
/// </summary>
public class ScreenManagerMobil : ScreenManagerBase
{
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;
    [SerializeField] GameObject QuizScreenPrefab;
    [SerializeField] GameObject WaitroomScreenPrefab;
    [SerializeField] GameObject FinishScreenPrefab;

    protected override void Start()
    {
        base.Start();
        CurrentScreen = AppScreen.MAIN;
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
        RegisterScreen(AppScreen.QUIZ, LoadPrefab(QuizScreenPrefab));
        RegisterScreen(AppScreen.WAITROOM, LoadPrefab(WaitroomScreenPrefab));
        RegisterScreen(AppScreen.FINISH, LoadPrefab(FinishScreenPrefab));
    }
}
