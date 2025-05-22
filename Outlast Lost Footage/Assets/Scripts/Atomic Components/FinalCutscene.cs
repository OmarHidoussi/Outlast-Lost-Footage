using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalCutscene : MonoBehaviour
{
    #region Variables

    LevelController Controller;
    public bool SnapPlayerToCutsceneLocation;

    public Animator Elevator_Anim;
    public Animator Broken_Glass;
    public GameObject Rusty_Crocks_FinalCutscene;
    public Transform Location;

    //public Image Cursor;

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

            other.GetComponentInParent<InputManager>().GetComponent<CharacterMovement>().enabled = false;
            other.GetComponentInParent<InputManager>().GetComponent<Animator>().applyRootMotion = false;

            /*Color cursorColor = Cursor.color;
            cursorColor.a = Mathf.Lerp(cursorColor.a, 0f, 3f * Time.deltaTime);
            Cursor.color = cursorColor;*/

            //Keep Snapping Player To Cutscene Location
            if (SnapPlayerToCutsceneLocation)
            {
                Behaviour.SnapPlayerToPosition();
            }

            if (Vector3.Distance(other.transform.position, Location.position) <= 0.15f)
            {
                other.transform.position = Location.position;

                other.GetComponentInParent<InputManager>().GetComponent<Animator>().enabled = true;
                other.GetComponentInParent<InputManager>().GetComponent<Animator>().SetBool("FinalCutscene", true);
                Elevator_Anim.SetBool("FinalCutscene", true);
                Broken_Glass.SetBool("FinalCutscene", true);
                Rusty_Crocks_FinalCutscene.SetActive(true);
                
                Destroy(this.gameObject);
            }
        }
    }

    #endregion

    #region CustomMethods

    #endregion

}
