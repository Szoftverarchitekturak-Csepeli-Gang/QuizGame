using UnityEngine;

public class CameraManager : SingletonBase<CameraManager>
{
    private GameObject _dragCamera;
    private GameObject[] _villageCameras;
    public GameObject CurrentVirtualCamera { get; private set; }
    public GameObject MainCamera { get; private set; }

    public void Awake()
    { 
        base.Awake();
        MainCamera = GameObject.FindWithTag("MainCamera");
        _dragCamera = GameObject.FindWithTag("DragCamera");
        _villageCameras = GameObject.FindGameObjectsWithTag("VillageCamera");
    }

    public void UseMainCamera()
    {
        SetVillageCameras(false);
        SetMainCamera(true);
    }

    public void UseVillageCamera(GameObject village)
    {
        SetVillageCameras(false);
        SetMainCamera(false);
        SetVillageCamera(village, true);
    }

    private void SetVillageCameras(bool enable)
    {
        foreach (var camera in _villageCameras)
            camera.SetActive(enable);
    }

    private void SetMainCamera(bool enable)
    {
        _dragCamera.SetActive(enable);

        if(enable)
            CurrentVirtualCamera = _dragCamera;
    }

    private void SetVillageCamera(GameObject village, bool enable)
    {
        var camera = village.GetComponent<VillageController>().Camera;
        camera.SetActive(enable);

        if (enable)
            CurrentVirtualCamera = camera;
    }
}
