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


    bool PlayerIsDead;
    InputManager input;
    CharacterAnimator anim;
    public Animator FadePanelAnimator;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
        anim = GetComponent<CharacterAnimator>();
        PlayerIsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Reload) UpdateStats();

        if (RegainHealth) Heal();

        if (Health <= 0)
        {
            Debug.Log("Dying...");
            this.GetComponent<CharacterAnimator>().CharacterAnim.SetBool("Dead", true);
            if(!FadePanelAnimator.GetBool("DeadHandled") && !PlayerIsDead)
            {
                PlayerIsDead = true;
                FadePanelAnimator.SetTrigger("Die");
                FadePanelAnimator.SetBool("DeadHandled", true);
                //StartCoroutine(FadeOutaPanel());
            }
            input.Mov_Axis = Vector2.zero;
            input.IsDead = true;
            input.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationX;
            input.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezeRotationZ;
            if (anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") || anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Death") || 
                anim.CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                this.GetComponent<CharacterAnimator>().CharacterAnim.SetBool("Dead", false);
            }
        }
    }
    #endregion

    #region CustomMethods

    IEnumerator FadeOutaPanel()
    {
        Debug.Log("FadeOut");

        yield return null;
        FadePanelAnimator.SetBool("DeadHandled", false);
    }
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
