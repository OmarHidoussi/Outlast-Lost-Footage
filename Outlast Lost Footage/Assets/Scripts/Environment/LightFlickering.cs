using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{

    #region Variables
    AudioSource source;
    public AudioClip[] Shortclip;
    public AudioClip[] Longclip;
    #endregion

    #region BuiltInMethods
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void LongFlicker()
    {
        int RNG = Random.Range(0, Longclip.Length);
        source.clip = Longclip[RNG];
        source.PlayOneShot(Longclip[RNG]);
    }

    public void ShortFlicker()
    {
        int RNG = Random.Range(0, Shortclip.Length);
        source.clip = Shortclip[RNG];
        source.PlayOneShot(Shortclip[RNG]);
    }
    #endregion

    #region CustomMethods

    #endregion

}
