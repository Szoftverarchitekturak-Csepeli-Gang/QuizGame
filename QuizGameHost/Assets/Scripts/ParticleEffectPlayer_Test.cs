using Unity.Cinemachine;
using UnityEngine;

public class ParticleEffectPlayer_Test : MonoBehaviour
{
    //Only to test raycast hits
    [SerializeField] ParticleSystem _testParticleSystem;
    [SerializeField] CinemachineCamera _mainVirtualCamera;

    public void Start()
    {
        VillageRaycastHandler.Instance.OnVillageHit += HandleVillageHit;
    }

    public void HandleVillageHit(GameObject village)
    {
        _testParticleSystem.transform.position = village.transform.position + Vector3.up * 5.0f;
        _testParticleSystem.Play();

        var cinemachineCamera = village.transform.Find("Camera").GetChild(0);
        cinemachineCamera.gameObject.SetActive(true);

        //village.GetComponent<VillageController>().SpawnDefenders(5);
        village.GetComponent<VillageController>().SpawnAttackers(10);
    }
}
