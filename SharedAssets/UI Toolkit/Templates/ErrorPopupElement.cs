using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ErrorPopupElement : VisualElement
{
    [SerializeField] public string _errorMessage;
    Action _closeAction;

    public ErrorPopupElement()
    {
        dataSource = this;

        Label errorLabel = new();
        errorLabel.AddToClassList("red-text");
        errorLabel.SetBinding("text", new DataBinding
        {
            dataSourcePath = new PropertyPath(nameof(_errorMessage)),
            bindingMode = BindingMode.ToTarget,
        });
        errorLabel.style.flexGrow = 1;
        errorLabel.style.whiteSpace = WhiteSpace.Normal;
        errorLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        Add(errorLabel);

        _errorMessage = "this is an error";
    }

    public ErrorPopupElement(Action closeAction) : this()
    {
        _closeAction = closeAction;

        Button closeButton = new();
        closeButton.text = "OK";
        closeButton.AddToClassList("yellow-button");
        closeButton.clicked += OnCloseClicked;
        Add(closeButton);
    }

    private void OnCloseClicked()
    {
        _closeAction?.Invoke();
    }

    public void DisplayError(string message)
    {
        _errorMessage = message;
    }
}
