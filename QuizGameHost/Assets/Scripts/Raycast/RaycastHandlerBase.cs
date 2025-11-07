using UnityEngine;

public abstract class RaycastHandlerBase : MonoBehaviour
{
    protected virtual void Start()
    {
        SubscribeToClickRaycaster();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeFromClickRaycaster();
    }

    private void SubscribeToClickRaycaster()
    {
        if (ScreenClickRaycaster.Instance != null)
        {
            ScreenClickRaycaster.Instance.OnRaycastTriggered += HandleRaycast;
            Debug.Log($"{this.GetType().Name} subscribed to raycast events.");
        }
    }

    private void UnsubscribeFromClickRaycaster()
    {
        if (ScreenClickRaycaster.Instance != null)
        {
            ScreenClickRaycaster.Instance.OnRaycastTriggered -= HandleRaycast;
            Debug.Log($"{this.GetType().Name} subscribed to raycast events.");
        }
    }

    protected abstract void HandleRaycast(Ray ray);
}
