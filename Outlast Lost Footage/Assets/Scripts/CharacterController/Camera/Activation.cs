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
    public CameraFunctionalities functionalities;

    private Animator anim;
    private CameraScreenShot screenshot;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        screenshot = GetComponent<CameraScreenShot>();
    }

    // Update is called once per frame
    void Update()
    {

        if(input.CameraOn/* && !input.InfraredOn*/)
            Screenshot();

    }

    #endregion

    #region Custom Methods

    public void Reload()
    {
        functionalities.LowBatteryAnim.gameObject.GetComponent<Image>().color = functionalities.PureScreenColor;
        Source.Play();
    }

    public void InfraredLights()
    {
        functionalities.NV_Lights.SetActive(anim.GetBool("CameraOn") && input.InfraredOn && input.CameraOn);
        if (!input.CameraOn)
            functionalities.NV_Lights.SetActive(false);
    }

    public void ActivationState()
    {
        cameraGFX.SetActive(input.CameraOn);
        cameraOffObject.SetActive(!input.CameraOn);
        functionalities.NV_Lights.SetActive(false);
    }

    void Screenshot()
    {
        if (Input.GetKeyDown(KeyCode.G) || input.Screenshot)
        {
            screenshot.GetSetImage_BTM();
        }
    }

    #endregion

}
