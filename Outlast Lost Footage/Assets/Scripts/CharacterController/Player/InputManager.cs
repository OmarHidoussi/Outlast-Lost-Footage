using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class InputManager : MonoBehaviour
{

    #region Variables
    [HideInInspector] public Vector2 Mov_Axis = Vector2.zero;         
    
    [Header("Interaction")]
    public bool CanInteract;             
    public bool Interact;
    public bool Reload;

    [Header("Movement")]
    public bool CanMove;
    public bool CanStand;
    public bool MidAir;
    public bool IsSprinting;
    public bool IsCrouching;

    [Header("CameraState")]
    public bool CameraOn;
    public bool InfraredOn;

    [Header("Logic")]
    public TwoBoneIKConstraint constraint;
    public CameraFunctionalities cameraFunc;
    public CharacterInteraction interaction;
    public CharacterMovement movement;
    public CharacterAnimator anim;

    private Controls input = null;

    #endregion

    #region BuiltInMethods

    private void Awake()
    {
        input = new Controls();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;

        input.Player.Interact.performed += OnInteractPerformed;
        input.Player.Interact.canceled += OnInteractCanceled;

        input.Player.Crouch.performed += OnCrouchPerformed;
        input.Player.Crouch.canceled += OnCrouchCanceled;

        input.Player.Sprint.performed += OnSprintPerformed;
        input.Player.Sprint.canceled += OnSprintCanceled;

        input.Player.CameraOn.performed += OnCameraOnPerformed;
        input.Player.CameraOn.canceled += OnCameraOnCanceled;

        input.Player.InfraredOn.performed += OnInfraredOnPerformed;
        input.Player.InfraredOn.canceled += OnInfraredOnCanceled;
    }

    private void OnDisable()
    {
        input.Disable();

        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;

        input.Player.Interact.performed -= OnInteractPerformed;
        input.Player.Interact.canceled -= OnInteractCanceled;

        input.Player.Crouch.performed -= OnCrouchPerformed;
        input.Player.Crouch.canceled -= OnCrouchCanceled;

        input.Player.Sprint.performed -= OnSprintPerformed;
        input.Player.Sprint.canceled -= OnSprintCanceled;

        input.Player.CameraOn.performed -= OnCameraOnPerformed;
        input.Player.CameraOn.canceled -= OnCameraOnCanceled;

        input.Player.InfraredOn.performed -= OnInfraredOnPerformed;
        input.Player.InfraredOn.canceled -= OnInfraredOnCanceled;
    }

    // Start is called before the first frame update
    void Start()
    {
        MidAir = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!MidAir)
        {
            HandleInputs();
            HandleTransitionLogic();
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Mov_Axis.x = value.ReadValue<Vector2>().y;
        Mov_Axis.y = value.ReadValue<Vector2>().x;
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        Mov_Axis = Vector2.zero;
    }

    private void OnInteractPerformed(InputAction.CallbackContext Button)
    {
        if(CanInteract)
            Interact = Button.ReadValueAsButton();
    }

    private void OnInteractCanceled(InputAction.CallbackContext Button)
    {
        Interact = false;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext Button)
    {
        if (!IsSprinting)
        {
             if (CanStand)
             {
                 IsCrouching = !Button.ReadValueAsButton();
             }
        }
    }

    private void OnCrouchCanceled(InputAction.CallbackContext Button)
    {
        IsCrouching = false;
    }

    private void OnSprintPerformed(InputAction.CallbackContext Button)
    {
        //HandleTransitionLogic();
        if (!IsCrouching && !movement.isExhausted)
        {
            IsSprinting = Button.ReadValueAsButton();
        }

        if (movement.isExhausted)
            IsSprinting = false;

        if (anim.CharacterAnim.GetFloat("VelocityY") == 0 && IsSprinting)
        {
            IsSprinting = false;
        }
    }

    private void OnSprintCanceled(InputAction.CallbackContext Button)
    {
        IsSprinting = false;
    }

    private void OnCameraOnPerformed(InputAction.CallbackContext Button)
    {
        CameraOn = Button.ReadValueAsButton();
    }

    private void OnCameraOnCanceled(InputAction.CallbackContext Button)
    {
        CameraOn = false;
    }

    private void OnInfraredOnPerformed(InputAction.CallbackContext Button)
    {
        InfraredOn = Button.ReadValueAsButton();
    }

    private void OnInfraredOnCanceled(InputAction.CallbackContext Button)
    {
        InfraredOn = false;
    }

    #endregion

    #region CustomMethods

    
        void HandleInputs()
        {
            if (CanInteract)
            {
                Interact = Input.GetMouseButton(0);
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraOn = !CameraOn;
            }

            if (CameraOn)
                constraint.weight = Mathf.Lerp(constraint.weight, 1, 3f * Time.deltaTime);
            else
                constraint.weight = Mathf.Lerp(constraint.weight, 0, 3f * Time.deltaTime);

            if (CanMove)
            {
                Mov_Axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).normalized;
            }

            if (CameraOn)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    InfraredOn = !InfraredOn;
                }
            }

            if(Input.GetKeyUp(KeyCode.R))
            {
                if (cameraFunc.BatterySlider.value <= cameraFunc.BatterySlider.maxValue / 2)
                {
                    Reload = true;
                }
                else
                {
                    interaction.DisplayHelpText("Battery is full", true);
                }
            }

        }

        void HandleTransitionLogic()
        {

        if (!IsCrouching && !movement.isExhausted)
        {
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
        }

        if (movement.isExhausted)
            IsSprinting = false;

        if (anim.CharacterAnim.GetFloat("VelocityY") == 0 && IsSprinting)
        {
            IsSprinting = false;
        }
        
        if (!IsSprinting)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (CanStand)
                {
                    IsCrouching = !IsCrouching;
                }
            }
        }
    }
    
    #endregion

}
