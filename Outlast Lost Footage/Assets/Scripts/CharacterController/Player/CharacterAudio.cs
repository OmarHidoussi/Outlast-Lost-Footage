using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;

    [Header("FootSteps")]
    public float WalkVolume;
    public float RunVolume;
    public AudioClip[] WalkFootStepsClips;
    public AudioClip[] RunFootStepsClips;


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
        source.pitch = 1f;
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
        source.volume = WalkVolume;
        source.PlayOneShot(Walkclip());
    }

    public void RunStep()
    {
        source.pitch = GetRandomPitch();
        source.volume = RunVolume;
        source.PlayOneShot(Runclip());
    }

    private float GetRandomPitch() 
    {
        return UnityEngine.Random.Range(0.8f, 1.2f);
    }

    private AudioClip Walkclip()
    {
        return WalkFootStepsClips[UnityEngine.Random.Range(0, WalkFootStepsClips.Length)];
    }

    private AudioClip Runclip()
    {
        return RunFootStepsClips[UnityEngine.Random.Range(0, RunFootStepsClips.Length)];
    }

    #endregion

}
