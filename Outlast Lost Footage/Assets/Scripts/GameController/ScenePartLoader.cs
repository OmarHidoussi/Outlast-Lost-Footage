using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{
    public string SceneName;
    public bool isLoaded;
    public bool isLoading;
    public bool shouldLoad;

    private void Update()
    {
        TriggerCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }

    void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }

    void LoadScene()
    {
        if (!isLoaded && !isLoading)
        {
            if (!SceneManager.GetSceneByName(SceneName).isLoaded)
            {
                isLoading = true;
                SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive).completed += (op) =>
                {
                    isLoaded = true;
                    isLoading = false;
                };
            }
        }
    }

    void UnloadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneName);
            isLoaded = false;
        }
    }
}
