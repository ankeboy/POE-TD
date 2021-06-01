using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class node : MonoBehaviour
{
    public GameObject rangeIndicator;
    GameObject _rangeIndicator;
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public GameObject notEnoughMoneyUI;
    MoneyUI moneyUI;
    public Color pressColor;
    public Vector3 positionOffset;  //change position of the turret so that it spawns on the node rather than in the node.
    Item[] temporaryEquipment;  //used as a temporary storage for equipment when upgrading turret

    [HideInInspector]    //have it public and editable, but not through editor
    public GameObject turret;

    [HideInInspector]
    public TurretBlueprint turretBlueprint;

    [HideInInspector]
    public int isUpgraded = 0;
    public Renderer rend;
    [HideInInspector]
    public Color startColor;
    BuildManager buildManager;  //load Buildmanager as buildManager

    //load stuff into RAM
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    //set the correct position of the tower. Otherwise it will spawn inside the node
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    //When pressing the mouse button while hovering over node
    public void OnMouseDown()
    {
        //avoids that a single mouseclick does multiple actions at the same position. So when clicking a GameObject, it doesnt set a turret on a node.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        rend.material.color = pressColor;

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        // build turret on this node
        BuildTurret(buildManager.GetTurretToBuild());

    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money");
            StartCoroutine(NotEnoughMoneyBlinking(1f));
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);    //casting instantiated items into a GameObject allows it to be destroyed, freeing up memory
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f); // destroy the effect after 5 seconds to free up memory

        if (turret.name == "BonusHouse(Clone)")
            isUpgraded = 2;

        Debug.Log("Turret Build!");
    }

    public void UpgradeTurret()
    {
        if (isUpgraded == 0)
        {
            if (PlayerStats.Money < turretBlueprint.upgradeCost)
            {
                Debug.Log("Not enough money to upgrade");
                StartCoroutine(NotEnoughMoneyBlinking(1f));
                return;
            }

            PlayerStats.Money -= turretBlueprint.upgradeCost;

            temporaryEquipment = turret.GetComponent<turret>().currentEquipment;
            Debug.Log("temporaryEquipment.Length = " + temporaryEquipment.Length);
            //Get rid of the old turret
            Destroy(turret);

            //Build a new turret
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);    //casting instantiated items into a GameObject allows it to be destroyed, freeing up memory
            turret = _turret;
        }
        else if (isUpgraded == 1)
        {
            if (PlayerStats.Money < turretBlueprint.upgradeCost2)
            {
                Debug.Log("Not enough money to upgrade");
                StartCoroutine(NotEnoughMoneyBlinking(1f));
                return;
            }

            PlayerStats.Money -= turretBlueprint.upgradeCost2;

            temporaryEquipment = turret.GetComponent<turret>().currentEquipment;
            Debug.Log("temporaryEquipment.Length = " + temporaryEquipment.Length);
            //Get rid of the old turret
            Destroy(turret);
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab2, GetBuildPosition(), Quaternion.identity);    //casting instantiated items into a GameObject allows it to be destroyed, freeing up memory
            turret = _turret;
        }

        for (int i = 0; i < temporaryEquipment.Length; i++) //directly setting the temporaryEquipment to currentEquipment doesnt seem to work. Thus I have to manually equip them again.
        {
            if (temporaryEquipment[i] != null)
            {
                //Debug.Log("temporaryEquipment[" + i + "] = " + temporaryEquipment[0]);
                turret.GetComponent<turret>().Equip(temporaryEquipment[i]);
            }
        }

        //Debug.Log("new turret equipment " + turret.GetComponent<turret>().currentEquipment[0]);
        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f); // destroy the effect after 5 seconds to free up memory

        isUpgraded += 1;


        Debug.Log("Turret upgraded");
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount(isUpgraded);

        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null; //makes sure the node doesnt have a turret on it

        isUpgraded = 0;
    }

    //Every time the mouse passes by the collider
    void OnMouseEnter()
    {
        //avoids that when a mouse hovers over a GameObject that the underneath node is selected.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // dont highlight node if no turret is chosen
        if (!buildManager.CanBuild)
            return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
            _rangeIndicator = (GameObject)Instantiate(rangeIndicator, transform.position, Quaternion.identity);
            _rangeIndicator.transform.localScale = new Vector3(buildManager.turretRange * 2, 1, buildManager.turretRange * 2); //dont know why multiplying by 2 is corret. It just seems to fit.

        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
            //_rangeIndicator = (GameObject)Instantiate(rangeIndicator, transform.position, Quaternion.identity);
        }

    }

    // When the mouse exits the collider
    void OnMouseExit()
    {
        if (rend.material.color != Color.green)
            rend.material.color = startColor;
        if (_rangeIndicator)
            Destroy(_rangeIndicator);
    }

    IEnumerator NotEnoughMoneyBlinking(float seconds)
    {
        //notEnoughMoneyUI.SetActive(true);
        MoneyUI.instance.NotEnoughMoney();
        yield return new WaitForSeconds(seconds / 3);
        //notEnoughMoneyUI.SetActive(false);
        MoneyUI.instance.NotEnoughMoney();
        yield return new WaitForSeconds(seconds / 3);
        //notEnoughMoneyUI.SetActive(true);
        MoneyUI.instance.NotEnoughMoney();
        yield return new WaitForSeconds(seconds / 3);
        //notEnoughMoneyUI.SetActive(false);
        MoneyUI.instance.NotEnoughMoney();
    }
}
