using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons;

    void Start()
    {
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
