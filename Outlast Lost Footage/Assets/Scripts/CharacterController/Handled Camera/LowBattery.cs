using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBattery : MonoBehaviour
{

    #region Variable
    [Header("LowBattery")]
    public float SFXvolume;
    public AudioSource source;
    public AudioClip LowBatteryClip;

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

    public void PlayLowBatterySFX()
    {
        source.volume = SFXvolume;
        source.PlayOneShot(LowBatteryClip);
    }

    #endregion

}
