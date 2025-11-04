using UnityEngine;

public class ScreenManagerHost : ScreenManagerBase
{
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;
    [SerializeField] GameObject DashboardScreenPrefab;
    [SerializeField] GameObject WaitroomScreenPrefab;

    protected override void Start()
    {
        base.Start();
        ShowScreen(AppScreen.MAIN);
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
        RegisterScreen(AppScreen.DASHBOARD, LoadPrefab(DashboardScreenPrefab));
        RegisterScreen(AppScreen.WAITROOM, LoadPrefab(WaitroomScreenPrefab));
    }
}
