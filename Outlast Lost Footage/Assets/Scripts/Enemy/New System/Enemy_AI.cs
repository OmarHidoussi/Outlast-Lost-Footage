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


public class Enemy_AI : MonoBehaviour
{

    #region Variables
    public NavMeshAgent nav;
    private Enemy_Sighting sight;
    [HideInInspector] public GameObject Player;

    public List<PatrolLocation> locations = new List<PatrolLocation>();
    private EnemyPatrol patrol;

    [SerializeField] private LevelController controller;

    [Header("Patrol")]
    public int LocationIndex = 0;
    private int previousLocationIndex;

    public bool DebugMode;
    public enum EnemyState { Patrol, Chase, Investigate, Attack, Kill }
    public EnemyState currentState;

    public bool FirstPhaseEnded;

    public float PatrolSpeed;
    public float InvestigationSpeed;
    public float ChaseSpeed;
    public float TransitionSpeed = 2f;

    public bool PlayerInSight;
    public bool PlayerInRange;
    public bool PlayerInAttackRange;

    bool TriggerInvestigationState;

    public Vector3 LastSightPosition;
    public Vector3 LastPlayerPosition;
    [HideInInspector] public Vector3 resetPosition = new Vector3(1000, 1000, 1000);
    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        //FirstPhaseEnded = false;
        nav = GetComponent<NavMeshAgent>();
        sight = GetComponent<Enemy_Sighting>();
        Player = FindObjectOfType<InputManager>().gameObject;

        currentState = EnemyState.Patrol;
        nav.speed = PatrolSpeed;

        LastPlayerPosition = resetPosition;

        // Initialize patrol script
        if (patrol == null)
        {
            patrol = gameObject.AddComponent<EnemyPatrol>();
        }
        patrol.Initialize(nav, locations[LocationIndex].waypoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FirstPhaseEnded)
            return;
        if (DebugMode)
            AgentStateDebugger();
        else
            UpdateAgentState();

        WallDetection();
    }
    #endregion

    #region CustomMethods

    void WallDetection() 
    { 

    }

    void AgentStateDebugger()
    {
        //Debug Mode
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Investigate:
                Investigate();
                break;
            case EnemyState.Kill:
                Kill();
                break;
        }
    }
    void UpdateAgentState()
    {
        // Define local flags
        bool lostSightWhileChasing = !PlayerInSight && currentState == EnemyState.Chase;
        bool hesitantSight = PlayerInSight && !sight.PlayerChasingNPCRange;

        if (lostSightWhileChasing)
            TriggerInvestigationState = true;

        // Decide next state based on conditions
        if (PlayerInAttackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (PlayerInSight)
        {
            currentState = EnemyState.Chase;
        }
        else if (TriggerInvestigationState || hesitantSight)
        {
            currentState = EnemyState.Investigate;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        // Handle state logic
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Investigate:
                Investigate();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Kill:
                Kill();
                break;
        }

    }

    void Chase()
    {

        if (!CheckifDestinationIsOnNavMesh(Player.transform.position))
        {
            currentState = EnemyState.Patrol;
            PlayerInSight = false;
            FindObjectOfType<AdaptiveMusic>().AS_ChaseTrack.Stop();
            return;
        }

        Debug.Log("Chasing");
        controller.G_PlayerInSight = true;
        nav.speed = Mathf.Lerp(nav.speed, ChaseSpeed, TransitionSpeed * Time.deltaTime);
        nav.stoppingDistance = 1.1f;
        if (Player != null)
            nav.SetDestination(Player.transform.position);

        if (PlayerInAttackRange)
        {
            Attack();
        }
    }

    void Patrol()
    {
        Debug.Log("Patroling");
        /*nav.ResetPath();*/
        nav.speed = Mathf.Lerp(nav.speed, PatrolSpeed, TransitionSpeed * Time.deltaTime);
        nav.stoppingDistance = 2f;
        //nav.SetDestination(Player.transform.position);
        controller.G_PlayerInSight = false;
        patrol.ExecutePatrol();
    }

    float investigationWaitTime = 2f;
    float investigationTimer = 0f;
    bool waitingAtLastSightPosition = false;

    void Investigate()
    {
        Debug.Log("Investigating");

        controller.G_PlayerInSight = false;
        nav.speed = Mathf.Lerp(nav.speed, InvestigationSpeed,TransitionSpeed * Time.deltaTime);

        if (!waitingAtLastSightPosition)
        {
            if(CheckifDestinationIsOnNavMesh(LastSightPosition))
                nav.SetDestination(LastSightPosition);
            else
            {
                currentState = EnemyState.Patrol;
                FindObjectOfType<AdaptiveMusic>().AS_InvestigationTrack.Stop();
                return;
            }

            if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
            {
                waitingAtLastSightPosition = true;
                investigationTimer = 0f;
            }
        }
        else
        {
            investigationTimer += Time.deltaTime;
            if (investigationTimer >= investigationWaitTime)
            {
                // Finished waiting, switch to Patrol
                PlayerInSight = false;
                waitingAtLastSightPosition = false;
                investigationTimer = 0f;
                TriggerInvestigationState = false;

                //currentState = EnemyState.Patrol;
            }
        }
    }

    bool CheckifDestinationIsOnNavMesh(Vector3 target)
    {
        NavMeshHit hit;
        float maxDistance = 1.0f; // How far to search for a nearby NavMesh point

        if (NavMesh.SamplePosition(target, out hit, maxDistance, NavMesh.AllAreas))
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    void Attack()
    {
        // Attack the player
        Debug.Log("Attack UwU");
        nav.ResetPath();
    }

    void Kill()
    {
        nav.speed = 0;
        //Play Kill Animation
    }
    #endregion
}
