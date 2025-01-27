using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AdaptiveMusic : MonoBehaviour
{

    #region Variables
    public EnemyAI agent;
    public EnemySight sight;
    public LevelController controller;

    [Space]
    public bool ChaseStart;
    public bool IsChasing;
    public bool Investigating;
    public bool InvestigationEnd;

    [Space]
    public AudioSource AS_Hit;
    public AudioSource AS_ChaseTrack;
    public AudioSource AS_InvestigationTrack;
    public AudioSource AS_Closer;

    [Space]
    public AudioClip Hit_Clip;
    public AudioClip Chase_Clip;
    public AudioClip Investigation_Clip;
    public AudioClip Closer_Clip;

    [Space]
    public AudioMixerSnapshot Snapshot_ChaseStart;
    public AudioMixerSnapshot Snapshot_IsChasing;
    public AudioMixerSnapshot Snapshot_Investigating;
    public AudioMixerSnapshot Snapshot_Closer;

    public AnimationCurve ChaseCurve;

    [Space]
    public float ChseStart_TransitionSpeed, IsChasing_TransitionSpeed, Investigation_TransitionSpeed, Closer_TransitionSpeed;

    private bool PreviousSight;
    private bool currentSight;

    private bool PreviousAI_InvestigationState;
    private bool CurrentAI_InvestigationState;
    #endregion

    #region BuildInMethods
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<LevelController>();

        ChaseStart = false;
        IsChasing = false;
        Investigating = false;
        InvestigationEnd = false;

        currentSight = controller.G_PlayerInSight;
        PreviousSight = currentSight;

        CurrentAI_InvestigationState = agent.IsInvestigating;
        PreviousAI_InvestigationState = CurrentAI_InvestigationState;
    }

    // Update is called once per frame
    void Update()
    {
        currentSight = controller.G_PlayerInSight;
        if (currentSight != PreviousSight && PreviousSight == false)
            ChaseStart = true;
        else
            ChaseStart = false;

        PreviousSight = currentSight;

        CurrentAI_InvestigationState = agent.IsInvestigating;
        if (CurrentAI_InvestigationState != PreviousAI_InvestigationState && PreviousAI_InvestigationState == true)
            InvestigationEnd = true;
        else
            InvestigationEnd = false;

        PreviousAI_InvestigationState = CurrentAI_InvestigationState;

        IsChasing = agent.IsChasing;
        Investigating = agent.IsInvestigating;


        GameState();
        Adaptive();
    }
    #endregion

    #region CustomMethods

    void GameState()
    {
        if (ChaseStart)
            Debug.Log("Chase Started");
        if (IsChasing)
            Debug.Log("IsChasing");
        if (Investigating)
            Debug.Log("Investigating");
        if (InvestigationEnd)
            Debug.Log("Investigation Ended");
    }

    void Adaptive()
    {
        if (ChaseStart && !AS_Hit.isPlaying)
        {
            Snapshot_ChaseStart.TransitionTo(ChseStart_TransitionSpeed);
            AS_Hit.PlayOneShot(Hit_Clip);
            AS_InvestigationTrack.Stop();
            AS_ChaseTrack.Stop();
        }

        if (IsChasing && !AS_ChaseTrack.isPlaying)
        {
            Snapshot_IsChasing.TransitionTo(ChaseCurve.length * IsChasing_TransitionSpeed);
            AS_ChaseTrack.clip = Chase_Clip;
            AS_ChaseTrack.Play();
        }

        if (!IsChasing && Investigating && !AS_InvestigationTrack.isPlaying)
        {
            Snapshot_Investigating.TransitionTo(Investigation_TransitionSpeed);
            AS_InvestigationTrack.clip = Investigation_Clip;
            AS_InvestigationTrack.Play();
        }

        if (InvestigationEnd && !AS_Closer.isPlaying)
        {
            Snapshot_Closer.TransitionTo(Closer_TransitionSpeed);
            AS_Closer.PlayOneShot(Closer_Clip); 
            AS_InvestigationTrack.Stop();
            AS_ChaseTrack.Stop();
        }
    }

    #endregion

}
