using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;

using Unity.EditorCoroutines.Editor;
[SelectionBase]
[ExecuteInEditMode]
#endif
public class WallController : MonoBehaviour
{
    [SerializeField] private bool useBoxCollider;
    [SerializeField] private bool useMapVisualize;
    [HideInInspector] public bool showPath = false;
    public GameObject wallController;
    public List<GameObject> mapLines = new List<GameObject>();
    public List<BoxCollider> boxColliders = new List<BoxCollider>();

    [OnValueChanged("ChangeProporties")]
    public Vector2Int materialOffset = new Vector2Int(0, 0);
    [OnValueChanged("ChangeProporties")]
    public Vector2Int materialTiling = new Vector2Int(0, 0);

    [TableList(ShowIndexLabels = true)]
    public List<WallData> wallData = new List<WallData>();

    [OnValueChanged("ChangeProporties")]
    public int currentData = 0;


    [HideLabel]
    [Title("Previous", TitleAlignment = TitleAlignments.Centered)]
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





#if UNITY_EDITOR
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
        clone.GetComponent<WallController>().currentData = currentData;
        clone.GetComponent<WallController>().ChangeProporties();
        clone.GetComponent<WallController>().CreateData();
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
        clone.GetComponent<WallController>().currentData = currentData;
        clone.GetComponent<WallController>().ChangeProporties();
        clone.GetComponent<WallController>().CreateData();
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
        clone.GetComponent<WallController>().currentData = currentData;
        clone.GetComponent<WallController>().ChangeProporties();
        clone.GetComponent<WallController>().CreateData();
        Selection.activeObject = clone;
    }

    private void Start()
    {
        ChangeProporties();
        StartCoroutine(TurnOffMapVizualize());
    }
    IEnumerator TurnOffMapVizualize()
    {
        yield return new WaitForEndOfFrame();
        if (transform.position.y != 0)
        {
            foreach (GameObject mapLine in mapLines)
            {
                mapLine.SetActive(false);
            }
        }
    }

    public void ChangeProporties()
    {

        //gameObject.name = $"WallController {wallData[currentData].name}";
        var children = new List<GameObject>();
        boxColliders.Clear();
        mapLines.Clear();
        foreach (Transform child in transform)
        {
            if (child.name != "UnDestroyable")
                children.Add(child.gameObject);
        }

        children.ForEach(child =>
        {
            bool isPrefabInstance = PrefabUtility.GetPrefabParent(child) != null && PrefabUtility.GetPrefabObject(child.transform) != null;
            if (!isPrefabInstance)
                DestroyImmediate(child);
        });
        InstantiateWallWithData(wallData[currentData]);
    }



    private void InstantiateWallWithData(WallData wallData)
    {
        GameObject meshRendererObject = new GameObject();
        meshRendererObject.transform.SetParent(transform);
        meshRendererObject.transform.localPosition = Vector3.zero;
        meshRendererObject.name = "MeshRenderer";

        //MeshFilter
        MeshFilter meshFilter = meshRendererObject.AddComponent<MeshFilter>();
        meshFilter.mesh = wallData.mesh;

        //MeshRenderer
        MeshRenderer meshRenderer = meshRendererObject.AddComponent<MeshRenderer>();
        var currentEluers = this.transform.rotation;
        meshFilter.transform.rotation = currentEluers;
        var flags = StaticEditorFlags.ContributeGI | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.BatchingStatic | StaticEditorFlags.BatchingStatic | StaticEditorFlags.ReflectionProbeStatic;
        GameObjectUtility.SetStaticEditorFlags(meshFilter.gameObject, flags);
        //BoxColliders

        if (useBoxCollider)
        {
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
                boxColliders.Add(boxCollider);
            }
        }
        //Material
        Material[] material = new Material[wallData.materials.Count];
        for (int i = 0; i < wallData.materials.Count; i++)
        {
            material[i] = wallData.materials[i];

        }



        meshRenderer.sharedMaterials = material;
        meshRenderer.material.SetTextureOffset("_BaseMap", ((Vector2)materialOffset * 0.2f));
        meshRenderer.material.mainTextureScale = materialTiling;



        if (useMapVisualize)
        {
            for (int i = 0; i < wallData.meshMapPosition.Count; i++)
            {
                GameObject boxcolliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                boxcolliderObject.gameObject.layer = LayerMask.NameToLayer("Point");
                boxcolliderObject.transform.SetParent(transform);
                boxcolliderObject.transform.localPosition = wallData.meshMapPosition[i];
                boxcolliderObject.transform.localScale = wallData.meshMapScale[i];
                boxcolliderObject.transform.localRotation = Quaternion.Euler(wallData.meshMapRotation[i]);
                boxcolliderObject.GetComponent<MeshRenderer>().material = wallData.meshMapMaterial;

                mapLines.Add(boxcolliderObject);
            }
        }

        //    instantiatedVizualize = Instantiate(prefabVizualize, transform);
        //instantiatedVizualize.transform.rotation = transform.rotation;
        //instantiatedVizualize.transform.localPosition = new Vector3(-1.25f, 3, 0);
        EditorUtility.SetDirty(transform.gameObject);
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

        //previousmyGameObject.hideFlags = HideFlags.HideInHierarchy;




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

        MeshFilter meshFilter3 = currentGameObject.AddComponent<MeshFilter>();
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


    private void OnDrawGizmos()
    {
        if (showPath)
        {
            for (int i = 0; i < boxColliders.Count; i++)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(Vector3.zero + boxColliders[i].center, boxColliders[i].size);
            }
        }
    }
#endif
}

