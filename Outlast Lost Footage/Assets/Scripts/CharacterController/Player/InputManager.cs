using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InputManager : MonoBehaviour
{

    #region Variables
    [HideInInspector] public Vector2 Mov_Axis;         
    
    [Header("Interaction")]
    public bool CanInteract;             
    public bool Interact;
    public bool Reload;

    [Header("Movement")]
    public bool CanMove;
    public bool CanStand;
    public bool IsSprinting;
    public bool IsCrouching;

    [Header("CameraState")]
    public bool CameraOn;
    public bool InfraredOn;

    [Header("Logic")]
    public TwoBoneIKConstraint constraint;
    public CameraFunctionalities cameraFunc;
    public CharacterInteraction interaction;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleTransitionLogic();
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
        if (!IsCrouching)
        {
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
        }

        if(!IsSprinting)
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
