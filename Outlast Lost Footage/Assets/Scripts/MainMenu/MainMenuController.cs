using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    #region Variables
    public AudioSource source;
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject General_GRP;
    public GameObject Graphics_GRP;
    public GameObject FadeOutUI, FadeOutEnv;
    #endregion

    #region BuiltInMethods

    #endregion

    #region CustomMethods
    public void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void NewGame(string Level)
    {
        SceneManager.LoadScene(Level, LoadSceneMode.Single);
    }

    public void options()
    {
        Options.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void Back()
    {
        Options.SetActive(false);
        MainMenu.SetActive(true);

        FadeOutUI.SetActive(false);
        FadeOutEnv.SetActive(false);
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

    public void Exit()
    {
        Application.Quit();
    }
    #endregion

}
