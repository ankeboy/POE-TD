using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    turret turret;
    EquipmentManager equipmentManager;
    Inventory inventory;
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    private NodeUI target;

    public virtual void Use()
    {
        target = NodeUI.instance;
        Debug.Log("Equipped");
        target.Equip(this);
        //target.turret.GetComponent<turret>().Equip(this);
        //target.turret.GetComponent<turret>().Add(this);
        RemoveFromInventory();
    }

    public void RemoveFromInventory()
    {
        inventory.Remove(this);
    }
}