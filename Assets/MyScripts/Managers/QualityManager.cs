using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;

public class QualityManager : MonoBehaviour
{
    public Quality currentQuality;
    public enum Quality { High, Medium, Low };

    public CinemachineVirtualCamera myCamera;
    public Volume postPtocessingVolume;
    public List<VolumetricLightBeam> volumetricLights = new List<VolumetricLightBeam>();
    public SaveLoadDataManager saveLoadDataManager;





    private void Start()
    {
        ChangeSettings();
    }

    private void ChangeSettings()
    {
      int savedQualitySettings =  saveLoadDataManager.GetQualitySettings();

        switch (savedQualitySettings)
        {
            case 0:
                currentQuality = Quality.Low;
                break;

            case 1:
                currentQuality = Quality.Medium;
                break;

            case 2:
                currentQuality = Quality.High;
                break;
        }

        ChangeVolumetricLightsState();
        ChangePostProcessingState();
        ChangeFieldOfView();
    }

    private void ChangeFieldOfView()
    {
      



        if (currentQuality == Quality.Low)
        {
            RenderSettings.fogEndDistance = 14;
            RenderSettings.fogStartDistance = 10;
            myCamera.m_Lens.FarClipPlane = 15;
        }
        if (currentQuality == Quality.Medium)
        {
            RenderSettings.fogEndDistance = 20;
            RenderSettings.fogStartDistance = 14;
            myCamera.m_Lens.FarClipPlane = 21;
        }
        if (currentQuality == Quality.High)
        {
            RenderSettings.fogEndDistance = 25;
            RenderSettings.fogStartDistance = 20;
            myCamera.m_Lens.FarClipPlane = 26;
        }
    }

    private void ChangePostProcessingState()
    {
        if (currentQuality == Quality.Low)
        {
            postPtocessingVolume.enabled = false;
        }
        if (currentQuality == Quality.Medium)
        {
            postPtocessingVolume.enabled = false;
        }
        if (currentQuality == Quality.Medium)
        {
            postPtocessingVolume.enabled = true;
        }
    }

    private void ChangeVolumetricLightsState()
    {
        if (currentQuality == Quality.Low)
        {
            foreach (VolumetricLightBeam volumetricLight in volumetricLights)
            {
                volumetricLight.gameObject.SetActive(false);
            }
        }
        if (currentQuality == Quality.Medium)
        {
            foreach (VolumetricLightBeam volumetricLight in volumetricLights)
            {
                volumetricLight.noiseMode = NoiseMode.Disabled;
            }
        }

        if (currentQuality == Quality.High)
        {
            foreach (VolumetricLightBeam volumetricLight in volumetricLights)
            {
                volumetricLight.noiseMode = NoiseMode.LocalSpace;
            }
        }
    }
}
