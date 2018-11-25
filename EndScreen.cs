using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EndScreen : MonoBehaviour 
{
    public PostProcessingProfile endBlurProfile;
    public PostProcessingProfile defaultProfile;

    public PostProcessingBehaviour cameraPostProcess;

    public void EnableScreenBlur(bool state)
    {
        if(cameraPostProcess != null && endBlurProfile != null && defaultProfile != null)
        {
            cameraPostProcess.profile = (state) ? endBlurProfile : defaultProfile;
        }
    }
}
