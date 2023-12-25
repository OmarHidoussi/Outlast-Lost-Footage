using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour, IInteractable
{

    #region Variables

    [HideInInspector] public InputManager input;
    [HideInInspector] public CharacterStats stats;
    [HideInInspector] public CharacterAnimator anim;
    //[HideInInspector] public CharacterAudio _audio;
    [HideInInspector] public CharacterInteraction Ch_interaction;
    public Transform HandAim;

    Interaction interaction;

    #endregion

    #region BuiltInMethods

    void Start()
    {
        input = FindObjectOfType<InputManager>();
        stats = input.gameObject.GetComponent<CharacterStats>();
        anim = input.gameObject.GetComponent<CharacterAnimator>();
        //_audio = input.gameObject.GetComponentInChildren<CharacterAudio>();
        interaction = GetComponent<Interaction>();
        Ch_interaction = input.gameObject.GetComponentInChildren<CharacterInteraction>();
    }


    #endregion

    #region CustomMethods

    public void Interact()
    {

        if (stats.BatteryCounter <= stats.MaxCollectedBatteries - 1)
        {
            Collect();
        }
        else
        {
            Ch_interaction.DisplayHelpText(interaction.Helptext,true);
        }

    }

    void Collect()
    {
        anim.InteractionType("PickUp");
        HandAim.position = transform.position;
        input.CanInteract = false;
        input.Interact = false;
        stats.BatteryCounter += 1;
        Destroy(this.gameObject);
    }

    #endregion

}
