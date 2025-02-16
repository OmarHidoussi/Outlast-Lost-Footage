using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarredLight : MonoBehaviour
{

    #region Variables

    public Light m_light;
    public float MinTime = 0.5f;
    public float Threshold = 0.5f;

    public GameObject[] LightSource_OBJ;
    public bool HasSource;
    public bool Usable;

    private float lastTime = 0;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        if(!Usable)
        {
            m_light.enabled = false;

            foreach (GameObject light in LightSource_OBJ)
            {
                light.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Usable)
        {
            m_light.enabled = false;

            foreach (GameObject light in LightSource_OBJ)
            {
                light.SetActive(false);
            }

            return;
        }

        if ((Time.time - lastTime) > MinTime)
        {
            if(Random.value > Threshold)
            {
                m_light.enabled = false;
                lastTime = Time.time;

                if (HasSource)
                {
                    foreach(GameObject light in LightSource_OBJ)
                    {
                        light.SetActive(false);
                    }
                }
            }
            else
            {
                m_light.enabled = true;

                if (HasSource)
                {
                    foreach (GameObject light in LightSource_OBJ)
                    {
                        light.SetActive(true);
                    }
                }
            }
        }


    }

    #endregion

    #region CustomMethods

    #endregion

}
