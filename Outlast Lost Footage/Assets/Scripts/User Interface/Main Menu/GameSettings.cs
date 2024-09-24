using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{

    #region Variables

    [Header("General")]
    public bool ShowSubtitles;
    public bool ShowTutorial;
    public bool ShowCrosshair;
    public bool ReduceHeadMotion;
    public bool InvertUpAxis;
    public float LookSensitivity;
    public float volume;

    [Header("Graphics")]
    public bool Fullscreen;

    #endregion

    #region BuiltInMethods

    void Start()
    {
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region CustomMethods

    #endregion

}
