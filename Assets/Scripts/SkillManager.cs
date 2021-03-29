using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public SceneFader fader;
    public static SkillManager instance;
    Skill skill;
    public Button[] buttons;

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

        //Button[] buttons = this.GetComponentsInChildren<Button>();
        //Button[] buttons = this.GetComponentsInChildren<Button>(true); //also gets the inactive components
    }

    public void upgradeSkill(Skill skill)
    {
        //Create function to check if there is enough skill points left
        Debug.Log(buttons);
        skill.upgradeSkill();
    }
    public void Select(string scene)
    {
        fader.FadeTo(scene);
    }
}
