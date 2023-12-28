using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    #region Variables

    public CharacterStats Player;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Custom Methods

    public void Hit()
    {
        Player = FindObjectOfType<CharacterStats>();
        Player.Health -= 50;
    }

    #endregion

}
