using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    #region Variables

    public float Sensetivity;
    public float TransitionSpeed;
    public Transform PlayerBody;
    public Transform PlayerGFX;
    public Transform LookBackDirection;
    public Transform RotationCenter;
    public float RotationThreshold;
    public float RotationSpeed;
    public bool Lookback;
    public float LookbackTransitionSpeed;
    public float X_Max, X_Min, SprintX_Max, SprintX_Min, CrouchX_Max, CrouchX_Min;

    public InputManager input;
    public CharacterAnimator Characteranim;
    public CapsuleCollider col;
    public Vector3 offset;
    public Vector3 Crouchingoffset;
    public Vector3 Jumpoffset;

    public Transform CameraHolder;
    //public Animator CamHolderAnim;
    public Vector3 LeftTilt;
    public Vector3 RightTilt;

    private Controls MouseControls = null;
    [HideInInspector] public Vector2 Mouse_Axis = Vector2.zero;

    float xRotation;
    int Factor;

    [Space]
    [Header("Head Tilting")]
    [SerializeField, Range(0f, 0.1f)] private float Frequency = 10f;
    [SerializeField, Range(0f, 0.1f)] private float XAmplitude, ZAmplitude = 0.03f;
    public CharacterMovement movement;

    private Quaternion PreviousLookingDirection;
    bool HeadInTransition;

    public bool coroutineRunning = false;
    public bool hasLookedBack = false;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Awake()
    {
        HeadInTransition = false;

        MouseControls = new Controls();

        Cursor.lockState = CursorLockMode.Locked;

        GameSettings GameData = FindObjectOfType<GameSettings>();
        if (GameData != null)
        {
            Sensetivity = GameData.LookSensitivity;

            if (GameData.InvertUpAxis)
                Factor = -1;
            else
                Factor = 1;
        }
        else
        {
            Factor = 1;
        }
    }

    private void OnEnable()
    {
        MouseControls.Enable();

        MouseControls.Camera.RightStick.performed += OnMovementPerformed;
        MouseControls.Camera.RightStick.canceled += OnMovementCanceled;

    }

    private void OnDisable()
    {
        MouseControls.Disable();

        MouseControls.Camera.RightStick.performed -= OnMovementPerformed;
        MouseControls.Camera.RightStick.canceled -= OnMovementCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Mouse_Axis = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        Mouse_Axis = Vector2.zero;
    }

    private void Update()
    {

        Lookback = Input.GetKey(KeyCode.E) && !HeadInTransition;

        if (Lookback)
        {
            if (!hasLookedBack)
            {
                StartCoroutine(HandleLookBack());
            }
        }
        else if (hasLookedBack && !coroutineRunning)
        {
            StartCoroutine(ResetLookBackMechanic());
        }
    }

    IEnumerator HandleLookBack()
    {
        Debug.Log("LookingBack");

        coroutineRunning = true;

        Quaternion targetRotation = Quaternion.Euler(PreviousLookingDirection.x, 160f, PreviousLookingDirection.z);

        while (Lookback)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 0.025f);  // Adjust 0.1f to control the speed

            // Wait for the next frame before continuing
            yield return null;
        }

        hasLookedBack = true;
        coroutineRunning = false;
    }

    IEnumerator ResetLookBackMechanic()
    {
        Debug.Log("forward");

        coroutineRunning = true;
        HeadInTransition = true;

        while (Quaternion.Angle(transform.localRotation, PreviousLookingDirection) > 0.03f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, PreviousLookingDirection, 0.1f);

            yield return null;
        }

        Lookback = false;
        hasLookedBack = false;
        HeadInTransition = false;
        coroutineRunning = false;
    }


    
    float MouseX;
    float MouseY;
    // Update is called once per frame
    void LateUpdate()
    {
        if (input.EnableCameraMovement)
        {
            if (!Lookback && !coroutineRunning && !HeadInTransition && !hasLookedBack)
            {
                PreviousLookingDirection = transform.localRotation;

                MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * Sensetivity;
                MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensetivity * Factor;

                if (Mouse_Axis != Vector2.zero)
                {
                    MouseX = Mouse_Axis.x * Time.deltaTime * Sensetivity;
                    MouseY = Mouse_Axis.y * Time.deltaTime * Sensetivity * Factor;
                }

                if (input.IsSprinting)
                    xRotation = Mathf.Clamp(xRotation, SprintX_Min, SprintX_Max);
                else if (input.IsCrouching)
                    xRotation = Mathf.Clamp(xRotation, CrouchX_Min, CrouchX_Max);
                else
                {
                    xRotation = Mathf.Clamp(xRotation, X_Min, X_Max);
                }

                xRotation -= MouseY;


                transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                if (!input.SideWalk)
                    PlayerBody.Rotate(Vector3.up * MouseX);

                if (input.Mov_Axis == Vector2.zero)
                {
                    if (!input.SideWalk)
                        PlayerGFX.Rotate(Vector3.up * -MouseX);

                    float angleDifference = Quaternion.Angle(PlayerGFX.transform.rotation, PlayerBody.transform.rotation);

                    Vector3 cross = Vector3.Cross(PlayerGFX.transform.forward, PlayerBody.transform.forward);

                    if (cross.y < 0)
                    {
                        angleDifference = -angleDifference;
                    }

                    if (Mathf.Abs(angleDifference) > RotationThreshold)
                    {
                        //float t = Mathf.Clamp01(RotationSpeed * Time.deltaTime);
                        PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation,
                            RotationSpeed * Time.deltaTime);
                        //RotateBody(PlayerGFX, true, PlayerBody.rotation);
                        /*
                        if (angleDifference < 0)
                        {
                            Characteranim.CharacterAnim.SetBool("TurnLeft", true);
                            PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation, RotationSpeed / 100);
                        }
                        else if (angleDifference > 0)
                        {
                            Characteranim.CharacterAnim.SetBool("TurnRight", true);
                        }

                        else
                        {
                            Characteranim.CharacterAnim.SetBool("TurnRight", false);
                            Characteranim.CharacterAnim.SetBool("TurnLeft", false);
                        }*/
                    }
                }
                else
                {
                    //float t = Mathf.Clamp01(RotationSpeed * 5 * Time.deltaTime);
                    PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation, RotationSpeed * Time.deltaTime);
                    RotateBody(PlayerGFX, false, PlayerBody.rotation);
                    Characteranim.CharacterAnim.SetBool("TurnRight", false);
                    Characteranim.CharacterAnim.SetBool("TurnLeft", false);
                }

                if(xRotation > X_Min/2 && xRotation < X_Max / 2)
                {
                    ApplyHeadTilt();
                }
            }
            /*else
                HandleLookBack();*/

            HandleHeight();
        }
    }

    #endregion

    #region CustomMethods

    float X_tiltAmount;
    float Z_tiltAmount;
    void ApplyHeadTilt()
    {

        if (!input.IsSprinting)
        {
            X_tiltAmount = Mathf.Sin(Time.time * Frequency) * XAmplitude * input.Mov_Axis.x;
            Z_tiltAmount = Mathf.Sin(Time.time * Frequency / 2) * ZAmplitude * input.Mov_Axis.x;
        }
        else
        {
            X_tiltAmount = Mathf.Sin(1.3f * Time.time * Frequency) * (XAmplitude * 2)* input.Mov_Axis.x;
            Z_tiltAmount = Mathf.Sin(1.3f * Time.time * Frequency / 2) * (ZAmplitude * 2)* input.Mov_Axis.x;
        }

        transform.localRotation = new Quaternion(CameraHolder.localRotation.x + X_tiltAmount, CameraHolder.localRotation.y, Z_tiltAmount, CameraHolder.localRotation.w);
    }

    void HandleHeight()
    {
        if (input.IsCrouching)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + Crouchingoffset, TransitionSpeed * 15 * Time.deltaTime);
        }
        else if (input.Jump || Characteranim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("DeskSlideJumping_02")
            || Characteranim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Running Jump"))
        {
            float HeightDifference = col.center.y - (col.center.y - 1);
            if(HeightDifference >= Jumpoffset.y)
            {
                HeightDifference = Jumpoffset.y;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, HeightDifference, 0) + offset, TransitionSpeed * Time.deltaTime);
            xRotation = Mathf.Lerp(xRotation, 0, TransitionSpeed * 5 * Time.deltaTime);
        }
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + offset, 15 * Time.deltaTime);
    }

    public void RotateBody(Transform Body,bool lockPkayer, Quaternion targetRotation)
    {
        if(lockPkayer)
        {

            Body.rotation = Quaternion.RotateTowards(PlayerBody.rotation, targetRotation, TransitionSpeed * 100 * Time.deltaTime);

        }
    }

    #endregion

}
