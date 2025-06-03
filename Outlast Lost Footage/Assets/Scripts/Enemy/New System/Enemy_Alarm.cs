using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Alarm : MonoBehaviour
{

    #region Variables

    [Header("Alarm Settings")]
    public AudioClip[] alarmClips;          // Array of alarm sounds
    public AudioSource audioSource;        // Audio source to play them

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region CustomMethods

    public void TriggerAlarm()
    {
        if (alarmClips.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Missing alarm clips or AudioSource.");
            return;
        }

        if (!audioSource.isPlaying)
        {
            int randomIndex = Random.Range(0, alarmClips.Length);
            AudioClip selectedClip = alarmClips[randomIndex];
            audioSource.clip = selectedClip;
            audioSource.Play();
        }
    }

    #endregion
}
