using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum AppScreen
{
    MAIN, WAITROOM, QUIZ, FINISH, REGISTER, DASHBOARD
}

public abstract class ScreenManagerBase : MonoBehaviour
{
    [SerializeField] protected Transform ScreenParent;

    protected readonly Dictionary<AppScreen, ScreenController> _screens = new();
    protected AppScreen _currentScreen;

    public static ScreenManagerBase Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    virtual protected void Start()
    {
        LoadScreens();
        HideAllScreens();
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
            _currentScreen = screen;
        }
    }

    protected void RegisterScreen(AppScreen screen, ScreenController screenController) => _screens[screen] = screenController;

    abstract protected void LoadScreens();

    protected ScreenController LoadPrefab(GameObject prefab)
    {
        return Instantiate(prefab, ScreenParent).GetComponent<ScreenController>();
    }
}
