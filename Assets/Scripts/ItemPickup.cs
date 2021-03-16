using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public void PickUp()
    {
        Debug.Log("Picking up" + item.name);
        Inventory.instance.Add(item);       //add item to inventory
        Destroy(gameObject);            //destroys item

    }
}
