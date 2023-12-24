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
            other.GetComponentInParent<InputManager>().IsSprinting = false;
            other.GetComponentInParent<InputManager>().IsCrouching = false;
            cam.RotateBody(cam.PlayerBody,true, direction.rotation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = true;
            other.GetComponentInParent<InputManager>().IsSprinting = false;
            other.GetComponentInParent<InputManager>().IsCrouching = false;
            cam.RotateBody(cam.PlayerBody,true, direction.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = false;
            cam.RotateBody(cam.PlayerBody, false, transform.rotation);
        }
    }
}

