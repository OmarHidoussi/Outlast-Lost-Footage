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

    /*[Header("Run")]
    [SerializeField, Range(0, 0.1f)] private float RunYamplitude, RunXamplitude, RunBreathAmplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float Runfrequency, RunBreathfrequency = 10.0f;
    */
    [SerializeField] private Transform cam = null;
    [SerializeField] private Transform camHolder = null;

    //private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    private InputManager input;
    private CharacterMovement movement;

    #endregion

    #region BuiltInMethods

    void Awake()
    {
        startPos = cam.localPosition;

        input = GetComponent<InputManager>();
        movement = GetComponent<CharacterMovement>();

        GameSettings GameData = FindObjectOfType<GameSettings>();
        if(GameData != null)
        {
            enable = !GameData.ReduceHeadMotion;
        }
        else
        {
            enable = true;
        }
    }


    void Update()
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
    
    }

    private Vector3 FootStepMotion()
    {
        float ValueMultiplier = 0.35f;
        float FrequencyMultiplier = 1.12f;
        float SpeedMultiplier = movement.Speed / 5;

        /*if (movement.Speed > movement.WalkSpeed + 1)
        {
            ValueMultiplier = ValueMultiplier + (movement.Speed - movement.WalkSpeed - 1) / 3;
            FrequencyMultiplier = FrequencyMultiplier + (movement.Speed - movement.WalkSpeed - 1) / 3;
        }
        else
        {
            ValueMultiplier = Mathf.Lerp(ValueMultiplier, 1, 2f * Time.deltaTime);
            FrequencyMultiplier = Mathf.Lerp(ValueMultiplier, 1, 2f * Time.deltaTime);
        }*/

        /*if (!input.CameraOn)
        {
            ValueMultiplier = 0.35f;
            FrequencyMultiplier = 1.12f;
        }*/

        Vector3 pos = Vector3.zero;
        if (input.Mov_Axis.x == 0)
        {
            pos.y += Mathf.Sin(Time.time * Breathfrequency * ValueMultiplier * SpeedMultiplier) * BreathAmplitude * ValueMultiplier * SpeedMultiplier;
        }

        frequency = (movement.Speed * FrequencyMultiplier * SpeedMultiplier) * 1.4f;
        frequency = Mathf.Clamp(frequency, 8, 18);
        Yamplitude = (ValueMultiplier * SpeedMultiplier * input.Mov_Axis.x) / 36.5f;
        Xamplitude = (ValueMultiplier * SpeedMultiplier / 2 * input.Mov_Axis.x) / 90;

        pos.y += Mathf.Sin(Time.time * frequency) * Yamplitude;
        pos.x -= Mathf.Cos(Time.time * frequency/ 2) * Xamplitude;

        return pos;

    }

    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);

    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }

    #endregion

}
