using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AnimationEvents : MonoBehaviour
{

    #region Variables

    public bool State;
    public TextMeshProUGUI Helptext;

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

    public void DisableAnimation()
    {
        Helptext.gameObject.GetComponent<Animator>().SetBool("Display", State);
    }

    #endregion
}
