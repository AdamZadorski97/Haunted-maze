using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform endPoint;
    public Animator animator;
    public CapsuleCollider capsuleCollider;
    public GameObject mapVisualize;
    public EnemySpawnerController enemySpawnerController;
    public bool isDead;
    public Transform head;
    public void Start()
    {
        if (Mathf.Round(transform.position.y) != 3 * LevelManager.Instance.currentPlayerFloor)
            gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        StartCoroutine(EnableNavmeshDelay());
    }


    private void UpdatePlayerPos()
    {
        if (isDead)
            return;
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
    public void OnDie()
    {
        enemySpawnerController.spawnedEnemies.Remove(this);
        isDead = true;
        animator.SetTrigger("Die");
        capsuleCollider.enabled = false;
        mapVisualize.SetActive(false);
        navMeshAgent.enabled = false;
        Destroy(this.gameObject, 2f);
    }




    IEnumerator EnableNavmeshDelay()
    {
        yield return new WaitForSeconds(1);

        navMeshAgent.enabled = true;
        yield return new WaitForSeconds(0.1f);
        UpdatePlayerPos();
    }

}
