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
    public Button RoundBonusOption;
    public Button RoundBonusOption2;
    public Button SniperButton;
    public Button ArtilleryButton;
    public Button FrostTowerButton;
    public Button AllDMGButton;
    public Button BurningButton;
    public Button WeakenButton;
    public Button RangeButton;
    public Button CriticalButton;
    public Button BonusBountyButton;

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
        UnlockSkill();              //unlock skill at the start when it was already unlocked
    }

    void Start()    //reenable the skills when they are unlocked when loading the scene.
    {
        UnlockSkill();
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
        UnlockSkill();
    }
    public void Select(string scene)
    {
        fader.FadeTo(scene);
    }

    public void ResetButton(string scene)
    {
        PlayerPrefs.SetInt("Skill Points", PlayerPrefs.GetInt("Player Level", 0));
        PlayerPrefs.SetInt("Increased Damage", 0);
        PlayerPrefs.SetInt("Increased Fire Rate", 0);
        PlayerPrefs.SetInt("Increased Range", 0);
        PlayerPrefs.SetInt("Increased Projectile Speed", 0);
        PlayerPrefs.SetInt("Standard Turret Boost", 0);
        PlayerPrefs.SetInt("Missile Launcher Boost", 0);
        PlayerPrefs.SetInt("Laser Beamer Boost", 0);
        PlayerPrefs.SetInt("Frost Tower Boost", 0);
        PlayerPrefs.SetInt("Sniper Boost", 0);
        PlayerPrefs.SetInt("Artillery Boost", 0);
        PlayerPrefs.SetInt("Money Round Bonus", 0);
        PlayerPrefs.SetInt("Life Bonus", 0);
        PlayerPrefs.SetInt("Round Bonus Option", 0);
        PlayerPrefs.SetInt("Round Bonus Option2", 0);
        PlayerPrefs.SetInt("Increased Burning", 0);
        PlayerPrefs.SetInt("Increased Weaken", 0);
        PlayerPrefs.SetInt("Increased Critical", 0);
        PlayerPrefs.SetInt("All Damage Bonus", 0);
        PlayerPrefs.SetInt("Extra Bounty Bonus", 0);
        fader.FadeTo(scene);
    }

    void UnlockSkill()
    {
        // the interactable needs to be true or false, to activate/deactivate the button. Setting it to the statement that checks if the skill is lvl 5 also returns the boolean.
        RoundBonusOption.interactable = (PlayerPrefs.GetInt("Life Bonus", 0) == 5);
        RoundBonusOption2.interactable = (PlayerPrefs.GetInt("Money Round Bonus", 0) == 5);
        SniperButton.interactable = (PlayerPrefs.GetInt("Standard Turret Boost", 0) == 5);
        ArtilleryButton.interactable = (PlayerPrefs.GetInt("Missile Launcher Boost", 0) == 5);
        FrostTowerButton.interactable = (PlayerPrefs.GetInt("Laser Beamer Boost", 0) == 5);
        AllDMGButton.interactable = (PlayerPrefs.GetInt("Frost Tower Boost", 0) == 5 && PlayerPrefs.GetInt("Artillery Boost", 0) == 5 && PlayerPrefs.GetInt("Sniper Boost", 0) == 5);
        BurningButton.interactable = (PlayerPrefs.GetInt("Increased Damage", 0) == 5);
        WeakenButton.interactable = (PlayerPrefs.GetInt("Increased Damage", 0) == 5 && PlayerPrefs.GetInt("Increased Fire Rate", 0) == 5);
        RangeButton.interactable = (PlayerPrefs.GetInt("Increased Projectile Speed", 0) == 5 && PlayerPrefs.GetInt("Increased Fire Rate", 0) == 5);
        CriticalButton.interactable = (PlayerPrefs.GetInt("Increased Projectile Speed", 0) == 5);
        BonusBountyButton.interactable = (PlayerPrefs.GetInt("Money Round Bonus", 0) == 5);

        /*
        if (PlayerPrefs.GetInt("Life Bonus", 0) == 5)
        {
            RoundBonusOption.interactable = true;
        }
        else
        {
            RoundBonusOption.interactable = false;
        }
        
        

        if (PlayerPrefs.GetInt("Money Round Bonus", 0) == 5)
        {
            RoundBonusOption2.interactable = true;
        }
        else
        {
            RoundBonusOption2.interactable = false;
        }
        */
    }
}
