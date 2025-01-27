using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolLocation
{
    public string locationName; 
    public List<Transform> waypoints = new List<Transform>();
}

public class EnemyAI : MonoBehaviour
{
    #region Variables

    public List<PatrolLocation> locations = new List<PatrolLocation>();

    private EnemySight Sight;
    private EnemyAnimation anim;
    private NavMeshAgent nav;
    private CharacterStats Stats;

    public float patrolSpeed;
    public float chaseSpeed;
    public float attackSpeed;

    private EnemyPatrol patrol;
    private EnemyChase chase;

    [Header("Patrol")]
    public int LocationIndex = 0;
    private int previousLocationIndex;

    [Header("Chase & Investigation")]
    public float SearchRadius;
    public bool IsChasing, IsInvestigating;
    public float investigateTimer;

    #endregion

    #region BuiltIn Methods

    void Start()
    {
        Sight = GetComponent<EnemySight>();
        anim = GetComponentInChildren<EnemyAnimation>();
        nav = GetComponent<NavMeshAgent>();
        Stats = Sight.player.GetComponent<CharacterStats>();

        // Initialize patrol script
        patrol = gameObject.AddComponent<EnemyPatrol>();
        patrol.Initialize(nav, anim, locations[LocationIndex].waypoints, patrolSpeed);

        IsChasing = false;
    }

    void Update()
    {
        if (Stats.Health <= 0) return;

        if (Sight.PlayerInAttackRange)
            Attack();
        else if (Sight.PlayerInSight)
            Chase();
        else if (Sight.LastSightPosition != Sight.resetPosition)
            Investigate();
        else
        {
            patrol.ExecutePatrol(); // Call the patrol logic from the new script
        }
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

        anim.Speed = Mathf.Lerp(anim.Speed, chaseSpeed, 0.5f * Time.deltaTime);
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
                patrol.Initialize(nav, anim, locations[LocationIndex].waypoints, patrolSpeed);
            }
        }
    }

        Vector3 GetRandomPosition(Vector3 SearchArea)
        {
            SearchArea = Random.insideUnitSphere * SearchRadius;
            SearchArea += Stats.transform.position;

            return SearchArea;
        }
    
    #endregion

}

