using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{

    #region Variables

    [HideInInspector] public Transform Player;
    public CharacterCollision collision;
    public Vector3 Offset;
    public bool SetParent;
    public bool Cutscene;

    public AudioSource Elevatorsource;

    #endregion


    #region BuiltInMethods

    private void Awake()
    {
        if (!SceneManager.GetSceneByName("Main Menu").isLoaded && SceneManager.GetAllScenes().Length == 1)
        {
            SceneManager.LoadSceneAsync("Persistent Scene", LoadSceneMode.Additive);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Player = FindObjectOfType<InputManager>().transform;

        var inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            Player = inputManager.transform;
            if (Cutscene)
                Player.GetComponent<Animator>().SetTrigger("PrologueCutscene");
            else
                Player.GetComponent<CharacterMovement>().ResetAfterCutScene();
        }

        //collision = FindObjectOfType<CharacterCollision>();

        var collision = FindObjectOfType<CharacterCollision>();
        if (collision != null)
        {
            this.collision = collision;
            collision.enabled = false;
        }


        SetParent = false;

        Elevatorsource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (SetParent)
        {
            Player.GetComponentInParent<InputManager>().MidAir = false;
            Player.GetComponentInParent<Rigidbody>().useGravity = false;
            Player.transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) + Offset;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetParent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetParent = false;
            Player.GetComponentInParent<Rigidbody>().useGravity = true;
            collision.enabled = true;
        }
    }

    #endregion

    #region CustomMethods

    public void PlayAudio()
    {
        Elevatorsource.Play();
    }


    #endregion

}
