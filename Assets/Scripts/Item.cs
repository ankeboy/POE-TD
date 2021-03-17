using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    //EquipmentManager equipmentManager;

    private node target;

    public virtual void Use(node _target)
    {
        target = _target;
        int slotcount = target.turret.GetComponent<turret>().currentEquipment.Length - 1;
        if (target.turret.GetComponent<turret>().currentEquipment[slotcount] == null)
        {
            Debug.Log("Equipped");
            target.turret.GetComponent<turret>().Equip(this);
            RemoveFromInventory();
        }
        else
        {
            Debug.Log("No Free slots");
        }
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

}