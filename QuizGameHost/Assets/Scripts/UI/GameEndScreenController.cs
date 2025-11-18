using UnityEngine;
using UnityEngine.UIElements;

public class GameEndScreenController : ScreenController
{
    [SerializeField, HideInInspector] private string _conqueredVillagesText;
    private Button _exitButton;

    protected override void Awake()
    {
        base.Awake();
        _exitButton = _ui.Q<Button>("ExitButton");
    }

    void Start()
    {
        GameEndScreenPresenter.Instance.AttachScreen(this);
    }

    void OnDestroy()
    {
        GameEndScreenPresenter.Instance.DetachScreen();
    }

    void OnEnable()
    {
        _exitButton.clicked += OnExitClicked;
    }

    void OnDisable()
    {
        _exitButton.clicked -= OnExitClicked;
    }

    private async void OnExitClicked()
    {
        await RoomManager.Instance.LeaveRoom();
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.DASHBOARD;
    }

    public void OnGameEnded(int rightAnswers, int totalQuestions, int conqueredVillages, int totalVillages)
    {
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.FINISH;
        _conqueredVillagesText = $"{rightAnswers} / {totalQuestions}";
    }
}
