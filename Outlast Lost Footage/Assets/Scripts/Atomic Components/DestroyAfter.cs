using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(gameObject,Timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
