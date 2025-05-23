using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    #region Variables

    [SerializeField] private bool enable = true;

    [Header("Walk")]
    [SerializeField, Range(0, 0.1f)] private float Yamplitude, Xamplitude, BreathAmplitude = 0.015f;
    [SerializeField, Range(0, 60)] private float frequency, Breathfrequency = 10.0f;
    [SerializeField, Range(0, 1000f)] private float ZAmplitude = 0.02f; // Amplitude for Z-axis bobbing

    [SerializeField] private Transform cam = null;
    [SerializeField] private Transform camHolder = null;

    private Vector3 startPos;
    private Quaternion startRot;
    private InputManager input;
    private CharacterMovement movement;

    #endregion

    #region BuiltInMethods

    void Awake()
    {
        startPos = cam.localPosition;
        startRot = cam.localRotation;

        input = GetComponent<InputManager>();
        movement = GetComponent<CharacterMovement>();

        GameSettings GameData = FindObjectOfType<GameSettings>();
        if (GameData != null)
        {
            enable = !GameData.ReduceHeadMotion;
        }
        else
        {
            enable = true;
        }
    }

    void LateUpdate()
    {
        if (!enable) return;

        CheckMotion();
        ResetPosition();
        cam.LookAt(FocusTarget());
    }

    #endregion

    #region CustomMethods
    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }

    private void CheckMotion()
    {
        PlayMotion(FootStepMotion());
        ApplyHeadTilt(); // Apply simplified head tilt
    }

    // New method for head tilt when moving left or right
    private void ApplyHeadTilt()
    {

    }

    private Vector3 FootStepMotion()
    {
        float ValueMultiplier = 0.35f;
        float FrequencyMultiplier = 1.12f;
        float SpeedMultiplier = movement.Speed / 5;

        Vector3 pos = Vector3.zero;
        if (input.Mov_Axis.x == 0)
        {
            pos.y += Mathf.Sin(Time.time * Breathfrequency * ValueMultiplier * SpeedMultiplier) * BreathAmplitude * ValueMultiplier * SpeedMultiplier;
        }

        /*frequency = (movement.Speed * FrequencyMultiplier * SpeedMultiplier) * 1.4f;
        frequency = Mathf.Clamp(frequency, 11, 18);*/
        //Adding dynamic frequency turns out a bad idea as the camera start making an wanted transition effect when interpolating the value

        if (input.IsSprinting)
            frequency = 15f;
        else
            frequency = 11f;

        Yamplitude = (ValueMultiplier * SpeedMultiplier * input.Mov_Axis.x) / 36.5f;
        Xamplitude = (ValueMultiplier * SpeedMultiplier / 2 * input.Mov_Axis.x) / 90;

        pos.y += Mathf.Sin(Time.time * frequency) * Yamplitude;
        pos.x -= Mathf.Cos(Time.time * frequency / 2) * Xamplitude;

        return pos;
    }

    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);

        if (cam.localRotation == startRot) return;
        cam.localRotation = Quaternion.Lerp(cam.localRotation, startRot, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }

    #endregion
}
