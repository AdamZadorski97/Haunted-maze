using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
    private Vector3 rotation;
    private void Awake()
    {
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));
    }

    private void Update()
    {
        rotation += new Vector3(0.0f, 5.0f, 0.0f);
        foreach (GameObject item in pickablePointsGameObjects)
        {
            if(item.activeSelf)
            {
                item.transform.localEulerAngles = rotation;
            }
        }
    }
}
