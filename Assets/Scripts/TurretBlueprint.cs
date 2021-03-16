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
    public int GetSellAmount()
    {
        return cost / 2;
    }
}
