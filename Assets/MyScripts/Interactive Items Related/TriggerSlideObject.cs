using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TriggerSlideObject : MonoBehaviour
{
    [SerializeField] private float resetTime;
    public UnityEngine.AI.NavMeshObstacle navMeshObstacle;
    public BoxCollider boxCollider;

    public void Update()
    {
        if (PlayerController.Instance.isInSlideState)
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
}