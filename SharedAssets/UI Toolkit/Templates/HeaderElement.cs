using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class HeaderElement : VisualElement
{
    private Button _backButton;
    private Image _backIcon;
    private VisualElement _baseElement;

    [UxmlAttribute] public string TableReference { get; set; }

    [UxmlAttribute]
    public Color BackIconColor
    {
        get => _backIcon.tintColor;
        set => _backIcon.tintColor = value;
    }

    public HeaderElement()
    {
        _baseElement = new();
        _baseElement.AddToClassList("header-container");

        _backButton = new();
        _backButton.AddToClassList("icon-button");
        _backButton.AddToClassList("yellow-button");
        _backButton.RegisterCallback<ClickEvent>(OnBackButtonClicked);

        Image backImage = new()
        {
            sprite = Resources.Load<Sprite>("back-icon")
        };
        _backIcon = backImage;

        _backButton.Add(_backIcon);

        // LangButtonElement langButton = new();
        RegisterCallbackOnce<GeometryChangedEvent>(OnGeometryChanged);

        _baseElement.Add(_backButton);
        // _baseElement.Add(langButton);

        Add(_baseElement);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        if (TableReference == null) return;
        LangButtonElement langButton = new(TableReference);
        _baseElement.Add(langButton);
    }

    private async void OnBackButtonClicked(ClickEvent evt)
    {
        await NetworkManager.Instance.Logout();
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.MAIN;
    }
}
