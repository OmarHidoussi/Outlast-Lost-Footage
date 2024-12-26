using UnityEngine;

public class Interaction : MonoBehaviour
{

    #region Variables
    public GameObject Text;
    public KeyCode InteractionKey;
    public Sprite KeyboardInteractionButton;
    public Sprite GamepadInteractionButton;
    public string InteractionText;
    public string Helptext;
    public bool Interacted;
    public Transform InteractionLocation;
    #endregion

    //Destroying The interaction text after the player exists the trigger
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InteractionText = "";
            KeyboardInteractionButton = null;
            GamepadInteractionButton = null;
            Destroy(gameObject);
        }
    }

}
