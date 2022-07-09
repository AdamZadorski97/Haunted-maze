using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [OnValueChanged("SwitchPoint")]
    [SerializeField] private bool interactivePointsIsOn;
    public GameObject interactivePoint;
    [SerializeField] private MeshRenderer meshRenderer;

    public void SwitchMeshRenderer(bool state)
    {
        meshRenderer.enabled = state;
    }



    public void Start()
    {
        if (transform.position.y != 0)
        {
            SwitchPoint(false);
        }
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
