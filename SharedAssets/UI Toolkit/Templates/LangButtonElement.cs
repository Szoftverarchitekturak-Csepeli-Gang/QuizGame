using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LangButtonElement : VisualElement
{
    public LangButtonElement()
    {
        Button langButton = new();
        langButton.AddToClassList("icon-button");
        langButton.RegisterCallback<ClickEvent>(OnLangButtonClicked);
        var localizedTexture = new LocalizedTexture
        {
            TableReference = "Mobil Assets Table",
            TableEntryReference = "langFlag"
        };
        langButton.SetBinding("style.backgroundImage", localizedTexture);

        Add(langButton);
    }

    private void OnLangButtonClicked(ClickEvent evt)
    {
        LocaleManager.Instance.SwitchLocale();
    }
}
