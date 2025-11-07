using UnityEngine;

public class ParticleEffectPlayer_Test : MonoBehaviour
{
    //Only to test raycast hits
    [SerializeField] ParticleSystem _testParticleSystem;

    public void Start()
    {
        VillageRaycastHandler.Instance.OnVillageHit += HandleVillageHit;
    }

    public void HandleVillageHit(GameObject village)
    {
        _testParticleSystem.transform.position = village.transform.position + Vector3.up * 5.0f;
        _testParticleSystem.Play();
    }
}
