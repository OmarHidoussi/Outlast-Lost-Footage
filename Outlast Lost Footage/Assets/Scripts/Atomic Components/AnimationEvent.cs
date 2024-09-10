using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{

    public Animator anim;
    public string animationName;
    public AudioSource source;
    public AudioClip clip;

    private bool HasPlayed;

    // Start is called before the first frame update
    void Start()
    {
        HasPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!HasPlayed)
            {
                if(anim != null)
                    anim.SetBool(animationName, true);

                if (source != null)
                {
                    source.clip = clip;
                    source.PlayOneShot(clip);
                    HasPlayed = true;
                }
            }
        }
    }
}
