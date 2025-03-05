using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour,IInteractable
{

    #region Variables

    public LevelController controller;
    private Interaction interaction;
    public bool Generator_1, Generator_2, EngineActivated;
    public float AddedTimer;
    public float RotationSpeed;
    public Transform Engine;

    public AudioSource GeneratorAudio;
    public AudioClip PowerOn_Clip;

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

            if (EngineActivated)
                Engine.transform.Rotate(Engine.transform.TransformDirection(transform.forward) * RotationSpeed * Time.deltaTime * (controller.GlobalTimer / 25));
        }
        if (Generator_2)
        {
            interaction.Interacted = controller.Generator_2_Activated; 

            if (EngineActivated)
                Engine.transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime * (controller.GlobalTimer / 25), Space.Self);
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

        controller.GlobalTimer = AddedTimer;
        GeneratorAudio.PlayOneShot(PowerOn_Clip);
        EngineActivated = true;
    }

    #endregion
}
