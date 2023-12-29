using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAudio : MonoBehaviour
{

    #region Variables

    public AudioSource source;

    [Header("FootSteps")]
    public float WalkVolume;
    public float RunVolume;
    public AudioClip[] WalkFootStepsClips;
    public AudioClip[] RunFootStepsClips;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    #endregion

}
