using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    #region Variables

    // The nav mesh agent's speed when patrolling.
    public float patrolSpeed = 2f;
    // The nav mesh agent's speed when chasing.			
    public float chaseSpeed = 5f;
    // The amount of time to wait when the last sighting is reached.			
    public float chaseWaitTime = 5f;
    // The amount of time to wait when the patrol way point is reached.			
    public float patrolMinWaitTime = 1f;
    public float patrolMaxWaitTime = 5f;
    // An array of transforms for the patrol route.			
    public Transform[] patrolWayPoints;
    // A timer for the patrolWaitTime.				
    float patrolTimer;
    // A counter for the way point array.				
    int wayPointIndex;
    float patrolWaitTime;

    EnemySight Sight;
    NavMeshAgent nav;

    public CharacterStats Stats;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Sight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
    }

    public float investigateTimer = 0f;
    public float investigateDuration = 10f; // Adjust this duration as needed

    // Update is called once per frame
    void Update()
    {
        if (Stats.Health <= 0)
            return;

        if (Sight.PlayerInSight)
        {
            Chase();
        }
        else if (Sight.LastSightPosition != Sight.resetPosition)
        {
            Investigate();
        }
        else
            Patrol();
    }

    #endregion

    #region Custom Methods

    void Chase()
    {
        Debug.Log("Chasing");
        nav.speed = chaseSpeed;
        investigateTimer = investigateDuration;
        nav.SetDestination(Sight.LastPlayerPosition);
    }

    void Investigate()
    {
        Debug.Log("Investigating");
        investigateTimer -= Time.deltaTime;
        nav.SetDestination(Sight.LastSightPosition);

        if (investigateTimer <= 0)
        {
            Debug.Log("Investigation Complete");
            Patrol();
        }
    }

    void Patrol()
    {
        Debug.Log("Patrol");
        if (patrolWayPoints.Length == 0)
        {
            return;
        }

        Sight.LastSightPosition = Sight.resetPosition;
        Sight.LastPlayerPosition = Sight.resetPosition;

        investigateTimer = investigateDuration;

        nav.isStopped = false;

        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;

        // If near the next waypoint or there is no destination...
        if (nav.destination == Sight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            if (patrolTimer == 0)
            {
                patrolWaitTime = Random.Range(patrolMinWaitTime, patrolMaxWaitTime);
            }

            // ... increment the timer.
            patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else
        {
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        }

        // Set the destination to the patrolWayPoint.
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }

    #endregion

}
