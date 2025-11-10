using UnityEngine.Localization.Settings;

public class LocaleManager : SingletonBase<LocaleManager>
{
    private enum LocaleLang
    {
        EN = 0, HU = 1
    }

    private LocaleLang _currentLocale = LocaleLang.EN;

    public void SwitchLocale()
    {
        _currentLocale = _currentLocale == LocaleLang.EN ? LocaleLang.HU : LocaleLang.EN;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)_currentLocale];
    }
}
