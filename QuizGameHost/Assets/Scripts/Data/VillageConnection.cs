using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillageConnection
{
    public VillageController village;
    public List<VillageController> neighbors;
}