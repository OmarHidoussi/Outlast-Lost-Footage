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

    EnemySight Sight;

    public CharacterStats Stats;
    NavMeshAgent nav;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Sight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Stats.Health <= 0)
            return;

        if(Sight.PlayerInSight)
            Chase();
/*
        if (Sight.PlayerInAttackRange)
            Attack();
        else if (Sight.PlayerInSight)
            Chase();
        else if (Sight.LastSightPosition == Sight.LastPlayerPosition)
            Investigate();
        else
            Patrol();
*/
       // nav.SetDestination(Stats.gameObject.transform.position);
    }

    #endregion

    #region Custom Methods

    void Attack()
    {
        Debug.Log("Attacking");
        //Attack the player and Lower their Health
    }

    void Chase()
    {
        Debug.Log("Chasing");
        //Chase After the player
        nav.SetDestination(Stats.gameObject.transform.position);
    }

    void Investigate()
    {
        Debug.Log("Investigating");
        //Investigate the player and try to find where he's hiding
        nav.SetDestination(Sight.LastSightPosition);
    }

    void Patrol()
    {
        Debug.Log("Patrolling");
        //Patrol rotine
    }

    #endregion

}
