using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleManager : MonoBehaviour
{
    public static LocaleManager Instance { get; private set; }

    private enum LocaleLang
    {
        EN = 0, HU = 1
    }

    private LocaleLang _currentLocale = LocaleLang.EN;

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

    public void SwitchLocale()
    {
        _currentLocale = _currentLocale == LocaleLang.EN ? LocaleLang.HU : LocaleLang.EN;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)_currentLocale];
    }
}
