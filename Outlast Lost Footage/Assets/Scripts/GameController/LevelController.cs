using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    #region Variables

    [Header("Level 4")]
    [Space]
    public bool Generator_1_Activated;
    public bool Generator_2_Activated;
    public Animator ElectricDoor;

    public int ActiveGenerators;

    public float GlobalTimer;
    private float ResetTimer;

    public BarredLight[] LightsOff_Section1;
    public BarredLight[] LightsOff_Section2;
    public BarredLight[] AllLightOff;

    [Space]
    public bool G_PlayerInSight; //Global player sighting, this variable can be modified by other scripts
    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        G_PlayerInSight = false;
        ActiveGenerators = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGeneratorsState();
        UpdateGameState();
    }

    #endregion

    #region CustomMethods

    void UpdateGeneratorsState()
    {

        if (Generator_1_Activated && Generator_2_Activated)
        {
            ActiveGenerators = 2;
            ElectricDoor.SetBool("HalfOpened", false);
            ElectricDoor.SetBool("Opened", true);

            foreach (BarredLight lightoff in AllLightOff)
            {
                lightoff.Usable = true;
            }
        }

        else if (Generator_1_Activated || Generator_2_Activated)
        {
            ActiveGenerators = 1;
            ElectricDoor.SetBool("Opened", false);
            ElectricDoor.SetBool("HalfOpened", true);

            if (Generator_1_Activated)
            {
                foreach (BarredLight lightoff in LightsOff_Section1)
                {
                    lightoff.Usable = true;
                }
            }
            else
            {
                foreach (BarredLight lightoff in LightsOff_Section2)
                {
                    lightoff.Usable = true;
                }
            }
        }
        else
        {
            ActiveGenerators = 0;
            GlobalTimer = 0;
            ElectricDoor.SetBool("Opened", false);
            ElectricDoor.SetBool("HalfOpened", false);

            foreach (BarredLight lightoff in AllLightOff)
            {
                lightoff.Usable = false;
            }
        }

        if (ActiveGenerators > 0)
        {
            GlobalTimer -= Time.deltaTime;
            if (GlobalTimer <= 0)
            {
                Generator_1_Activated = false;
                Generator_2_Activated = false;
            }
        }
    }

    void UpdateGameState()
    {

    }

    #endregion

}
