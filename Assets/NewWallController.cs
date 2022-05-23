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


    [HideLabel]
    [Title("Previous", TitleAlignment =TitleAlignments.Centered)]
    [HorizontalGroup("mesh")]
    [VerticalGroup("mesh/a")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    public Object previouspreviewField;

    [HideLabel]
    [Title("Current", TitleAlignment = TitleAlignments.Centered)]
    [HorizontalGroup("mesh")]
    [VerticalGroup("mesh/b")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    public Object currentpreviewField;

    [HideLabel]
    [Title("Next", TitleAlignment = TitleAlignments.Centered)]
    [HorizontalGroup("mesh")]
    [VerticalGroup("mesh/c")]
    [PreviewField(150, ObjectFieldAlignment.Center)]
    public Object nextpreviewField;








    [Title("@returnPrevoiusName()")]
    
    [VerticalGroup("mesh/a")]
    [Button(ButtonSizes.Large)]
    public void PreviousPattern()
    {

        if (currentData <= 0)
        {
            currentData = wallData.Count - 1;
            CreateData();
            ChangeProporties();
            return;
        }
        if (currentData > 0)
        {
            currentData--;
            CreateData();
            ChangeProporties();
            return;
        }
    }

    [Title("@returnNextName()")]
    [VerticalGroup("mesh/c")]
    [Button(ButtonSizes.Large)]
    public void NextPattern()
    {

        if (currentData == wallData.Count - 1)
        {
            currentData = 0;
            ChangeProporties();
            CreateData();
            return;
        }
        if (currentData < wallData.Count)
        {
            currentData++;
            ChangeProporties();
            CreateData();
            return;
        }
    }


  
    [HorizontalGroup("mesh/b/Rotate")]
    [Button(ButtonSizes.Large)]
    public void RotateLeft()
    {
        transform.eulerAngles += new Vector3(0, -90, 0);
    }


    [HorizontalGroup("mesh/b/Rotate")]
    [Button(ButtonSizes.Large)]
    public void RotateRight()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
    }


    [Button(ButtonSizes.Large)]

    public void CloneUpAndSelect()
    {
        Debug.Log(this.transform.rotation);
        var currentEluers = this.transform.rotation;
        GameObject clone = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/1. Level Creator/WallController.prefab", typeof(GameObject)), null) as GameObject;
        clone.transform.position = transform.position + (transform.up * 3f);
        clone.transform.rotation = currentEluers;
        clone.GetComponent<NewWallController>().currentData = currentData;
        clone.GetComponent<NewWallController>().ChangeProporties();
        clone.GetComponent<NewWallController>().CreateData();
        Selection.activeObject = clone;
    }



    [VerticalGroup("mesh/a")]
    [Button(ButtonSizes.Large)]

    public void CloneLeftAndSelect()
    {
        Debug.Log(this.transform.rotation);
        var currentEluers = this.transform.rotation;
        GameObject clone = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/1. Level Creator/WallController.prefab", typeof(GameObject)), null) as GameObject;
        clone.transform.position = transform.position + (transform.right * 2.5f);
        clone.transform.rotation = currentEluers;
        clone.GetComponent<NewWallController>().currentData = currentData;
        clone.GetComponent<NewWallController>().ChangeProporties();
        clone.GetComponent<NewWallController>().CreateData();
        Selection.activeObject = clone;
    }




    [VerticalGroup("mesh/c")]
    [Button(ButtonSizes.Large)]

    public void CloneRightAndSelect()
    {
        Debug.Log(this.transform.rotation);
        var currentEluers = this.transform.rotation;
        GameObject clone = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/1. Level Creator/WallController.prefab", typeof(GameObject)), null) as GameObject;
        clone.transform.position = transform.position + (-transform.right * 2.5f);
        clone.transform.rotation = currentEluers;
        clone.GetComponent<NewWallController>().currentData = currentData;
        clone.GetComponent<NewWallController>().ChangeProporties();
        clone.GetComponent<NewWallController>().CreateData();
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


    public string returnPrevoiusName()
    {
        if (currentData - 1 < 0)
            return wallData[currentData].name;
        else
            return wallData[currentData - 1].name;
    }
    public string returnNextName()
    {
        if (currentData == wallData.Count - 1)
        {
            return wallData[currentData].name;
        }
        else
        {
            return wallData[currentData + 1].name;
        }
    }

    public int returnPrevoiusNumber()
    {
        if (currentData - 1 < 0)
        {
            return wallData.Count - 1;
        }
        else
        {
            return currentData - 1;
        }

    }
    public int returnNextNumber()
    {
        if (currentData == wallData.Count - 1)
        {
            return 0;
        }
        else
        {
            return currentData + 1;
        }
    }

    private void CreateData()
    {


        GameObject previousmyGameObject = new GameObject();
        previousmyGameObject.name = "Obvious Name";

        MeshFilter meshFilter = previousmyGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = wallData[returnPrevoiusNumber()].mesh;
        MeshRenderer meshRenderer = previousmyGameObject.AddComponent<MeshRenderer>();


        Material[] material = new Material[wallData[returnPrevoiusNumber()].materials.Count];
        for (int i = 0; i < wallData[returnPrevoiusNumber()].materials.Count; i++)
        {
            material[i] = wallData[returnPrevoiusNumber()].materials[i];
        }
        meshRenderer.materials = material;

        previousmyGameObject.hideFlags = HideFlags.HideInHierarchy;




        GameObject nextGameObject = new GameObject();
        nextGameObject.name = "Obvious Name";

        MeshFilter meshFilter2 = nextGameObject.AddComponent<MeshFilter>();
        meshFilter2.mesh = wallData[returnNextNumber()].mesh;
        MeshRenderer meshRenderer2 = nextGameObject.AddComponent<MeshRenderer>();


        Material[] material2 = new Material[wallData[returnNextNumber()].materials.Count];
        for (int i = 0; i < wallData[returnNextNumber()].materials.Count; i++)
        {
            material2[i] = wallData[returnNextNumber()].materials[i];
        }
        meshRenderer2.materials = material2;

        nextGameObject.hideFlags = HideFlags.HideInHierarchy;

        previouspreviewField = previousmyGameObject;
        nextpreviewField = nextGameObject;


        GameObject currentGameObject = new GameObject();
        nextGameObject.name = "Obvious Name";

        MeshFilter meshFilter3= currentGameObject.AddComponent<MeshFilter>();
        meshFilter3.mesh = wallData[currentData].mesh;
        MeshRenderer meshRenderer3 = currentGameObject.AddComponent<MeshRenderer>();


        Material[] material3 = new Material[wallData[currentData].materials.Count];
        for (int i = 0; i < wallData[currentData].materials.Count; i++)
        {
            material3[i] = wallData[currentData].materials[i];
        }
        meshRenderer3.materials = material3;

        currentGameObject.hideFlags = HideFlags.HideInHierarchy;

        previouspreviewField = previousmyGameObject;
        nextpreviewField = nextGameObject;
        currentpreviewField = currentGameObject;

    }
}
