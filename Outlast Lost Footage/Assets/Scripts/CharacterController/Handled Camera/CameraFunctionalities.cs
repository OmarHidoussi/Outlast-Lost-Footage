using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class CameraFunctionalities : MonoBehaviour
{

    #region Variables

    [Header("SliderFunction")]
    public Slider BatterySlider;
    public float CurrentValue, MinValue, MaxValue;
    public int CheckEvery = 4;
    float previoustime, time;

    [Header("SliderAnimation")]
    public Animator Backgroundanimator;
    public Animator FillAreaanimator;
    public Color NormalColor;
    public Color LowBatteryColor;
    public Color BlueScreenColor;
    public Color PureScreenColor;
    public Image Background, FillArea;
    public int LowBatteryDivision = 4;
    public Animator LowBatteryAnim;
    public Light NightVisionlight;
    public CameraProperties properties;

    [Header("CameraState")]
    public GameObject NV_Lights;
    public PostProcessVolume m_volume;
    public PostProcessProfile CameraOFF, CameraON, CameraNightVision;
    public Image NightVision, NightVisionLights;
    public Sprite NV_ON, NV_OFF, NVL_ON, NVL_OFF;
    public InputManager input;

    [Header("CameraAudio")]
    public AudioSource source;
    public AudioClip NV_On_Clip, NV_OFF_Clip;
    public float TransitionSpeed;
    public AudioMixerSnapshot NightVisionOn;
    public AudioMixerSnapshot NightVisionOff;

    [Header("BatteryCounter")]
    public CharacterStats stats;
    public TextMeshProUGUI BatteryCounter;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        BatterySlider.value = CurrentValue;
        MinValue = BatterySlider.minValue;
        MaxValue = BatterySlider.maxValue;

        time = CurrentValue;
        previoustime = time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBatterySlider();
        HandleColor();
        HandleNightVision();
        HandleCameraState();
        HandleLowBattery();
        HandleBatteryCounter();
    }

    #endregion

    #region CustomMethods

    void HandleBatterySlider()
    {
        if (input.Reload)
        {
            time = BatterySlider.maxValue;
            previoustime = time;
            BatterySlider.value = time;
            LowBatteryAnim.SetBool("CameraBlue", false);
            input.Reload = false;
        }

        if (BatterySlider.value != BatterySlider.minValue)
        {
            if (input.CameraOn)
            {
                time -= Time.deltaTime;
            }
        }

        if (time <= previoustime - CheckEvery)
        {
            BatterySlider.value = time;
            previoustime = time;
        }
    }
    
    void HandleColor()
    {
        if(BatterySlider.value <= BatterySlider.maxValue / LowBatteryDivision) 
        {
            Background.color = LowBatteryColor;
            FillArea.color = LowBatteryColor;
            Backgroundanimator.SetBool("LowBattery", true);
            FillAreaanimator.SetBool("LowBattery", true);
        }
        else
        {
            Background.color = NormalColor;
            FillArea.color = NormalColor;
            Backgroundanimator.SetBool("LowBattery", false);
            FillAreaanimator.SetBool("LowBattery", false);
        }
    }

    private bool soundPlayed = false;
    [HideInInspector] public bool InfraredOn = false;
    void HandleNightVision()
    {
        if (input.InfraredOn && !soundPlayed)
        {
            NightVisionOn.TransitionTo(TransitionSpeed);
            source.PlayOneShot(NV_On_Clip);
            soundPlayed = true;
            NightVision.sprite = NV_ON;
            NightVisionLights.sprite = NVL_ON;
            InfraredOn = true;
        }
        else if(!input.InfraredOn && soundPlayed)
        {
            NightVisionOff.TransitionTo(TransitionSpeed);
            source.PlayOneShot(NV_OFF_Clip);
            soundPlayed = false;
            NightVision.sprite = NV_OFF;
            NightVisionLights.sprite = NVL_OFF;
            InfraredOn = false;
        }


        if (input.CameraOn)
        {
            //NV_Lights.SetActive(input.InfraredOn);
            if(input.InfraredOn)
                NightVisionOn.TransitionTo(TransitionSpeed);
        }
        else
        {
            //NV_Lights.SetActive(false);
            NightVisionOff.TransitionTo(TransitionSpeed);
        }
    }

    void HandleCameraState()
    {
        if (!input.CameraOn)
        {
            m_volume.profile = CameraOFF;
        }
        
        if (input.CameraOn)
        {
            m_volume.profile = CameraON;
        }

        if (input.InfraredOn)
        {
            m_volume.profile = CameraNightVision;
        }
    }

    void HandleLowBattery()
    {
        if (BatterySlider.value <= BatterySlider.minValue)
        {
            LowBatteryAnim.gameObject.GetComponent<Image>().color = BlueScreenColor;
            LowBatteryAnim.SetBool("CameraBlue", true);
        }
        else if(!LowBatteryAnim.GetBool("LowBattery"))
            LowBatteryAnim.gameObject.GetComponent<Image>().color = PureScreenColor;

        if (BatterySlider.value <= BatterySlider.maxValue / LowBatteryDivision)
        {
            LowBatteryAnim.enabled = true;
            LowBatteryAnim.SetBool("LowBattery", true);
            if(BatterySlider.value <= 2)
            {
                NightVisionlight.intensity = Mathf.Lerp(NightVisionlight.intensity, 0, 0.7f * Time.deltaTime);
            }
        }
        else
        {
            LowBatteryAnim.SetBool("LowBattery", false);
            NightVisionlight.intensity = properties.CurrentIntensity;
        }
    }

    void HandleBatteryCounter()
    {
        BatteryCounter.text = stats.BatteryCounter.ToString("0") + "/8";
    }

    #endregion

}
