using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PauseMenu : MonoBehaviour
{

    #region Variables

    public InputManager input;
    public HeadBob headBob;
    public GameObject pauseMenu;
    public GameObject Options;

    public GameObject General_GRP;
    public GameObject Graphics_GRP;

    public GameObject PlayerCursor;

    public VolumeProfile Player_Volume;
    DepthOfField Depth;
    public float FocusDistance = 10;
    public float TransitionSpeed;

    [Header("Audio")]
    public AudioSource source;
    public float TransitionDelay = 0.1f;
    public AudioMixerSnapshot PauseMenu_Activated;
    public AudioMixerSnapshot PauseMenu_Deactivated;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        input = FindObjectOfType<InputManager>();
        headBob = input.GetComponent<HeadBob>();

        if (Player_Volume.TryGet(out Depth))
        {
            Depth.focusDistance.value = FocusDistance;
            Depth.active = false;
        }
        else
        {
            Debug.LogError("Failed to get DepthOfField settings from the Volume Profile.");
        }

        PauseMenu_Deactivated.TransitionTo(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GamePaused)
        {
            if(!source.isPlaying)
                source.Play();

            if (!input.InOptionsMenu)
            {
                StartCoroutine(PauseMenuAppearanceTimer(0.3f, 0.1f, true));
                Time.timeScale = 0;
            }
        }
        else
        {
            source.Stop();
            StartCoroutine(PauseMenuAppearanceTimer(0.3f, FocusDistance, false));
            Time.timeScale = 1;
        }

    }

    #endregion

    #region CustomMethods

    IEnumerator PauseMenuAppearanceTimer(float timer, float newFocusDistanceValue, bool menuState)
    {
        if (menuState)
        {
            LockPlayer(true);
            Cursor.lockState = CursorLockMode.None;
            Depth.active = true;
            PlayerCursor.SetActive(false);
            pauseMenu.SetActive(true);
            headBob.enabled = false;
            input.GetComponent<CharacterMovement>().enabled = false;
            PauseMenu_Activated.TransitionTo(0.01f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            input.GetComponent<CharacterMovement>().enabled = true;
            headBob.enabled = true;
            PauseMenu_Deactivated.TransitionTo(0.01f);
        }
        // Smooth transition for Depth focus distance
        float elapsedTime = 0f;
        float startFocus = Depth.focusDistance.value;

        while (elapsedTime < timer)
        {
            Depth.focusDistance.value = Mathf.Lerp(startFocus, newFocusDistanceValue, elapsedTime / timer);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Depth.focusDistance.value = newFocusDistanceValue; // Ensure it reaches exact target

        if (!menuState)
        {
            pauseMenu.SetActive(false);
            PlayerCursor.SetActive(true);
            Depth.active = false;
            LockPlayer(false);
        }
    }

    private void LockPlayer(bool isLocked)
    {
        input.CanMove = !isLocked;
        input.CanCrouch = !isLocked;
        input.CanJump = !isLocked;
        input.EnableCameraMovement = !isLocked;
    }

    public void Resume_BTN()
    {
        input.GamePaused = false;
        //StartCoroutine(PauseMenuAppearanceTimer(0.3f, FocusDistance, false));
    }

    public void Options_BTN()
    {
        input.InOptionsMenu = true;
        Options.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void General()
    {
        General_GRP.SetActive(true);
        Graphics_GRP.SetActive(false);
    }

    public void Graphics()
    {
        General_GRP.SetActive(false);
        Graphics_GRP.SetActive(true);
    }

    public void Back_BTN()
    {
        input.InOptionsMenu = false;
        pauseMenu.SetActive(true);
        Options.SetActive(false);
    }

    public void Exit_BTN()
    {
        Application.Quit();
    }

    #endregion

}
