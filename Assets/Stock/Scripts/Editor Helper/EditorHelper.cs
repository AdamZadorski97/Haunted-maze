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
    public GameObject floorPrefab;
    public Vector2 gridSize;

    [MenuItem("EditorTools/OpenTools")]
    private static void OpenWindow()
    {
        GetWindow<EditorHelper>().Show();
    }

    [Button("Spawn Grid")]
    private void SpawnFloorGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject instantiadtedGrid = Instantiate(floorPrefab);
                instantiadtedGrid.transform.position = new Vector3(x * 2.5f, 0, y * 2.5f);
            }
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
    }
#endif