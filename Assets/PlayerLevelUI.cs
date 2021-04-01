using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour
{
    public Text levelText;
    public Text EXPBarText;
    public Image EXPBar;
    void Start()
    {
        levelText.text = "LEVEL " + PlayerPrefs.GetInt("Player Level", 0);

        EXPBar.fillAmount = (PlayerPrefs.GetInt("Level EXP", 0) / 100f);
        EXPBarText.text = PlayerPrefs.GetInt("Level EXP", 0) + " / 100";
    }

    public void HardReset()
    {
        PlayerPrefs.DeleteAll();
    }

}
