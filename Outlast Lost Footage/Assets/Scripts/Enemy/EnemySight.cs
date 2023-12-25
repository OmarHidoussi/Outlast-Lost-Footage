using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemySight : MonoBehaviour
{

    #region Variables

    private SphereCollider col;
    public Transform player;

    public float FieldOfView;
    public float ViewDistance;
    public float AttackRange;
    public bool PlayerInSight;
    public bool PlayerInAttackRange;

    public Vector3 LastSightPosition;
    public Vector3 LastPlayerPosition;
    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponentInChildren<SphereCollider>();
        col.radius = ViewDistance;
        col.enabled = true;

        player = FindObjectOfType<InputManager>().transform;
        LastPlayerPosition = new Vector3(1000, 1000, 1000);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ViewDistance);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*if(!PlayerInSight)
        {
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);

            PlayerInSight = false;
            PlayerInAttackRange = false;

            if (angle < FieldOfView * 0.5f)
            {
                Ray ray = new Ray(transform.position, other.transform.position + transform.up);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, ViewDistance))
                {
                    if(hit.collider.tag == "Player")
                    {
                        PlayerInSight = true;

                        LastPlayerPosition = other.transform.position;
                        LastSightPosition = LastPlayerPosition;

                        float distance = (LastPlayerPosition - transform.position).magnitude;
                        if (distance <= AttackRange)
                        {
                            PlayerInAttackRange = true;
                        }
                        else
                        {
                            PlayerInAttackRange = false;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LastSightPosition = LastPlayerPosition;
            PlayerInSight = false;
        }
    }

    #endregion

    #region Custom Methods

    #endregion

}
