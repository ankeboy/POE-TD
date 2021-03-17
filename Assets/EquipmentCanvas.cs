using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCanvas : MonoBehaviour
{
    public Transform itemsParent;
    EquipmentManager equipmentManager;
    EquipmentSlot[] slots;
    void Start()
    {
        //equipmentManager = EquipmentManager.instance;
        //equipmentManager.onItemChangedCallBack += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<EquipmentSlot>();
    }
    /*
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < equipmentManager.items.Count)
            {
                slots[i].AddItem(equipmentManager.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
    */
}