using UnityEngine;

public class NightVisionLight : MonoBehaviour
{
    #region Variables

    public float minIntensity = 0.05f;
    public float maxIntensity = 1f;
    public float maxDistance = 10f;
    public float AdjustmentSpeed = 15f;
    public AnimationCurve IntensityCurve;

    public float IntensityFactor;
    public Transform rayOrigin;
    public LayerMask mask;

    #endregion

    #region BuiltInMethods

    void Update()
    {
        AdjustIntensity();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        RaycastHit hit;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            // Draw ray up to the hit point
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * hit.distance);
        }
        else
        {
            // Draw a default-length ray if nothing is hit
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * 100f);
        }
    }


    #endregion

    #region CustomMethods

    void AdjustIntensity()
    {
        RaycastHit hit;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        float targetIntensity = 1f;

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.collider.name);
            if (hit.distance <= maxDistance)
            {
                float distance = hit.distance;
                //Debug.Log("Object Distance: " + distance);

                float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

                // Apply AnimationCurve to modify intensity
                float curveMultiplier = IntensityCurve.Evaluate(normalizedDistance);

                // Lerp intensity and apply curve
                targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedDistance);
            }
            else
            {
                targetIntensity = maxIntensity;
            }
        }

        // Smooth transition
        IntensityFactor = Mathf.Lerp(IntensityFactor, targetIntensity, Time.deltaTime * 5f);
    }


    #endregion
}

//N3N ZABOUR OM EL UNITY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!