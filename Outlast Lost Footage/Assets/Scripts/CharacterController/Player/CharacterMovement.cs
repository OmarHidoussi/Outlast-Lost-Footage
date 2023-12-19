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

    [Header("Fatigue")]
    public bool isExhausted;
    public float RunDuration;
    public float StaminaRegainTimer;

    [HideInInspector] public float StaminaTimer;
    [HideInInspector] public float RunRestartTimer;

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

        RunRestartTimer = RunDuration;
        RunDuration = 1;
        StaminaTimer = StaminaRegainTimer;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleCrouch();
        HandleWallDetection();
        HandleFatigue();
    }

    #endregion

    #region CustomMethods

    void HandleMove()
    {
        float targetSpeed;

        if (input.IsCrouching)
            targetSpeed = CrouchSpeed;
        else if (input.IsSprinting && !isExhausted)
            targetSpeed = RunSpeed;
        else
            targetSpeed = WalkSpeed;

        if (input.Mov_Axis.y != 0 && input.Mov_Axis.x != 0)
            targetSpeed *= 0.75f;

        Speed = Mathf.Lerp(Speed, targetSpeed, 5.0F * Time.deltaTime);



        if (input.Mov_Axis.y == 0 && input.Mov_Axis.x == 0)
            Speed = Mathf.Lerp(Speed, 0, 5.0F * Time.deltaTime);

        transform.Translate(input.Mov_Axis.y * Speed * Time.deltaTime, 0,
            input.Mov_Axis.x * Speed * Time.deltaTime);
    }

    void HandleWallDetection()
    {
        RaycastHit hit;
        Ray ray = new Ray(RayDirection.transform.position, RayDirection.transform.forward);

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.tag == "Wall")
            {
                if (input.Mov_Axis.x > 0)
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

    void HandleFatigue()
    {
        if(isExhausted)
        {
            StaminaRegainTimer -= Time.deltaTime;

            if(StaminaRegainTimer <= 1)
            {
                isExhausted = false;
                StaminaRegainTimer = StaminaTimer + 1;
            }
            
        }

        if(input.IsSprinting)
        {
            RunDuration += Time.deltaTime;

            if(RunDuration >= RunRestartTimer + 1)
            {
                isExhausted = true;
                RunDuration = 1;
            }
        }
        else
        {
            RunDuration -= Time.deltaTime;
            if (RunDuration <= 1)
                RunDuration = 1;
        }
    }

    #endregion

}

