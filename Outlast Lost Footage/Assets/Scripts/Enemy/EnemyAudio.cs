using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;
    public AudioSource DialogueSource;

    [Header("FootSteps")]
    public float WalkVolume;
    public float RunVolume;
    public AudioClip[] WalkFootStepsClips;
    public AudioClip[] RunFootStepsClips;


    [Space]
    [Header("Dialogues")]
    EnemyAI Behavior;
    EnemySight Sight;
    public float DialogueVolume;
    public AudioMixerGroup MixerDialogueGroup;

    [Header("Patrolling Audio")]
    public AudioClip[] PatrolDialogueClips;

    [Header("Chasing Audio")]
    public AudioClip[] ChasingDialogueClips;

    [Header("Investigating Audio")]
    public AudioClip[] InvestigatingClips;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Behavior = GetComponent<EnemyAI>();
        Sight = GetComponent<EnemySight>();

        DialogueSource.outputAudioMixerGroup = MixerDialogueGroup;
        DialogueSource.volume = DialogueVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //TriggeringDialogueFunctions
       /* if (Behavior.isSearching)
            Investigate();
        else if (Behavior.IsChasing)
            Chase();
        else
            Patrol();*/
    }

    #endregion

    #region Custom Methods
    
    public void Step()
    {
        source.volume = WalkVolume;
        source.pitch = GetRandomPitch();
        source.PlayOneShot(GetRandomClip(WalkFootStepsClips));
    }

    public void RunStep()
    {
        source.pitch = GetRandomPitch();
        source.volume = RunVolume;
        source.PlayOneShot(GetRandomClip(RunFootStepsClips));
    }

    private AudioClip GetRandomClip(AudioClip[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    private float GetRandomPitch()
    {
        return UnityEngine.Random.Range(0.8f, 1.2f);
    }

    void Patrol()
    {
        //While the enemy is patrolling he say dialogue every 25 seconds
        if (DialogueSource.isPlaying)
        {
            source.PlayOneShot(GetRandomClip(PatrolDialogueClips));
        }
    }

    void Chase()
    {
        //When the enemy sees the player he sometimes starts the chase with a dialogue line
        if (DialogueSource.isPlaying)
        {
            source.PlayOneShot(GetRandomClip(ChasingDialogueClips));
        }
    }

    void Investigate()
    {
        //When the enemy is investigating the player he says a line when the player is too close (5 meters distance)
        //When IsSearching is enabled
        if (DialogueSource.isPlaying)
        {
            source.PlayOneShot(GetRandomClip(InvestigatingClips));
        }
    }

    #endregion

}
