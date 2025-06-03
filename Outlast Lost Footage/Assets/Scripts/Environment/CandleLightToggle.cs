using UnityEngine;

public class CandleLightToggle : MonoBehaviour
{
    private Light candleLight;
    private Transform player;
    public float activationDistance = 30f;

    private void Start()
    {
        candleLight = GetComponentInChildren<Light>();
        player = FindObjectOfType<InputManager>().transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        candleLight.enabled = dist < activationDistance;
    }
}