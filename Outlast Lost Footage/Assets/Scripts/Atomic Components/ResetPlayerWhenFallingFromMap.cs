using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerWhenFallingFromMap : MonoBehaviour
{
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
        if(other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().transform.position = GameSettings.Instance.RestartLocation.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().transform.position = GameSettings.Instance.RestartLocation.position;
        }
    }
}
