using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalk : MonoBehaviour
{

    private CameraMovement cam;
    public Transform direction;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = true;
            other.GetComponentInParent<InputManager>().IsSprinting = true;
            cam.RotatePlayerBody(true, direction.rotation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = true;
            cam.RotatePlayerBody(true, direction.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = false;
            cam.RotatePlayerBody(false, transform.rotation);
        }
    }
}

