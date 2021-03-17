using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Singleton pattern: make sure that there is only 1 instance of a build manager and that it is accessible by all nodes.
    public static BuildManager instance; //Reference itself

    // Awaken is called right before start.
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
        }

        instance = this;    //This manager is going to put into the instance variable, which is accessed from anywhere.
    }

    public GameObject buildEffect;
    public GameObject sellEffect;

    private TurretBlueprint turretToBuild;
    private node selectedNode;
    public NodeUI nodeUI;
    public Inventory inventory;

    // check if a turret is selected
    public bool CanBuild { get { return turretToBuild != null; } } //property. We only allow it to get something. It can never be set. Return true if turretToBuild is not equal to null otherwise return false.
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    public void SelectNode(node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;

        Debug.Log("BuildManager Node" + node);
        nodeUI.SetTarget(node); //open UI for the node when the selected node has a turret on it.
        inventory.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }
    //Get whatever turret is selected (from the Shop.cs script) and put it into the "turretToBuild" variable, which is then used in the "BuildTurretOn" function.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }
    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}
