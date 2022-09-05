using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening;
using EPOOutline;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public Outlinable outlinable;
    public NavMeshAgent navMeshAgent;
    public Transform endPoint;
    public Animator animator;
    public CapsuleCollider capsuleCollider;
    public GameObject mapVisualize;
    public EnemySpawnerController enemySpawnerController;
    public bool isDead;
    public Transform head;
    public EnemyProporties enemyProporties;
    private double maxHP;
    private double currentHealth;
    public bool isBoss;
    [SerializeField] GameObject healthBar;
    [SerializeField] private TMP_Text currentLevel;

    public List<GameObject> enemiesSkins = new List<GameObject>();
    [SerializeField] private bool runOnStart = true;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image hpBar;

    [SerializeField] private ParticleSystem coinParticles;

    [SerializeField] private bool canThrow;
    [SerializeField] private GameObject throwPrefab;
    [SerializeField] private Transform handPosition;
    [SerializeField] private AnimationCurve throwEase;


    private float speed;
    public void Start()
    {
        if (runOnStart)
        {
            healthBar.SetActive(false);
            currentLevel.text = "LVL " + (LevelManager.Instance.dataManager.CurrentMultipler + 1).ToString();
            navMeshAgent.speed = enemyProporties.speed;
            hpBar.fillAmount = 1;
            if (canThrow)
                StartCoroutine(ThrowCoroutine());

        }
    }

    IEnumerator ThrowCoroutine()
    {
        yield return new WaitUntil(() => Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < enemyProporties.throwMaxDistance && CheckCanTrow());
       if(!isDead)
        Throw();
        yield return new WaitForSeconds(2);
        StartCoroutine(ThrowCoroutine());
    }

    private void Throw()
    {


        navMeshAgent.speed = 0;
        animator.SetTrigger("Throw");
        GameObject slimeball = Instantiate(throwPrefab);
        slimeball.transform.SetParent(handPosition);
        slimeball.transform.transform.position = handPosition.position;
        slimeball.transform.localScale = Vector3.zero;
        Sequence throwSequence = DOTween.Sequence();
        throwSequence.Append(slimeball.transform.DOScale(Vector3.one * 0.75f, 0.8f));
        throwSequence.AppendCallback(() => slimeball.transform.SetParent(null));

        throwSequence.Append(slimeball.transform.DOJump(GetOffsetPosition(), enemyProporties.throwCurvePower, 0, enemyProporties.throwSpeed).SetSpeedBased(true).SetEase(throwEase));
        throwSequence.AppendCallback(() => Destroy(slimeball));
        throwSequence.AppendInterval(0.5f);
        throwSequence.AppendCallback(() => navMeshAgent.speed = enemyProporties.speed);
    }

    private Vector3 GetOffsetPosition()
    {
        return PlayerController.Instance.cameraPivot.TransformPoint(new Vector3(0,0,1));
        
    }

    private bool CheckCanTrow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward * enemyProporties.throwMaxDistance, out hit, Mathf.Infinity))
        {
            if (hit.transform.GetComponent<PlayerController>())
                return true;

            if (hit.transform.GetComponent<PlayerEnemyTrigger>())
                return true;

            if (hit.transform.GetComponent<CoinPicker>())
                return true;

        }
        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), transform.forward * enemyProporties.throwMaxDistance, Color.red);
        return false;
    }

    private Vector3 lastPosition;

    void FixedUpdate()
    {
        Speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.1f);
        lastPosition = transform.position;
        animator.SetFloat("Speed", Speed);
    }

    public double MaxHealth
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public double CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }


    public float Speed
    {
        get { return speed; }
        set { speed = value; }
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
            if (navMeshAgent.enabled)
                sequence.AppendCallback(() => navMeshAgent.SetDestination(endPoint.transform.position));
            sequence.AppendInterval(1);
        }
        sequence.AppendCallback(() => UpdatePlayerPos());
    }
    public void EnableNavMesh()
    {
        StartCoroutine(EnableNavmeshDelay());
    }
    public void OnHit(double hitValue)
    {
        healthBar.SetActive(true);
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
        hpBar.DOFillAmount((float)GetHealthPercent(), 0.25f);
    }

    public double GetHealthPercent()
    {
        return currentHealth / MaxHealth;
    }

    public void OnDie()
    {
        outlinable.enabled = false;
        coinParticles.Play();
        LevelManager.Instance.dataManager.CurrentKilledUnits++;
        if (enemySpawnerController != null)
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
        yield return new WaitForSeconds(0.1f);

        navMeshAgent.enabled = true;
        yield return new WaitForSeconds(0.1f);
        UpdatePlayerPos();
    }

}
