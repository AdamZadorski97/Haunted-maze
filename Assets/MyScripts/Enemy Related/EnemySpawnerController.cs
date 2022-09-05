using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    public List<EnemyController> enemiesPrefabs = new List<EnemyController>();

    public List<EnemyController> bossPrefabs = new List<EnemyController>();




    public FloorController[] floorControllers;
    public List<float> SpawningFrequency = new List<float>();


    public Transform enemiesParrent;
    public Transform middleOfMap;
    public float minDistanceFromPlayer;
    public float maxDistanceFromPlayer;
    public LayerMask floorLayermask;
    public Vector3 newEnemyPosition;
    public bool canSpawn;
    public PlayerController playerController;
    public bool debugSpawnArea;

    public float delayAndSpawnRate = 2;
    public float timeUntilSpawnRateIncrease = 30;
    public List<HintTriggerController> hintTriggerControllers = new List<HintTriggerController>();
    private float bossTimer = 120;
    public float bossSpawnTime;
    private bool canUpdateBossTime;
    void Start()
    {
        bossTimer = bossSpawnTime;
        GetFloorList();
        StartCoroutine(SpawnObject(delayAndSpawnRate));
    }

    private void Update()
    {
        UpdateBossTime();
    }
    private void UpdateBossTime()
    {
        if (!canUpdateBossTime) return;

        bossTimer -= Time.deltaTime;
        LevelManager.Instance.uIManager.textSpawnBossTime.text = Formatter.TimeFormatter((int)bossTimer);
        if (bossTimer <= 0)
        {
            bossTimer = bossSpawnTime;
            SpawnBoss();
        }
    }
    public void SpawnBoss()
    {
        for (int i = 0; i < 150; i++)
        {
            CheckCanSpawnEnemy();
            if (canSpawn == true)
            {
                EnemyController spawnedBoss = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Count)], enemiesParrent);
                spawnedEnemies.Add(spawnedBoss);
                spawnedBoss.MaxHealth = spawnedBoss.enemyProporties.hp * (LevelManager.Instance.dataManager.CurrentMultipler+1);
                spawnedBoss.CurrentHealth = spawnedBoss.enemyProporties.hp * (LevelManager.Instance.dataManager.CurrentMultipler+1);
                spawnedBoss.isBoss = true;
                spawnedBoss.transform.position = newEnemyPosition;
                spawnedBoss.endPoint = playerController.transform;
                spawnedBoss.EnableNavMesh();
                spawnedBoss.enemySpawnerController = this;
                break;
            }
        }
    }


    IEnumerator SpawnObject(float firstDelay)
    {
        yield return new WaitUntil(() => hintTriggerControllers.Count == 0);
        canUpdateBossTime = true;

        float spawnRateCountdown = timeUntilSpawnRateIncrease;
        float spawnCountdown = firstDelay;
        while (true)
        {
            yield return null;
            spawnRateCountdown -= Time.deltaTime;
            spawnCountdown -= Time.deltaTime;

            // Should a new object be spawned?
            if (spawnCountdown < 0)
            {
                spawnCountdown += delayAndSpawnRate;
                SpawnEnemy();
                // StartCoroutine(SpawnObject(delayAndSpawnRate));
            }

            // Should the spawn rate increase?
            if (spawnRateCountdown < 0 && delayAndSpawnRate > 1)
            {
                spawnRateCountdown += timeUntilSpawnRateIncrease;
                delayAndSpawnRate -= 0.2f;
            }
        }
    }

    [Button]
    public void SpawnEnemy()
    {
        int counter = 0;
        for (int i = 0; i < 150; i++)
        {
            counter++;
            CheckCanSpawnEnemy();
            if (canSpawn == true)
            {
                Debug.Log(LevelManager.Instance.dataManager.CurrentMultipler + 1);
                EnemyController spawnedEnemy = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)], enemiesParrent);
                spawnedEnemies.Add(spawnedEnemy);
                spawnedEnemy.MaxHealth = spawnedEnemy.enemyProporties.hp * (LevelManager.Instance.dataManager.CurrentMultipler+1);
                spawnedEnemy.CurrentHealth = spawnedEnemy.enemyProporties.hp * (LevelManager.Instance.dataManager.CurrentMultipler+1);
                spawnedEnemy.transform.position = newEnemyPosition;
                spawnedEnemy.endPoint = playerController.transform;
                spawnedEnemy.EnableNavMesh();
                spawnedEnemy.enemySpawnerController = this;
                canSpawn = false;
                break;
            }
        }
    }
    public void GetFloorList()
    {
        floorControllers = GameObject.FindObjectsOfType<FloorController>();
    }
    private void CheckCanSpawnEnemy()
    {

        FloorController floorController = floorControllers[(int)Random.Range(0, floorControllers.Length - 1)];
        Vector3 checkPosition = new Vector3(floorController.interactivePoint.transform.position.x, floorController.transform.position.y, floorController.interactivePoint.transform.position.z);

        //if(floorController.transform.position.y  != LevelManager.Instance.currentPlayerFloor * 3)
        //{
        //    return;
        //}    

        if (Vector3.Distance(checkPosition, playerController.transform.position) < minDistanceFromPlayer)
        {
            return;
        }
        if (Vector3.Distance(checkPosition, playerController.transform.position) > maxDistanceFromPlayer)
        {
            return;
        }

        canSpawn = true;
        newEnemyPosition = checkPosition;

    }
}
