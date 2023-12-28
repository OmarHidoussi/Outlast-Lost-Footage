using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolLocation : MonoBehaviour
{

    #region Variables

    public int LocationIndex;
    public EnemyAI Behavior;

    #endregion

    #region BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
        Behavior = FindObjectOfType<EnemyAI>();
    }
    #endregion

    #region Custom Methods

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Behavior.LocationIndex = LocationIndex;
        }
    }

    #endregion

}
