using System;
using UnityEngine;

public class VillageRaycastHandler : RaycastHandlerBase
{
    public static VillageRaycastHandler Instance { get; private set; }

    public event Action<GameObject> OnVillageHit;
    public event Action<GameObject> OnVillageSelectChanged;

    private GameObject[] _villages;
    private GameObject _currentSelectedVillage = null;

    public GameObject CurrentSelectedVillage { get; private set; }

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

    protected override void Start()
    {
        base.Start();
        _villages = GameObject.FindGameObjectsWithTag("Village");
        Debug.Log($"Found {_villages.Length} villages in the scene.");
    }

    protected override void HandleRaycast(Ray ray)
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
            Debug.Log($"Ray hit {closestVillage.name} at distance {closestDistance}");
            OnVillageHit?.Invoke(closestVillage);

            if (closestVillage != _currentSelectedVillage)
            {
                _currentSelectedVillage = closestVillage;
                OnVillageSelectChanged?.Invoke(_currentSelectedVillage);
            }
        }
    }
}
