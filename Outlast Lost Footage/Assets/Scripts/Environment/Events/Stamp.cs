using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{

    #region Variables


    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<InputManager>().CanDeskSlide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<InputManager>().CanDeskSlide = false;
        }
    }

    #endregion

    #region CustomMethods

    #endregion

}
