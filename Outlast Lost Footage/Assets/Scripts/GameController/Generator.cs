using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour,IInteractable
{

    #region Variables

    public LevelController controller;
    private Interaction interaction;
    public bool Generator_1, Generator_2;
    public float AddedTimer;
    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        interaction = GetComponent<Interaction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Generator_1)
        {
            interaction.Interacted = controller.Generator_1_Activated;
        }
        if (Generator_2)
        {
            interaction.Interacted = controller.Generator_2_Activated;
        }
    }
    #endregion

    #region CostumeMethods

    public void Interact()
    {
        if (Generator_1)
        {
            controller.Generator_1_Activated = true;
        }

        if (Generator_2)
        {
            controller.Generator_2_Activated = true;
        }

        controller.GlobalTimer += AddedTimer;
    }

    #endregion
}
