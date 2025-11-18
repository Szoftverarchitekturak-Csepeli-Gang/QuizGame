using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageManager : SingletonBase<VillageManager>
{
    private GameObject[] _villages;
    [SerializeField] private List<VillageConnection> _villageConnections; //EZT MÉG VÉLETLENÜL SE NEVEZD ÁT! RIP CONNECTIONS IN EDITOR!!!!
    [SerializeField] private VillageController[] _startingVillages;

    public bool AllVillageConquered => _villages.Length == _villages.Count(village => village.GetComponent<VillageController>().IsConquered);

    private void Awake()
    {
        base.Awake();
        _villages = GameObject.FindGameObjectsWithTag("Village");
    }

    private void Start()
    {
        foreach (var village in _villages)
            village.GetComponent<VillageController>().SetState(VillageState.Inaccessible);

        foreach (var village in _startingVillages)
            village.SetState(VillageState.Conquerable);
    }

    public void VillageConquered(VillageController village)
    {
        if (village.State == VillageState.Conquerable)
        { 
            village.SetState(VillageState.Conquered);
            GameDataManager.Instance.ConqueredVillages++;
        }

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

    //Test function to test game ended state
    public void SetAllVillageToConqueredTest()
    {
        foreach (var village in _villages)
        {
            var villageController = village.GetComponent<VillageController>();

            if (villageController.State != VillageState.Conquered)
            {
                villageController.SetState(VillageState.Conquered);
                GameDataManager.Instance.ConqueredVillages++;
            }
        }
    }
}
