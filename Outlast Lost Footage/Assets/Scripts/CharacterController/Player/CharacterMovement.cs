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
            Speed = Mathf.Lerp(Speed, RunSpeed, 5.0F * Time.deltaTime);
        else
            Speed = Mathf.Lerp(Speed, WalkSpeed, 5.0F * Time.deltaTime);

        transform.Translate(input.Mov_Axis.y * Speed * Time.deltaTime, 0,  
            input.Mov_Axis.x * Speed * Time.deltaTime);
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
