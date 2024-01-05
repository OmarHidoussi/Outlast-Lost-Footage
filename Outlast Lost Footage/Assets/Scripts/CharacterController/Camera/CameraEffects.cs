using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    #region Variables

    public CharacterMovement movement;
    public CharacterAudio info;
    public CharacterStats stats;
    public PostProcessVolume Player_Volume;
    public float TransitionSpeed;
    public float CurrentHealth;


    [Header("Effects")]
    Vignette vignette;
    LensDistortion distortion;
    ChromaticAberration aberration;
    MotionBlur motionBlur;
    public float vignetteMaxIntensity, distortionMaxIntensity, aberrationMaxIntensity;
    public float frequency;
    public Color RedVignette;

    #endregion

    #region BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponentInChildren<CharacterAudio>();
        stats = GetComponentInParent<CharacterStats>();

        CurrentHealth = stats.Health;
        if (Player_Volume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.value = Mathf.Clamp((movement.RunDuration / movement.RunRestartTimer) * 0.3f, 0f, 0.3f);
        }
        else
        {
            Debug.LogError("Failed to get Vignette settings from the PostProcessVolume.");
        }

        if(Player_Volume.profile.TryGetSettings(out distortion))
        {
            distortion.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Failed to get Lens Distortion settings from the PostProcessVolume.");
        }

        if (Player_Volume.profile.TryGetSettings(out aberration))
        {
            aberration.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Failed to get Chromatic Aberration settings from the PostProcessVolume.");
        }

        if(Player_Volume.profile.TryGetSettings(out motionBlur))
        {
            motionBlur.shutterAngle.value = 120f;
        }
        else
        {
            Debug.LogError("Failed to get Motion Blur settings from the PostProcessVolume.");
        }

        vignette.intensity.value = 0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerStamina();
        PlayerSlide();
        PlayerHealth();
    }
    #endregion

    #region Custom Methods

    void PlayerStamina()
    {   
        if(movement.isExhausted)
        {
            vignette.intensity.value = (movement.StaminaRegainTimer / movement.StaminaTimer) * vignetteMaxIntensity;
            distortion.intensity.value = Mathf.Sin(Time.time * movement.StaminaRegainTimer);

            distortion.intensity.value = Mathf.Sin(Time.time * frequency) * movement.StaminaRegainTimer;

            aberration.intensity.value = Mathf.Sin((Time.time * 2 - 1) * frequency) * (movement.StaminaRegainTimer / movement.StaminaTimer);
        }
        else
        {
            if(movement.RunDuration > movement.StaminaRegainTimer)
            {
                float targetIntensity = (movement.RunDuration / movement.RunRestartTimer) * vignetteMaxIntensity;
                targetIntensity = Mathf.Clamp(targetIntensity, 0f, vignetteMaxIntensity);
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, TransitionSpeed * Time.deltaTime);

                distortion.intensity.value = Mathf.Sin(Time.time * frequency) * movement.RunDuration - movement.StaminaRegainTimer * distortionMaxIntensity;

                aberration.intensity.value = Mathf.Sin((Time.time * 2 - 1) * frequency) * (movement.RunDuration - movement.StaminaRegainTimer / movement.RunRestartTimer - movement.StaminaTimer) * aberrationMaxIntensity;
            }
        }
    }
    
    void PlayerSlide()
    {
        if(info.IsSliding)
        {
            motionBlur.shutterAngle.value = Mathf.Lerp(motionBlur.shutterAngle.value, 360, TransitionSpeed * 10 * Time.deltaTime);
            distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, -30, TransitionSpeed * 10 * Time.deltaTime);
        }
        else
        {
            motionBlur.shutterAngle.value = Mathf.Lerp(motionBlur.shutterAngle.value, 120, TransitionSpeed * Time.deltaTime);
            distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, 0, TransitionSpeed * Time.deltaTime);
        }    
    }

    void PlayerHealth()
    {
        // in this function handles the camera effect
        if (CurrentHealth > stats.Health)       
        {
            vignette.color.value = RedVignette;
            vignette.intensity.value = 0.45f;
            stats.RegainHealth = true;
        }

        if (!stats.RegainHealth)
            CurrentHealth = stats.Health;
    }

    #endregion
}
