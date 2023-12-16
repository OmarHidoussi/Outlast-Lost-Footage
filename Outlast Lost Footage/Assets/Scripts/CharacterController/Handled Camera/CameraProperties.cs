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

        HandleZoom();
        ApertureAdjustment();
        UIAdjustment();
    }
    #endregion

    #region CustomMethods

    void HandleZoom()
    {
        float Zoom = cam.fieldOfView;
        Zoom -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSpeed * Time.deltaTime;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Zoom, LerpSpeed * Time.deltaTime);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, MinFOV, MaxFOV);
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
