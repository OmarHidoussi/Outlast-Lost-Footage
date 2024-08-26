using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalk : MonoBehaviour
{

    private CameraMovement cam;
    public Transform direction;
    private bool RegisterCamera;
    public bool TightArea;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraMovement>();
        RegisterCamera = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            RegisterCamera = false;

            other.GetComponentInParent<InputManager>().SideWalk = true;
            other.GetComponentInParent<InputManager>().IsSprinting = false;
            other.GetComponentInParent<InputManager>().IsCrouching = false;
            if(TightArea)
            {
                if (other.GetComponentInParent<InputManager>().CameraOn)
                {
                    RegisterCamera = true;
                    other.GetComponentInParent<InputManager>().CameraOn = false;
                }
            }
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
            if(TightArea)
            {
                other.GetComponentInParent<InputManager>().CameraOn = false;
            }
            cam.RotateBody(cam.PlayerBody,true, direction.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<InputManager>().SideWalk = false;
            cam.RotateBody(cam.PlayerBody, false, transform.rotation);
            if(RegisterCamera)
                other.GetComponentInParent<InputManager>().CameraOn = true;
        }
    }
}

