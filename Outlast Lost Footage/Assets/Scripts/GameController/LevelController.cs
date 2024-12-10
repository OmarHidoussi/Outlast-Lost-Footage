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

    public BarredLight[] LightsOff;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        Level4Start();
    }

    // Update is called once per frame
    void Update()
    {
        Level4Update();
    }

    #endregion

    #region CustomMethods

    void Level4Start()
    {
        ActiveGenerators = 0;
    }

    void Level4Update()
    {
        if (Generator_1_Activated && Generator_2_Activated)
        {
            ActiveGenerators = 2;
            ElectricDoor.SetBool("HalfOpened", false);
            ElectricDoor.SetBool("Opened", true);

            foreach (BarredLight lightoff in LightsOff)
            {
                lightoff.Usable = true;
            }
        }
        else if (Generator_1_Activated || Generator_2_Activated)
        {
            ActiveGenerators = 1;
            ElectricDoor.SetBool("Opened", false);
            ElectricDoor.SetBool("HalfOpened", true);
        }
        else
        {
            ActiveGenerators = 0;
            GlobalTimer = 0;
            ElectricDoor.SetBool("Opened", false);
            ElectricDoor.SetBool("HalfOpened", false);

            foreach (BarredLight lightoff in LightsOff)
            {
                lightoff.Usable = false;
            }
        }

        if (ActiveGenerators > 0)
        {
            GlobalTimer -= Time.deltaTime;
            if(GlobalTimer <= 0)
            {
                Generator_1_Activated = false;
                Generator_2_Activated = false;
            }
        }

    }

    #endregion

}
