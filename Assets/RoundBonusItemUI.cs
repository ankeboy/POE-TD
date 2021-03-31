using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundBonusItemUI : MonoBehaviour
{
    Button[] options;
    public Inventory inventory;
    public GameManager gameManager;
    public Item[] itemlist = new Item[5];
    int optionNo;



    void Awake()
    {
        optionNo = PlayerPrefs.GetInt("BonusItemOptions", 3);
        options = GetComponentsInChildren<Button>(true);
        shuffleBonusItem();
    }

    void shuffleBonusItem()
    {
        for (int i = 0; i < 5; i++)                             //hardcode max number of options
        {
            if (i < optionNo)
            {
                options[i].onClick.RemoveAllListeners();
                options[i].gameObject.SetActive(true);
                Item randomItem = itemlist[Random.Range(0, options.Length)];    //pick a random Item
                options[i].GetComponent<Image>().sprite = randomItem.icon;      //set the buttonUI to the random item
                options[i].onClick.AddListener(delegate { clickEvent(randomItem); });
            }
            else                                                //deactivate all options beyond the number of options given in the playerprefs.
            {
                options[i].gameObject.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        shuffleBonusItem();
    }

    void clickEvent(Item item)
    {
        inventory.Add(item);
        gameManager.RoundBonusItem();
    }
}
