using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{

    #region Variables
    public Transform location;
    public GameObject SupportingPlatform;
    public AvatarTarget target;
    public MatchTargetWeightMask mask;


    #endregion

    #region BuiltIn Mehthods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //SupportingPlatform.SetActive(!FindObjectOfType<CharacterAudio>().IsSnapping);

            if (other.GetComponentInParent<InputManager>().Jump)
            {
                other.GetComponentInChildren<CharacterAudio>().Location = location;
                other.GetComponentInParent<CharacterAnimator>().HandleWallClimb();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SupportingPlatform.SetActive(false);
        }
    }

    #endregion

    #region Custom Methods

    #endregion

}
