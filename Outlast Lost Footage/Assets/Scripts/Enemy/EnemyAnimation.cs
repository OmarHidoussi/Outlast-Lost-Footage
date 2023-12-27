using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{

    #region Variables

    public Animator Anim;
    private NavMeshAgent nav;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Anim.speed = nav.speed;
        Anim.SetFloat("Speed", nav.speed);
    }

    #endregion

    #region Custom Methods

    #endregion

}
