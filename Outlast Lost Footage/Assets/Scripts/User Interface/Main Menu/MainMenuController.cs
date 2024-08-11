using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    #region Variables
    public AudioSource source;
    public AudioSource Music_source;
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject General_GRP;
    public GameObject Graphics_GRP;
    public GameObject FadeOutUI, FadeOutEnv;
    public GameObject loadingScreen;

    public string Env_Scene_1, Env_Scene_2;
    #endregion

    #region BuiltInMethods

    private void Awake()
    {
        SceneManager.LoadScene(Env_Scene_1, LoadSceneMode.Additive);
        SceneManager.LoadScene(Env_Scene_2, LoadSceneMode.Additive);
    }

    #endregion

    #region CustomMethods
    public void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void NewGame(string Level)
    {
        loadingScreen.SetActive(true);
        Music_source.Pause();
        StartCoroutine(LoadingScreen(Level));
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

    IEnumerator LoadingScreen(string Level)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(Random.Range(8f, 15f));
        SceneManager.LoadScene(Level, LoadSceneMode.Single);
    }

    #endregion

}
