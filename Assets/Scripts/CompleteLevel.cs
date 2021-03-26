using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public string nextLevel = "Level02";
    public int levelToUnlock = 2;
    public SceneFader sceneFader;

    public void Continue()
    {
        if (PlayerPrefs.GetInt("levelReached") < levelToUnlock)
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(nextLevel);
    }

    public void Menu()
    {
        if (PlayerPrefs.GetInt("levelReached") < levelToUnlock)
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(menuSceneName);
    }

}
