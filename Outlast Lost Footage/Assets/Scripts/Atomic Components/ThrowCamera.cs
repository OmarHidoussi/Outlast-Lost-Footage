using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCamera : MonoBehaviour
{
    public Animator CameraThrow_Anim;
    public AudioSource CinematicSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowCameraAnimation()
    {
        CameraThrow_Anim.SetBool("Throw", true);
        if (!CinematicSFX.isPlaying)
            CinematicSFX.Play();
    }

}
