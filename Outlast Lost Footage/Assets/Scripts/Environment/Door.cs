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
    [HideInInspector] public CharacterBehaviour _audio;
    [HideInInspector] public CharacterInteraction Ch_interaction;

    public bool unlocked;
    public Transform HandAim;
    public Transform HandAimPosition;

    private Animator anim;

    Interaction interaction;

    public bool HasEvent;
    public AudioSource AudioEvent;
    public AudioClip clip;
    #endregion

    #region BuiltInMethods

    void Start()
    {
        input = FindObjectOfType<InputManager>();
        if(input != null)
        {
            _audio = input.gameObject.GetComponentInChildren<CharacterBehaviour>();
            Characteranim = input.gameObject.GetComponent<CharacterAnimator>();
            Ch_interaction = input.gameObject.GetComponentInChildren<CharacterInteraction>();
        }
        interaction = GetComponent<Interaction>();
        AudioEvent = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
    }

    #endregion

    #region CustomMethods
    private bool canInteract = true;
    private float interactCooldown = 0.2f;

    public void Interact()
    {
        if (canInteract)
        {
            HandAim.position = HandAimPosition.transform.position;

            if (unlocked)
            {
                OpenDoor();
                Characteranim.InteractionType("OpenDoor");
                //Characteranim.CharacterAnim.SetBool("OpenDoor", true);
            }
            else
            {
                Characteranim.InteractionType("OpenDoor");
                //Characteranim.CharacterAnim.SetBool("OpenDoor", true);
                Ch_interaction.DisplayHelpText(interaction.Helptext, true);
                _audio.DoorState(false);
            }

            StartCoroutine(InteractCooldown());
        }
    }

    IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        //Characteranim.CharacterAnim.SetBool("OpenDoor", false);
        canInteract = true;
    }

    void OpenDoor()
    {
        _audio.DoorState(true);
        anim.SetBool("Open", true);
        EventTrigger(HasEvent);
        unlocked = false;
        interaction.Helptext = "";
        input.CanInteract = false;
        input.Interact = false;
    }

    public void EventTrigger(bool Event)
    {
        if (!Event)
            return;
        else
            AudioEvent.PlayOneShot(clip);
    }

    #endregion

}
