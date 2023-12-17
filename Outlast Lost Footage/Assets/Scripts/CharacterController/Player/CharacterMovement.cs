using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    #region Variables

    public float Speed;
    public float WalkSpeed;
    public float RunSpeed;
    public float CrouchSpeed;
    public float CrouchHeight;
    public float LerpSpeed;
    public Transform RayDirection;

    float NormalHeight;
    InputManager input;
    CapsuleCollider col;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
        col = GetComponentInChildren<CapsuleCollider>();
        NormalHeight = col.height;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleCrouch();
        HandleWallDetection();
    }

    #endregion

    #region CustomMethods

    void HandleMove()
    {
        if (input.IsCrouching)
            Speed = Mathf.Lerp(Speed, CrouchSpeed, 5.0F * Time.deltaTime);
        else
            Speed = Mathf.Lerp(Speed, WalkSpeed, 5.0F * Time.deltaTime);

        if(input.IsSprinting)
            Speed = Mathf.Lerp(Speed, RunSpeed, RunSpeed * Time.deltaTime);
        else
            Speed = Mathf.Lerp(Speed, WalkSpeed, 5.0F * Time.deltaTime);

        if(input.Mov_Axis.y == 0 && input.Mov_Axis.y == 0)
            Speed = Mathf.Lerp(Speed, 0, 5.0F * Time.deltaTime);

        transform.Translate(input.Mov_Axis.y * Speed * Time.deltaTime, 0,  
            input.Mov_Axis.x * Speed * Time.deltaTime);

    }

    void HandleWallDetection()
    {
        RaycastHit hit;
        Ray ray = new Ray(RayDirection.transform.position, RayDirection.transform.forward);

        if(Physics.Raycast(ray, out hit,1f))
        {
            if(hit.transform.tag == "Wall")
            {
                if(input.Mov_Axis.x > 0)
                {
                    Speed = 0;
                }
            }
        }
    }

    void HandleCrouch()
    {
        if (input.IsCrouching)
            col.height = Mathf.Lerp(col.height, CrouchHeight, LerpSpeed * Time.deltaTime);
        else
            col.height = Mathf.Lerp(col.height, NormalHeight, LerpSpeed * Time.deltaTime);
    }

    #endregion

}
