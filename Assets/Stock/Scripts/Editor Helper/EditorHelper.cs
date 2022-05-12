#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using DG.Tweening;
using UnityEngine.AI;

public class EditorHelper : OdinEditorWindow
{
    private bool showPath;
    private bool showCeiling;
    private bool showWalls;
    private bool showFloor;
    private bool showiInteractivePoints;



    [TabGroup("Tabs", "Room Builder")] public Vector2Int gridSize = new Vector2Int(5, 5);
    [TabGroup("Tabs", "Room Builder")] public bool isCorridor;
    [TabGroup("Tabs", "Room Builder")] public bool useWindows;
    [TabGroup("Tabs", "Room Builder")] public GameObject wallPrefab;
    [TabGroup("Tabs", "Room Builder")] public GameObject windowPrefab;
    [TabGroup("Tabs", "Room Builder")] public GameObject floorPrefab;
    [TabGroup("Tabs", "Room Builder")] public GameObject ceilingPrefab;

 
   
    [TabGroup("Tabs", "Room Builder")]  [PreviewField(70, ObjectFieldAlignment.Left)] public Material ceilingMatBottom;
    [TabGroup("Tabs", "Room Builder")] public bool isDoubleMaterialCelling = false;
    [TabGroup("Tabs", "Room Builder")] [ShowIf("isDoubleMaterialCelling")] [Title("Celling")] [PreviewField(70, ObjectFieldAlignment.Left)] public Material ceilingMatTop;

    [TabGroup("Tabs", "Room Builder")] [Title("Wall")] [PreviewField(70, ObjectFieldAlignment.Left)] public Material insideMaterial;
    [TabGroup("Tabs", "Room Builder")] public bool isDoubleMaterialWall = false;
    [TabGroup("Tabs", "Room Builder")] [ShowIf("isDoubleMaterialWall")] [PreviewField(70, ObjectFieldAlignment.Left)] public Material outSideMaterial;


    [TabGroup("Tabs", "Room Builder")] [Title("Floor")] [PreviewField(70, ObjectFieldAlignment.Left)] public Material floorMatTop;
    [TabGroup("Tabs", "Room Builder")] public bool isDoubleMaterialFloor = false;
    [TabGroup("Tabs", "Room Builder")] [ShowIf("isDoubleMaterialFloor")] [PreviewField(70, ObjectFieldAlignment.Left)] public Material floorMatBottom;

    [MenuItem("EditorTools/OpenTools")]
    private static void OpenWindow()
    {
        GetWindow<EditorHelper>().Show();
    }



    [Button]
    [TabGroup("Tabs", "Transform")]
    [BoxGroup("Tabs/Transform/Rotation")]

