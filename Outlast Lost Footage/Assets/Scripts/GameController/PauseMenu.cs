using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PauseMenu : MonoBehaviour
{

    #region Variables

    public InputManager input;
    public GameObject pauseMenu;
    public GameObject Options;

    public GameObject PlayerCursor;

    public VolumeProfile Player_Volume;
    DepthOfField Depth;
    public float FocusDistance = 10;
    public float TransitionSpeed;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        input = FindObjectOfType<InputManager>();

        if (Player_Volume.TryGet(out Depth))
        {
            Depth.focusDistance.value = FocusDistance;
            Depth.active = false;
        }
        else
        {
            Debug.LogError("Failed to get DepthOfField settings from the Volume Profile.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GamePaused)
        {
            StartCoroutine(PauseMenuAppearanceTimer(0.3f, 0.1f, true));
            Time.timeScale = 0;
        }
        else
        {
            StartCoroutine(PauseMenuAppearanceTimer(0.3f, FocusDistance, false));
            Time.timeScale = 1;
        }

    }

    #endregion

    #region CustomMethods

    IEnumerator PauseMenuAppearanceTimer(float Timer, float NewFocusDistanceValue , bool MenuState)
    {
        if (MenuState)
        {
            //Lock Player Movement
            input.CanMove = false;
            input.CanCrouch = false;
            input.CanJump = false;
            input.EnableCameraMovement = false;

            Cursor.lockState = CursorLockMode.None;
            Depth.active = true;
            Depth.focusDistance.value = Mathf.Lerp(Depth.focusDistance.value, NewFocusDistanceValue, TransitionSpeed * Time.unscaledDeltaTime);
            PlayerCursor.SetActive(false);
            pauseMenu.SetActive(true);
            yield return new WaitForSeconds(Timer);

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(Timer);
            Depth.focusDistance.value = Mathf.Lerp(Depth.focusDistance.value, NewFocusDistanceValue, TransitionSpeed * Time.unscaledDeltaTime);
            PlayerCursor.SetActive(true);
            pauseMenu.SetActive(false);

            Depth.active = false;

            //Restore Player Movement
            input.CanMove = true;
            input.CanCrouch = true;
            input.CanJump = true;
            input.EnableCameraMovement = true;

        }
    }

    #endregion

}
