using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJumpObject : MonoBehaviour
{
    public UnityEngine.AI.NavMeshObstacle navMeshObstacle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<PlayerController>().isInJumpState)
            {
                other.GetComponent<PlayerController>().OnJumpObstacleHit(navMeshObstacle);
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