using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class CameraProperties : MonoBehaviour
{

    #region Variables
    public Camera cam;
    public float MinFOV, MaxFOV;
    public PostProcessVolume m_Volume;

    [Header("Aperture")]
    public float MaxAperture;
    DepthOfField depthOfField;

    [Header("Zoom")]
    public float mouseScrollSpeed;
    public float LerpSpeed;
    public Slider ZoomSlider;
    public TextMeshProUGUI ApertureText;

    [Header("Infrared Lights")]
    public Light spotLight;
    public float MinRange, MaxRange, CurrentRange;
    public float MinIntensity,MaxIntensity, CurrentIntensity;
    public float MinLightField, MaxLightField, CurrentLightField;

    [Header("ZoomSFX")]
    public AudioSource source;
    public AudioClip ZoomLoop;
    public AudioMixerGroup CameraSFX_GRP;
    public float ZoomInPitch, ZoomOutPitch;

    private Controls input = null;
    #endregion

    #region BuiltInMethods
    private void Awake()
    {
        input = new Controls();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.CameraZoom.performed += CameraZoomPerformed;
        input.Player.CameraZoom.canceled += CameraZoomCanceled;
    }

    private void OnDisable()
    {
        input.Disable();

        input.Player.CameraZoom.performed -= CameraZoomPerformed;
        input.Player.CameraZoom.canceled -= CameraZoomCanceled;
    }

    // Start is called before the first frame update
    void Start()
    {

        cam = GetComponent<Camera>();

        DepthOfField depth;
        if(m_Volume.profile.TryGetSettings(out depth))
        {
            depthOfField = depth;
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        DepthOfField depth;
        if (m_Volume.profile.TryGetSettings(out depth))
        {
            depthOfField = depth;
        }

        if (cam.fieldOfView == PreviousFOV)
        {
          //  StopZoomSFX();
        }

        spotLight.range = CurrentRange;
        spotLight.intensity = CurrentIntensity;
        spotLight.spotAngle = CurrentLightField;

        HandleZoom();
        ApertureAdjustment();
        UIAdjustment();
    }
    #endregion

    #region CustomMethods

    private void CameraZoomPerformed(InputAction.CallbackContext value)
    {
        zoomInput = value.ReadValue<float>() * 0.015f;
    }

    private void CameraZoomCanceled(InputAction.CallbackContext value)
    {
        zoomInput = 0;
    }

    private float PreviousFOV;
    //private float zoomVelocity = 0.0f;
    private bool isZooming = false;
    private float zoomTimer = 0f;
    public float zoomDelay = 0.01f;
    float zoomInput;

     void HandleZoom()
     {
         //zoomInput = Input.GetAxis("Mouse ScrollWheel");

         if (Mathf.Abs(zoomInput) > 0.01f)
         {
            float zoomChange = zoomInput * mouseScrollSpeed * Time.deltaTime;
            float newZoom = Mathf.Clamp(cam.fieldOfView - zoomChange, MinFOV, MaxFOV);

            if (Mathf.Abs(newZoom - cam.fieldOfView) > 0.01f)
            {
                CurrentRange = Mathf.Clamp(CurrentRange + (zoomChange * 0.1f), MinRange, MaxRange);
                CurrentIntensity = Mathf.Clamp(CurrentIntensity + zoomChange * 0.015f, MinIntensity, MaxIntensity);
                CurrentLightField = Mathf.Clamp(CurrentLightField - zoomChange * 1.5f, MinLightField, MaxLightField);

                cam.fieldOfView = newZoom;

                HandleZoomSFX();
                ResetZoomTimer(); // Reset the timer since zooming is happening
            }
         }
         else
         {
             if (!isZooming)
             {
                 StartZoomTimer(); // Start the timer when zooming stops
             }
         }

         // Update the timer
         if (isZooming)
         {
             zoomTimer -= Time.deltaTime;
             if (zoomTimer <= 0f)
             {
                 StopZoomSFX();
             }
         }

         if(cam.fieldOfView == MinFOV || cam.fieldOfView == MaxFOV)
         {
            StopZoomSFX();
         }
    }


     void StartZoomTimer()
     {
         isZooming = true;
         zoomTimer = zoomDelay;
     }

     void ResetZoomTimer()
     {
         isZooming = false;
         zoomTimer = zoomDelay;
     }

     void HandleZoomSFX()
     {
         if (!source.isPlaying)
         {
            source.pitch = zoomInput > 0 ? ZoomInPitch : ZoomOutPitch;
            source.clip = ZoomLoop;
            source.outputAudioMixerGroup = CameraSFX_GRP;
            source.loop = true;
            source.Play();
        }
     }

     void StopZoomSFX()
     {
         source.loop = false;
         source.Stop();
     }
     
    void ApertureAdjustment()
    {
        float Aperture = depthOfField.aperture.value;
        float CurrentFOV = cam.fieldOfView;
        float ApertureScaler = ((CurrentFOV - MinFOV) + MinFOV) / MinFOV;
        depthOfField.aperture.value = MaxAperture / ApertureScaler; 
    }

    void UIAdjustment()
    {
        ZoomSlider.value = depthOfField.aperture.value;
        ApertureText.text = "F" + depthOfField.aperture.value.ToString("0.0");
    }

    #endregion

}
