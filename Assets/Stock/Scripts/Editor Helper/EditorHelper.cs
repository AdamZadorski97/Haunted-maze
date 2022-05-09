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

public class EditorHelper : OdinEditorWindow
{
    private bool showPath;
    private bool showInteractiveItems;
    

 

    [MenuItem("EditorTools/OpenTools")]
    private static void OpenWindow()
    {
        GetWindow<EditorHelper>().Show();
    }

    [Button("Spawn Grid")]
    private void SpawnFloorGrid(Vector2 gridSize, GameObject floorPrefab)
    {
        GameObject floorParrent = new GameObject("Floor Parrent");
       
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject instantiadtedGrid = Instantiate(floorPrefab, floorParrent.transform);
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
                GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
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
                GameObject instantiadtedGrid = Instantiate(floorPrefab, floorParrent.transform);
                instantiadtedGrid.transform.position = new Vector3(x * 2.5f, 0, y * 2.5f);
            }
        }

        GameObject wallParrent = new GameObject("Wall Parrent");
        wallParrent.transform.SetParent(room.transform);
        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
            instantiadtedGrid.transform.position = new Vector3(-2.5f, 0, i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        for (int i = 0; i < gridSize.y; i++)
        {
            GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
            instantiadtedGrid.transform.position = new Vector3((gridSize.x * 2.5f) - 2.5f, 0, i * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        for (int i = 0; i < gridSize.x; i++)
        {
            GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
            instantiadtedGrid.transform.position = new Vector3((i * 2.5f), 0, 0);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < gridSize.x; i++)
        {
            GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
            instantiadtedGrid.transform.position = new Vector3((i * 2.5f), 0, gridSize.y * 2.5f);
            instantiadtedGrid.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //for (int i = 0; i < gridSize.y; i++)
        //{
        //    GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
        //    instantiadtedGrid.transform.position = new Vector3(0, 0, (i * 2.5f) + 2.5f);
        //    instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        //}

        //for (int i = 0; i < gridSize.y; i++)
        //{
        //    GameObject instantiadtedGrid = Instantiate(wallPrefab, wallParrent.transform);
        //    instantiadtedGrid.transform.position = new Vector3(1* gridSize.y, 0, (i * 2.5f) + 2.5f);
        //    instantiadtedGrid.transform.eulerAngles = new Vector3(0, 90, 0);
        //}


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
}
#endif