    [HorizontalGroup("Tabs/Transform/Rotation/x")]
    private void TurnFront()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(-rotationDegrees, 0, 0);
    }
    [BoxGroup("Tabs/Transform/Rotation")]
    public float rotationDegrees = 90;

    [Button]
    [HorizontalGroup("Tabs/Transform/Rotation/b")]

    private void TurnLeft()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0, -rotationDegrees, 0);
    }


    [Button]
    [HorizontalGroup("Tabs/Transform/Rotation/b")]
    private void TurnRight()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0, rotationDegrees, 0);
    }


    [Button]
    [HorizontalGroup("Tabs/Transform/Rotation/c")]

    private void TurnBack()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(rotationDegrees, 0, 0);
    }

    [BoxGroup("Tabs/Transform/Snap")]

    [HorizontalGroup("Tabs/Transform/Snap/d")]
    public LayerMask floorLayermask;

    [Button]
    [HorizontalGroup("Tabs/Transform/Snap/g")]
    private void SnapToGround()
    {
        RaycastHit groundHit;
        Transform objectToSnap = Selection.activeTransform.transform;
        if (Physics.Raycast(objectToSnap.position, Vector3.down, out groundHit, Mathf.Infinity, floorLayermask))
        {
            objectToSnap.position = groundHit.point;
        }
        else
        {
            Debug.Log("No floor detected");
        }
    }
    [Button]
    [HorizontalGroup("Tabs/Transform/Snap/h")]
    private void SnapToCeiling()
    {
        RaycastHit groundHit;
        Transform objectToSnap = Selection.activeTransform.transform;
        if (Physics.Raycast(objectToSnap.position, Vector3.up, out groundHit, Mathf.Infinity, floorLayermask))
        {
            objectToSnap.position = groundHit.point;
        }
        else
        {
            Debug.Log("No Ceiling detected");
        }
    }

    [TabGroup("Tabs", "Room Builder")]
    [Button("SetupWithDefaults")]
    private void SetupWithDefault()
    {
        wallPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Stock/Prefabs/Building/Wall.prefab", typeof(GameObject));
        floorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Stock/Prefabs/Building/Floor.prefab", typeof(GameObject));
        ceilingPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Stock/Prefabs/Building/Ceiling.prefab", typeof(GameObject));
        windowPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Stock/Prefabs/Building/Window.prefab", typeof(GameObject));

        floorMatTop = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Wood_02.mat", typeof(Material));
        floorMatBottom = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Wood_Fancy_01.mat", typeof(Material));

        insideMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Horror_Wall_03.mat", typeof(Material));
        outSideMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Weatherboard_01.mat", typeof(Material));

        ceilingMatTop = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Wood_01.mat", typeof(Material));
        ceilingMatBottom = (Material)AssetDatabase.LoadAssetAtPath("Assets/ThirdParty Assets/PolygonHorrorMansion/Materials/Building/Wood_Fancy_01.mat", typeof(Material));


    }





    [TabGroup("Tabs", "Room Builder")]
    [Button("Spawn Room")]
    private void SpawnRoom()
    {
        GameObject room = new GameObject("Room Parrent");


        GameObject floorParrent = new GameObject("Floor Parrent");
        floorParrent.transform.SetParent(room.transform);
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(floorPrefab, floorParrent.transform) as GameObject;
                instantiadtedGrid.transform.position = new Vector3(x * 2.5f, 0, y * 2.5f);
                if (floorMatBottom != null || floorMatTop != null)
                    SetFloorMaterial(instantiadtedGrid.GetComponent<FloorController>(), floorMatBottom, floorMatTop);
            }
        }

        GameObject ceilingParrent = new GameObject("Ceiling Parrent");
        ceilingParrent.transform.SetParent(room.transform);
        for (int x = 0; x < gridSize.x; x++)
        {

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(ceilingPrefab, ceilingParrent.transform) as GameObject;
                instantiadtedGrid.transform.position = new Vector3(x * 2.5f, 3f, y * 2.5f);
                if (ceilingMatBottom != null || ceilingMatTop != null)
                    SetCeilingMaterial(instantiadtedGrid.GetComponent<CeilingController>(), ceilingMatBottom, ceilingMatTop);
            }
        }

        GameObject wallParrent = new GameObject("Wall Parrent");
        wallParrent.transform.SetParent(room.transform);
        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject tempWall = null;
            if (i % 2 == 0 && useWindows)
            {
                tempWall = windowPrefab;
            }
            else
            {
                tempWall = wallPrefab;
            }

            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(tempWall, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3(-2.5f, 0, 2.5f + i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, -90, 0);
            if (insideMaterial != null || outSideMaterial != null)
                SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), insideMaterial, outSideMaterial);
        }

        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject tempWall = null;
            if (i % 2 == 0 && useWindows)
            {
                tempWall = windowPrefab;
            }
            else
            {
                tempWall = wallPrefab;
            }

            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(tempWall, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3((gridSize.x * 2.5f) - 2.5f, 0, i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
            if (insideMaterial != null || outSideMaterial != null)
                SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), insideMaterial, outSideMaterial);
        }

        if (!isCorridor)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                GameObject tempWall = null;
                if (i % 2 == 0 && useWindows)
                {
                    tempWall = windowPrefab;
                }
                else
                {
                    tempWall = wallPrefab;
                }
                GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(tempWall, wallParrent.transform) as GameObject;
                instantiadtedGrid.transform.position = new Vector3((-2.5f + i * 2.5f), 0, 0);
                instantiadtedGrid.transform.eulerAngles = new Vector3(0, 180, 0);
                if (insideMaterial != null || outSideMaterial != null)
                    SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), insideMaterial, outSideMaterial);
            }

            for (int i = 0; i < gridSize.x; i++)
            {
                GameObject tempWall = null;
                if (i % 2 == 0 && useWindows)
                {
                    tempWall = windowPrefab;
                }
                else
                {
                    tempWall = wallPrefab;
                }
                GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(tempWall, wallParrent.transform) as GameObject;
                instantiadtedGrid.transform.position = new Vector3((i * 2.5f), 0, gridSize.y * 2.5f);
                instantiadtedGrid.transform.eulerAngles = new Vector3(0, 0, 0);
                if (outSideMaterial != null || insideMaterial != null)
                    SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), insideMaterial, outSideMaterial);
            }
        }
    }


    private void SetCeilingMaterial(CeilingController ceilingController, Material ceilingMatBottom, Material ceilingMatTop)
    {
        ceilingController.ceilingMatBottom = ceilingMatBottom;
        if(isDoubleMaterialCelling)
        ceilingController.ceilingMatTop = ceilingMatTop;
        ceilingController.ChangeFloorMaterial();
    }


    private void SetFloorMaterial(FloorController floorController, Material floorMatBottom, Material floorMatTop)
    {
        if (isDoubleMaterialCelling)
            floorController.FloorMatBottom = floorMatBottom;
        floorController.FloorMatTop = floorMatTop;
        floorController.ChangeFloorMaterial();
    }

    private void SetWallMaterials(WallController wallController, Material insideMaterial, Material outSideMaterial)
    {
        wallController.WallMat0 = insideMaterial;
        if(isDoubleMaterialWall)
        wallController.WallMat1 = outSideMaterial;
        wallController.ChangeWallMaterial();
    }

    [TabGroup("Tabs", "Visibility")]
    [Button("Switch Ceiling Mesh Renderer")]
    private void SwitchCeilingMeshRenderer()
    {
        if (showCeiling) showCeiling = false; else showCeiling = true;

        CeilingController[] components = GameObject.FindObjectsOfType<CeilingController>();
        foreach (var item in components)
        {
            item.SwitchMeshRenderer(showCeiling);
        }
    }

    [TabGroup("Tabs", "Visibility")]
    [Button("Show Wall Mesh Renderer")]
    private void SwitchWallMeshRenderer()
    {
        if (showWalls) showWalls = false; else showWalls = true;

        WallController[] components = GameObject.FindObjectsOfType<WallController>();
        foreach (var item in components)
        {
            if (item.isWall)
                item.SwitchMeshRenderer(showWalls);
        }
    }
    [TabGroup("Tabs", "Visibility")]
    [Button("Show Wall Colliders")]
    private void ShowMazeColliders()
    {
        if (showPath) showPath = false; else showPath = true;

        WallController[] components = GameObject.FindObjectsOfType<WallController>();
        foreach (var item in components)
        {
            item.showPath = showPath;
        }
    }
    [TabGroup("Tabs", "Visibility")]
    [Button("Show Floor Mesh Renderer")]
    private void ShowFloorMeshRenderer()
    {
        if (showFloor) showFloor = false; else showFloor = true;

        FloorController[] components = GameObject.FindObjectsOfType<FloorController>();
        foreach (var item in components)
        {
            item.SwitchMeshRenderer(showFloor);
        }
    }

    [TabGroup("Tabs", "Room Builder")]
    [Button("Bake Navmesh")]
    private void BakeNavmesh()
    {

        NavMeshSurface[] components = GameObject.FindObjectsOfType<NavMeshSurface>();
        foreach (var item in components)
        {
            item.BuildNavMesh();

        }
        Selection.objects = components;
    }
    [TabGroup("Tabs", "Visibility")]
    [Button("Show Points")]
    private void ShowInteractiveItems()
    {
        if (showiInteractivePoints) showiInteractivePoints = false; else showiInteractivePoints = true;

        FloorController[] components = GameObject.FindObjectsOfType<FloorController>();
        foreach (var item in components)
        {
            item.SwitchPoint(showiInteractivePoints);
        }
    }
    [TabGroup("Tabs", "Player")]
    [Button("Setup Player")]
    private void SetupPlayer()
    {
        PlayerController[] components = GameObject.FindObjectsOfType<PlayerController>();
        foreach (var item in components)
        {
            item.SnapToGround();
        }
    }

    [TabGroup("Tabs", "Player")]
    [Button("Find Player")]

    private void FindPlayer()
    {

        var view = SceneView.lastActiveSceneView;
        if (view != null)
        {
            view.orthographic = false;
            var target = new GameObject();
            var player = GameObject.FindObjectOfType<PlayerController>().transform;
            target.transform.position = player.position + (player.forward * -3) + new Vector3(0, 5, 0);
            target.transform.rotation = player.rotation;
            target.transform.eulerAngles -= new Vector3(-45, 0, 0);
            Selection.activeObject = player.gameObject;
            view.AlignViewToObject(target.transform);
            GameObject.DestroyImmediate(target);
        }
    }


}
#endif