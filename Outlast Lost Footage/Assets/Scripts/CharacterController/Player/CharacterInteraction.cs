using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterInteraction : MonoBehaviour
{

    #region Variables

    public InputManager input;
    public CharacterStats stats;
    public TextMeshProUGUI Helptext;
    public TextMeshProUGUI interactionText;
    public Image interactionButton;
    public Rigidbody m_rigidbody;

    private Sprite interactionType;
    private Transform TargetInteractionLocation;
    #endregion

    #region BuiltInMethods

    private void Update()
    {
        if (TargetInteractionLocation != null)
        {
            StartCoroutine(SnapPlayerToInteractionPosition());
        }
    }

    private void OnTriggerEnter(Collider other) //Checking wether the player is using a Gamepad or a keyboard
    {

        var interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable == null)
            return;

        Interaction interaction = other.gameObject.GetComponent<Interaction>();
        if (interaction == null)
            return;

        // Check if any joystick (gamepad) is connected
        bool isGamepadConnected = Input.GetJoystickNames().Length > 0;

        // Check if the keyboard/mouse is being used
        bool isKeyboardOrMouseUsed = Input.anyKey;

        if (isKeyboardOrMouseUsed && !isGamepadConnected)
        {
            interactionType = interaction.KeyboardInteractionButton;
        }
        else
        {
            interactionType = interaction.GamepadInteractionButton;
        }

    }

    void OnTriggerStay(Collider other)
    {
        var Interactable = other.gameObject.GetComponent<IInteractable>();
        if (Interactable == null)
            return;
        else
        {
            input.CanInteract = true;
            Interaction interaction = other.gameObject.GetComponent<Interaction>();

            if (input.Interact)
            {
                if (!interaction.Interacted)
                {
                    DisplayInteractText(interactionType, interaction.InteractionText, false);
                    TargetInteractionLocation = interaction.InteractionLocation;
                    Interactable.Interact();
                }

                interaction.Interacted = true;

            }

            if (!interaction.Interacted)
            {
                DisplayInteractText(interactionType, interaction.InteractionText,true);
            }
        }
    }

    IEnumerator SnapPlayerToInteractionPosition()
    {
        input.CanMove = false;
        input.EnableCameraMovement = false;
        m_rigidbody.useGravity = false;
        input.Mov_Axis = Vector2.zero;
        m_rigidbody.transform.position = Vector3.MoveTowards(m_rigidbody.transform.position, TargetInteractionLocation.position, 2.2f * Time.deltaTime);
        m_rigidbody.transform.rotation = Quaternion.Slerp(m_rigidbody.transform.rotation, TargetInteractionLocation.rotation, 15f * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        input.CanMove = true;
        input.EnableCameraMovement = true;
        m_rigidbody.useGravity = true;
        TargetInteractionLocation = null;
    }

    void OnTriggerExit(Collider other)
    {
        var Interactable = other.gameObject.GetComponent<IInteractable>();
        if (Interactable == null)
            return;
        else
        {
            input.CanInteract = false;
            Interaction interaction = other.gameObject.GetComponent<Interaction>();

            DisplayInteractText(interactionType, interaction.InteractionText, false);
            DisplayHelpText("",false);
        }

    }

    #endregion

    #region CustomMethods

    public void DisplayHelpText(string Text, bool display)
    {
        Helptext.gameObject.GetComponent<Animator>().SetBool("Display", display);
        Helptext.text = Text;
    }

    void DisplayInteractText(Sprite Button,string text, bool display)
    {
        if (interactionButton != null)
        {
            interactionText.text = text;
            interactionButton.sprite = Button;

            interactionButton.gameObject.GetComponent<Animator>().SetBool("Display", display);
            interactionText.gameObject.GetComponent<Animator>().SetBool("Display", display);
        }
        else
            interactionButton.sprite = null;
    }

    #endregion

}
