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
    private float dialogueTimer = 0f;
    public float dialogueInterval = 20f;

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
        DialogueSource.PlayOneShot(PatrolDialogueClips[0]);
    }

    private bool hasPlayedChasingDialogue = false;
    // Update is called once per frame
    void Update()
    {
        //TriggeringDialogueFunctions
        if (Behavior.isSearching)
        {
            Investigate();
        }
        else if (Behavior.IsChasing)
        {
            Chase();
        }
        else
            Patrol();
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
        Debug.Log("PatrolDialogue");

        // Update the dialogue timer
        dialogueTimer += Time.deltaTime;

        // Check if enough time has passed for a new dialogue
        if (dialogueTimer >= dialogueInterval)
        {
            // Reset the timer
            dialogueTimer = 0f;

            // Stop the audio source before playing a new dialogue
            DialogueSource.Stop();

            // Play the dialogue if not already playing
            if (!DialogueSource.isPlaying)
            {
                DialogueSource.PlayOneShot(GetRandomClip(PatrolDialogueClips));
            }
        }
    }

    void Chase()
    {
        // Check if the player is in the line of sight and the dialogue hasn't been played in this chase
        if (Behavior.IsChasing && !hasPlayedChasingDialogue)
        {
            // Stop the audio source before playing a new dialogue
            DialogueSource.Stop();

            // Play the dialogue
            DialogueSource.PlayOneShot(GetRandomClip(ChasingDialogueClips));

            // Set the flag to true to indicate that the dialogue has been played in this chase
            hasPlayedChasingDialogue = true;
        }
        else if (!Behavior.IsChasing)
        {
            // Reset the flag when not in the chasing state
            hasPlayedChasingDialogue = false;
        }
    }

    void Investigate()
    {
        hasPlayedChasingDialogue = false;

        dialogueTimer += Time.deltaTime;

        // Check if enough time has passed for a new dialogue
        if (dialogueTimer >= dialogueInterval)
        {
            // Reset the timer
            dialogueTimer = 0f;

            // Stop the audio source before playing a new dialogue
            DialogueSource.Stop();

            // Play the dialogue if not already playing
            if (!DialogueSource.isPlaying)
            {
                DialogueSource.PlayOneShot(GetRandomClip(InvestigatingClips));
            }
        }
    }

    #endregion

}
