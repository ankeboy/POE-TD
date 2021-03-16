using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    Inventory inventory;
    public static int numSlots = 2;
    Item[] currentEquipment;

    void Start()
    {
        currentEquipment = new Item[numSlots];
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;     //whenever something is changing in the inventory, we call the function OnItemchangedCallBack
    public List<Item> items = new List<Item>();
    public void Equip(Item newItem)
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (currentEquipment[i] == null)
            {
                currentEquipment[i] = newItem;
                continue;
            }
        }

        Debug.Log("No Free slots");
    }
    public void Add(Item item)
    {
        items.Add(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }
    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

}
