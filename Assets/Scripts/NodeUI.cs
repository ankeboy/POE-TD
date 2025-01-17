﻿using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject rangeIndicator;
    GameObject _rangeIndicator;
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Text sellAmount;
    private node target;
    public Color SelectedColor;
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

        target.turret.GetComponent<turret>().onItemChangedCallBack += UpdateUI; //updates ui upon equiping
        slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>(true);       //the "true" option allows to include inactive children (includeInactive = false).
        //Debug.Log("numSlots" + target.turret.GetComponent<turret>().numSlots);    //check for the correct numslots of the turret
        //Debug.Log("slots.Length = " + slots.Length);                              //check the number of components received from GetComponentsInChildren<EquipmentSlot>
        //enabling and disabling equipment slot based on the number of slots of the turret
        for (int i = 0; i < 4; i++)
        {
            slots[i].equipmentSlot.SetActive(true);
            //Debug.Log("equipmentslot number " + i);                           //make sure that it loops through all (4) equipment slots.
            if (i >= target.turret.GetComponent<turret>().numSlots)
            {
                slots[i].equipmentSlot.SetActive(false);
                //Debug.Log("Setactive(false)" + i);                            //check that the correct equipmentslot is deactivated.
            }
        }

        transform.position = target.GetBuildPosition();

        if (target.isUpgraded == 0)
        {
            turretName.text = target.turretBlueprint.turretName;
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else if (target.isUpgraded == 1)
        {
            turretName.text = target.turretBlueprint.turretName + " \u2605";
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost2;
            upgradeButton.interactable = true;
        }
        else
        {
            turretName.text = target.turretBlueprint.turretName + " \u2605" + "\u2605";
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount(target.isUpgraded);



        //target = this specific node (from node script). turret = points to the GameObject (prefab) in the node script. GetComponent<turret> gets the numbers off the prefab based on the curret script 
        /* //moved to UpdateUI, not needed anymore.
        turretDamage.text = target.turret.GetComponent<turret>().damage.ToString();
        turretRange.text = target.turret.GetComponent<turret>().range.ToString();
        turretFireRate.text = target.turret.GetComponent<turret>().fireRate.ToString() + "/sec";
        turretSpecialEffect.text = target.turret.GetComponent<turret>().specialEffect.ToString();
        */

        UpdateUI();             //required to update the UI when clicking on a different node without changing anything.
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
        if (_rangeIndicator != null)
            Destroy(_rangeIndicator);
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
        //Debug.Log("Called from NodeUI");
        target.turret.GetComponent<turret>().currentEquipment[0] = newItem;
    }

    private void UpdateUI()
    {
        if (_rangeIndicator != null)
            Destroy(_rangeIndicator);

        _rangeIndicator = (GameObject)Instantiate(rangeIndicator, transform.position, Quaternion.identity);
        _rangeIndicator.transform.localScale = new Vector3(target.turret.GetComponent<turret>().range * 2, 1, target.turret.GetComponent<turret>().range * 2); //dont know why multiplying by 2 is corret. It just seems to fit.

        //Updates stats as the item gets equiped.
        for (int i = 0; i < target.turret.GetComponent<turret>().numSlots; i++)
        {
            //Debug.Log("Itemindex is " + i + ". Item is " + target.turret.GetComponent<turret>().currentEquipment[i]);
            if (target.turret.GetComponent<turret>().currentEquipment[i] != null)
            {
                slots[i].AddItem(target.turret.GetComponent<turret>().currentEquipment[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        turretDamage.text = target.turret.GetComponent<turret>().damage.ToString();
        turretRange.text = target.turret.GetComponent<turret>().range.ToString();
        turretFireRate.text = target.turret.GetComponent<turret>().fireRate.ToString() + "/sec";
        turretSpecialEffect.text = target.turret.GetComponent<turret>().specialEffect.ToString();
    }
}
