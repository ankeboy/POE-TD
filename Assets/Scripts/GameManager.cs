using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;  //static objects are only called once while the program is open.

    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public GameObject RoundBonusUI;
    void Start()
    {
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

    void EndGame()
    {
        GameIsOver = true;

        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        GameIsOver = true;
        completeLevelUI.SetActive(true);
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
}
