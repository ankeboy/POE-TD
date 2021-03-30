using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons;

    public Text level_1_Max;
    public Text level_2_Max;
    public Text level_3_Max;
    public Text level_4_Max;
    public Text level_5_Max;
    public Text level_6_Max;
    public Text level_7_Max;
    public Text level_8_Max;


    void Start()
    {
        level_1_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level01_Max", 0);
        level_2_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level02_Max", 0);
        level_3_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level03_Max", 0);
        level_4_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level04_Max", 0);
        level_5_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level05_Max", 0);
        level_6_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level06_Max", 0);
        level_7_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level07_Max", 0);
        level_8_Max.text = "Highest Wave: " + PlayerPrefs.GetInt("Level08_Max", 0);
        //Debug.Log("Number of buttons" + levelButtons.Length);
        int levelReached = PlayerPrefs.GetInt("levelReached", 3);      //access the savefile. Set the default levelReached to 1 for new saves.

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
        }
    }
    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }
}
