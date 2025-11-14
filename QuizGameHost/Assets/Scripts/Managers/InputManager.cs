using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class InputManager : SingletonBase<InputManager>
{
    PlayerInput[] _playerInputs;
    CinemachineInputAxisController[] _cameraInputs;

    public Vector2 DragDelta { get; private set; }
    public float ZoomDelta { get; private set; }
    public bool IsDragging { get; private set; }
    public bool IsClicked { get; private set; }

    void Start()
    {
        _playerInputs = Object.FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
        _cameraInputs = Object.FindObjectsByType<CinemachineInputAxisController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void EnableInputControl()
    {
        SetInputControl(true);
    }

    public void DisableInputControl()
    {
        SetInputControl(false);
    }

    private void SetInputControl(bool enable)
    {
        foreach (PlayerInput input in _playerInputs)
            input.enabled = enable;

        foreach (CinemachineInputAxisController cameraInput in _cameraInputs)
            cameraInput.enabled = enable;
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        DragDelta = context.ReadValue<Vector2>();
    }

    public void OnDragClick(InputAction.CallbackContext context)
    {
        IsDragging = context.performed;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        ZoomDelta = context.ReadValue<float>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        IsClicked = context.canceled;
    }

    public void LateUpdate()
    {
        IsClicked = false;
        DragDelta = Vector2.zero;
        ZoomDelta = 0f;
    }
}
