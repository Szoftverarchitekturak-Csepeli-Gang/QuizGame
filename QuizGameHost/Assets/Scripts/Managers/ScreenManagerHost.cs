using UnityEngine;

public class ScreenManagerHost : ScreenManagerBase
{
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;
    [SerializeField] GameObject DashboardScreenPrefab;
    [SerializeField] GameObject WaitroomScreenPrefab;
    [SerializeField] GameObject GameScreenPrefab;
    [SerializeField] GameObject GameEndScreenPrefab;

    protected override void Start()
    {
        base.Start();
        CurrentScreen = AppScreen.MAIN;
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
        RegisterScreen(AppScreen.DASHBOARD, LoadPrefab(DashboardScreenPrefab));
        RegisterScreen(AppScreen.WAITROOM, LoadPrefab(WaitroomScreenPrefab));
        RegisterScreen(AppScreen.GAME, LoadPrefab(GameScreenPrefab));
        RegisterScreen(AppScreen.FINISH, LoadPrefab(GameEndScreenPrefab));
    }
}
