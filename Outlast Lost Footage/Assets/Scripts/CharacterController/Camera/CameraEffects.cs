using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    #region Variables

    public CharacterMovement movement;
    public PostProcessVolume Player_Volume;
    public float TransitionSpeed;

    [Header("Effects")]
    Vignette vignette;
    LensDistortion distortion;
    ChromaticAberration aberration;
    public float MaxIntensity;
    public float frequency;

    #endregion

    #region BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
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
            distortion.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Failed to get ChromaticAberration settings from the PostProcessVolume.");
        }

        vignette.intensity.value = 0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerStamina();
    }
    #endregion

    #region Custom Methods

    void PlayerStamina()
    {   
        if(movement.isExhausted)
        {
            vignette.intensity.value = (movement.StaminaRegainTimer / movement.StaminaTimer) * MaxIntensity;
            distortion.intensity.value = Mathf.Sin(Time.time * movement.StaminaRegainTimer);

            if(distortion.intensity.value >= 0.1f)
            {
                distortion.intensity.value = Mathf.Sin(Time.time * frequency) * movement.StaminaRegainTimer;

            }

            if (aberration.intensity.value >= 0.05f)
            {
                aberration.intensity.value = Mathf.Sin((Time.time * 2 - 1) * frequency) * (movement.StaminaRegainTimer / movement.StaminaTimer) * MaxIntensity;
            }
        }
        else
        {
            if(movement.RunRestartTimer / movement.RunDuration > 0.4f)
            {
                float targetIntensity = (movement.RunDuration / movement.RunRestartTimer) * MaxIntensity;
                targetIntensity = Mathf.Clamp(targetIntensity, 0f, MaxIntensity);
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, TransitionSpeed * Time.deltaTime);

                distortion.intensity.value = Mathf.Sin(Time.time * frequency) * movement.RunDuration;

                aberration.intensity.value = Mathf.Sin((Time.time * 2 - 1) * frequency) * (movement.RunDuration / movement.RunRestartTimer) * MaxIntensity;
            }
        }
    }
    
    #endregion
}
