using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public string SkillTree = "MainLevel";

    public SceneFader sceneFader;
    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public void ChangeToSkillTree()
    {
        sceneFader.FadeTo(SkillTree);
    }
}
