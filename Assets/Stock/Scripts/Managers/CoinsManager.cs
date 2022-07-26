using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private List<FloorController> pickablePointsGameObjects = new List<FloorController>();
    private Vector3 rotation;
    private void Awake()
    {
        var foundCanvasObjects = FindObjectsOfType<FloorController>();

        foreach (FloorController s in foundCanvasObjects)
        {
            pickablePointsGameObjects.Add(s);
        }
    }

    private void FixedUpdate()
    {
        rotation += new Vector3(0.0f, 5.0f, 0.0f);
        foreach (FloorController item in pickablePointsGameObjects)
        {
            if(item.interactivePoint.activeSelf)
            {
                item.interactivePoint.transform.localEulerAngles = rotation;
            }
        }
    }
}
