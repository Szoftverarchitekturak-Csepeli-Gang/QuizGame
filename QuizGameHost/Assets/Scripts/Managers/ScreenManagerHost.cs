using UnityEngine;

public class ScreenManagerHost : ScreenManagerBase
{
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;
    [SerializeField] GameObject DashboardScreenPrefab;

    protected override void Start()
    {
        base.Start();
        ShowScreen(AppScreen.DASHBOARD);
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
        RegisterScreen(AppScreen.DASHBOARD, LoadPrefab(DashboardScreenPrefab));
    }
}
