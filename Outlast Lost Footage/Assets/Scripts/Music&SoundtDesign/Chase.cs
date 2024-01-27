using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Chase : MonoBehaviour
{

    public AudioSource source;
    public AudioSource PlayerSource;

    public EnemySight Behavior;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Behavior.PlayerInSight || Behavior.LastSightPosition != Behavior.resetPosition)
        {
            source.volume = Mathf.Lerp(source.volume, 0.2f, 2f * Time.deltaTime);

            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            source.volume = Mathf.Lerp(source.volume, 0, 2 * Time.deltaTime);
            source.Stop();
        }
    }
}
