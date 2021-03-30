using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public SceneFader fader;
    public static SkillManager instance;
    public Text skillPointsUI;
    int skillPoints;
    Skill skill;
    //public Button[] buttons;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

        skillPoints = PlayerPrefs.GetInt("Skill Points", 0);
        skillPointsUI.text = "Skill Points: " + skillPoints;

        //Button[] buttons = this.GetComponentsInChildren<Button>();
        //Button[] buttons = this.GetComponentsInChildren<Button>(true); //also gets the inactive components
    }

    public void upgradeSkill(Skill skill)
    {
        if (skillPoints <= 0)
        {
            Debug.Log("Not Enough Skill Points");
        }
        else
        {
            skill.upgradeSkill();
            skillPoints--;
            PlayerPrefs.SetInt("Skill Points", skillPoints);
            skillPointsUI.text = "Skill Points: " + skillPoints.ToString();
        }
    }
    public void Select(string scene)
    {
        fader.FadeTo(scene);
    }

    public void ResetButton(string scene)
    {
        PlayerPrefs.SetInt("Skill Points", PlayerPrefs.GetInt("Player Level"));
        PlayerPrefs.SetInt("Increased Damage", 0);
        PlayerPrefs.SetInt("Increased Fire Rate", 0);
        PlayerPrefs.SetInt("Increased Range", 0);
        PlayerPrefs.SetInt("Increased Projectile Speed", 0);
        PlayerPrefs.SetInt("Standard Turret Boost", 0);
        PlayerPrefs.SetInt("Missile Launcher Boost", 0);
        PlayerPrefs.SetInt("Laser Beamer Boost", 0);
        PlayerPrefs.SetInt("Frost Tower Boost", 0);
        fader.FadeTo(scene);
    }
}
