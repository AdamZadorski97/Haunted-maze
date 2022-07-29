using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private List<Transform> pickablePointsGameObjects = new List<Transform>();
    private Vector3 rotation;
    private void Awake()
    {
        var foundCanvasObjects = FindObjectsOfType<FloorController>();

        foreach (FloorController s in foundCanvasObjects)
        {
            pickablePointsGameObjects.Add(s.transform.GetChild(0).transform);
        }
    }

    private void FixedUpdate()
    {
        rotation += new Vector3(0.0f, 5.0f, 0.0f);
        foreach (Transform item in pickablePointsGameObjects)
        {
            if(item.gameObject.activeSelf)
            {
                item.transform.localEulerAngles = rotation;
            }
        }
    }
}
