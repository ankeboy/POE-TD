using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public AudioClip PickUpAudioClip;
    AudioSource _audioSource;
    public static Inventory instance;
    [HideInInspector]
    public node target;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = PickUpAudioClip;
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
        }
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;     //whenever something is changing in the inventory, we call the function OnItemchangedCallBack

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        Debug.Log("Inventory.Add()");
        if (items.Count < InventoryCanvas.slots.Length)
        {
            items.Add(item);
            _audioSource.Play();
        }
        else
        {
            Debug.Log("Not enough space in Inventory. Equip a skill to to free up space");
        }

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }
    public void SetTarget(node _target)
    {
        //Debug.Log("Inventory SetTarget" + _target);
        target = _target;
    }

}
