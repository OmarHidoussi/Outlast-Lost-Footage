using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    #region Variables
    public GameObject Player;
    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    { 
        transform.LookAt(Camera.main.transform); 
    }
    #endregion

    #region CustomMethods

    #endregion

}
