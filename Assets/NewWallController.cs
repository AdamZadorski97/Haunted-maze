using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class NewWallController : MonoBehaviour
{
    public GameObject wallController;
    [TableList(ShowIndexLabels = true)]
    public List<WallData> wallData = new List<WallData>();

    [OnValueChanged("ChangeProporties")]
    public int currentData = 0;

    [HorizontalGroup("Pattern")]
    [Button]
    public void NextPattern()
    {
        if (currentData == wallData.Count - 1)
        {
            currentData = 0;
            ChangeProporties();
            return;
        }
        if (currentData < wallData.Count)
        {
            currentData++;
            ChangeProporties();
            return;
        }
    }

    [HorizontalGroup("Pattern")]
    [Button]
    public void PreviousPattern()
    {

        if (currentData < 0)
        {
            currentData = wallData.Count - 1;
            ChangeProporties();
            return;
        }
        if (currentData > 0)
        {
            currentData--;
            ChangeProporties();
            return;
        }
    }


    [HorizontalGroup("Rotate")]
    [Button]
    public void RotateLeft()
    {
        transform.eulerAngles += new Vector3(0, -90, 0);
    }


    [HorizontalGroup("Rotate")]
    [Button]
    public void RotateRight()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
    }

    [HorizontalGroup("Clone")]
    [Button]

    public void CloneLeftAndSelect()
    {
        Debug.Log(this.transform.rotation);
        var currentEluers = this.transform.rotation;
        GameObject clone = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/1. Level Creator/WallController.prefab", typeof(GameObject)), null) as GameObject;
        clone.transform.position = transform.position + (transform.right * 2.5f);
        clone.transform.rotation = currentEluers;
        clone.GetComponent<NewWallController>().currentData = currentData;
        clone.GetComponent<NewWallController>().ChangeProporties();
        Selection.activeObject = clone;
    }
    [HorizontalGroup("Clone")]
    [Button]

    public void CloneRightAndSelect()
    {
        Debug.Log(this.transform.rotation);
        var currentEluers = this.transform.rotation;
        GameObject clone = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/1. Level Creator/WallController.prefab", typeof(GameObject)), null) as GameObject;
        clone.transform.position = transform.position + (-transform.right * 2.5f);
        clone.transform.rotation = currentEluers;
        clone.GetComponent<NewWallController>().currentData = currentData;
        clone.GetComponent<NewWallController>().ChangeProporties();
        Selection.activeObject = clone;
    }

  


    public void ChangeProporties()
    {
        gameObject.name = $"WallController {wallData[currentData].name}";


        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));

        InstantiateWallWithData(wallData[currentData]);
    }




    private void InstantiateWallWithData(WallData wallData)
    {
        GameObject meshRendererObject = new GameObject();
        meshRendererObject.transform.SetParent(transform);
        meshRendererObject.transform.localPosition = Vector3.zero;
        meshRendererObject.name = "MeshRenderer";
        MeshFilter meshFilter = meshRendererObject.AddComponent<MeshFilter>();
        meshFilter.mesh = wallData.mesh;
        MeshRenderer meshRenderer = meshRendererObject.AddComponent<MeshRenderer>();
        var currentEluers = this.transform.rotation;
        meshFilter.transform.rotation = currentEluers;

        for (int i = 0; i < wallData.boxCollidersCenter.Count; i++)
        {
            GameObject boxcolliderObject = new GameObject();
            BoxCollider boxCollider = boxcolliderObject.AddComponent<BoxCollider>();
            boxCollider.transform.SetParent(transform);
            boxcolliderObject.transform.localPosition = Vector3.zero;
            boxCollider.size = wallData.boxCollidersSize[i];
            boxCollider.center = wallData.boxCollidersCenter[i];
            boxcolliderObject.name = $"BoxCollider{i}";
            boxcolliderObject.layer = LayerMask.NameToLayer("Wall");
            var boxcolliderRotation = this.transform.rotation;
            boxcolliderObject.transform.rotation = boxcolliderRotation;
        }



        Material[] material = new Material[wallData.materials.Count];
        for (int i = 0; i < wallData.materials.Count; i++)
        {
            material[i] = wallData.materials[i];
        }
        meshRenderer.materials = material;
    }
}
