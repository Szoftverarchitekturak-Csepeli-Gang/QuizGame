using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public abstract class ScreenController : MonoBehaviour
{
    protected VisualElement _ui;
    protected ErrorPopupElement _errorPopup;
    protected virtual void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _ui.dataSource = this;

        _errorPopup = new ErrorPopupElement(HideErrorMessage);
        _errorPopup.AddToClassList("hide");
        _ui.Add(_errorPopup);
    }

    public bool IsVisible
    {
        set
        {
            if (value)
                _ui.RemoveFromClassList("hide");
            else
                _ui.AddToClassList("hide");
        }
    }

    public abstract void ResetUIState();

    protected void SetEnabledState(bool isEnabled)
    {
        VisualElement[] children = _ui.Children().ToArray();
        if (children.Length > 1)
        {
            foreach (var child in children[0].Children())
            {
                child.SetEnabled(isEnabled);
            }
        }
    }

    public void DisplayErrorMessage(string message)
    {
        _errorPopup.RemoveFromClassList("hide");
        _errorPopup.DisplayError(message);
        SetEnabledState(false);
    }

    protected void HideErrorMessage()
    {
        _errorPopup.AddToClassList("hide");
        SetEnabledState(true);
    }
}
