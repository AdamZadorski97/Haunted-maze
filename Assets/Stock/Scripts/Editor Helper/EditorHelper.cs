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
    private bool showWalls;
    private bool showInteractiveItems;
    

 

    [MenuItem("EditorTools/OpenTools")]
    private static void OpenWindow()
    {
        GetWindow<EditorHelper>().Show();
    }

   [ButtonGroup("Rotate Object")] [Button("Left")]
    private void TurnLeft()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0,-90,0);
    }
    [ButtonGroup("Rotate Object")][Button("Right")]
    private void TurnRight()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(0, 90, 0);
    }
    [ButtonGroup("Rotate Object")] [Button("Front")]
    private void TurnFront()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(-90, 0, 0);
    }
    [ButtonGroup("Rotate Object")][Button("Back")]
    private void TurnBack()
    {
        Selection.activeTransform.transform.eulerAngles += new Vector3(90, 0, 0);
    }



    [Button("Spawn Grid")]
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

    [Button("Spawn Room")]
    private void SpawnRoom(Vector2 gridSize, GameObject wallPrefab, GameObject floorPrefab)
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
            }
        }

        GameObject wallParrent = new GameObject("Wall Parrent");
        wallParrent.transform.SetParent(room.transform);
        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(wallPrefab, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3(-2.5f, 0, i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(wallPrefab, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3((gridSize.x * 2.5f) - 2.5f, 0, i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        for (int i = 0; i < gridSize.x; i++)
        {
            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(wallPrefab, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3((i * 2.5f), 0, 0);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < gridSize.x; i++)
        {
            GameObject instantiadtedGrid = PrefabUtility.InstantiatePrefab(wallPrefab, wallParrent.transform) as GameObject;
            instantiadtedGrid.transform.position = new Vector3((i * 2.5f), 0, gridSize.y * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }






    [Button("Show Interactive Items")]
    private void ShowInteractiveItems()
    {
        if (showInteractiveItems) showInteractiveItems = false; else showInteractiveItems = true;

        PickablePoint[] components = GameObject.FindObjectsOfType<PickablePoint>();
        foreach (var item in components)
        {
            item.InteractivePointMesh.SetActive(showInteractiveItems);
        }
    }

    [Button("Show Maze Colliders")]
    private void ShowMazeColliders()
    {
        if (showPath) showPath = false; else showPath = true;

        DrawPathGizmos[] components = GameObject.FindObjectsOfType<DrawPathGizmos>();
        foreach (var item in components)
        {
            item.showPath = showPath;
        }
    }

    [Button("Switch Wall Mesh Renderer")]
    private void SwitchWallMeshRenderer()
    {
        if (showWalls) showWalls = false; else showWalls = true;

        DrawPathGizmos[] components = GameObject.FindObjectsOfType<DrawPathGizmos>();
        foreach (var item in components)
        {
            if (item.isWall)
                item.SwitchMeshRenderer(showWalls);
        }
    }

    [Button("Bake Navmesh")]
    private void BakeNavmesh()
    {
      
        NavMeshSurface[] components = GameObject.FindObjectsOfType<NavMeshSurface>();
        foreach (var item in components)
        {
            item.BuildNavMesh();
               
        }
    }
}
#endif