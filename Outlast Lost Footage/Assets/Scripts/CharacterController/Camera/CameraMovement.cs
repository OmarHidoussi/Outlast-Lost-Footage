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

        // Calculate the target rotation for looking back
        Quaternion lookBackRotation = Quaternion.LookRotation(LookBackDirection.forward, transform.up);

        // Smoothly interpolate between the current rotation and the target rotation
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, lookBackRotation, elapsedTime);
            elapsedTime += Time.deltaTime * LookbackTransitionSpeed;
            yield return null;
        }

        isTransitioning = false;
    }

    IEnumerator ReturnToForwardSmoothly()
    {
        isTransitioning = true;

        // Smoothly interpolate back to the original rotation
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion originalRotation = Quaternion.Euler(xRotation, 0, 0);

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime);
            elapsedTime += Time.deltaTime * LookbackTransitionSpeed;
            yield return null;
        }

        isTransitioning = false;
    }

    #endregion

}
