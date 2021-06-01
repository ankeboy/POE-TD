using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]   //Create options in the inspector. These are not attached to an object, but can be called in a script.
public class TurretBlueprint
{
    public GameObject prefab;
    public string turretName = "TurretName";
    public int cost;
    public GameObject upgradedPrefab;
    public int upgradeCost;
    public GameObject upgradedPrefab2;
    public int upgradeCost2;
    public int GetSellAmount(int upgrade = 0)
    {
        if (upgrade == 0)
            return cost / 2;
        else if (upgrade == 1)
            return upgradeCost / 2;
        else
            return upgradeCost2 / 2;
    }
}
