using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraState { CameraOff, CameraOnWithoutInfrared, CameraOnWithInfrared }
public class CharacterStats : MonoBehaviour
{

    #region Variables

    CameraState CamState;
    public int MaxCollectedBatteries;
    public int BatteryCounter;
    public float Health;
    public bool RegainHealth;

    InputManager input;
    CharacterAnimator anim;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
        anim = GetComponent<CharacterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Reload) UpdateStats();

        if (RegainHealth) Heal();

        if (Health <= 0)
        {
            this.GetComponent<CharacterAnimator>().CharacterAnim.SetBool("Dead", true);
            input.Mov_Axis = Vector2.zero;

            if (anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") || anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Death"))
            {
                this.GetComponent<CharacterAnimator>().CharacterAnim.SetBool("Dead", false);
            }
        }
    }
    #endregion

    #region CustomMethods
    void UpdateStats()
    {
        if (BatteryCounter > 0)
        {
            BatteryCounter -= 1;
        }
    }

    public float healthIncreaseRate = 10.0f;
    void Heal()
    {
        if(Health >= 100)
        {
            Health = 100;
            RegainHealth = false;
        }

        if (Health < 100)
        {
            Health += healthIncreaseRate * Time.deltaTime;

            // Ensure health doesn't exceed the maximum
            Health = Mathf.Min(Health, 100f);
        }
    }

    #endregion

}
