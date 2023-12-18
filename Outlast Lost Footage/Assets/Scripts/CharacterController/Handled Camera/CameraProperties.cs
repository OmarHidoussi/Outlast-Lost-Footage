using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("ZoomSFX")]
    public AudioSource source;
    public AudioClip ZoomLoop;
    public float ZoomInPitch, ZoomOutPitch;

    #endregion

    #region BuiltInMethods
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
            StopZoomSFX();
        }

        HandleZoom();
        ApertureAdjustment();
        UIAdjustment();
    }
    #endregion

    #region CustomMethods

    private float PreviousFOV;
    //private float zoomVelocity = 0.0f;
    private bool isZooming = false;
    private float zoomTimer = 0f;
    public float zoomDelay = 0.1f;

    void HandleZoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(zoomInput) > 0.01f)
        {
            float zoomChange = zoomInput * mouseScrollSpeed * Time.deltaTime;
            float newZoom = Mathf.Clamp(cam.fieldOfView - zoomChange, MinFOV, MaxFOV);

            if (Mathf.Abs(newZoom - cam.fieldOfView) > 0.01f)
            {
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

    // Rest of the code remains unchanged

    void HandleZoomSFX()
    {
        if (!source.isPlaying)
        {
            source.pitch = Input.GetAxis("Mouse ScrollWheel") > 0 ? ZoomInPitch : ZoomOutPitch;
            source.clip = ZoomLoop;
            source.loop = true;
            source.Play();
        }
    }

    void StopZoomSFX()
    {
        source.loop = false;
        source.Stop();
    }
    

    /* void HandleZoom()
     {
         PreviousFOV = cam.fieldOfView;
         float Zoom = cam.fieldOfView;
         Zoom -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSpeed * Time.deltaTime;
         cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Zoom, LerpSpeed * Time.deltaTime);
         cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, MinFOV, MaxFOV);

         HandleZoomSFX();
     }

     void HandleZoomSFX()
     {
         if (cam.fieldOfView < PreviousFOV - Offset)
         {
             source.pitch = ZoomOutPitch;
             source.PlayOneShot(ZoomLoop);
         }
         else if (cam.fieldOfView > PreviousFOV + Offset)
         {
             source.pitch = ZoomInPitch;
             source.PlayOneShot(ZoomLoop);
         }
         else
             source.pitch = 1f;
     }*/

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
