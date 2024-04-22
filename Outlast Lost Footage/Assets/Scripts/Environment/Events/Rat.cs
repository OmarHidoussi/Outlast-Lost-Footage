using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{

    #region Variables
    
    public Animator anim;
    public Transform forward;
    public float Speed;

    #endregion

    #region
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(forward.TransformDirection(transform.forward) * Speed * Time.deltaTime);
    }
    #endregion

    #region

    #endregion
}
