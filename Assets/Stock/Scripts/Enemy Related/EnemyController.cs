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
       
    }
    private void UpdatePlayerPos()
    {
        var sequence = DOTween.Sequence();

        if (endPoint!=null)
        {
            sequence.AppendCallback(() => navMeshAgent.SetDestination(endPoint.transform.position));
            sequence.AppendInterval(1);
        }
        sequence.AppendCallback(()=>UpdatePlayerPos());
  
    }
    public void EnableNavMesh()
    {
        StartCoroutine(EnableNavmeshDelay());
    }
    IEnumerator EnableNavmeshDelay()
    {
        yield return new WaitForSeconds(1);

        navMeshAgent.enabled = true;
        yield return new WaitForSeconds(0.1f);
        UpdatePlayerPos();
    }

}
