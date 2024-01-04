using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressButtonTrigger : MonoBehaviour, IInteractable
{
    #region Variables

    public InputManager input;
    [HideInInspector] public CharacterInteraction Ch_interaction;

    #endregion

    public void Interact()
    {
        Destroy(gameObject);
    }
}