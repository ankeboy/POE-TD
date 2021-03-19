using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//delete this. not needed
public class EquipmentCanvas : MonoBehaviour
{
    public Transform itemsParent;
    EquipmentManager equipmentManager;
    EquipmentSlot[] slots;
    void Start()
    {
        slots = itemsParent.GetComponentsInChildren<EquipmentSlot>();
    }

}