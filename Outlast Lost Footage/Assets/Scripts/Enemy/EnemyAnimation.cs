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

    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;
    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        Behavior = GetComponent<EnemyAI>();
        Sight = GetComponent<EnemySight>();

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
        worldDeltaPosition.y = 0; // if jumping or offmesh links it needs more work

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        Velocity = SmoothDeltaPosition / Time.deltaTime;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            Velocity = Vector2.Lerp(Vector2.zero, Velocity, nav.remainingDistance / nav.stoppingDistance);
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
    }

    #endregion

}
