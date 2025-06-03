using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

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

    [Header("GameSystem")]
    public bool RestartFromPreviousCheckPoint = false;
    public Transform RestartLocation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(RestartLocation.gameObject);
    }
}
