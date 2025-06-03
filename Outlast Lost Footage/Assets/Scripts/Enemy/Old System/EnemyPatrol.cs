using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent nav;
    private Enemy_Animation anim;
    public List<Transform> waypoints;

    private float patrolTimer;
    private float patrolWaitTime;
    private int wayPointIndex;

    public float patrolSpeed;
    public float patrolMinWaitTime = 1f;
    public float patrolMaxWaitTime = 5f;

    public void Initialize(NavMeshAgent agent, List<Transform> patrolWaypoints)
    {
        //waypoints.Clear();
        nav = agent;
        waypoints = patrolWaypoints;
    }

    public void ExecutePatrol()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        //anim.Speed = patrolSpeed;
        nav.isStopped = false;

        // If near the next waypoint or there is no destination...
        if (nav.destination == Vector3.zero || nav.remainingDistance < nav.stoppingDistance)
        {
            if (patrolTimer == 0)
            {
                patrolWaitTime = Random.Range(patrolMinWaitTime, patrolMaxWaitTime);
            }

            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                wayPointIndex = (wayPointIndex + 1) % waypoints.Count;
                patrolTimer = 0;
            }
        }
        else
        {
            patrolTimer = 0;
        }

        // Set the destination to the current waypoint.
        nav.destination = waypoints[wayPointIndex].position;
    }
}
