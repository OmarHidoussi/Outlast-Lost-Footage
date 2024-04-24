using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemySight : MonoBehaviour
{

    #region Variables

    private EnemyAI Behavior;
    private SphereCollider col;
    public Transform player;

    public float FieldOfView;
    private float resetFieldOfView;
    public float ViewDistance;
    public float AttackRange;
    public bool PlayerInSight;
    public bool PlayerInAttackRange;

    public Vector3 LastSightPosition;
    public Vector3 LastPlayerPosition;
    [HideInInspector] public Vector3 resetPosition = new Vector3(1000, 1000, 1000);

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {

        Behavior = GetComponent<EnemyAI>();

        col = GetComponentInChildren<SphereCollider>();
        col.radius = ViewDistance;
        col.enabled = true;

        player = FindObjectOfType<InputManager>().transform;
        LastPlayerPosition = resetPosition;

        resetFieldOfView = FieldOfView;

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
        CapsuleCollider collider;
        collider = player.GetComponentInChildren<CapsuleCollider>();
        //Vector3 direction = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z)/* + transform.up */- transform.position;
        Vector3 direction = player.transform.position - transform.position;
        //if (player.GetComponentInParent<InputManager>().IsCrouching)
          //  direction = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z)/* + transform.up */- transform.position;
        Gizmos.DrawLine(transform.position + transform.up, direction.normalized * ViewDistance);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (PlayerInSight || Behavior.IsInvestigating)
        {
            FieldOfView = 280f;
        }
        else
            FieldOfView = resetFieldOfView;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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

            PlayerInSight = false;
            PlayerInAttackRange = false;

            // continue to chase the player
            if (other.GetComponentInParent<CharacterMovement>().Speed > 4f)
            {
                LastPlayerPosition = other.transform.position;
                PlayerInSight = true;
            }

            if (angle < FieldOfView * 0.5f)
            {
                // Update the ray origin based on effective height
                Ray ray = new Ray(transform.position + new Vector3(0, effectiveHeight * 0.5f, 0), direction.normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, ViewDistance))
                {
                    if (hit.collider.tag == "Player")
                    {
                        //Debug.Log("Player Seen");

                        PlayerInSight = true;

                        LastPlayerPosition = other.transform.position;
                    }

                    // Attacking the player when near the enemy
                    float distance = (LastPlayerPosition - transform.position).magnitude;
                    if (distance <= AttackRange && PlayerInSight)
                    {
                        PlayerInAttackRange = true;
                    }
                }
            }

            LastSightPosition = LastPlayerPosition;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LastPlayerPosition = resetPosition;
            PlayerInSight = false;
        }
    }

    #endregion

    #region Custom Methods

    #endregion

}
