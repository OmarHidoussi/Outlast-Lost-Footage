using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    #region Variables

    public CharacterStats Player;
    public int Damage;
    public Animator anim;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<CharacterStats>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Custom Methods

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(anim.GetCurrentAnimatorStateInfo(1).IsName("Standing Melee Attack Downward"))
            {
                Player.Health -= Damage;
            }
        }
    }

    #endregion

}
