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

    #endregion

    #region BuiltInMethods

    void Update()
    {
        AdjustIntensity();
    }

    private void OnDrawGizmos()
    {
        // Set the color of the Gizmo line
        Gizmos.color = Color.green;

        // Draw the ray in the Scene view
        Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * 100);
    }

    #endregion

    #region CustomMethods

    void AdjustIntensity()
    {
        RaycastHit hit;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        float targetIntensity = 1f;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance <= maxDistance)
            {
                float distance = hit.distance;
                //Debug.Log("Object Distance: " + distance);

                // Normalize distance (0 = close, 1 = maxDistance)
                float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

                // Apply AnimationCurve to modify intensity
                float curveMultiplier = IntensityCurve.Evaluate(normalizedDistance);

                // Lerp intensity and apply curve
                targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedDistance)/* * curveMultiplier*/;
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