using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform endPoint;



    public void Start()
    {
        UpdatePlayerPos();
    }
    private void UpdatePlayerPos()
    {
        var sequence = DOTween.Sequence();
  
        sequence.AppendCallback(() => navMeshAgent.SetDestination(endPoint.transform.position));
        sequence.AppendInterval(1);
        sequence.AppendCallback(()=>UpdatePlayerPos());
  
    }

}
