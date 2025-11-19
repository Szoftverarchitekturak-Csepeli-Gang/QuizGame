using UnityEngine.UIElements;

public class WaitroomScreenController : ScreenController
{
    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        NetworkManager.Instance.GameStartedEvent += HandleGameStarted;
    }

    private void OnDisable()
    {
        NetworkManager.Instance.GameStartedEvent -= HandleGameStarted;
    }

    private void HandleGameStarted()
    {
        ScreenManagerMobil.Instance.CurrentScreen = AppScreen.QUIZ;
    }

    public override void ResetUIState()
    {

    }
}
