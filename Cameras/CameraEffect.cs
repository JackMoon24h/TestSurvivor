using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffect : MonoBehaviour 
{
    public PostProcessingProfile blurProfile;
    public PostProcessingProfile redBlurProfile;
    public PostProcessingProfile lightBlurProfile;
    public PostProcessingProfile endBlurProfile;
    public PostProcessingProfile defaultProfile;

    public PostProcessingBehaviour cameraPostProcess;

    public void EnableCameraBlur(bool state)
    {
        if (cameraPostProcess != null && blurProfile != null && defaultProfile != null)
        {
            cameraPostProcess.profile = (state) ? blurProfile : defaultProfile;
        }
    }

    public void EnableRedCameraBlur(bool state)
    {
        if (cameraPostProcess != null && redBlurProfile != null && defaultProfile != null)
        {
            cameraPostProcess.profile = (state) ? redBlurProfile : defaultProfile;
        }
    }

    public void EnableLightCameraBlur(bool state)
    {
        if (cameraPostProcess != null && lightBlurProfile != null && defaultProfile != null)
        {
            cameraPostProcess.profile = (state) ? lightBlurProfile : defaultProfile;
        }
    }

    public void EnableEndScreenBlur(bool state)
    {
        if (cameraPostProcess != null && endBlurProfile != null && defaultProfile != null)
        {
            cameraPostProcess.profile = (state) ? endBlurProfile : defaultProfile;
        }
    }
}
