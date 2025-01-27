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
            if(Behavior != null)
            {
                //Behavior.LocationIndex = LocationIndex;
                Behavior.GetComponent<EnemyPatrol>().waypoints = Behavior.locations[LocationIndex].waypoints;
                //Debug.Log("Location is set to:" + LocationIndex);
            }


        }
    }

    #endregion

}
