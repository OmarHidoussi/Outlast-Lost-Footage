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
    public Vector3 LeftTilt;
    public Vector3 RightTilt;

    private Controls MouseControls = null;
    [HideInInspector] public Vector2 Mouse_Axis = Vector2.zero;

    float xRotation;
    int Factor;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Awake()
    {
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

    float MouseX;
    float MouseY;
    // Update is called once per frame
    void LateUpdate()
    {
        if (!Lookback)
        {
            
            MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * Sensetivity;
            MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensetivity * Factor;

            if(Mouse_Axis != Vector2.zero)
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
                    PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation, RotationSpeed / 100);

                    /*if (angleDifference < 0)
                    {
                        Characteranim.CharacterAnim.SetBool("TurnLeft", true);
                        PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation, RotationSpeed / 100);
                    }
                    else if (angleDifference > 0)
                    {
                        Characteranim.CharacterAnim.SetBool("TurnRight", true);
                    }*/
                }
            }
            else
            {
                //float t = Mathf.Clamp01(RotationSpeed * 5 * Time.deltaTime);
                PlayerGFX.transform.rotation = Quaternion.Slerp(PlayerGFX.transform.rotation, PlayerBody.transform.rotation, RotationSpeed);
                Characteranim.CharacterAnim.SetBool("TurnRight", false);
                Characteranim.CharacterAnim.SetBool("TurnLeft", false);
            }
        }

        HandleHeight();
        //HandleLookback();
    }

    #endregion

    #region CustomMethods
    void HandleHeight()
    {
        if (input.IsCrouching)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + Crouchingoffset, TransitionSpeed * 15 * Time.deltaTime);
        }
        else if (input.Jump || Characteranim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("DeskSlideJumping_02"))
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

    public void RotatePlayerBody(bool lockPkayer, Quaternion targetRotation)
    {
        if(lockPkayer)
        {
            PlayerBody.rotation = Quaternion.RotateTowards(PlayerBody.rotation, targetRotation, TransitionSpeed * 100 * Time.deltaTime);
            //PlayerBody.LookAt(direction.forward);
            //PlayerBody.rotation = Quaternion.RotateTowards(PlayerBody.rotation, targetRotation, TransitionSpeed * Time.deltaTime);

        }
    }

    #endregion
  
}
