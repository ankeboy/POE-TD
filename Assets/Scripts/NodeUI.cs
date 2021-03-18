﻿using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public turret turret;
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellAmount;
    private node target;
    [Header("Turret stats")]
    public Text turretName;
    public Text turretDamage;
    public Text turretRange;
    public Text turretFireRate;
    public Text turretSpecialEffect;

    [Header("Turret Equipment")]
    public Transform equipmentParent;
    EquipmentSlot[] slots;
    public void SetTarget(node _target)
    {
        target = _target;

        target.turret.GetComponent<turret>().onItemChangedCallBack += UpdateUI;
        slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();

        transform.position = target.GetBuildPosition();

        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        turretName.text = target.turretBlueprint.turretName;

        //target = this specific node (from node script). turret = points to the GameObject (prefab) in the node script. GetComponent<turret> gets the numbers off the prefab based on the curret script 
        turretDamage.text = target.turret.GetComponent<turret>().damage.ToString();
        turretRange.text = target.turret.GetComponent<turret>().range.ToString();
        turretFireRate.text = target.turret.GetComponent<turret>().fireRate.ToString() + "/sec";
        turretSpecialEffect.text = target.turret.GetComponent<turret>().specialEffect.ToString();

        ui.SetActive(true);
        UpdateUI();
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Equip(Item newItem)
    {
        Debug.Log("Called from NodeUI");
        target.turret.GetComponent<turret>().currentEquipment[0] = newItem;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < target.turret.GetComponent<turret>().numSlots; i++)
        {
            Debug.Log("Itemindex is " + i + ". Item is " + target.turret.GetComponent<turret>().currentEquipment[i]);
            if (target.turret.GetComponent<turret>().currentEquipment[i] != null)
            {
                slots[i].AddItem(target.turret.GetComponent<turret>().currentEquipment[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
