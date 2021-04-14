using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class turret : MonoBehaviour
{
    [Header("Equipment")]
    public int numSlots = 2;
    public Item[] currentEquipment;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;     //whenever something is changing in the inventory, we call the function OnItemchangedCallBack

    [Header("Target")]
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public float baserange = 15f;
    [HideInInspector]
    public float range;
    public float basefireRate = 1f;
    [HideInInspector]
    public float fireRate;
    private float fireCountdown = 0f;
    public float basedamage = 20f;
    [HideInInspector]
    public float damage;
    public float basecritChance = 0f;
    [HideInInspector]
    public float critChance = 0f;
    public float basecritDMGMult = 2f;
    [HideInInspector]
    public float critDMGMult = 2f;
    public string specialEffect = "None";
    public float slowAmount = 0f;
    public float burningMult = 0.5f;

    [HideInInspector]
    private float projSpeedMod = 1f;
    public float freezeSeconds = 0f;

    [Header("Support Modifier")]    //need to make these private, otherwise got to change them on every turret
    private float incRange = 1.5f;     //done
    private float incRangeSkillBonus = 0.1f;
    private float incFireRate = 1.5f;      //done
    private float incFireRateSkillBonus = 0.1f;
    private float incDMG = 1.5f;       //done
    private float incDMGSkillBonus = 0.1f;
    private float incProjSpeed = 2f;     //done.
    private float incProjSpeedSkillBonus = 0.2f;
    private bool doubleAttack = false;       //done (doesnt work with laser tower)
    private bool burning = false;
    private bool weaken = false;
    private float incCritChance = 0.2f;     //increase crit chance
    private float incCritDMGMult = 1f;       //increase crit DMG
    private float incCritDMGMultSkillBonus = 0.2f;     //increase crit chance 
    //private bool generosity = false;     //need to recode, since money gained is not tower specific.
    private float incBurningMult = 0.5f;            //base burning multiplier (i.e. 50% of damage per second)
    private float incBurningMultSkillBonus = 0.1f;  //additional 0.1 burning multiplier per level (i.e. 100% of damage per second at level 5)

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;

    [Header("Use Laser")]   //the below stats are called in the Laser() function
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Use surround AOE")]   //the below stats are called in the Laser() function
    public bool useSurroundAOE = false;
    public GameObject AOEeffectPrefab;
    public float slowLength = 2f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;

    // Start is called before the first frame update
    void Awake()    //Need to switch from Start() to Awake(), as Awake(), but not Start() is called upon instantiating an object. Start() is called before update (And thus calling equip() right after upgrading turret doesnt work as the Item[] slots are still 0). 
    {
        //sets up basedamage of turrets based on passive skill tree
        float AllTowerDamageBonus = (1 + (0.02f * PlayerPrefs.GetInt("All Damage Bonus", 0)));
        if (this.name.StartsWith("StandardTurret"))
        {
            Debug.Log("Standard Turret Boost level: " + PlayerPrefs.GetInt("Standard Turret Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Standard Turret Boost", 0))) * AllTowerDamageBonus;
        }
        else if (this.name.StartsWith("MissileLauncher"))
        {
            Debug.Log("Missile Launcher Boost level: " + PlayerPrefs.GetInt("Missile Launcher Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Missile Launcher Boost", 0))) * AllTowerDamageBonus;
        }
        else if (this.name.StartsWith("LaserBeamer"))
        {
            Debug.Log("Laser Beamer Boost level: " + PlayerPrefs.GetInt("Laser Beamer Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Laser Beamer Boost", 0))) * AllTowerDamageBonus;
        }
        else if (this.name.StartsWith("FrostTower"))
        {
            Debug.Log("Frost Tower Boost level: " + PlayerPrefs.GetInt("Frost Tower Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Frost Tower Boost", 0))) * AllTowerDamageBonus;
        }
        else if (this.name.StartsWith("Artillery"))
        {
            Debug.Log("Artillery Boost level: " + PlayerPrefs.GetInt("Artillery Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Artillery Boost", 0))) * AllTowerDamageBonus;
        }
        else if (this.name.StartsWith("Sniper"))
        {
            Debug.Log("Sniper Boost level: " + PlayerPrefs.GetInt("Sniper Boost", 0));
            basedamage = basedamage * (1 + (0.1f * PlayerPrefs.GetInt("Sniper Boost", 0))) * AllTowerDamageBonus;
        }
        else
        {
            Debug.Log(this.name + " has no bonuses.");
        }

        //sets up item bonus qunatity based on skill tree
        incRange += (incRangeSkillBonus * PlayerPrefs.GetInt("Increased Range", 0));
        incFireRate += (incFireRateSkillBonus * PlayerPrefs.GetInt("Increased Fire Rate", 0));
        incDMG += (incDMGSkillBonus * PlayerPrefs.GetInt("Increased Damage", 0));
        incProjSpeed += (incProjSpeedSkillBonus * PlayerPrefs.GetInt("Increased Projectile Speed", 0));
        incCritDMGMult += (incCritDMGMultSkillBonus * PlayerPrefs.GetInt("Increased Critical", 0));
        incBurningMult += (incBurningMultSkillBonus * PlayerPrefs.GetInt("Increased Burning", 0));

        InvokeRepeating("UpdateTarget", 0f, 0.5f);  //makes the function update similar to Update(), with customizable start time and repeat rate
        currentEquipment = new Item[numSlots];
        //Debug.Log("Start Invoked. currentEquipment.Length = " + currentEquipment.Length); //Debug as upgrading turret didnt invoke Start() before equiping item.
    }

    public void Equip(Item newItem)
    {
        for (int i = 0; i < numSlots; i++)
        {
            //Debug.Log("Item to equip = " + newItem);
            if (currentEquipment[i] == null)
            {
                currentEquipment[i] = newItem;
                UpdateStats();
                if (onItemChangedCallBack != null)
                    onItemChangedCallBack.Invoke();
                //Debug.Log("Equip() at Slot : " + i + " is " + currentEquipment[i].name);
                return;
            }
        }
    }

    // find the closest target
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);  //find all enemies (make sure it is plural)
        float shortestDistance = Mathf.Infinity;    //set initial shortest distance to infinity
        GameObject nearestEnemy = null;     //initial nearest enemy is null (doesnt exist)

        foreach (GameObject enemy in enemies)  //for each enemy
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); //find the distance to the selected enemy
            if (distanceToEnemy < shortestDistance) //if the distance to the selected enemy is shorter than the previously recorded one 
            {
                shortestDistance = distanceToEnemy; //set the new shortest distance to the distance to the selected enemy
                nearestEnemy = enemy;    //set the new nearest enemy to the currently selected enemy
            }
        }

        // if you have found a nearest enemy and the distance is within (turret) range
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;    //set the target to the nearest enemy
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateStats();
        fireCountdown -= Time.deltaTime;

        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                if (useSurroundAOE)
                {
                    AOE();
                    if (doubleAttack)
                    {
                        StartCoroutine(SecondAttack());
                    }
                }
                else
                {
                    Shoot();
                    if (doubleAttack)
                    {
                        StartCoroutine(SecondAttack());
                    }
                }
                fireCountdown = 1f / fireRate;
            }
        }




    }

    void LockOnTarget()
    {
        //Target lockon
        Vector3 dir = target.position - transform.position;  //get the direction to look at in 3D coordinate
        Quaternion lookRotation = Quaternion.LookRotation(dir); //convert the 3d coordinate to rotational "math"
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; //change quaternion rotation numbers to 3D euler coordinates
        //Lerp smooth out transition from the first variable to the next variable over a number of iterations given by the third variable
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);  //only rotate in the y-axis
    }

    void Laser()
    {
        float randValue = Random.value;
        if (randValue < critChance)
        {
            //Debug.Log("CRITICAL");
            targetEnemy.TakeDamage(damage * fireRate * Time.deltaTime * critDMGMult);
            ApplyStats(targetEnemy, damage * burningMult * fireRate * Time.deltaTime * critDMGMult);
            targetEnemy.frozen(0.5f * Time.deltaTime);     //fire rate doesnt affect how often the enemy ticks damage.
            targetEnemy.Slow(slowAmount);
        }
        else
        {
            targetEnemy.TakeDamage(damage * fireRate * Time.deltaTime);
            ApplyStats(targetEnemy, damage * burningMult * fireRate * Time.deltaTime);
            targetEnemy.Slow(slowAmount);
        }

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();        //Play instead of enable makes sure that the particles exist during their whole lifetime, rather than suddenly disappear at the end.
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;   //normalized normalizes the vector value to 1

        impactEffect.transform.rotation = Quaternion.LookRotation(dir); //quaternion translates a 3D vector into rotational direction

    }
    void AOE()
    {
        if (useSurroundAOE)
            AOEeffectPrefab.transform.localScale = new Vector3(range / 7, 1, range / 7);  //need to normalize it. Hard coding the base range to 7

        //Debug.Log("AOE: " + range);
        GameObject AOEeffectGO = (GameObject)Instantiate(AOEeffectPrefab, firePoint.position, firePoint.rotation);
        Destroy(AOEeffectGO, 1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);  //get all colliders (items) that are in the Sphere of explosion radius
        // for each collider, check if it is tagged as an enemy. If so, damage them.
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }

    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.damage = damage * (1 + (0.1f * PlayerPrefs.GetInt("Increased Projectile Speed", 0)));
        bullet.critChance = critChance;
        //Debug.Log("critDMGMult: " + critDMGMult + ". incCritDMGMult: " + incCritDMGMult);
        bullet.critDMGMult = critDMGMult;
        bullet.speed = bullet.speed * projSpeedMod;
        bullet.freezeSeconds = freezeSeconds;
        bullet.burning = burning;
        bullet.BurningDoT = damage * (1 + (0.1f * PlayerPrefs.GetInt("Increased Projectile Speed", 0))) * burningMult;
        bullet.weaken = weaken;
        bullet.sourceFireRate = fireRate;
        bullet.dir2 = partToRotate.forward;
        bullet.target2 = target.position;

        if (bullet != null)
            bullet.Seek(target);
    }

    // Draw the range of the turrent when selecting the turrent. If to show permanent, use OnDrawGizmos()
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();  //this gets a specific enemy component, instead of affecting all enemies.

        if (e != null)
        {
            float randValue = Random.value;
            if (randValue < critChance)
            {
                //Debug.Log("CRITICAL");
                e.TakeDamage(damage * critDMGMult);
                ApplyStats(e, damage * burningMult * critDMGMult);
                e.frozen(0.8f / fireRate);        //hardcoded the stun to be 0.8 of the firerate. So, the higher the fire rate, the shorter the stun.
                return;
            }
            else
            {
                e.TakeDamage(damage);
                ApplyStats(e, damage * burningMult);
                e.frozen(freezeSeconds);
            }
        }
    }
    IEnumerator SecondAttack()
    {
        yield return new WaitForSeconds(1f / (5f * fireRate));    // 0.2 of the delay between shooting.
        UpdateStats();
        if (useSurroundAOE)
        {
            AOE();
        }
        else
        {
            Shoot();
        }
    }

    void UpdateStats()
    {
        //resets stats at the beginning of every update loop. Otherwise turret strength increases with every loop.
        range = baserange;
        fireRate = basefireRate;
        damage = basedamage;
        doubleAttack = false;
        projSpeedMod = 1f;
        critChance = basecritChance;
        critDMGMult = basecritDMGMult;
        burning = false;
        weaken = false;
        burningMult = 0f;

        //Checking the stat change on every loop can prevent bugs and reduce complexity.
        for (int i = 0; i < numSlots; i++)
        {
            if (currentEquipment[i] != null)
            {
                if (currentEquipment[i].name == "Increased Fire Rate")
                {
                    fireRate = fireRate * incFireRate;
                }
                else if (currentEquipment[i].name == "Increased Damage")
                {
                    damage = damage * incDMG;
                }
                else if (currentEquipment[i].name == "Double Attack")
                {
                    doubleAttack = true;
                }
                else if (currentEquipment[i].name == "Increased Range")
                {
                    range = range * incRange;
                }
                else if (currentEquipment[i].name == "Increased Projectile Speed")
                {
                    projSpeedMod = projSpeedMod * incProjSpeed;
                }
                else if (currentEquipment[i].name == "Increased Critical")
                {
                    critChance = Mathf.Clamp(critChance + incCritChance, 0f, 1f);   //clamps the value so that it cannot go below 0 or above 1.
                    critDMGMult = critDMGMult + incCritDMGMult;
                }
                else if (currentEquipment[i].name == "Burning")
                {
                    burning = true;
                    burningMult += incBurningMult;
                }
                else if (currentEquipment[i].name == "Weaken")
                {
                    weaken = true;
                }
            }
        }
    }

    public void ApplyStats(Enemy target, float BurningDoT)
    {
        if (burning == true)
        {
            Debug.Log("burning turret BurningDoT: " + BurningDoT + "target.BurningDoT: " + target.BurningDoT);
            if (target.BurningDoT == BurningDoT)                     //if the single hit damage is the same as the burning damage
            {
                Debug.Log("target.BurningDoT == BurningDoT");
                target.CallingBurnFromEnemy();
            }
            else if (target.BurningDoT < BurningDoT)            //if the single hit damage is greater than the current burning damage
            {
                Debug.Log("target.BurningDoT < BurningDoT");
                target.BurningDoT = BurningDoT;                     //set the new burninig damage to the larger damage hit
                target.CallingBurnFromEnemy();
            }
        }
        if (weaken == true)
        {
            target.CallingWeakFromEnemy();
        }
    }
}