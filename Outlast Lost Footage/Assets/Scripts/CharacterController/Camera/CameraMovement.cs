using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    #region Variables

    public float Sensetivity;
    public float TransitionSpeed;
    public Transform PlayerBody;
    public float X_Max;

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
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * Sensetivity;
        float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensetivity * Factor;


        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -X_Max, X_Max);

        transform.localRotation = Quaternion.Euler(xRotation,0,0);

        PlayerBody.Rotate(Vector3.up * MouseX);

        HandleHeight();
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

    #endregion

}
