using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : SingletonBase<VillageManager>
{
    private GameObject[] _villages;
    [SerializeField] private List<VillageConnection> _villageConnections; //EZT MÉG VÉLETLENÜL SE NEVEZD ÁT! RIP CONNECTIONS IN EDITOR!!!!
    [SerializeField] private VillageController[] _startingVillages;

    private void Start()
    {
        _villages = GameObject.FindGameObjectsWithTag("Village");

        foreach (var village in _villages)
            village.GetComponent<VillageController>().SetState(VillageState.Inaccessible);

        foreach (var village in _startingVillages)
            village.SetState(VillageState.Conquerable);
    }

    public void VillageConquered(VillageController village)
    {
        village.SetState(VillageState.Conquered);

        var villageConnection = _villageConnections.Find(connection => connection.village == village);

        if (villageConnection != null)
        {
            foreach (var neighbor in villageConnection.neighbors)
            {
                if (neighbor.State == VillageState.Inaccessible)
                    neighbor.SetState(VillageState.Conquerable);
            }
        } 
    }
}
