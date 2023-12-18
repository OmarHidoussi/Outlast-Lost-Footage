using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    #region Variables

    public float Sensetivity;
    public float TransitionSpeed;
    public Transform PlayerBody;
    public Transform LookBackDirection;
    public Transform RotationCenter;
    public float RotationSpeed;
    public bool Lookback;
    public float LookbackTransitionSpeed;
    public float X_Max, X_Min;

    public InputManager input;
    public CapsuleCollider col;
    public Vector3 offset;
    public Vector3 Crouchingoffset;

    float xRotation;
    int Factor;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GameSettings GameData = FindObjectOfType<GameSettings>();
        if (GameData != null)
        {
            Sensetivity = GameData.LookSensitivity;

            if (GameData.InvertUpAxis)
                Factor = -1;
            else
                Factor = 1;
        }
        else
        {
            Factor = 1;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Lookback)
        {
            float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * Sensetivity;
            float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensetivity * Factor;


            xRotation -= MouseY;
            xRotation = Mathf.Clamp(xRotation, X_Min, X_Max);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            PlayerBody.Rotate(Vector3.up * MouseX);
        }

        HandleHeight();
        HandleLookback();
    }

    #endregion

    #region CustomMethods
    void HandleHeight()
    {
        transform.localPosition = new Vector3(0, col.center.y, 0) + offset;

        if (input.IsCrouching)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + Crouchingoffset, 50f);
        }
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, col.center.y, 0) + offset, 50f);
    }

    /*void HandleLookback()
    {
        Lookback = Input.GetKey(KeyCode.LeftAlt);

        if (Lookback)
        {
            Quaternion lookBackRotation = Quaternion.LookRotation(LookBackDirection.forward, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookBackRotation, Time.deltaTime * LookbackTransitionSpeed);
        }
        else
        {
            // If not looking back, smoothly interpolate back to the original rotation
            Quaternion originalRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * LookbackTransitionSpeed);
        }
    }*/
    bool isTransitioning = false;

    void HandleLookback()
    {
        Lookback = Input.GetKey(KeyCode.LeftAlt);

        if (Lookback && !isTransitioning)
        {
            StartCoroutine(LookBackSmoothly());
        }
        else if (!Lookback && isTransitioning)
        {
            StartCoroutine(ReturnToForwardSmoothly());
        }
    }

    IEnumerator LookBackSmoothly()
    {
        isTransitioning = true;

        // Get the position of the point you want to rotate around
        Vector3 rotateAroundPoint = RotationCenter.transform.position;

        // Calculate the target rotation for looking back
        Quaternion lookBackRotation = Quaternion.LookRotation(rotateAroundPoint - transform.position, Vector3.up);

        // Smoothly interpolate between the current rotation and the target rotation
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, lookBackRotation, elapsedTime);

            // Orbit the camera around the RotationCenter
            transform.RotateAround(rotateAroundPoint, Vector3.up, Time.deltaTime * RotationSpeed);

            elapsedTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        // Ensure the final rotation is set to the exact target
        transform.rotation = lookBackRotation;

        isTransitioning = false;
    }


    IEnumerator ReturnToForwardSmoothly()
    {
        isTransitioning = true;

        // Get the position of the point you want to rotate around
        Vector3 rotateAroundPoint = RotationCenter.transform.position;

        // Calculate the target rotation for returning to the forward position
        Quaternion originalRotation = Quaternion.LookRotation(rotateAroundPoint - transform.position, transform.up);

        // Smoothly interpolate back to the original rotation
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime);
            transform.RotateAround(rotateAroundPoint, Vector3.up, Time.deltaTime * RotationSpeed); // Rotate the camera around the point
            elapsedTime += Time.deltaTime * TransitionSpeed;
            yield return null;
        }

        transform.rotation = originalRotation; // Ensure the final rotation is set to the exact target
        isTransitioning = false;
    }
    #endregion

}
