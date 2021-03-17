using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private node target;
    public Image icon;
    Item item;
    public Inventory inventory;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
    public void SetTarget(node _target)
    {
        target = _target;
    }
    public void UseItem()
    {
        if (item != null)
        {
            item.Use(inventory.target);
        }

    }


}
