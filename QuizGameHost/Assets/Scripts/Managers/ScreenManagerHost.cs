using UnityEngine;

public class ScreenManagerHost : ScreenManagerBase
{
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;

    protected override void Start()
    {
        base.Start();
        ShowScreen(AppScreen.MAIN);
    }

    protected override void LoadScreens()
    {
        RegisterScreen(AppScreen.MAIN, LoadPrefab(MainScreenPrefab));
    }
}
