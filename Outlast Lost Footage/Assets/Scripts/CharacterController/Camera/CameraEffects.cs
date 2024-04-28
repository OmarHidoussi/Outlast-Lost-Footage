using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CameraEffects : MonoBehaviour
{
    #region Variables

    public CharacterMovement movement;
    public CharacterBehaviour info;
    public CharacterStats stats;
    public VolumeProfile Player_Volume;
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
        info = GetComponentInChildren<CharacterBehaviour>();
        stats = GetComponentInParent<CharacterStats>();

        CurrentHealth = stats.Health;
        if (Player_Volume.TryGet(out vignette))
        {
            vignette.intensity.value = Mathf.Clamp((movement.RunDuration / movement.RunRestartTimer) * 0.3f, 0f, 0.3f);
        }
        else
        {
            Debug.LogError("Failed to get Vignette settings from the Volume Profile.");
        }

        if (Player_Volume.TryGet(out distortion))
        {
            distortion.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Failed to get Lens Distortion settings from the Volume Profile.");
        }

        if (Player_Volume.TryGet(out aberration))
        {
            aberration.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Failed to get Chromatic Aberration settings from the Volume Profile.");
        }

        if (Player_Volume.TryGet(out motionBlur))
        {
            motionBlur.intensity.value = 0.25f;
        }
        else
        {
            Debug.LogError("Failed to get Motion Blur settings from the Volume Profile.");
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
        if (movement.isExhausted)
        {
            vignette.intensity.value = (movement.StaminaRegainTimer / movement.StaminaTimer) * vignetteMaxIntensity;
            distortion.intensity.value = (Mathf.Sin(Time.time * movement.StaminaRegainTimer)) / 100f;

            distortion.intensity.value = (Mathf.Sin(Time.time * frequency) * movement.StaminaRegainTimer) / 100f;

            aberration.intensity.value = (Mathf.Sin((Time.time * 2 - 1) * frequency) * (movement.StaminaRegainTimer / movement.StaminaTimer)) / 100f;
        }
        else
        {
            if (movement.RunDuration > movement.StaminaRegainTimer)
            {
                float targetIntensity = (movement.RunDuration / movement.RunRestartTimer) * vignetteMaxIntensity;
                targetIntensity = Mathf.Clamp(targetIntensity, 0f, vignetteMaxIntensity);
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, TransitionSpeed * Time.deltaTime);

                distortion.intensity.value = Mathf.Sin(Time.time * frequency) * (movement.RunDuration - movement.StaminaRegainTimer * distortionMaxIntensity) / 100f;

                aberration.intensity.value = Mathf.Sin((Time.time * 2 - 1) * frequency) * ((movement.RunDuration - movement.StaminaRegainTimer / movement.RunRestartTimer - movement.StaminaTimer) * aberrationMaxIntensity) / 100f;
            }
        }
    }

    void PlayerSlide()
    {
        if (info.IsSliding)
        {
            motionBlur.intensity.value = Mathf.Lerp(motionBlur.intensity.value, 0.5f, TransitionSpeed * 10 * Time.deltaTime);
            distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, -0.20f, TransitionSpeed * 10 * Time.deltaTime);
        }
        else
        {
            motionBlur.intensity.value = Mathf.Lerp(motionBlur.intensity.value, 0.25f, TransitionSpeed * Time.deltaTime);
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
