using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //required whenever you want to load/reload a (new) scene

public class GameOver : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

    public void Retry()
    {
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);   //we can hard code which scene to load, but by retrieving the current scene we can always reload the current scene, even if the scene index changes.
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
