using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCrouch : MonoBehaviour
{

    #region Variables

    public InputManager input;
    public Animator anim;

    #endregion

    #region Variables

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            input.CanStand = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            input.CanStand = true;
        }
    }

    #endregion

    #region Custom Methods

    #endregion

}