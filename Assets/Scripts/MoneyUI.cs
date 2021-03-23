using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI instance;
    public Text moneyText;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MoneyUI found!");
        }
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();
    }

    public void NotEnoughMoney()
    {
        if (moneyText.color == Color.red)
        {
            moneyText.color = Color.yellow;
        }
        else
        {
            moneyText.color = Color.red;
        }
    }
}
