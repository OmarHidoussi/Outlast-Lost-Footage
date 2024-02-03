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
    public Rigidbody m_rigidbody;

    [Header("Fatigue")]
    public bool isExhausted;
    public float RunDuration;
    public float StaminaRegainTimer;

    [HideInInspector] public float StaminaTimer;
    [HideInInspector] public float RunRestartTimer;

    float NormalHeight;
    InputManager input;
    CapsuleCollider col;
    CharacterAnimator anim;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
        anim = GetComponent<CharacterAnimator>();
        col = GetComponentInChildren<CapsuleCollider>();
        m_rigidbody = GetComponent<Rigidbody>();


        NormalHeight = col.height;

        RunRestartTimer = RunDuration;
        RunDuration = 1;
        StaminaTimer = StaminaRegainTimer;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleSideWalk();
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
        else if (input.SideWalk)
            targetSpeed = CrouchSpeed - 1f;
        else
            targetSpeed = WalkSpeed;

        if (input.Mov_Axis.y != 0 && input.Mov_Axis.x != 0)
            targetSpeed *= 0.75f;

        if (input.Mov_Axis.x < 0)
            targetSpeed *= 0.63f;

        Speed = Mathf.Lerp(Speed, targetSpeed, 5.0F * Time.deltaTime);

        if (anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Running Jump") ||
            anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("DeskSlideJumping_02"))
        {
            input.IsCrouching = false;
            transform.Translate(0, 0, RunSpeed * Time.deltaTime);
            m_rigidbody.drag = 10f;
        }
        else
        {
            if (input.Mov_Axis.y == 0 && input.Mov_Axis.x == 0)
                Speed = Mathf.Lerp(Speed, 0, 5.0F * Time.deltaTime);

            transform.Translate(input.Mov_Axis.y * Speed * Time.deltaTime, 0,
                input.Mov_Axis.x * Speed * Time.deltaTime);
            m_rigidbody.drag = 0f;
        }

        Speed = Mathf.Clamp(Speed,0, RunSpeed);
        m_rigidbody.velocity = new Vector3(0, m_rigidbody.velocity.y, 0);
    }

    void HandleWallDetection()
    {
        RaycastHit hit;

        Ray ray = new Ray(RayDirection.transform.position, RayDirection.transform.forward);

        // Check if the distance is less than 2.0f
        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            if (hit.transform.tag == "Wall")
            {
                if (input.Mov_Axis.x > 0)
                {
                    Speed = 0;
                    input.Mov_Axis.x = 0;
                    anim.CharacterAnim.SetFloat("VelocityY", 0);
                    anim.CharacterAnim.SetBool("Sprinting", false);
                    anim.CharacterAnim.SetBool("Jump", false);
                }
            }
        }
    }


    void HandleCrouch()
    {
        if (input.IsCrouching || anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Climbing"))
        {
            col.height = Mathf.Lerp(col.height, CrouchHeight, LerpSpeed * Time.deltaTime);
        }
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

    void HandleSideWalk()
    {
        if(input.SideWalk)
        {
            anim.CharacterAnim.SetBool("SideWalk", true);
        }
        else
            anim.CharacterAnim.SetBool("SideWalk", false);

    }


    public void ResetAfterCutScene()
    {
        input.CanMove = true;
        this.GetComponentInChildren<CameraMovement>().Sensetivity = 400;
    }

    #endregion

}

