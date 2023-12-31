using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activation : MonoBehaviour
{

    #region Variables
    
    public InputManager input;
    public GameObject cameraGFX;
    public GameObject cameraOffObject;

    public AudioSource Source;
    private Animator anim;
    public CameraFunctionalities functionalities;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        cameraOffObject.SetActive(!input.CameraOn);
    }

    public void Reload()
    {
        functionalities.LowBatteryAnim.gameObject.GetComponent<Image>().color = functionalities.PureScreenColor;
        Source.Play();
    }

    public void InfraredLights()
    {
        //fix Bug
        functionalities.NV_Lights.SetActive(anim.GetBool("CameraOn"));
    }

    #endregion

}
