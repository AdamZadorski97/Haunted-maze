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



    [MenuItem("EditorTools/OpenTools")]
    private static void OpenWindow()
    {
        GetWindow<EditorHelper>().Show();
    }

    [PropertySpace]
    [Title("Rotation")]
    [HorizontalGroup("ForwardRotation", 80f, marginLeft: 40)]
    [Button("Front", ButtonStyle.Box)]
    private void TurnFront()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(-90, 0, 0);
    }
    [PropertySpace]
    [HorizontalGroup("SideRotation", 80f)]
    [Button("Left", ButtonStyle.Box)]
    private void TurnLeft()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0, -90, 0);
    }
    [PropertySpace]
    [HorizontalGroup("SideRotation", 80f)]
    [Button("Right", ButtonStyle.Box)]
    private void TurnRight()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0, 90, 0);
    }
    [PropertySpace]
    [HorizontalGroup("BackRotation", 80f, marginLeft: 40)]
    [Button("Back", ButtonStyle.Box)]
    private void TurnBack()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(90, 0, 0);
    }

    [Title("Maze spawner")]
    [VerticalGroup("Maze spawner")]
    [Button("Spawn floor")]
    private void SpawnFloorGrid(Vector2 gridSize, GameObject floorPrefab)
    {
        GameObject floorParrent = new GameObject("Floor Parrent");

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {

                GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(floorPrefab, floorParrent.transform) as GameObject;
                instantiadtedGrid.transform.position = new Vector3(x * 2.5f, 0, y * 2.5f);
            }
        }
    }
    [VerticalGroup("Maze spawner")]
    [Button("Spawn Wall")]
    private void SpawnWall(float wallLenght, GameObject wallPrefab)
    {
        GameObject wallParrent = new GameObject("Wall Parrent");

        for (int i = 0; i < wallLenght; i++)
        {
            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(wallPrefab, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3(i * 2.5f, 0, 0);
        }
    }
    [VerticalGroup("Maze spawner")]
    [Button("Spawn Room")]
    private void SpawnRoom(
        Vector2 gridSize,
        bool isCorridor,
        bool useWindows,
        GameObject wallPrefab,
         GameObject windowPrefab,
        GameObject floorPrefab,
        GameObject ceilingPrefab,
      [Title("Celling")][HorizontalGroup("Maze spawner")][PreviewField(70, ObjectFieldAlignment.Left)] Material ceilingMatTop,
       [PreviewField(70, ObjectFieldAlignment.Left)] Material ceilingMatBottom,
      [Title("Wall")][HorizontalGroup("Maze spawner")][PreviewField(70, ObjectFieldAlignment.Left)] Material insideMaterial,
       [PreviewField(70, ObjectFieldAlignment.Left)] Material outSideMaterial,
      [Title("Floor")][HorizontalGroup("Maze spawner")][PreviewField(70, ObjectFieldAlignment.Left)] Material floorMatTop,
       [PreviewField(70, ObjectFieldAlignment.Left)] Material floorMatBottom)
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
            SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), insideMaterial, outSideMaterial);
        }

        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject tempWall = null;
            if (i % 2 == 0 &&  useWindows)
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
                instantiadtedGrid.transform.position = new Vector3(-2.5f + (i * 2.5f), 0, gridSize.y * 2.5f);
                instantiadtedGrid.transform.eulerAngles = new Vector3(0, 180, 0);
                SetWallMaterials(instantiadtedGrid.GetComponent<WallController>(), outSideMaterial, insideMaterial);
            }
        }
    }


    private void SetCeilingMaterial(CeilingController ceilingController, Material ceilingMatBottom, Material ceilingMatTop)
    {
        ceilingController.ceilingMatBottom = ceilingMatBottom;
        ceilingController.ceilingMatTop = ceilingMatTop;
        ceilingController.ChangeFloorMaterial();
    }


    private void SetFloorMaterial(FloorController floorController, Material floorMatBottom, Material floorMatTop)
    {
        floorController.FloorMatBottom = floorMatBottom;
        floorController.FloorMatTop = floorMatTop;
        floorController.ChangeFloorMaterial();
    }

    private void SetWallMaterials(WallController wallController, Material insideMaterial, Material outSideMaterial)
    {
        wallController.WallMat0 = insideMaterial;
        wallController.WallMat1 = outSideMaterial;
        wallController.ChangeWallMaterial();
    }

    [TitleGroup("Ceiling")]
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

    [TitleGroup("Walls")]
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
    [TitleGroup("Walls")]
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

    [TitleGroup("Floor")]
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

    [TitleGroup("Floor")]
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

    [TitleGroup("Interactive Items")]
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

    [TitleGroup("Player")]
    [Button("Setup Player")]
    private void SetupPlayer()
    {
        PlayerController[] components = GameObject.FindObjectsOfType<PlayerController>();
        foreach (var item in components)
        {
            item.SnapToGround();
        }
    }

    [TitleGroup("Player")]
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