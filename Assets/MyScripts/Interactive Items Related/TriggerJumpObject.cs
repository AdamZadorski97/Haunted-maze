using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJumpObject : MonoBehaviour
{
    public UnityEngine.AI.NavMeshObstacle navMeshObstacle;
    public bool wasTriggered;

    [SerializeField] private float resetTime;

    public BoxCollider boxCollider;

    public void Update()
    {
        if(PlayerController.Instance.isInJumpState)
        {
            boxCollider.enabled = false;
            navMeshObstacle.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
            navMeshObstacle.enabled = true;
        }
    }







    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<PlayerController>())
    //    {
    //        LevelManager.Instance.uIManager.HighlitghtAction(LevelManager.Instance.uIManager.imageJumpTimer, true);
    //        other.GetComponent<PlayerController>().canRun = false;
    //    }
    //    if (other.GetComponent<EnemyController>())
    //    {
    //        other.GetComponent<EnemyController>().navMeshAgent.enabled = false;
       
    //    }
    //}


    //private void OnTriggerStay(Collider other)
    //{

    //    if (other.GetComponent<PlayerController>())
    //    {
    //        if (other.GetComponent<PlayerController>().moveBack)
    //        {
    //            wasTriggered = true;
    //            other.GetComponent<PlayerController>().moveSpeed = other.GetComponent<PlayerController>().defaultMoveSpeed;
    //            StopCoroutine(RestState());
    //            StartCoroutine(RestState());
    //            return;
    //        }

    //        else if (!other.GetComponent<PlayerController>().isInJumpState && !wasTriggered)
    //        {
    //            wasTriggered = true;
    //            other.GetComponent<PlayerController>().OnJumpObstacleHit(navMeshObstacle);
    //            return;
    //        }

    //        else
    //        {
    //            wasTriggered = true;
    //            StopCoroutine(RestState());
    //            StartCoroutine(RestState());
    //            navMeshObstacle.enabled = false;
    //        }

    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    
    //    if (other.GetComponent<PlayerController>())
    //    {
    //        navMeshObstacle.enabled = true;
    //        LevelManager.Instance.uIManager.HighlitghtAction(LevelManager.Instance.uIManager.imageJumpTimer, false);
    //        other.GetComponent<PlayerController>().canRun = true;

    //    }


    //    if (other.GetComponent<EnemyController>())
    //    {
    //        other.GetComponent<EnemyController>().navMeshAgent.enabled = true;
    //    }
    //}

    //IEnumerator RestState()
    //{
    //    yield return new WaitForSeconds(resetTime);
    //    wasTriggered = false;
    //}
}