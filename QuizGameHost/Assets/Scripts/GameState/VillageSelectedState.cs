using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VillageSelectedState : IGameState
{
    public GameStateType Type => GameStateType.VillageSelected;

    public void Enter()
    {
        CameraManager.Instance.UseMainCamera();
        InputManager.Instance.DisableInputControl();
        BlurManager.Instance.ActivateBlurEffect();

        var village = RaycastManager.Instance.CurrentSelectedVillage;
        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.ShowVillagePanel(
            village,
            () => {
                GameStateManager.Instance.ChangeState(GameStateType.VillageConquer);
            },
            () => { 
                GameStateManager.Instance.ChangeState(GameStateType.Idle);
                RaycastManager.Instance.ResetSelectedVillage();
            }
        );
    }

    public void Exit()
    {
        var uiController = (ScreenManagerHost.Instance.CurrentScreenController as GameScreenController);
        uiController.HideVillagePanel();
    }

    public void Update()
    {
        return;
    }
}
