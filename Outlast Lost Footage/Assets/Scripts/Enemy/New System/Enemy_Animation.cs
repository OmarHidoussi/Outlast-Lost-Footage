using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation : MonoBehaviour
{

    #region Variables

    public Enemy_AI enemyAI;
    public Animator animator;

    public bool ShouldMove;

    [SerializeField] private float AnimationSpeed;
    public float AnimatorTransitionSpeed;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<Enemy_AI>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ShouldMove = enemyAI.nav.remainingDistance >= enemyAI.nav.stoppingDistance;
        animator.SetBool("Move", ShouldMove);

        animator.SetBool("Chase", enemyAI.currentState == Enemy_AI.EnemyState.Chase);
        /*if(enemyAI.currentState == Enemy_AI.EnemyState.Chase)
            InterpolateAnimatorSpeed(1);*/

        //Control the animator Speed when AI agent is investigating
        if (enemyAI.currentState == Enemy_AI.EnemyState.Investigate)
        {
            InterpolateAnimatorSpeed(0.5f);
        }
        else
            InterpolateAnimatorSpeed(0.6f);

        if (enemyAI.PlayerInAttackRange)
            animator.SetBool("Attack", true);
    }

    #endregion

    #region CustomMethods

    void InterpolateAnimatorSpeed(float DesiredSpeed)
    {
        AnimationSpeed = Mathf.Lerp(animator.speed, DesiredSpeed, AnimatorTransitionSpeed * Time.deltaTime);
        animator.SetFloat("Speed", AnimationSpeed);
    }

    public void Attack()
    {
        animator.SetBool("Attack", false);
        Vector3 directionToPlayer = enemyAI.Player.transform.position - transform.position;
        directionToPlayer.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the player during attack
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    #endregion

}
