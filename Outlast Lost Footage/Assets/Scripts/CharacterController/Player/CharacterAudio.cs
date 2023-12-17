using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{

    #region Variables
    public AudioSource source;

    [Header("FootSteps")]
    public AudioClip[] FootStepsClips;

    [Header("Battery")]
    public AudioClip Battery_Collect;
    public float BC_Volume;


    #endregion

    #region BuiltInMethods

    #endregion

    #region CustomMethods
    public void CollectBattery()
    {
        source.clip = Battery_Collect;
        source.volume = BC_Volume;
        source.PlayOneShot(Battery_Collect);
    }

    public void Door_Open()
    {

    }

    public void Door_Locked()
    {

    }

    public void Step()
    {
        source.pitch = GetRandomPitch();
        source.PlayOneShot(clip());
    }

    private float GetRandomPitch() 
    {
        return UnityEngine.Random.Range(0.8f, 1.2f);
    }

    private AudioClip clip()
    {
        return FootStepsClips[UnityEngine.Random.Range(0, FootStepsClips.Length)];
    }

    #endregion

}
