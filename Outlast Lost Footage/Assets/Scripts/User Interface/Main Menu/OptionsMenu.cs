using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    #region Variables

    GameSettings data;
    [SerializeField] private AudioMixer m_AudioMixer;

    Resolution[] resolutions;

    [Header("General")]
    public Button Options;
    public Button General;
    public Toggle Subtitles_Toggle;
    public Toggle Tutorial_Toggle;
    public Toggle Crosshair_Toggle;
    public Toggle HeadMotion_Toggle;
    public Toggle InvertUpAxis_Toggle;
    public Slider Sensitivity_Slider;
    public Slider V_Slider;

    [Header("Graphics")]
    public Toggle Fullscreen_Toggle;
    public TMP_Dropdown resolutionsDropdown;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        data = FindObjectOfType<GameSettings>();

        data.LookSensitivity = Sensitivity_Slider.value;

        HandleResolutionDropdown();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGeneral();
        HandleGraphics();
    }

    private void OnEnable()
    {
        General.Select();
    }

    private void OnDisable()
    {
        Options.Select();
    }
    #endregion

    #region CustomMethods

    public void SetVolume()
    {
        m_AudioMixer.SetFloat("MasterVolume", Mathf.Log10(V_Slider.value) * 20);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void SetTextureQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex, true);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void HandleGeneral()
    {
        data.ShowSubtitles = Subtitles_Toggle.isOn;
        data.ShowTutorial = Tutorial_Toggle.isOn;
        data.ShowCrosshair = Crosshair_Toggle.isOn;
        data.ReduceHeadMotion = HeadMotion_Toggle.isOn;
        data.InvertUpAxis = InvertUpAxis_Toggle.isOn;
        data.LookSensitivity = Sensitivity_Slider.value;
        data.volume = V_Slider.value;
    }

    void HandleGraphics()
    {
        data.Fullscreen = Fullscreen_Toggle.isOn;
    }
    
    void HandleResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }
    
    #endregion

}
