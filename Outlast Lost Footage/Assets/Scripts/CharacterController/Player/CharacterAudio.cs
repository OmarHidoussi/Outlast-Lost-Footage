using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;
    public CharacterAnimator Characteranim;

    [Header("FootSteps")]
    public float WalkVolume;
    public float RunVolume;
    public AudioClip[] WalkFootStepsClips;
    public AudioClip[] RunFootStepsClips;

    [Header("AudioMixer")]
    public AudioMixerGroup Master;
    public AudioMixerGroup InteractionGrp;
    public AudioMixerGroup FootStepsGrp;

    [Header("Interactions")]
    public AudioClip Battery_Collect;
    public AudioClip Door_Unlocked, Door_Locked;
    public float BC_Volume;


    #endregion

    #region BuiltInMethods
    private void Update()
    {

    }
    #endregion

    #region CustomMethods
    public void CollectBattery()
    {
        source.outputAudioMixerGroup = Master;
        source.pitch = 1f;
        source.volume = BC_Volume;
        source.PlayOneShot(Battery_Collect);
        Characteranim.CharacterAnim.SetBool("PickUp", false);
    }

    public void DoorState(bool State)
    {
        if (State)
        {
            DoorOpen();
            State = !State;
        }
        else
        {
            DoorLocked();
            State = !State;
        }
    }

    public void DoorOpen()
    {
        source.outputAudioMixerGroup = InteractionGrp;
        source.PlayOneShot(Door_Unlocked);
    }

    public void DoorLocked()
    {
        source.outputAudioMixerGroup = InteractionGrp;
        source.PlayOneShot(Door_Locked);
        Characteranim.CharacterAnim.SetBool("OpenDoor", false);
    }

    public void Step()
    {
        source.outputAudioMixerGroup = FootStepsGrp;
        source.pitch = GetRandomPitch();
        source.volume = WalkVolume;
        source.PlayOneShot(Walkclip());
    }

    public void RunStep()
    {
        source.outputAudioMixerGroup = FootStepsGrp;
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
