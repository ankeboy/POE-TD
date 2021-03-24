using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint frostTower;
    //reference to build manager
    BuildManager buildManager;
    Image StadardTurretIcon;
    Image MissileLauncherIcon;
    Image LaserBeamerIcon;
    Image FrostTowerIcon;


    void Start()
    {
        buildManager = BuildManager.instance;

        StadardTurretIcon = this.transform.Find("StandardTurretItem").GetComponent<Image>();
        MissileLauncherIcon = this.transform.Find("MissileLauncherItem").GetComponent<Image>();
        LaserBeamerIcon = this.transform.Find("LaserBeamerItem").GetComponent<Image>();
        FrostTowerIcon = this.transform.Find("FrostTower").GetComponent<Image>();
    }

    //select the type of turret from the shop by clicking on the turret icon. This then feeds the type of turret to the "SelectTurretToBuild" to the buildManager.cs script
    public void SelectStandardTurrent()
    {
        DeselectTurretUI();
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
        StadardTurretIcon.color = new Color(62 / 255, 255 / 255, 65 / 255, 1f); //all 0 = black. Last digit is alpha (opacity)
    }

    public void SelectMissileLauncher()
    {
        DeselectTurretUI();
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
        MissileLauncherIcon.color = new Color(62 / 255, 255 / 255, 65 / 255, 1f);
    }
    public void SelectLaserBeamer()
    {
        DeselectTurretUI();
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
        LaserBeamerIcon.color = new Color(62 / 255, 255 / 255, 65 / 255, 1f);
    }
    public void SelectFrostTower()
    {
        DeselectTurretUI();
        Debug.Log("Frost Tower Selected");
        buildManager.SelectTurretToBuild(frostTower);
        FrostTowerIcon.color = new Color(62 / 255, 255 / 255, 65 / 255, 1f);
    }

    void DeselectTurretUI()
    {
        StadardTurretIcon.color = new Color(1, 1, 1, 1f);
        MissileLauncherIcon.color = new Color(1, 1, 1, 1f);
        LaserBeamerIcon.color = new Color(1, 1, 1, 1f);
        FrostTowerIcon.color = new Color(1, 1, 1, 1f);
    }
}
