using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    LivesUI livesUI;
    private Transform target;
    private int wavepointIndex = 0;
    private Enemy enemy;
    bool CR_running = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }
    void Update()
    {
        if (enemy.froze > 0f)                               //check that froze must be larger than 0
        {
            if (CR_running == false)                        //the previous frozen state wont be overwritten by new ones (even if they are longer) while coroutine is runnning.
                StartCoroutine(frozen(enemy.froze));
            return;
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= (enemy.speed / 25))  //determine leeway so that fast moving targets dont swing around the waypoint.
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;     //returns the speed of the enemy to its original speed
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives = PlayerStats.Lives - enemy.healthDamage;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
        PlayerStats.Money += (enemy.worth * 2);
        LivesUI.instance.ScreenFlash();
    }

    public IEnumerator frozen(float seconds)
    {
        CR_running = true;
        enemy.speed = 0f;
        yield return new WaitForSeconds(seconds);
        CR_running = false;
        enemy.froze = 0f;
    }
}
