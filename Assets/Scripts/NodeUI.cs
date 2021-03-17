using UnityEngine;
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
    void Start()
    {
        //target.turret.GetComponent<turret>().onItemChangedCallBack += UpdateUI;

        slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();
    }
    public void SetTarget(node _target)
    {
        target = _target;

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

        target.turret.GetComponent<turret>().onItemChangedCallBack += UpdateUI;
        ui.SetActive(true);
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

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < target.turret.GetComponent<turret>().currentEquipment.Length)
            {
                slots[i].AddItem(target.turret.GetComponent<turret>().currentEquipment[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
    public void Equip(Item newItem)
    {
        Debug.Log("Called from NodeUI");
        target.turret.GetComponent<turret>().currentEquipment[0] = newItem;
        /*
        for (int i = 0; i < target.turret.GetComponent<turret>().numSlots; i++)
        {
            if (target.turret.GetComponent<turret>().currentEquipment[i] == null)
            {
                target.turret.GetComponent<turret>().currentEquipment[i] = newItem;
                continue;
            }
        }

        Debug.Log("No Free slots");
        */
    }
}
