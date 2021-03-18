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
    public float basefireRate = 1f;
    private float fireCountdown = 0f;
    public float basedamage = 20f;
    public string specialEffect = "None";
    public float slowAmount = 0.5f;

    [HideInInspector]
    public float range;
    [HideInInspector]
    public float fireRate;
    [HideInInspector]
    public float damage;

    [Header("Support Modifier")]    //need to make these private, otherwise got to change them on every turret
    private float incRange = 1.5f;     //done
    private float incFireRate = 1.5f;      //done
    private float incDMG = 1.5f;       //done
    public bool doubleAttack = false;       //done (doesnt work with laser tower)
    public bool critChance = false;     //crit chance and damage in one skill. Doesnt make sense when one gets crit damage before crit chance.
    public bool generosity = false;     //need to recode, since money gained is not tower specific.
    public bool incEffect = false;      //only laser tower for now.
    public bool pierce = false;         //wouldnt work with laser or frost tower. Doesnt make sense for missile tower.
    public float incProjSpeed = 1f;     //wouldnt work with laser or frost tower. Doesnt make sense for missile tower.

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
    void Start()
    {
        range = baserange;
        fireRate = basefireRate;
        damage = basedamage;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        currentEquipment = new Item[numSlots];
    }

    public void Equip(Item newItem)
    {
        Debug.Log("current equipment: " + currentEquipment);
        for (int i = 0; i < numSlots; i++)
        {
            if (currentEquipment[i] == null)
            {
                currentEquipment[i] = newItem;
                if (onItemChangedCallBack != null)
                    onItemChangedCallBack.Invoke();
                Debug.Log("Added equipment at Slot : " + i + " is " + currentEquipment[i].name);
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
        //resets stats at the beginning of every update loop. Otherwise turret strength increases with every loop.
        range = baserange;
        fireRate = basefireRate;
        damage = basedamage;
        doubleAttack = false;

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
                else
                {
                    Debug.Log(i);
                }
            }
        }

        if (useSurroundAOE)
            AOEeffectPrefab.transform.localScale = new Vector3(range / 7, 1, range / 7);  //need to normalize it. Hard coding the base range to 7

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
            fireCountdown -= Time.deltaTime;
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
        targetEnemy.TakeDamage(damage * fireRate * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

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
        bullet.damage = damage;

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
            e.TakeDamage(damage);
        }
    }
    IEnumerator SecondAttack()
    {
        yield return new WaitForSeconds(fireRate / 10f);
        if (useSurroundAOE)
        {
            AOE();
        }
        else
        {
            Shoot();
        }
    }
}