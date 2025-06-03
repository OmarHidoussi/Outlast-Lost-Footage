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
    public float WalkLerpSpeed, RunLerpSpeed;

    public AnimationCurve WalkCurve;
    public AnimationCurve RunCurve;
    public AnimationCurve JumpCurve;

    public Transform RayDirection;
    public Rigidbody m_rigidbody;
    public Animator CutsceneAnim;

    [Header("Fatigue")]
    public bool isExhausted;
    public float RunDuration;
    public float StaminaRegainTimer;

    [HideInInspector] public float StaminaTimer;
    [HideInInspector] public float RunRestartTimer;

    bool WallDetected;
    float NormalHeight;
    InputManager input;
    CapsuleCollider col;
    CharacterAnimator anim;
    CharacterCollision collision;

    #endregion

    #region BuiltInMethods

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
        anim = GetComponent<CharacterAnimator>();
        col = GetComponentInChildren<CapsuleCollider>();
        collision = GetComponentInChildren<CharacterCollision>();
        m_rigidbody = GetComponent<Rigidbody>();

        //anim.enabled = false;

        NormalHeight = col.height;
        WallDetected = false;
             
        RunRestartTimer = RunDuration;
        RunDuration = 1;
        StaminaTimer = StaminaRegainTimer;
    }

    // Update is called once per frame
    void Update()
    {
        HandleWallDetection();
        HandleMove();
        HandleSideWalk();
        HandleCrouch();
        HandleFatigue();
    }

    #endregion

    #region CustomMethods

    [HideInInspector] public float targetSpeed;
    void HandleMove()
    {
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

        if (!collision.isGrounded)
        {
            targetSpeed -= 0;
        }

        if (!WallDetected)
        {
            if (targetSpeed == RunSpeed)
                Speed = Mathf.Lerp(Speed, targetSpeed, RunCurve.Evaluate(RunLerpSpeed * Time.deltaTime));
            else if (targetSpeed == WalkSpeed && Speed > WalkSpeed)
                Speed = Mathf.Lerp(Speed, WalkSpeed, 5f * Time.deltaTime);
            else
                Speed = Mathf.Lerp(Speed, targetSpeed, WalkCurve.Evaluate(WalkLerpSpeed * Time.deltaTime));
        }

        if (anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Running Jump") ||
    anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("DeskSlideJumping_02"))
        {
            input.IsCrouching = false;
            transform.Translate(0, 0, JumpCurve.Evaluate(Speed * Time.deltaTime) * 8f);
            m_rigidbody.drag = 10f;
            //return;
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

        WallDetected = false;

        Ray ray = new Ray(RayDirection.transform.position, RayDirection.transform.forward);

        float Raydistance;
        if (input.IsCrouching)
            Raydistance = 0.1f;
        else
            Raydistance = 0.35f;

        // Check if the distance is less than 0.35f
        if (Physics.Raycast(ray, out hit, Raydistance))
        {
            if (hit.transform.tag == "Wall")
            {
                if (input.Mov_Axis.x >= 0)
                {
                    Speed = 0;
                    input.Mov_Axis.x = 0;
                    anim.CharacterAnim.SetFloat("VelocityY", 0);
                    anim.CharacterAnim.SetBool("Sprinting", false);
                    anim.CharacterAnim.SetBool("Jump", false);
                    WallDetected = true;
                }
            }
        }
    }


    void HandleCrouch()
    {
        if (input.IsCrouching || anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Climbing"))
        {
            col.height = Mathf.Lerp(col.height, CrouchHeight, WalkLerpSpeed * Time.deltaTime);
        }
        else
            col.height = Mathf.Lerp(col.height, NormalHeight, WalkLerpSpeed * Time.deltaTime);

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
        CutsceneAnim.enabled = false;
        this.GetComponentInChildren<CharacterCollision>().enabled = true;
    }

    #endregion

}

