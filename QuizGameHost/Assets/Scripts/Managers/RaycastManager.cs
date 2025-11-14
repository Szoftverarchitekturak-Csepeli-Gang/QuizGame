using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class RaycastManager : SingletonBase<RaycastManager>
{
    public event Action<GameObject> OnVillageHit;
    public event Action<GameObject> OnVillageSelectChanged;

    private GameObject[] _villages;
    private bool _enableRaycast = true; 

    public GameObject CurrentSelectedVillage { get; private set; }

    void Start()
    {
        _villages = GameObject.FindGameObjectsWithTag("Village");
    }

    void Update()
    {
        if (InputManager.Instance.IsClicked && _enableRaycast)
        {
            var mainCamera = CameraManager.Instance.MainCamera;

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenPos);

            Debug.Log("Raycasting from screen position: " + screenPos);

            IntersectWithVillages(ray);
        }
    }
    public void ResetSelectedVillage()
    { 
        CurrentSelectedVillage = null;
        OnVillageSelectChanged?.Invoke(CurrentSelectedVillage);
    }

    public void DisableRaycast()
    {
        _enableRaycast = false;
    }

    public void EnableRaycast()
    {
        _enableRaycast = true;
    }

    private void IntersectWithVillages(Ray ray)
    {
        GameObject closestVillage = null;
        float closestDistance = Mathf.Infinity;

        foreach (var village in _villages)
        {
            var hitbox = village.transform.Find("RayCastHitbox");
            if (hitbox == null) continue;

            Collider collider = hitbox.GetComponent<Collider>();
            if (collider == null) continue;

            if (collider.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                if (hitInfo.distance < closestDistance)
                {
                    closestDistance = hitInfo.distance;
                    closestVillage = village;
                }
            }
        }

        if (closestVillage != null)
        {
            OnVillageHit?.Invoke(closestVillage);
        }

        if (closestVillage != CurrentSelectedVillage)
        {
            CurrentSelectedVillage = closestVillage;
            OnVillageSelectChanged?.Invoke(CurrentSelectedVillage);
        }
    }
}