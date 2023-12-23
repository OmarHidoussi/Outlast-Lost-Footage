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
    public Transform LookBackDirection;
    public Transform RotationCenter;
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

            PlayerBody.Rotate(Vector3.up * MouseX);
        }

        HandleHeight();
        //HandleLookback();
    }

    #endregion

    #region CustomMethods
    void HandleHeight()
    {
        transform.localPosition = new Vector3(0, col.center.y, 0) + offset;

        if (input.IsCrouching)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + Crouchingoffset, TransitionSpeed * Time.deltaTime);
        }
        else if (!input.m_rigidbody.useGravity)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Jumpoffset, TransitionSpeed / 5 * TransitionSpeed);
        }
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + offset, TransitionSpeed / 105 * Time.deltaTime);


    }

  /*
    bool isTransitioning = false;

    void HandleLookback()
    {
        Lookback = Input.GetMouseButton(2);

        if (Lookback && !isTransitioning)
        {
            StartCoroutine(LookBackSmoothly());
        }
        else if (!Lookback && isTransitioning)
        {
            StartCoroutine(ReturnToForwardSmoothly());
        }
    }

    IEnumerator LookBackSmoothly()
    {
        isTransitioning = true;

        Quaternion targetRotation = Quaternion.LookRotation(LookBackDirection.forward, Vector3.up);

        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);

            elapsedTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        transform.rotation = targetRotation;

        isTransitioning = false;
    }

    IEnumerator ReturnToForwardSmoothly()
    {
        isTransitioning = true;

        Vector3 targetForward = PlayerBody.forward;
        float maxAngle = Vector3.Angle(transform.forward, targetForward);

        while (maxAngle > 0.1f)
        {
            // Calculate the rotation step
            float step = RotationSpeed * Time.deltaTime;

            // Rotate towards the target forward direction
            transform.forward = Vector3.RotateTowards(transform.forward, targetForward, step, 0.0f);

            // Update the maximum angle
            maxAngle = Vector3.Angle(transform.forward, targetForward);

            yield return null;
        }

        isTransitioning = false;
    }
    */

    #endregion
  
}
