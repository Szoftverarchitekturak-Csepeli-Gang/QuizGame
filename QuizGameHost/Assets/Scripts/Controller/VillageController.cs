using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum VillageState
{
    None,
    Inaccessible,
    Conquerable,
    Conquered
}

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
    [SerializeField] private GameObject _stateLight;
    [SerializeField] private Color _inaccessibleStateColor;
    [SerializeField] private Color _conquerableStateColor;
    [SerializeField] private Color _conqueredStateColor;

    [Header("Navigation")]
    [SerializeField] private GameObject _ground;
    [SerializeField] private int _sampleMaxAttempts = 100;

    private VillageState _state = VillageState.None;

    public Village Info => _info;
    public VillageState State => _state;

    public bool IsConquerable => _state == VillageState.Conquerable;

    public GameObject Ground => _ground;
    public GameObject Camera => _camera;
    public VillageConfig Config => _config;
    public GameObject AttackerSpawnPointParent => _attackerSpawnPointParent;
    public GameObject DefenderSpawnPointParent => _defenderSpawnPointParent;
    public GameObject CharacterContainer { get => _characterContainer; set => _characterContainer = value; }

    public void Awake()
    {
        SetState(VillageState.None);
    }

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

    public void SetState(VillageState state)
    {
        switch (state)
        { 
            case VillageState.None:
                _stateLight.SetActive(false);
                _state = state;
                break;
            case VillageState.Inaccessible:
                _stateLight.SetActive(true);
                _stateLight.GetComponent<Light>().color = _inaccessibleStateColor;
                _state = state;
                break;
            case VillageState.Conquerable:
                _stateLight.SetActive(false);
                _stateLight.GetComponent<Light>().color = _conquerableStateColor;
                _state = state;
                break;
            case VillageState.Conquered:
                _stateLight.SetActive(true);
                _stateLight.GetComponent<Light>().color = _conqueredStateColor;
                _state = state;
                break;
        }
    }
}
