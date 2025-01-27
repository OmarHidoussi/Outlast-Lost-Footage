using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private NavMeshAgent nav;
    private EnemyAnimation anim;

    private Vector3 targetPosition;
    private float chaseSpeed;

    public void Initialize(NavMeshAgent agent, EnemyAnimation animation, float speed)
    {
        nav = agent;
        anim = animation;
        chaseSpeed = speed;
    }

    public void ExecuteChase(Vector3 position)
    {
        if (nav == null || anim == null) return;

        targetPosition = position;

        anim.Speed = Mathf.Lerp(anim.Speed, chaseSpeed, 0.2f * Time.deltaTime);
        nav.SetDestination(targetPosition);
    }
}
