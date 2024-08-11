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
    public bool CheckCollision;
    public bool CanMove;
    public bool CanStand;
    public bool CanCrouch;
    public bool CanJump;
    public bool CanDeskSlide;
    public bool MidAir;
    public bool IsSprinting;
    public bool IsCrouching;
    public bool Jump;
    public bool SideWalk;

    [Header("CameraState")]
    public bool EnableCameraMovement;
    public bool IsTitling;
    public bool CameraOn;
    public bool InfraredOn;
    public bool Screenshot;

    [Header("Logic")]
    public TwoBoneIKConstraint constraint;
    public CameraFunctionalities cameraFunc;
    public CharacterInteraction interaction;
    public CharacterMovement movement;
    public CharacterAnimator anim;
    public Rigidbody m_rigidbody;

    [HideInInspector] public Controls input = null;

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
        
        input.Player.Reload.performed += OnReloadPerformed;
        input.Player.Reload.canceled += OnReloadCanceled;
        
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;
        
        if(!IsCrouching)
        {
            input.Player.Crouch.performed += OnCrouchPerformed;
            input.Player.Crouch.canceled += OnCrouchCanceled;
        }
        if (CanStand)
        {
            if (!CrouchOff)
            {
                input.Player.CrouchOff.performed += OnCrouchOffPerformed;
                input.Player.CrouchOff.canceled += OnCrouchOffCanceled;
            }
        }

        input.Player.Sprint.performed += OnSprintPerformed;
        input.Player.Sprint.canceled += OnSprintCanceled;

        if(!CameraOn)
        {
            input.Player.CameraOn.performed += OnCameraOnPerformed;
            input.Player.CameraOn.canceled += OnCameraOnCanceled;
        }
        if (!CameraOff)
        {
            input.Player.CameraOff.performed += OnCameraOffPerformed;
            input.Player.CameraOff.canceled += OnCameraOffCanceled;
        }

        if (!CameraOn)
        {
            if (!InfraredOn)
            {
                input.Player.InfraredOn.performed += OnInfraredOnPerformed;
                input.Player.InfraredOn.performed += OnInfraredOnCanceled;
            }
            if (!InfraredOff)
            {
                input.Player.InfraredOff.performed += OnInfraredOffPerformed;
                input.Player.InfraredOff.canceled += OnInfraredOffCanceled;
            }
        }

        input.Player.Screenshot.performed += OnScreenshotPerformed;
        input.Player.Screenshot.canceled += OnScreenshotCanceled;

    }

    private void OnDisable()
    {
        input.Disable();

        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;

        input.Player.Interact.performed -= OnInteractPerformed;
        input.Player.Interact.canceled -= OnInteractCanceled;

        input.Player.Reload.performed -= OnReloadPerformed;
        input.Player.Reload.canceled -= OnReloadCanceled;
        
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;
        
        if (!IsCrouching)
        {
            input.Player.Crouch.performed += OnCrouchPerformed;
            input.Player.Crouch.canceled += OnCrouchCanceled;
        }
        if(CanStand)
        {
            if (!CrouchOff)
            {
                input.Player.CrouchOff.performed -= OnCrouchOffPerformed;
                input.Player.CrouchOff.canceled -= OnCrouchOffCanceled;
            }
        }

        input.Player.Sprint.performed -= OnSprintPerformed;
        input.Player.Sprint.canceled -= OnSprintCanceled;

        if (!CameraOn)
        {
            input.Player.CameraOn.performed -= OnCameraOnPerformed;
            input.Player.CameraOn.canceled -= OnCameraOnCanceled;
        }
        if(!CameraOff)
        {
            input.Player.CameraOff.performed -= OnCameraOffPerformed;
            input.Player.CameraOff.canceled -= OnCameraOffCanceled;
        }

        if (!InfraredOn)
        {
            input.Player.InfraredOn.performed -= OnInfraredOnPerformed;
            input.Player.InfraredOn.performed -= OnInfraredOnCanceled;
        }
        if (!InfraredOff)
        {
            input.Player.InfraredOff.performed -= OnInfraredOffPerformed;
            input.Player.InfraredOff.canceled -= OnInfraredOffCanceled;
        }

        input.Player.Screenshot.performed -= OnScreenshotPerformed;
        input.Player.Screenshot.canceled -= OnScreenshotCanceled;

    }

    // Start is called before the first frame update
    void Start()
    {
        MidAir = false;
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MidAir)
        {
            HandleInputs();
        }

        if (movement.isExhausted)
            IsSprinting = false;
    }
    #endregion

    #region CustomMethods_KeyboardSupport

    void HandleInputs()
    {
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
            if (SideWalk)
                Mov_Axis.x = 0;
        }

        if (CameraOn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InfraredOn = !InfraredOn;
            }
        }

        /*if (Input.GetKeyUp(KeyCode.R))
        {
            if (!CameraOn)
                return;

            if (cameraFunc.BatterySlider.value <= cameraFunc.BatterySlider.maxValue / 2)
            {
                Reload = true;
            }
            else
            {
                interaction.DisplayHelpText("Battery is full", true);
            }
        }*/

        Sprint();
    }

    void Sprint()
    {
        if (movement.isExhausted)
            return;

        if (anim.CharacterAnim.GetFloat("VelocityY") == 0 && IsSprinting)
            return;

        if (IsCrouching)
            return;

        if (SideWalk)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsSprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsSprinting = false;
        }
    }

    #endregion

    #region CustomMethods_GamepadSupport

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (CanMove)
        {
            if (!SideWalk)
                Mov_Axis.x = value.ReadValue<Vector2>().y;

            Mov_Axis.y = value.ReadValue<Vector2>().x;
        }

    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        Mov_Axis = Vector2.zero;
        IsSprinting = false;
    }

    private void OnInteractPerformed(InputAction.CallbackContext Button)
    {
        if (CanInteract)
            Interact = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext Button)
    {
        Interact = false;
    }

    private void OnReloadPerformed(InputAction.CallbackContext Button)
    {
        if (!CameraOn)
            return;

        if (cameraFunc.BatterySlider.value <= cameraFunc.BatterySlider.maxValue / 2)
        {
            Reload = Button.ReadValueAsButton();
        }
        else
        {
            interaction.DisplayHelpText("Battery is full", true);
        } 
    }

    private void OnReloadCanceled(InputAction.CallbackContext Button) { }
    
    private void OnJumpPerformed(InputAction.CallbackContext Button) 
    {
        if(CanJump)
        {
            if (IsSprinting && !CanInteract)
            {
                if (Mov_Axis.x > 0.85f)
                {
                    Jump = Button.ReadValueAsButton();
                }
            }
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext Button)
    {
        Jump = false;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext Button)
    {
        if (!CanStand)
            return;

        if (SideWalk)
            return;

        if(CanCrouch)
        {
            if (Button.ReadValueAsButton())
                IsCrouching = !IsCrouching;
            CrouchOff = false;
            StartCoroutine(CrouchCountdown());
        }
    }
    
    IEnumerator CrouchCountdown()
    {
        CanCrouch = false;
        yield return new WaitForSeconds(0.6f);
        CanCrouch = true;
    }


    bool CrouchOff = true;
    private void OnCrouchOffPerformed(InputAction.CallbackContext Button)
    {
        if (!CanStand)
            return;

        if (Button.ReadValueAsButton())
        {
            if (CanStand)
            {
                CrouchOff = !CrouchOff;
                IsCrouching = false;
            }
        }
    }

    private void OnCrouchOffCanceled(InputAction.CallbackContext Button) { }

    private void OnCrouchCanceled(InputAction.CallbackContext Button) { }

    private void OnSprintPerformed(InputAction.CallbackContext Button)
    {
        if (movement.isExhausted)
            return;
        
        if (anim.CharacterAnim.GetFloat("VelocityY") == 0 && IsSprinting)
            return;

        if (IsCrouching)
            return;

        if (SideWalk)
            return;

        IsSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext Button) { }

    private void OnCameraOnPerformed(InputAction.CallbackContext Button)
    {
        if (CanMove)
        {
            if (Button.ReadValueAsButton())
                CameraOn = !CameraOn;
            CameraOff = false;
        }
    }

    bool CameraOff = true;
    private void OnCameraOffPerformed(InputAction.CallbackContext Button)
    {
        CameraOff = Button.ReadValueAsButton();
        CameraOn = false;
    }

    private void OnCameraOnCanceled(InputAction.CallbackContext Button) { }

    private void OnCameraOffCanceled(InputAction.CallbackContext Button) { }

    bool InfraredOff = true;
    private void OnInfraredOnPerformed(InputAction.CallbackContext Button)
    {
        if (Button.ReadValueAsButton())
            InfraredOn = !InfraredOn;
        InfraredOff = false;
    }

    private void OnInfraredOffPerformed(InputAction.CallbackContext Button)
    {
        InfraredOff = Button.ReadValueAsButton();
        InfraredOn = false;
    }

    private void OnInfraredOffCanceled(InputAction.CallbackContext Button) { }

    private void OnInfraredOnCanceled(InputAction.CallbackContext Button) { }

    private void OnScreenshotPerformed(InputAction.CallbackContext Button)
    {
        Screenshot = Button.ReadValueAsButton();
    }

    private void OnScreenshotCanceled(InputAction.CallbackContext Button) 
    {
        Screenshot = false;
    }

    #endregion

}
