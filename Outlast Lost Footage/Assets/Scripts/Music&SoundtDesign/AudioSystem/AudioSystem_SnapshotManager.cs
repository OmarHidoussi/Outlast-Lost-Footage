using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem_SnapshotManager : MonoBehaviour
{

    #region Variables

    public AudioMixerSnapshot OnStay;
    public AudioMixerSnapshot OnExit;

    public float TransitionSpeed = 0.2f;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnStay.TransitionTo(TransitionSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnExit.TransitionTo(TransitionSpeed);
        }
    }

    #endregion

    #region CustomMethods

    #endregion

}
