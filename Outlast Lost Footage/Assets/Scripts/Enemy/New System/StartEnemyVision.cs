using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemyVision : MonoBehaviour
{
    public Enemy_Sighting sight;
    public float Raduis;

    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponentInParent<Enemy_Sighting>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sight.enabled = true;
            sight.GetComponent<SphereCollider>().radius = Raduis;
            FindObjectOfType<Enemy_AI>().FirstPhaseEnded = true;
        }
    }
}
