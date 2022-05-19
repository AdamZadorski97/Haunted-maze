using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class NewWallController : MonoBehaviour
{
    public List<WallData> wallData = new List<WallData>();
    public int currentData = 0;
    public Transform dataParrent;

    [Button]
    public void ChangeProportiesLeft()
    {
        if(transform.childCount>0)
        Destroy(transform.GetChild(0));
        InstantiateWallWithData(wallData[currentData]);
        currentData++;
    }
    private void InstantiateWallWithData(WallData wallData)
    {
        GameObject WallObject = new GameObject();
        WallObject.name = "WallObject";
        MeshFilter meshFilter = WallObject.AddComponent<MeshFilter>();
        meshFilter.mesh = wallData.mesh;
        MeshRenderer meshRenderer = WallObject.AddComponent<MeshRenderer>();

        Material[] material = meshRenderer.materials;
        for (int i = 0; i < wallData.materials.Count; i++)
        {
            material[i] = wallData.materials[i];
        }
        meshRenderer.materials = material;

        for (int i = 0; i < wallData.boxCollidersOffset.Count; i++)
        {
            GameObject boxcolliderObject = new GameObject();
            BoxCollider boxCollider = boxcolliderObject.AddComponent<BoxCollider>();
            boxCollider.size = wallData.boxCollidersOffset[i];
            boxCollider.center = wallData.boxCollidersOffset[i];
            boxCollider.transform.SetParent(WallObject.transform);
            boxcolliderObject.name = $"BoxCollider{i}";
        }
    }
}
