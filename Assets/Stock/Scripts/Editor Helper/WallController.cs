using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
public class WallController : MonoBehaviour
{


    [TabGroup("Front")]
    [OnValueChanged("SwitchFrontMesh")]
    public Mesh frontMesh;
    [TabGroup("Front")]
    [OnValueChanged("SwitchFrontMeshRenderer")]
    public bool frontMeshRenderer;
    [TabGroup("Front")]
    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [OnValueChanged("ChangeFrontMaterial")]
    public Material frontMaterial;


    [TabGroup("Back")]
    [OnValueChanged("SwitchBackMesh")]
    public Mesh backMesh;
    [TabGroup("Back")]
    [OnValueChanged("SwitchBackMeshRenderer")]
    public bool backMeshRenderer;
    [TabGroup("Back")]
    [AssetList(Path = "/ThirdParty Assets/PolygonHorrorMansion/Materials/Building")]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [OnValueChanged("ChangeBackMaterial")]
   
    
    
    
    public Material backMaterial;

    public BoxCollider boxCollider;
    public bool showPath;
    public GameObject mapVisualization;
    public bool isWall;

    public MeshRenderer frontMeshrenderer;
    public MeshRenderer backMeshrenderer;

    public MeshFilter frontMeshFilter;
    public MeshFilter backMeshFilter;

    private void SwitchFrontMesh()
    {
        frontMeshFilter.mesh = frontMesh;
    }
    private void SwitchBackMesh()
    {
        backMeshFilter.mesh = backMesh;
    }


    private void SwitchFrontMeshRenderer()
    {
        frontMeshrenderer.enabled = frontMeshRenderer;
    }
    private void SwitchBackMeshRenderer()
    {
        backMeshrenderer.enabled = backMeshRenderer;
    }

    public void Start()
    {
        mapVisualization.SetActive(true);
    }

    public void SwitchMeshRenderer(bool state)
    {
        frontMeshrenderer.enabled = state;
        backMeshrenderer.enabled = state;
    }

    public void ChangeFrontMaterial()
    {
        Material[] material = frontMeshrenderer.materials;
        material[0] = frontMaterial;
        frontMeshrenderer.materials = material;
    }

    public void ChangeBackMaterial()
    {
        Material[] material = backMeshrenderer.materials;
        material[0] = backMaterial;
        backMeshrenderer.materials = material;
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

