using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;
    public AudioSource MetaSource;
    public CharacterAnimator Characteranim;
    public CharacterMovement movement;

    [Header("FootSteps")]
    public float WalkVolume;
    public float RunVolume;
    public AudioClip[] WalkFootStepsClips;
    public AudioClip[] RunFootStepsClips;

    [Header("Exhausted")]
    public float HeartBeatingMaxVolume = 0.1f;
    public float HeartBeatingMaxpitch = 0.05f;
    public CameraEffects effects;
    public AudioClip HeartBeating;

    [Header("AudioMixer")]
    public AudioMixerGroup Master;
    public AudioMixerGroup InteractionGrp;
    public AudioMixerGroup FootStepsGrp;

    [Header("Interactions")]
    public float BC_Volume;
    public AudioClip Battery_Collect;
    public AudioClip Door_Unlocked, Door_Locked;


    #endregion

    #region BuiltInMethods
    private void Update()
    {
        if (movement.isExhausted || movement.RunDuration > movement.StaminaRegainTimer)
        {
            Exhusted();
        }
        else 
        {
            MetaSource.Stop();
        }
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

    float targetvolume;
    float targetpitch;
    public void Exhusted()
    {
        if (!MetaSource.isPlaying)
        {
            MetaSource.clip = HeartBeating;
            MetaSource.Play();
        }

        if (movement.isExhausted)
        {
            targetvolume = Mathf.Lerp(targetvolume, (movement.StaminaRegainTimer / movement.StaminaTimer) * HeartBeatingMaxVolume, 2f * Time.deltaTime);
            targetpitch = Mathf.Lerp(targetpitch, (movement.StaminaRegainTimer / movement.StaminaTimer) + 1 + HeartBeatingMaxVolume, 2f * Time.deltaTime);
        }
        else
        {
            targetvolume = Mathf.Lerp(targetvolume, (movement.RunDuration / movement.RunRestartTimer) * HeartBeatingMaxVolume, 2f * Time.deltaTime);
            targetpitch = Mathf.Lerp(targetpitch, (movement.RunDuration / movement.RunRestartTimer) + HeartBeatingMaxVolume, 2f * Time.deltaTime);
        }

        MetaSource.pitch = targetpitch;
        MetaSource.volume = targetvolume;
    }

    public void Land()
    {

    }


    public void RestoreCamera()
    {

    }

    #endregion

}
