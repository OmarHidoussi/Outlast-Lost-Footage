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

    public float PatrolSpeed;
    public float InvestigationSpeed;
    public float ChaseSpeed;

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
        bool lostSightWhileChasing = !PlayerInSight && currentState == EnemyState.Chase;
        if (lostSightWhileChasing)
            TriggerInvestigationState = true;

        bool hesitantSight = PlayerInSight && !sight.PlayerChasingNPCRange;

        

        if (PlayerInAttackRange)
            currentState = EnemyState.Attack;
        else if (hesitantSight || TriggerInvestigationState)
        {
            currentState = EnemyState.Investigate;
        }
        else if (PlayerInSight)
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Patrol;

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

    void Chase()
    {
        controller.G_PlayerInSight = true;
        nav.speed = ChaseSpeed;
        if (Player != null)
            nav.SetDestination(Player.transform.position);

        if (PlayerInAttackRange)
        {
            Attack();
        }
    }

    void Patrol()
    {
        /*nav.ResetPath();*/
        nav.speed = PatrolSpeed;
        //nav.SetDestination(Player.transform.position);
        controller.G_PlayerInSight = false;
        patrol.ExecutePatrol();
    }

    float investigationWaitTime = 2f;
    float investigationTimer = 0f;
    bool waitingAtLastSightPosition = false;

    void Investigate()
    {
        controller.G_PlayerInSight = false;
        nav.speed = InvestigationSpeed;

        if (!waitingAtLastSightPosition)
        {
            nav.SetDestination(LastSightPosition);
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
