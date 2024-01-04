using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHolder : MonoBehaviour
{

    #region Variables    
    
    public CharacterStats stats;
    public GameObject[] Batteries;

    #endregion


    #region BuiltIn Methods

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Batteries.Length; i++)
        {
            if (i < stats.BatteryCounter)
            {
                Batteries[i].SetActive(true);
            }
            else
                Batteries[i].SetActive(false);

        }
    }

    #endregion

    #region Custom Methods

    #endregion

}
