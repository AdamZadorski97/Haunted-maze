using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
 
    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    [HorizontalGroup("Wall Material")]
    [OnValueChanged("ChangeWallMaterial")]
    public Material WallMat0;

    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    [HorizontalGroup("Wall Material")]
    [OnValueChanged("ChangeWallMaterial")]
    public Material WallMat1;

    public BoxCollider boxCollider;
    public MeshRenderer meshRenderer;
    public bool showPath;
    public GameObject mapVisualization;
    public bool isWall;




    public void Start()
    {
        mapVisualization.SetActive(true);
    }

    public void SwitchMeshRenderer(bool state)
    {
        meshRenderer.enabled = state;
    }

    public void ChangeWallMaterial()
    {
        Material[] material = meshRenderer.materials;
        material[0] = WallMat0;
        material[1] = WallMat1;
        meshRenderer.materials = material;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider != null && showPath)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(Vector3.zero + boxCollider.center, boxCollider.size);
        }
    }
}

