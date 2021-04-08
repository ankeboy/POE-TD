using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    [HideInInspector]
    public float damage = 50;       //damage value is taken from the tower
    [HideInInspector]
    public float freezeSeconds = 0;
    public float speed = 70f;
    public float explosionRadius = 0f;
    public GameObject impactEffect;
    [HideInInspector]
    public Vector3 dir;
    [HideInInspector]
    public Vector3 dir2;
    [HideInInspector]
    public Vector3 target2;
    Vector3 impactLocation;
    public float bulletLife = 0.5f;
    Vector3 bulletPrevPos;                           //save the previous direction, for double shot when the enemy == null
    public bool missile = false;
    public bool shell = false;
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Start()
    {
        bulletPrevPos = transform.position; //start position of bullet for Raycasting
        if (target != null)
        {
            dir = target.position - transform.position;
            dir.y = 0;
            impactLocation = target.position;
            impactLocation.y = 0.8f;        //roughly the location of the environment path. Alittle inside the path, to simulate better impact.
        }
        Destroy(gameObject, bulletLife);
        return;
    }

    // Update is called once per frame
    void Update()
    {


        if (missile == true)   //missile follows enemy
        {
            //if the target is gone before bullet hits, destroy the bullet
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            // bullet movement
            dir = target.position - transform.position; //direction is the difference between bullets current position and the targets position
            float distanceThisFrame = speed * Time.deltaTime;   // distance moved per instance/frame

            // if the move distance in a frame is less than the distance to the target, there is a hit.
            if (dir.magnitude <= distanceThisFrame)
            {
                GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(effectIns, 2f);
                Explode();
                Destroy(gameObject);
                return;                     //stop executing subsequent code after bullet is dead.
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);   //Space.World sets relative position to the worlds absolute position, so the bullet doesnt circle around target.
            transform.LookAt(target);
            return;
        }
        else if (shell == true)
        {
            if (target == null)
            {
                //Debug.Log("target2: " + target2);
                impactLocation = target2;
                impactLocation.y = 0.8f;
            }

            dir = impactLocation - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, Quaternion.identity);    //no rotation
                Destroy(effectIns, 2f);
                Explode();
                Destroy(gameObject);
                return;                     //stop executing subsequent code after bullet is dead.
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            return;
        }
        else    //bullet go in straight line
        {
            if (target == null)
            {
                dir = dir2;
                dir.y = 0;
            }
            RaycastHit hit;                                 //create raycasting hit object. Info of hit (target, position, etc) are stored in this variable.
            bulletPrevPos = transform.position;             //calculate the ray from the bullets current position
            float distanceThisFrame = speed * Time.deltaTime;   // distance moved per instance/frame
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);   //Space.World sets relative position to the worlds absolute position, so the bullet doesnt circle around target.

            if (Physics.Raycast(bulletPrevPos, dir.normalized, out hit, distanceThisFrame))     //if Physics.Raycasting hits something, it returns true and a hit is realized
            {
                GameObject effectIns = (GameObject)Instantiate(impactEffect, hit.transform.position, transform.rotation);   //had to change the position of instantiation of the effect from the position of the bullet(transform.position) to the position of the hit(hit.transfrom.position)
                Destroy(effectIns, 2f);  //destroy instances so it frees up memory
                Destroy(gameObject);
                Damage(hit.transform);
            }
        }
    }

    /* //not necessary anymore, as hit is calculated via raycasting instead of unities collision detection.
    void OnCollisionEnter(Collision collisionInfo)      
    {
        //Debug.Log(collisionInfo.collider.tag);
        if (collisionInfo.collider.tag == "Enemy")
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);  //destroy instances so it frees up memory
            Damage(collisionInfo.collider.transform);
            Destroy(gameObject);
            return;
        }
    }
    */

    /* //not necessary anymore, this HitTarget() function was only used by the missile turret.
    // When a target is hit:
    void HitTarget()
    {
        //create effect
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);  //destroy instances so it frees up memory

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);     //not needed anymore as the bullet hit is calculated using raycasting
        }

        // destroy bullet
        Destroy(gameObject);
    }
    */
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);  //get all colliders (items) that are in the Sphere of explosion radius
        // for each collider, check if it is tagged as an enemy. If so, damage them.
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }
    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();  //this gets a specific enemy component, instead of affecting all enemies.

        if (e != null)
        {
            e.TakeDamage(damage);
            e.frozen(freezeSeconds);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}