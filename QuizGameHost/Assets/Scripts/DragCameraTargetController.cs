using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class DragCameraTargetController : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed = 20f;
    public float _zoomSpeed = 10f;
    public float _minHeight = 20f;
    public float _maxHeight = 500f;

    private bool _isDragging;
    private Vector2 _dragDelta;
    private float _zoomDelta;

    public void Start()
    {
        transform.position = new Vector3(500f, Mathf.Clamp(200f, _minHeight, _maxHeight), 150f);
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        _dragDelta = context.ReadValue<Vector2>();
    }

    public void OnDragClick(InputAction.CallbackContext context)
    {
        _isDragging = context.performed;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        _zoomDelta = context.ReadValue<float>();
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 rightMovement = -1 * transform.right * _dragDelta.x * _moveSpeed * Time.deltaTime;
            Vector3 forwardMovement = -1 * transform.forward * _dragDelta.y * _moveSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + rightMovement + forwardMovement;
            newPosition.y = Mathf.Clamp(newPosition.y, _minHeight, _maxHeight);
            transform.position = newPosition;

            //Debug.Log($"Drag Delta: {_dragDelta}, New Position: {newPosition}");
        }

        if (Mathf.Abs(_zoomDelta) > 0.01f)
        {
            Vector3 newPosition = -1 * transform.position + transform.up * _zoomDelta * _zoomSpeed * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, _minHeight, _maxHeight);
            transform.position = newPosition;

            //Debug.Log($"Zoom Delta: {_zoomDelta}, New Height: {newPosition.y}");
        }

        _dragDelta = Vector2.zero;
        _zoomDelta = 0f;
    }
}
