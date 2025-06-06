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

        StartCoroutine(ForcePipelineRefresh());
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

        hdCam.antialiasing = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;

        yield return null; // Wait one frame (flush)

        // Step 2: Wait 10 seconds before the reset
        yield return new WaitForSeconds(10f);

        hdCam.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;

        Debug.Log("HDRP pipeline refreshed via coroutine.");
        FunctionIsPlaying = false;

        // 🔁 Restart the coroutine after 10 seconds
        yield return new WaitForSeconds(10f);
        StartCoroutine(ForcePipelineRefresh());
    }
}
