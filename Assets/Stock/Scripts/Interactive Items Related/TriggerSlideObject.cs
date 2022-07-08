using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TriggerSlideObject : MonoBehaviour
{
    public NavMeshObstacle navMeshObstacle;
    private bool isInTrigger;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!other.GetComponent<PlayerController>().isInSlideState)
            {
                if (!isInTrigger)
                {
                    isInTrigger = true;
                    other.GetComponent<PlayerController>().OnSlideObstacleHit(navMeshObstacle);
                }
            }

            else
            {
                navMeshObstacle.enabled = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            navMeshObstacle.enabled = true;
        }
    }

}