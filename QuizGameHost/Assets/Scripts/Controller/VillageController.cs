using UnityEngine;

public class VillageController : MonoBehaviour
{
    [Header("Village Info")]
    [SerializeField] private Village _info;

    [Header("Village Config")]
    [SerializeField] private VillageConfig _config;

    [Header("Attachments")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _characterContainer;
    [SerializeField] private GameObject _defenderSpawnPointParent;
    [SerializeField] private GameObject _attackerSpawnPointParent;

    [Header("Navigation")]
    [SerializeField] private GameObject _ground;
    [SerializeField] private int _sampleMaxAttempts = 100;
    
    public Village Info => _info;

    public GameObject Ground => _ground;
    public GameObject Camera => _camera;
    public VillageConfig Config => _config;
    public GameObject AttackerSpawnPointParent => _attackerSpawnPointParent;
    public GameObject DefenderSpawnPointParent => _defenderSpawnPointParent;
    public GameObject CharacterContainer { get => _characterContainer; set => _characterContainer = value; }

    public void Start()
    {
        foreach (Transform spawnPoint in AttackerSpawnPointParent.transform)
        {
            var renderer = spawnPoint.gameObject.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

        foreach (Transform spawnPoint in DefenderSpawnPointParent.transform)
        {
            var renderer = spawnPoint.gameObject.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }
}
