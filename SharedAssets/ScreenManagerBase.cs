using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AppScreen
{
    MAIN, WAITROOM, QUIZ, FINISH, DASHBOARD, GAME
}

public abstract class ScreenManagerBase : SingletonBase<ScreenManagerBase>
{
    [SerializeField] protected Transform ScreenParent;

    protected readonly Dictionary<AppScreen, ScreenController> _screens = new();
    protected AppScreen _currentScreen;

    public AppScreen CurrentScreen
    {
        get { return _currentScreen; }
        set
        {
            ShowScreen(value);
            if (value == AppScreen.GAME && _currentScreen != AppScreen.GAME)
            {
                SetGameSceneActive(true);
            }
            else if (_currentScreen == AppScreen.GAME && value != AppScreen.GAME)
            {
                SetGameSceneActive(false);
            }
            _currentScreen = value;
        }
    }

    virtual protected void Start()
    {
        LoadScreens();
        HideAllScreens();
    }

    protected void SetGameSceneActive(bool isGameSceneActive)
    {
        if (isGameSceneActive)
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync("GameScene");
        }
    }

    protected void HideAllScreens()
    {
        foreach ((_, ScreenController obj) in _screens)
        {
            obj.IsVisible = false;
        }
    }

    public void ShowScreen(AppScreen screen)
    {
        if (_currentScreen != screen && _screens.TryGetValue(_currentScreen, out ScreenController curScreen))
        {
            curScreen.IsVisible = false;
        }

        if (_screens.TryGetValue(screen, out ScreenController newScreen))
        {
            newScreen.IsVisible = true;
        }
    }

    protected void RegisterScreen(AppScreen screen, ScreenController screenController) => _screens[screen] = screenController;

    abstract protected void LoadScreens();

    protected ScreenController LoadPrefab(GameObject prefab)
    {
        return Instantiate(prefab, ScreenParent).GetComponent<ScreenController>();
    }
}
