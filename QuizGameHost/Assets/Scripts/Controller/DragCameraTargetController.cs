using UnityEngine;
public class DragCameraTargetController : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed = 20f;
    public float _zoomSpeed = 10f;
    public float _minHeight = 0f;
    public float _maxHeight = 500f;

    public void Start()
    {
        transform.position = new Vector3(500f, Mathf.Clamp(200f, _minHeight, _maxHeight), 150f);
    }

    private void Update()
    {
        var isDragging = InputManager.Instance.IsDragging;
        var dragDelta = InputManager.Instance.DragDelta;
        var zoomDelta = InputManager.Instance.ZoomDelta;

        if (isDragging)
        {
            Vector3 rightMovement = -1 * transform.right * dragDelta.x * _moveSpeed * Time.deltaTime;
            Vector3 forwardMovement = -1 * transform.forward * dragDelta.y * _moveSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + rightMovement + forwardMovement;
            newPosition.y = Mathf.Clamp(newPosition.y, _minHeight, _maxHeight);
            transform.position = newPosition;

            //Debug.Log($"Drag Delta: {_dragDelta}, New Position: {newPosition}");
        }

        if (Mathf.Abs(InputManager.Instance.ZoomDelta) > 0.01f)
        {
            Vector3 upMovement = -1 * transform.up * zoomDelta * _zoomSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + upMovement;
            newPosition.y = Mathf.Clamp(newPosition.y, _minHeight, _maxHeight);
            transform.position = newPosition;

            //Debug.Log($"Zoom Delta: {_zoomDelta}, New Height: {newPosition.y}");
        }
    }
}
