using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sighting : MonoBehaviour, IPlayerSighter
{

    #region Variables

    public Enemy_AI enemyAI;
    public Transform player;
    private SphereCollider col;

    public bool DebugPlayerSighting;

    public bool PlayerChasingNPCRange;

    public float FieldOfView;
    private float resetFieldOfView;
    public Enemy_Alarm alarm;
    public float ViewDistance;
    public float AttackRange;

    public LayerMask VisionMask;
    /*
    public Vector3 LastSightPosition;
    public Vector3 LastPlayerPosition;
    [HideInInspector] public Vector3 resetPosition = new Vector3(1000, 1000, 1000);
    */
    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = FindObjectOfType<Enemy_AI>();

        if (this.GetComponent<Enemy_Alarm>())
            alarm = GetComponent<Enemy_Alarm>();

        col = GetComponentInChildren<SphereCollider>();
        col.radius = ViewDistance;
        col.enabled = true;

        player = FindObjectOfType<InputManager>().transform;
        //LastPlayerPosition = resetPosition;

        resetFieldOfView = FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugPlayerSighting)
            NotifyPlayerSeen();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the view distance wire sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ViewDistance);

        // Draw the lines connecting the arc to the sphere
        Vector3 leftRayDirection = Quaternion.AngleAxis(-FieldOfView * 0.5f, transform.up) * transform.forward;
        Vector3 rightRayDirection = Quaternion.AngleAxis(FieldOfView * 0.5f, transform.up) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * ViewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * ViewDistance);

        Gizmos.color = Color.red;
        player = FindObjectOfType<InputManager>().transform;

        Vector3 start = transform.position + transform.up; // e.g. eye height
        Vector3 direction = (player.transform.position - start).normalized;

        Gizmos.DrawLine(start, start + direction * ViewDistance);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.GetComponent<Enemy_AI>())
                PlayerChasingNPCRange = true;

            CapsuleCollider collider = other.GetComponent<CapsuleCollider>();

            // Separate variable to store effective height
            float effectiveHeight = collider.height;

            if (other.GetComponentInParent<InputManager>().IsCrouching)
            {
                // Use crouch height if the player is crouching
                effectiveHeight = collider.height;
            }

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);

            //PlayerInSight = false;
            enemyAI.PlayerInAttackRange = false;

            // continue to chase the player
            if (other.GetComponentInParent<CharacterMovement>().Speed > 4f)
            {
                enemyAI.LastPlayerPosition = other.transform.position;
                NotifyPlayerSeen();
            }

            if (angle < FieldOfView * 0.5f)
            {
                // Update the ray origin based on effective height
                Ray ray = new Ray(transform.position + new Vector3(0, effectiveHeight * 0.5f, 0), direction.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, ViewDistance, VisionMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.tag == "Player")
                    {
                        Debug.Log("Player Hit");

                        NotifyPlayerSeen();
                        enemyAI.LastPlayerPosition = other.transform.position;
                    }

                    // Attacking the player when near the enemy
                    float distance = (enemyAI.LastPlayerPosition - transform.position).magnitude;
                    if (distance <= AttackRange && enemyAI.PlayerInSight)
                    {
                        enemyAI.PlayerInAttackRange = true;
                    }
                }
            }

            enemyAI.LastSightPosition = enemyAI.LastPlayerPosition;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.GetComponent<Enemy_AI>())
                PlayerChasingNPCRange = false;

            if (this.GetComponent<Enemy_AI>())
            {
                enemyAI.PlayerInSight = false;
            }
        }
    }

    #endregion

    #region CustomMethods
    public void NotifyPlayerSeen()
    {
        if (enemyAI != null)
            enemyAI.PlayerInSight = true;

        if(alarm != null)
            alarm.TriggerAlarm();

    }
    #endregion

}
