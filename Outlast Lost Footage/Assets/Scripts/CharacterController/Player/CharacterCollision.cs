using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{

    #region Variables

    public InputManager input;
    public CharacterStats stats;
    public CharacterMovement movement;
    public Animator anim;
    public float raycastDistance = 0.1f;
    public float DieDistance = 3f;
    public Vector3 Offset;
    public float DeathForce;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public float MidAirDeathCounter;

    [HideInInspector] public Vector3 LastGroundPosition, LandingPosition;

    #endregion

    #region BuiltIn Methods

    bool CanDie;

    void Update()
    {
        //Start the counter while player is in mid air and kill him if he's more than 3 seconds on mid air
        if (!isGrounded)
        {
            MidAirDeathCounter += Time.deltaTime;
        }
        else
        {
            MidAirDeathCounter = 0;
            LastGroundPosition = input.transform.position;
        }

        if (MidAirDeathCounter >= 3f)
        {
            CanDie = true;
            StartCoroutine(KillPlayer());
            MidAirDeathCounter = 0;
        }

        // Perform raycast downward to check if the player is grounded
        if (input.CheckCollision)
        {
            isGrounded = CheckGrounded();
        }
        else
            return;

        if (isGrounded && CanDie)
        {
            anim.SetBool("Dead", true);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") || anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Death"))
            {
                anim.SetBool("Dead", false);
            }
        }

        // Update the animator parameter based on the grounded state
        anim.SetBool("MidAir", !isGrounded);

        Ray ray = new Ray(transform.position + Offset, Vector3.down);
        RaycastHit hit;

        // Cast the ray and check the number of hits
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider && hit.collider.CompareTag("Walkable") && hit.collider.gameObject != gameObject)
            {
                if(hit.distance > DieDistance)
                {
                    //anim.SetBool("Dead", true);
                    CanDie = true;
                }
            }
        }
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
        /*if(Vector3.Distance(LastGroundPosition,LandingPosition) > DieDistance)
        {
            CanDie = true;
            anim.SetBool("Dead", true);
        }*/

        if (collision.gameObject.tag == "Walkable")
        {
            input.MidAir = false;
            anim.SetBool("MidAir", false);
        }

        if(collision.gameObject.tag == "Weapon")
        {
            //Low Health
            //stats.Health -= 80;
        }
    }

    #endregion

    #region Custom Methods
    IEnumerator KillPlayer()
    {
        anim.SetBool("Dead", true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        anim.SetBool("Dead", false);
    }

    #endregion

}