using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CharacterAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;
    public AudioSource DialogueSource;
    public AudioSource MetaSource;
    public CharacterAnimator Characteranim;
    public CharacterMovement movement;
    public CameraMovement CamMovement;
    public Rigidbody m_rigidbody;
    public Transform CharacterHead;

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
    public AudioMixerGroup Dialogue;
    public AudioMixerGroup DeathSound;
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
    public bool CanBreath;
    public float BreathingVolume;
    public AudioClip NormalBreath;

    [Header("PlayerDie")]
    public bool Died;
    public float TransitionDelay;
    public AudioMixerSnapshot DeathSoundOn;
    public float ScoreVolume;
    public AudioClip DieScore_SFX;
    public float PlayerDieVolume;
    public AudioClip[] PlayerDieClip;


    #endregion

    #region BuiltInMethods

    private void Start()
    {
        RestoreCameraMovement = CamMovement.Sensetivity;
        Died = false;
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

        if(CanBreath)
        {
            Breath();
        }
        else
            DialogueSource.volume = Mathf.Lerp(DialogueSource.volume, 0, 2 * Time.deltaTime);

        if (Died)
        {
            DeathSoundOn.TransitionTo(TransitionDelay);
        }

        if (IsSnapping)
            SnapPlayerToPosition();

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
        source.PlayOneShot(GetRandomClip(WalkFootStepsClips));

    }

    public void RunStep()
    {
        if (m_rigidbody.useGravity)
        {
            source.outputAudioMixerGroup = FootStepsGrp;
            source.pitch = GetRandomPitch();
            source.volume = RunVolume;
            source.PlayOneShot(GetRandomClip(RunFootStepsClips));
        }
    }

    private float RestoreCameraMovement;
    public void Jumped()
    {
        m_rigidbody.useGravity = false;
        CamMovement.Sensetivity = 100;
        source.volume = JumpVolume;
        source.outputAudioMixerGroup = Dialogue;
        source.pitch = 1;
        source.PlayOneShot(GetRandomClip(Jumping));
    }

    public void JumpLand()
    {
        CamMovement.Sensetivity = RestoreCameraMovement;
        m_rigidbody.useGravity = true;
        movement.GetComponent<InputManager>().Jump = false;
    }

    [HideInInspector] public bool IsSnapping = false;
    [HideInInspector] public Transform Location;
    public void StartSnapping()
    {
        movement.GetComponent<InputManager>().CanMove = false;
        IsSnapping = true;
        m_rigidbody.useGravity = false;
        CamMovement.Sensetivity = 0;
    }

    public void EndSnapping()
    {
        movement.GetComponent<InputManager>().CanMove = true;
        movement.GetComponent<InputManager>().IsCrouching = true;
        Characteranim.CharacterAnim.SetBool("WallClimb", false);
        IsSnapping = false;
        m_rigidbody.transform.position = Location.position;
        m_rigidbody.useGravity = true;
        CamMovement.Sensetivity = RestoreCameraMovement;
    }

    public void SnapPlayerToPosition()
    {
        movement.GetComponent<InputManager>().Mov_Axis = Vector2.zero;
        m_rigidbody.transform.position = Vector3.MoveTowards(m_rigidbody.transform.position, Location.position, 3f * Time.deltaTime);
        m_rigidbody.transform.rotation = Quaternion.Slerp(m_rigidbody.transform.rotation, Location.rotation, 15 * Time.deltaTime);
        if(Vector3.Distance(m_rigidbody.transform.position, Location.position) < 0.075f)
        {
            movement.GetComponent<InputManager>().CanMove = true;
            movement.GetComponent<InputManager>().IsCrouching = true;
            Characteranim.CharacterAnim.SetBool("WallClimb", false);
            m_rigidbody.useGravity = true;
            CamMovement.Sensetivity = RestoreCameraMovement;
            m_rigidbody.transform.position = Location.position;
            IsSnapping = false;
        }
    }

    private float GetRandomPitch() 
    {
        return UnityEngine.Random.Range(0.8f, 1.2f);
    }

    private AudioClip GetRandomClip(AudioClip[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
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
        source.PlayOneShot(GetRandomClip(RunningSlide));
    }

    public void SlideEnded()
    {
        IsSliding = false;
        movement.GetComponent<InputManager>().IsSprinting = false;
    }

    public void Die()
    {
        Died = true;
        Characteranim.CharacterAnim.SetBool("Dead", false);
        movement.GetComponent<InputManager>().CanMove = false;
        CamMovement.Sensetivity = 0;
        DialogueSource.volume = PlayerDieVolume;
        DialogueSource.outputAudioMixerGroup = DeathSound;
        DialogueSource.PlayOneShot(GetRandomClip(PlayerDieClip));
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

    void Breath()
    {
        if(!DialogueSource.isPlaying)
        {
            DialogueSource.outputAudioMixerGroup = Dialogue;
            DialogueSource.PlayOneShot(NormalBreath);
        }
        DialogueSource.volume = Mathf.Lerp(DialogueSource.volume, BreathingVolume, 2 * Time.deltaTime);

    }

    #endregion

}
