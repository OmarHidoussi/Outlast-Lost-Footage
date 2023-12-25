using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    #region Variables

    EnemySight Sight;

    #endregion

    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        Sight = GetComponent<EnemySight>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Custom Methods

    #endregion

}
