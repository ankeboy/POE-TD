using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    public Transform itemsParent;
    Inventory inventory;
    public static InventorySlot[] slots;   //need to make this public, so that the inventory can access how many slots there are.
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallBack += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

}
