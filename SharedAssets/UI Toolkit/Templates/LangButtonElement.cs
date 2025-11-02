using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LangButtonElement : VisualElement
{
    Button _langButton;

    public LangButtonElement()
    {
        _langButton = new();
        _langButton.AddToClassList("icon-button");
        _langButton.RegisterCallback<ClickEvent>(OnLangButtonClicked);

        Add(_langButton);
    }

    public LangButtonElement(string tableReference) : this()
    {
        LoadAssetReference(tableReference);
    }

    public void LoadAssetReference(string tableReference)
    {
        var localizedTexture = new LocalizedTexture
        {
            TableReference = tableReference,
            TableEntryReference = "langFlag"
        };
        _langButton.SetBinding("style.backgroundImage", localizedTexture);
    }

    private void OnLangButtonClicked(ClickEvent evt)
    {
        LocaleManager.Instance.SwitchLocale();
    }

}
