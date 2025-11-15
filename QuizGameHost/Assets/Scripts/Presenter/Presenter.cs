public interface IPresenter<TScreen>
    where TScreen : ScreenController
{
    void AttachScreen(TScreen screen);
    void DetachScreen();
}