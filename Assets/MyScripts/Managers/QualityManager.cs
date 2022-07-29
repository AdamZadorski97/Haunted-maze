using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public List<GameObject> volumetricLights = new List<GameObject>();

    private void Start()
    {
        ChangeVolumetricLightsState(false);
    }

    private void ChangeQualitySettings()
    {

    }

    private void ChangeVolumetricLightsState(bool state)
    {
        foreach(GameObject light in volumetricLights)
        {
           Destroy(light);
        }
    }
}
