using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{
    #region Variables

    LevelController Controller;
    public bool SnapPlayerToCutsceneLocation;

    public Transform Location;

    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        Controller = FindObjectOfType<LevelController>();
        SnapPlayerToCutsceneLocation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.ActivationsCounter < 2)
            return;
        else
            SnapPlayerToCutsceneLocation = true;
            
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CharacterBehaviour Behaviour = other.gameObject.GetComponentInChildren<CharacterBehaviour>();
            Behaviour.Location = Location;
            //Keep Snapping Player To Cutscene Location
            if(SnapPlayerToCutsceneLocation)
            {
                Behaviour.SnapPlayerToPosition();
            }
        }
    }

    #endregion

    #region CustomMethods

    #endregion

}
