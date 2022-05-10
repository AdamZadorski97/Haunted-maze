using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingController : MonoBehaviour
{
   
    public MeshRenderer meshRenderer;

    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    [HorizontalGroup("Floor Material")]
    [OnValueChanged("ChangeFloorMaterial")]
    public Material ceilingMatTop;

    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    [HorizontalGroup("Floor Material")]
    [OnValueChanged("ChangeFloorMaterial")]
    public Material ceilingMatBottom;


    public void ChangeFloorMaterial()
    {
        Material[] material = meshRenderer.materials;
        material[0] = ceilingMatBottom;
        material[1] = ceilingMatTop;
        meshRenderer.materials = material;
    }
}
