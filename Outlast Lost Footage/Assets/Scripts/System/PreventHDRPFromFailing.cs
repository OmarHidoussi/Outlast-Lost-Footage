using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PreventHDRPFromFailing : MonoBehaviour
{
    public Camera monitoredCamera;
    private HDAdditionalCameraData hdCam;
    private bool FunctionIsPlaying;
    void Start()
    {
        FunctionIsPlaying = false;

        if (!monitoredCamera.TryGetComponent(out hdCam))
        {
            Debug.LogError("HDRPFailSafe: No HDAdditionalCameraData on the camera.");
        }
    }

    public void TriggerPipelineRefresh()
    {
        if (hdCam != null)
        {
            StartCoroutine(ForcePipelineRefresh());
        }
    }

    public IEnumerator ForcePipelineRefresh()
    {
        FunctionIsPlaying = true;
        // Step 1: Disable anti-aliasing to reset pipeline
        hdCam.antialiasing = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;

        // Step 2: Wait for one frame to let HDRP flush the change
        //yield return null;
        yield return new WaitForSeconds(5f);

        // Step 3: Re-enable FXAA to rebuild the pipeline
        hdCam.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
        FunctionIsPlaying = false;
        Debug.Log("HDRP pipeline refreshed via coroutine.");
    }

    void Update()
    {
        
        float frameTime = Time.deltaTime * 1000f;

        float resolutionFactor = Mathf.InverseLerp(1080f, 2160f, Screen.width); // 0 at 1080p, 1 at 2160p
        float frameTimeThreshold = Mathf.Lerp(30f, 40f, resolutionFactor);      // interpolates between 25 and 40

        if (frameTime > frameTimeThreshold && !FunctionIsPlaying)
        {
            TriggerPipelineRefresh();
        }
        // Optional: add RenderTexture anomaly detection (black screen, nan color, etc.)

    }
}
