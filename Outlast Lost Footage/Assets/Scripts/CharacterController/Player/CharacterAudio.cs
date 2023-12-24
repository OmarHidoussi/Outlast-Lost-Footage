using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CharacterAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;
    public AudioSource MetaSource;
    public CharacterAnimator Characteranim;
    public CharacterMovement movement;
    public CameraMovement CamMovement;
    public Rigidbody m_rigidbody;

    [HideInInspector] public bool IsSliding = false;

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

    [Header("PlayerSFX")]
    public float SlideVolume;
    public AudioClip[] RunningSlide;
    public float JumpVolume;
    public AudioClip[] Jumping;

    [Header("PlayerDie")]
    public float ScoreVolume;
    public AudioClip DieScore_SFX;

    #endregion

    #region BuiltInMethods
    
    private void Start()
    {
        RestoreCameraMovement = CamMovement.Sensetivity;
    }
    
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

        if(IsSliding)
        {
            movement.transform.Translate(0, 0, movement.Speed * Time.deltaTime);
        }
    }
    #endregion

    #region CustomMethods
    public void LockHand()
    {

    }
    
    public void CollectBattery()
    {
        Characteranim.CharacterAnim.SetBool("PickUp", false);
        source.outputAudioMixerGroup = Master;
        source.pitch = 1f;
        source.volume = BC_Volume;
        source.PlayOneShot(Battery_Collect);
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

    private float RestoreCameraMovement;
    public void Jumped()
    {
        m_rigidbody.useGravity = false;
        CamMovement.Sensetivity = 0;
        source.volume = JumpVolume;
        source.pitch = 1;
        source.PlayOneShot(JumpClip());
    }

    public void JumpLand()
    {
        CamMovement.Sensetivity = RestoreCameraMovement;
        m_rigidbody.useGravity = true;
        movement.GetComponent<InputManager>().Jump = false;
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

    private AudioClip Slideclip()
    {
        return RunningSlide[UnityEngine.Random.Range(0, RunningSlide.Length)];
    }

    private AudioClip JumpClip()
    {
        return Jumping[UnityEngine.Random.Range(0, Jumping.Length)];
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
            targetpitch = Mathf.Lerp(targetpitch, (movement.StaminaRegainTimer / movement.StaminaTimer) + HeartBeatingMaxVolume, 2f * Time.deltaTime);
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
        movement.Speed = 0;
    }

    public void RestoreCamera()
    {

    }

    public void Slide() 
    {
        IsSliding = true;
        source.volume = SlideVolume;
        source.outputAudioMixerGroup = FootStepsGrp;
        source.PlayOneShot(Slideclip());
    }

    public void SlideEnded()
    {
        IsSliding = false;
        movement.GetComponent<InputManager>().IsSprinting = false;
    }

    public void Die()
    {
        Characteranim.CharacterAnim.SetBool("Dead", false);
        movement.GetComponent<InputManager>().CanMove = false;
        CamMovement.Sensetivity = 0;
        source.volume = ScoreVolume;
        source.PlayOneShot(DieScore_SFX);
        StartCoroutine(BackToMainMenu());
    }

    IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

    public void DieScore()
    {

    }

    public void Turn()
    {
        Characteranim.CharacterAnim.SetBool("TurnLeft",false);
        Characteranim.CharacterAnim.SetBool("TurnRight", false);
    }

    #endregion

}
