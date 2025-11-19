using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public abstract class ScreenController : MonoBehaviour
{
    protected VisualElement _ui;
    protected virtual void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _ui.dataSource = this;
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
}
