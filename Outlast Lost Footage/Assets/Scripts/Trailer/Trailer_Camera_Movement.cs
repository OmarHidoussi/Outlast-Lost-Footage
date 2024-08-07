using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer_Camera_Movement : MonoBehaviour
{

    public float speed;
    private bool ChangeTransform;

    public Transform[] Locations;
    int CurrentIndex;

    // Start is called before the first frame update
    void Start()
    {
        CurrentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ChangeToNextPosition();
    }

    void MoveCamera()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool Up = Input.GetKey(KeyCode.E);
        bool Down = Input.GetKey(KeyCode.A);

        if (!Up && !Down)
        {
            transform.Translate(h * speed * Time.deltaTime, 0, v * speed * Time.deltaTime);
        }
        else if (!Down && Up)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else
            transform.Translate(0, -speed * Time.deltaTime, 0);

    }

    void ChangeToNextPosition()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentIndex++;

            if (CurrentIndex == Locations.Length)
            {
                CurrentIndex = 0;
            }

            Debug.Log(CurrentIndex);

            transform.position = Locations[CurrentIndex].transform.position;
            transform.rotation = Locations[CurrentIndex].transform.rotation;
        }
    }
}
