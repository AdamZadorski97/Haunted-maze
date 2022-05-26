using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [OnValueChanged("SwitchPoint")]
    public bool interactivePointsIsOn;
    public GameObject interactivePoint;
    public MeshRenderer meshRenderer;

    public void SwitchMeshRenderer(bool state)
    {
        meshRenderer.enabled = state;
    }

    public void SwitchPoint(bool state)
    {
        if(interactivePointsIsOn)
        {
            interactivePoint.SetActive(state);
        }
        else
        {
            interactivePoint.SetActive(false);
        }
    }
}
