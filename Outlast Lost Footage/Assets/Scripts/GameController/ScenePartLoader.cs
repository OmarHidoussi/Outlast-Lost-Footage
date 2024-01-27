using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{

    #region Variables
    //Scene State
    public bool isLoaded;
    public bool shouldLoad;

    public string SceneName;
    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TriggerCheck();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
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
    #endregion

    #region CustomMethods
    void LoadScene()
    {
        if(!isLoaded)
        {
            SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    void UnloadScene()
    {
        if(isLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneName);
            isLoaded = false;
        }
    }

    void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }
        else
            UnloadScene();
    }

    #endregion

}
