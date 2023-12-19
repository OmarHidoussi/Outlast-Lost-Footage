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
    public float MaxIntensity;

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
        }
        else
        {
            float targetIntensity = (movement.RunDuration / movement.RunRestartTimer) * MaxIntensity;

            targetIntensity = Mathf.Clamp(targetIntensity, 0f, MaxIntensity);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, TransitionSpeed * Time.deltaTime);
        }
    }
    
    #endregion
}
