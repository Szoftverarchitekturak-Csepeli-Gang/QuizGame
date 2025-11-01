using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ScreenController : MonoBehaviour
{
    protected VisualElement _ui;
    protected virtual void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _ui.dataSource = this;
    }
}
