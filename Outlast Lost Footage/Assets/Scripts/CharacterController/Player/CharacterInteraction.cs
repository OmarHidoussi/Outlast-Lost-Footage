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

    #endregion

    #region BuiltInMethods

    void OnTriggerStay(Collider other)
    {
        var Interactable = other.gameObject.GetComponent<IInteractable>();
        if (Interactable == null)
            return;
        else
        {
            input.CanInteract = true;
            Interaction interaction = other.gameObject.GetComponent<Interaction>();
            //input.Interact = Input.GetKeyDown(interaction.InteractionKey);


            if (input.Interact)
            {
                interaction.Interacted = true;
                DisplayInteractText(interaction.InteractionButton, interaction.InteractionText, false);
                
                Interactable.Interact();
            }

            if (!interaction.Interacted)
            {
                DisplayInteractText(interaction.InteractionButton,interaction.InteractionText,true);
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
            
            DisplayInteractText(interaction.InteractionButton, interaction.InteractionText, false);
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
        interactionText.text = text;
        interactionButton.sprite = Button;

        interactionButton.gameObject.GetComponent<Animator>().SetBool("Display", display);
        interactionText.gameObject.GetComponent<Animator>().SetBool("Display", display);
    }

    #endregion

}
