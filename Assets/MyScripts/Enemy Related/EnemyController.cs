using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
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
    public EnemyProporties enemyProporties;
    private float currentHealth;

    public List<GameObject> enemiesSkins = new List<GameObject>();
    [SerializeField] private bool runOnStart = true;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image hpBar;
    public void Start()
    {
        if (runOnStart)
        {
            navMeshAgent.speed = enemyProporties.speed;
            currentHealth = enemyProporties.hp;
            hpBar.fillAmount = 1;
            if (Mathf.Round(transform.position.y) != 3 * LevelManager.Instance.currentPlayerFloor)
                gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SetSkin();
        StartCoroutine(EnableNavmeshDelay());
    }

    public void SetSkin()
    {
        enemiesSkins[Random.Range(0, enemiesSkins.Count)].SetActive(true);
    }

    private void UpdatePlayerPos()
    {
        if (isDead)
            return;
        var sequence = DOTween.Sequence();

        if (endPoint != null)
        {
            sequence.AppendCallback(() => navMeshAgent.SetDestination(endPoint.transform.position));
            sequence.AppendInterval(1);
        }
        sequence.AppendCallback(() => UpdatePlayerPos());
    }
    public void EnableNavMesh()
    {
        StartCoroutine(EnableNavmeshDelay());
    }
    public void OnHit(float hitValue)
    {
        canvas.SetActive(true);
        currentHealth -= hitValue;



        if (currentHealth <= 0)
        {
            canvas.SetActive(false);
            OnDie();
            return;
        }
        animator.SetTrigger("Hit");

        Sequence hitSequence = DOTween.Sequence();
        hitSequence.AppendCallback(() => navMeshAgent.speed = 0);
        hitSequence.AppendInterval(2f);
        hitSequence.AppendCallback(() => navMeshAgent.speed = enemyProporties.speed);
        hpBar.DOFillAmount(GetHealthPercent(), 0.25f);
    }

    public float GetHealthPercent()
    {
        return currentHealth / enemyProporties.hp;
    }

    public void OnDie()
    {

        LevelManager.Instance.dataManager.CurrentKilledUnits++;
        if(enemySpawnerController!=null)
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
