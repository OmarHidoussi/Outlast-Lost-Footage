using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteractable
{

    #region Variables

    [HideInInspector] public InputManager input;
    [HideInInspector] public CharacterStats stats;
    [HideInInspector] public CharacterAudio _audio;
    [HideInInspector] public CharacterInteraction Ch_interaction;

    public bool unlocked;

    private Animator anim;

    Interaction interaction;


    #endregion

    #region BuiltInMethods

    void Start()
    {
        input = FindObjectOfType<InputManager>();
        _audio = input.gameObject.GetComponent<CharacterAudio>();
        Ch_interaction = input.gameObject.GetComponentInChildren<CharacterInteraction>();
        interaction = GetComponent<Interaction>();

        anim = GetComponent<Animator>();
    }

    #endregion

    #region CustomMethods
    public void Interact()
    {

        if (unlocked)
        {
            OpenDoor();  
        }
        else 
        {
            Ch_interaction.DisplayHelpText(interaction.Helptext, true);
            _audio.Door_Locked();
        }

    }

    void OpenDoor()
    {
        _audio.Door_Open();
        anim.SetBool("Open", true);
        unlocked = false;
        interaction.Helptext = "";
        input.CanInteract = false;
        input.Interact = false;
    }

    #endregion

}
