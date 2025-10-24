using System;
using System.Collections.Generic;
using UnityEngine;

public enum AppScreen
{
    MAIN, WAITROOM, QUIZ, FINISH
}

/// <summary>
/// Singleton class to swap between screens of the application.
/// </summary>
public class ScreenManager : MonoBehaviour
{
    [SerializeField] Transform ScreenParent;
    [Header("Screen Prefabs")]
    [SerializeField] GameObject MainScreenPrefab;
    [SerializeField] GameObject QuizScreenPrefab;
    [SerializeField] GameObject WaitroomScreenPrefab;
    [SerializeField] GameObject FinishScreenPrefab;

    private readonly Dictionary<AppScreen, GameObject> _screens = new();
    private AppScreen _currentScreen;

    public static ScreenManager Instance { get; private set; }

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

    void Start()
    {
        _screens.Add(AppScreen.MAIN, Instantiate(MainScreenPrefab, ScreenParent));
        _screens.Add(AppScreen.QUIZ, Instantiate(QuizScreenPrefab, ScreenParent));
        _screens.Add(AppScreen.WAITROOM, Instantiate(WaitroomScreenPrefab, ScreenParent));
        _screens.Add(AppScreen.FINISH, Instantiate(FinishScreenPrefab, ScreenParent));

        HideAllScreens();
        ShowScreen(AppScreen.MAIN);
    }

    private void HideAllScreens()
    {
        foreach ((_, GameObject obj) in _screens)
        {
            obj.SetActive(false);
        }
    }

    public void ShowScreen(AppScreen screen)
    {
        if (_currentScreen != screen)
        {
            _screens[_currentScreen].SetActive(false);
        }

        _screens[screen].SetActive(true);
        _currentScreen = screen;
    }
}
