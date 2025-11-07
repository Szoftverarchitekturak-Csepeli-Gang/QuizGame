using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenClickRaycaster : MonoBehaviour
{
    public static ScreenClickRaycaster Instance { get; private set; }

    public event Action<Ray> OnRaycastTriggered;

    GameObject _mainCamera;

    private void Awake()
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

    private void Start()
    {
        _mainCamera = GameObject.FindWithTag("MainCamera");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(!context.canceled) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.GetComponent<Camera>().ScreenPointToRay(screenPos);

        Debug.Log("Raycasting from screen position: " + screenPos);
        OnRaycastTriggered?.Invoke(ray);
    }
}
