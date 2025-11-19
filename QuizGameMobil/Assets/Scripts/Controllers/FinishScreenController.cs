using UnityEngine.UIElements;

public class FinishScreenController : ScreenController
{
    public override void ResetUIState()
    {

    }

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Mobil Assets Table");
        _ui.Q<Button>("ReturnButton").clicked += OnReturnButtonClicked;
    }

    private async void OnReturnButtonClicked()
    {
        await NetworkManager.Instance.LeaveRoom();
        ScreenManagerMobil.Instance.CurrentScreen = AppScreen.MAIN;
    }
}
