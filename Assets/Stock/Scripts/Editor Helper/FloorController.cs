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

    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    [OnValueChanged("ChangeFloorMaterial")]
    public Material FloorMatTop;

    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]

    [OnValueChanged("ChangeFloorMaterial")]
    public Material FloorMatBottom;



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
    public void ChangeFloorMaterial()
    {
        Material[] material = meshRenderer.materials;
        material[0] = FloorMatBottom;
        material[1] = FloorMatTop;
        meshRenderer.materials = material;
    }
}
