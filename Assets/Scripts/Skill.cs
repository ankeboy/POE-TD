using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{

    public string skillName;
    public Sprite skillSprite;

    [TextArea(1, 3)]
    public string skillDescription;
    public int maxSkilllevel = 5;
    public int skillLevel = 0;
    public Text skillLevelUI;
    public bool Upgradable;
    Button button;
    Image image;

    void Start()
    {
        skillLevel = PlayerPrefs.GetInt(skillName, skillLevel); //sets the level of the skill to the saved level if exist. Else use the default value of 0 in the initial setting.
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        skillLevelUI.text = "Level " + skillLevel.ToString();
        checkMaxLevel();
    }

    public void upgradeSkill()
    {
        Debug.Log(image);
        if (skillLevel < maxSkilllevel)
        {
            skillLevel++;
            skillLevelUI.text = "Level " + skillLevel.ToString();
            PlayerPrefs.SetInt(skillName, skillLevel);  //Updates the level of the skill when it gets leveled up
            //DebugSkillInfo();

            checkMaxLevel();
        }
        else
        {
            Debug.Log("Already at maximum Level");
            //DebugSkillInfo();
        }
    }

    void DebugSkillInfo()
    {
        Debug.Log("Skill Name " + skillName);
        Debug.Log("skillLevel " + skillLevel);
        Debug.Log("Upgradable " + Upgradable);
    }

    void checkMaxLevel()
    {
        if (skillLevel == maxSkilllevel)
        {
            Upgradable = false;
            //button.interactable = false;      //leaves the button on the screen with the disabled color and the user can't click it
            button.enabled = false;             //leaves the button on the screen, the user can't click it, but does NOT use the disabled color
            //button.gameObject.SetActive(false); //removes the button from the UI entirely
            image.color = Color.yellow;
            //Debug.Log("Reached Max level");
        }
    }
}
