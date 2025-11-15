using UnityEngine.Localization;
using UnityEngine.UIElements;

public static class LocalizationHelper
{
    public static LocalizedString LoadLocalization(string stringRef)
    {
        return new LocalizedString
        {
            TableReference = "Host String Table",
            TableEntryReference = stringRef
        };
    }

    public static void BindLabel(TextElement element, LocalizedString localized)
    {
        localized.StringChanged += s => element.text = s;
        localized.RefreshString();
    }
}