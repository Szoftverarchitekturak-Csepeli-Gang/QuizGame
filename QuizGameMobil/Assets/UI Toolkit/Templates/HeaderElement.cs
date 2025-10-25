using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class HeaderElement : VisualElement
{
    private Button _backButton;
    private Image _backIcon;

    [UxmlAttribute]
    public Color BackIconColor
    {
        get => _backIcon.tintColor;
        set => _backIcon.tintColor = value;
    }

    public HeaderElement()
    {
        VisualElement baseElement = new();
        baseElement.AddToClassList("header-container");

        _backButton = new();
        _backButton.AddToClassList("icon-button");
        _backButton.RegisterCallback<ClickEvent>(OnBackButtonClicked);

        Image backImage = new()
        {
            sprite = Resources.Load<Sprite>("back-icon")
        };
        _backIcon = backImage;

        _backButton.Add(_backIcon);

        LangButtonElement langButton = new();

        baseElement.Add(_backButton);
        baseElement.Add(langButton);

        Add(baseElement);
    }

    private void OnBackButtonClicked(ClickEvent evt)
    {
        // TODO: handle leaving the room if currently inside game or waiting room
        ScreenManager.Instance.ShowScreen(AppScreen.MAIN);
    }
}
