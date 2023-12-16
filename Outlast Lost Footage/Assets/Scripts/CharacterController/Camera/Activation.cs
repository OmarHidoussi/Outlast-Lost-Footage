using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{

    #region Variables
    
    public InputManager input;
    public GameObject cameraGFX;

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

    #endregion

    #region Custom Methods

    public void ActivationState()
    {
        cameraGFX.SetActive(input.CameraOn);
    }

    #endregion

}
