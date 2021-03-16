using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint frostTower;
    //reference to build manager
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    //select the type of turret from the shop by clicking on the turret icon. This then feeds the type of turret to the "SelectTurretToBuild" to the buildManager.cs script
    public void SelectStandardTurrent()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }
    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }
    public void SelectFrostTower()
    {
        Debug.Log("Frost Tower Selected");
        buildManager.SelectTurretToBuild(frostTower);
    }
}
