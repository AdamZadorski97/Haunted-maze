using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJumpObject : MonoBehaviour
{
    public UnityEngine.AI.NavMeshObstacle navMeshObstacle;
    [SerializeField]private bool isInTrigger;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            LevelManager.Instance.uIManager.HighlitghtAction(LevelManager.Instance.uIManager.imageJumpTimer, true);
        }
    }


    private void OnTriggerStay(Collider other)
    {
      
        if (other.GetComponent<PlayerController>())
        {
            Debug.Log("Player");
            if (!other.GetComponent<PlayerController>().isInJumpState)
            {
                if (!isInTrigger)
                {
                    isInTrigger = true;
                    other.GetComponent<PlayerController>().OnJumpObstacleHit(navMeshObstacle);
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
      //  isInTrigger = false;
        if (other.GetComponent<PlayerController>())
        {
            navMeshObstacle.enabled = true;
            LevelManager.Instance.uIManager.HighlitghtAction(LevelManager.Instance.uIManager.imageJumpTimer, false);
        }
    }
}