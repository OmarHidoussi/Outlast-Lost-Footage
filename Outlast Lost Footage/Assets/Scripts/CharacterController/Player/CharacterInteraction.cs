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

    private Sprite interactionType;
    #endregion

    #region BuiltInMethods

    private void OnTriggerEnter(Collider other)
    {
        var Interactable = other.gameObject.GetComponent<IInteractable>();
        if (Interactable == null)
            return;
        else
        {
            Interaction interaction = other.gameObject.GetComponent<Interaction>();

            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || 
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || 
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                interactionType = interaction.KeyboardInteractionButton;
            }
            else
            {
                interactionType = interaction.GamepadInteractionButton;
            }
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
