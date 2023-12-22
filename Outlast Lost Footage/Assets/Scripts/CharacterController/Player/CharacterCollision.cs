using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    #region Variables

    public InputManager input;
    public Animator anim;
    public float raycastDistance = 0.1f;
    public Vector3 Offset;
    public float DeathForce;

    #endregion

    #region BuiltIn Methods
    void Update()
    {
        // Perform raycast downward to check if the player is grounded
        bool isGrounded = CheckGrounded();

        // Update the animator parameter based on the grounded state
        anim.SetBool("MidAir", !isGrounded);

    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position + Offset, Vector3.down * raycastDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray);
    }

    private bool CheckGrounded()
    {
        Ray ray = new Ray(transform.position + Offset, Vector3.down);
        RaycastHit hit;

        // Cast the ray and check the number of hits
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider && hit.collider.CompareTag("Walkable") && hit.collider.gameObject != gameObject)
            {
                return true;
            }
            else
                return false;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Walkable")
        {
            Debug.Log("Impact");
            input.MidAir = false;
            anim.SetBool("MidAir", false);
        }
    }
    #endregion
}