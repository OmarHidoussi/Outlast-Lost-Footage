using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IInteractable
{

    #region Variables

    [HideInInspector] public InputManager input;
    [HideInInspector] public CharacterStats stats;
    [HideInInspector] public CharacterAnimator Characteranim;
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
        _audio = input.gameObject.GetComponentInChildren<CharacterAudio>();
        Characteranim = input.gameObject.GetComponent<CharacterAnimator>();
        Ch_interaction = input.gameObject.GetComponentInChildren<CharacterInteraction>();
        interaction = GetComponent<Interaction>();

        anim = GetComponent<Animator>();
    }

    #endregion

    #region CustomMethods
    private bool canInteract = true;
    private float interactCooldown = 1f;

    public void Interact()
    {
        if (canInteract)
        {
            if (unlocked)
            {
                OpenDoor();
                Characteranim.InteractionType("OpenDoor");
            }
            else
            {
                Ch_interaction.DisplayHelpText(interaction.Helptext, true);
                _audio.DoorState(false);
                Characteranim.InteractionType("OpenDoor");
            }

            StartCoroutine(InteractCooldown());
        }
    }

    IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }

    void OpenDoor()
    {
        _audio.DoorState(true);
        anim.SetBool("Open", true);
        unlocked = false;
        interaction.Helptext = "";
        input.CanInteract = false;
        input.Interact = false;
    }

    #endregion

}
