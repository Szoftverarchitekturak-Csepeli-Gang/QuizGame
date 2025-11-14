using NUnit.Framework;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.AppUI.UI;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class VillageController : MonoBehaviour
{
    [Header("Village Info")]
    [SerializeField] private Village _info;

    [Header("Attachments")]
    [SerializeField] private GameObject _characterContainer;
    [SerializeField] private GameObject _defenderPrefab;
    [SerializeField] private GameObject _attackerPrefab;
    [SerializeField] private GameObject _defenderSpawnPointParent;
    [SerializeField] private GameObject _attackerSpawnPointParent;

    [Header("Navigation")]
    [SerializeField] private GameObject _ground;
    [SerializeField] private int _sampleMaxAttempts = 100;

    private List<GameObject> _defenders = new List<GameObject>();
    private List<GameObject> _attackers = new List<GameObject>();
    
    public Village Info => _info;

    public void Start()
    {
        //SpawnDefenders(5);
        //SpawnAttackers(5);
    }

    public void SpawnDefenders(int count)
    {
        for(int i = 0; i < count; i++)
        {
            SpwanCharacter(_defenderPrefab, true);
        }
    }

    public void SpawnAttackers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpwanCharacter(_attackerPrefab, false);
        }
    }

    public void SpwanCharacter(GameObject character, bool isDefender)
    {
        Vector3 position = isDefender ? GetRandomPointOnNavMesh(10f, 30f) : GetRandomPointOnNavMesh(40f, 65f);
        
        if(position == Vector3.zero)
        {
            Debug.LogError("Failed to find a valid spawn point on NavMesh.");
            return;
        }

        //Vector3 position = GetSpwanPoint(isDefender ? _defenderSpawnPointParent : _attackerSpawnPointParent);

        GameObject newCharacter = Instantiate(character, position, Quaternion.identity);
        newCharacter.transform.parent = _characterContainer.transform;

        if (isDefender)
            _defenders.Add(newCharacter);
        else
            _attackers.Add(newCharacter);
    }

    private Vector3 GetSpwanPoint(GameObject spawnPointParent)
    {
        int spawnIndex = Random.Range(0, spawnPointParent.transform.childCount);
        return spawnPointParent.transform.GetChild(spawnIndex).transform.position;
    }

    public Vector3 GetRandomPointOnNavMesh(float radiusStart, float radiusEnd)
    {
        int sampleCounter = 0;
        bool success = false;
        Vector3 center = _ground.transform.position;
        Vector3 result = Vector3.zero;

        while (!success && sampleCounter < _sampleMaxAttempts)
        {
            float radius = Random.Range(radiusStart, radiusEnd);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            Vector3 randomPos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 15.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                success = true;
            }

            sampleCounter++;
        }

        return result;
    }
}
