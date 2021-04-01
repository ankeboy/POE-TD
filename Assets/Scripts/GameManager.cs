using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;  //static objects are only called once while the program is open.
    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public GameObject RoundBonusUI;
    public string nextLevel = "Level02";
    public int levelToUnlock = 2;
    public SceneFader sceneFader;
    public static int playerLevel;
    int levelEXP;
    int maxEXP = 90;
    string savedata;

    void Start()
    {
        savedata = SceneManager.GetActiveScene().name + "_Max";
        playerLevel = PlayerPrefs.GetInt("Player Level", 0);
        levelEXP = PlayerPrefs.GetInt("Level EXP", 0);
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver)
            return;

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()      //this is gameover
    {
        GameIsOver = true;
        GainLevelEXP(WaveSpawner.waveIndex * 3);       // get EXP equal to double the level reached
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        GameIsOver = true;
        completeLevelUI.SetActive(true);
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        GainLevelEXP(maxEXP);
        sceneFader.FadeTo(nextLevel);
    }

    public void RoundBonusItem()
    {
        RoundBonusUI.SetActive(!RoundBonusUI.activeSelf);   //! before the ui.activeSelf "flips" the state

        if (RoundBonusUI.activeSelf)
        {
            Time.timeScale = 0f;    //stops the time from moving. If change to non-integer value, also set Time.fixDeltaTime
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void GainLevelEXP(int gainEXP)
    {
        PlayerPrefs.SetInt("Level EXP", levelEXP + gainEXP);
        if (PlayerPrefs.GetInt("Level EXP", 0) >= 100)                                     //level up when EXP is over 100
        {
            int noOfLevels = (int)(PlayerPrefs.GetInt("Level EXP", 0) / 100);              //in case more than 1 level up
            PlayerPrefs.SetInt("Player Level", PlayerPrefs.GetInt("Player Level", 0) + noOfLevels);
            PlayerPrefs.SetInt("Level EXP", PlayerPrefs.GetInt("Level EXP", 0) % 100);     // carry over remaining EXP to the next level
            PlayerPrefs.SetInt("Skill Points", PlayerPrefs.GetInt("Skill Points", 0) + noOfLevels); //get skill points equal to the number of level ups
        }
        if (PlayerPrefs.GetInt(savedata, 0) < WaveSpawner.waveIndex)
            PlayerPrefs.SetInt(savedata, WaveSpawner.waveIndex);
    }
}
