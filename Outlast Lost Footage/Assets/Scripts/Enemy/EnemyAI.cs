using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolLocation
{
    public string locationName; // Name for the location
    public List<Transform> waypoints = new List<Transform>();
}

public class EnemyAI : MonoBehaviour
{

    #region Variables

    public List<PatrolLocation> locations = new List<PatrolLocation>();

    EnemySight Sight;
    EnemyAnimation anim;
    NavMeshAgent nav;

    public CharacterStats Stats;

    public float patrolSpeed;
    public float chaseSpeed;
    public float attackSpeed;

    [Header("Patrol")]
    public int LocationIndex = 0;
    public float patrolMinWaitTime = 1f;    // The amount of time to wait when the patrol way point is reached.			
    public float patrolMaxWaitTime = 5f;    // An array of transforms for the patrol route.			
    float patrolTimer;    // A timer for the patrolWaitTime.				
    int wayPointIndex;    // A counter for the way point array.				
    float patrolWaitTime;

    [Header("Chase & Investigation")]
    public float SearchRadius;
    public bool IsChasing, IsInvestigating;


    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Sight = GetComponent<EnemySight>();
        anim = GetComponentInChildren<EnemyAnimation>();
        nav = GetComponent<NavMeshAgent>();

        IsChasing = false;
    }

    [HideInInspector] public float investigateTimer = 0f;
    public float investigateDuration = 10f; // Adjust this duration as needed

    // Update is called once per frame
    void Update()
    {
        if (Stats.Health <= 0)
            return;

        if (Sight.PlayerInAttackRange)
            Attack();
        else if (Sight.PlayerInSight)
            Chase();
        else if (Sight.LastSightPosition != Sight.resetPosition)
            Investigate();
        else
            Patrol(locations[LocationIndex].waypoints.ToArray());
    }

    #endregion

    #region Custom Methods

    void Attack()
    {
        anim.Speed = attackSpeed;
    }

    void Chase()
    {
        IsChasing = true;
        IsInvestigating = false;

        anim.Speed = chaseSpeed;

        investigateTimer = investigateDuration;
        nav.SetDestination(Sight.LastPlayerPosition);
    }

    [HideInInspector] public bool isSearching = false;
    [HideInInspector] public Vector3 SearchPoint = new Vector3(1000, 1000, 1000);
    public float searchCooldown = 5f; 
    private float currentCooldown = 5f;
    void Investigate()
    {
        if (Sight.PlayerInSight)
            return;

        IsChasing = false;
        IsInvestigating = true;

        anim.Speed = patrolSpeed;

        investigateTimer -= Time.deltaTime;

        if (SearchPoint == Sight.resetPosition && !isSearching)
        {
            nav.SetDestination(Sight.LastSightPosition);
        }
        else
            nav.SetDestination(SearchPoint);

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                // Investigate the area around the player
                SearchPoint = GetRandomPosition(Sight.LastSightPosition);

                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(SearchPoint, out navMeshHit, SearchRadius, NavMesh.AllAreas))
                {
                    // If a valid random point is found, set it as the next waypoint
                    SearchPoint = navMeshHit.position;

                    //Debug.Log("IsSearching");
                    //Debug.Log(SearchPoint);
                    
                    nav.SetDestination(SearchPoint);

                    currentCooldown = searchCooldown;
                    isSearching = true;
                }
            }
            else
            {
                // Reset the isSearching flag after the cooldown period
                SearchPoint = Sight.resetPosition;
                isSearching = false;
            }
            if (investigateTimer <= 0)
            {
                //Debug.Log("Investigation Complete");
                Patrol(locations[LocationIndex].waypoints.ToArray());
            }
        }
    }

    Vector3 GetRandomPosition(Vector3 SearchArea)
    {
        SearchArea = Random.insideUnitSphere * SearchRadius;
        SearchArea += Stats.transform.position;

        return SearchArea;
    }

    void Patrol(Transform[] waypoints)
    {
        //Debug.Log("Patrol");

        if (waypoints.Length == 0)
        {
            return;
        }

        IsChasing = false;
        IsInvestigating = false;

        anim.Speed = patrolSpeed;

        Sight.LastSightPosition = Sight.resetPosition;
        Sight.LastPlayerPosition = Sight.resetPosition;

        investigateTimer = investigateDuration;

        nav.isStopped = false;

        // If near the next waypoint or there is no destination...
        if (nav.destination == Sight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            if (patrolTimer == 0)
            {
                anim.Anim.SetFloat("Speed", 0);
                patrolWaitTime = Random.Range(patrolMinWaitTime, patrolMaxWaitTime);
            }

            // ... increment the timer.
            patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if (wayPointIndex == waypoints.Length - 1)
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

        // Set the destination to the current waypoint.
        nav.destination = waypoints[wayPointIndex].position;
    }

    #endregion

}
