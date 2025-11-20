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
    [SerializeField] GameObject AnswerSentScreenPrefab;

    protected override void Start()
    {
        base.Start();
        CurrentScreen = AppScreen.MAIN;

        NetworkManager.Instance.HostDisconnectedEvent += HandleHostDisconnected;
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
        RegisterScreen(AppScreen.QUIZ, LoadPrefab(QuizScreenPrefab));
        RegisterScreen(AppScreen.WAITROOM, LoadPrefab(WaitroomScreenPrefab));
        RegisterScreen(AppScreen.FINISH, LoadPrefab(FinishScreenPrefab));
        RegisterScreen(AppScreen.ANSWER_SENT, LoadPrefab(AnswerSentScreenPrefab));
    }

    protected void HandleHostDisconnected()
    {
        //TODO: Show some error message about the disconnected host
        CurrentScreen = AppScreen.MAIN;
    }
}
