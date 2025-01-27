using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    
    #region Variables

    public Animator Anim;
    private NavMeshAgent nav;
    private EnemyAI Behavior;
    private EnemySight Sight;

    public float Speed;

    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;
    #endregion

    #region BuiltInMethods
    /*
    void Awake()
    {
        Behavior = GetComponent<EnemyAI>();

        Anim = GetComponentInParent<Animator>();
        nav = GetComponentInParent<NavMeshAgent>();
        Sight = GetComponentInParent<EnemySight>();
    }

    private void Update()
    {
        bool shouldMove = Anim.velocity.magnitude > 0.1f && nav.remainingDistance > nav.stoppingDistance;
        Anim.SetBool("Move", shouldMove);

        if (!Sight.PlayerInAttackRange)
            Anim.SetBool("Chase", Behavior.IsChasing);
    }
*/
    #endregion


    
    #region BuiltIn Methods

    // Start is called before the first frame update
    void Awake()
    {
        Behavior = GetComponent<EnemyAI>();

        Anim = GetComponentInParent<Animator>();
        nav = GetComponentInParent<NavMeshAgent>();
        Sight = GetComponentInParent<EnemySight>();

        Anim.applyRootMotion = true;

        nav.updatePosition = false;
        nav.updateRotation = true;
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = Anim.rootPosition;
        rootPosition.y = nav.nextPosition.y;
        transform.position = rootPosition;
        transform.rotation = Anim.rootRotation;
        nav.nextPosition = rootPosition;
    }

    // Update is called once per frame
    void Update()
    {
        SynchronizeAnimatoraAndAgent();

        if (Sight.PlayerInAttackRange)
            Anim.SetBool("Attack", true);
    }

    #endregion

    #region Custom Methods

    void SynchronizeAnimatoraAndAgent()
    {
        Vector3 worldDeltaPosition = nav.nextPosition - transform.position;
        worldDeltaPosition.y = 0; 
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);
        Velocity = SmoothDeltaPosition / Time.deltaTime;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            //Velocity = Vector2.Lerp(Vector2.zero, Velocity, nav.remainingDistance / nav.stoppingDistance);
            Velocity.x = Mathf.Lerp(0, Velocity.x, nav.remainingDistance / nav.stoppingDistance);
            Velocity.y = Mathf.Lerp(0,  Speed, nav.remainingDistance / nav.stoppingDistance);
        }

        bool souldMove = Velocity.magnitude > 0.5f && nav.remainingDistance > nav.stoppingDistance;
        Anim.SetBool("Move", souldMove);
        Anim.SetFloat("Speed", Velocity.magnitude);
        if(!Sight.PlayerInAttackRange)
            Anim.SetBool("Chase", Behavior.IsChasing);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > nav.radius / 2f)
        {
            transform.position = Vector3.Lerp(Anim.rootPosition, nav.nextPosition, smooth);
        }
    }

    public void Attack()
    {
        Anim.SetBool("Attack", false);
        Vector3 directionToPlayer = Sight.player.position - transform.position;
        directionToPlayer.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the player during attack
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    #endregion
    
}
