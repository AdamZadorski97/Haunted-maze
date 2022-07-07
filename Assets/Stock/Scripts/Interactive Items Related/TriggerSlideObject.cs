using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TriggerSlideObject : MonoBehaviour
{
    public NavMeshObstacle navMeshObstacle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<PlayerController>().isInSlideState)
            {
                navMeshObstacle.enabled = false;
                other.GetComponent<PlayerController>().OnSlideObstacleHit(navMeshObstacle);
            }

            else
            {
                navMeshObstacle.enabled = false;
                Invoke("EnableNavmesh", 1);
            }

        }
    }

    private void EnableNavmesh()
    {
        navMeshObstacle.enabled = true;
    }
}