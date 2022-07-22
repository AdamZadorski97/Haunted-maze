using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    public List<EnemyController> enemiesPrefabs = new List<EnemyController>();
    public FloorController[] floorControllers;
    public List<float> SpawningFrequency = new List<float>();

    
    public Transform enemiesParrent;
    public Transform middleOfMap;
    public float maxSpawnPointFromMiddleX;
    public float maxSpawnPointFromMiddleZ;
    public float minDistanceFromPlayer;
    public LayerMask floorLayermask;
    public Vector3 newEnemyPosition;
    public bool canSpawn;
    public PlayerController playerController;
    public bool debugSpawnArea;

    public float delayAndSpawnRate = 2;
    public float timeUntilSpawnRateIncrease = 30;

    void Start()
    {

        GetFloorList();
        StartCoroutine(SpawnObject(delayAndSpawnRate));
    }

    IEnumerator SpawnObject(float firstDelay)
    {
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
                EnemyController spawnedEnemy = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)], enemiesParrent);
                spawnedEnemies.Add(spawnedEnemy);
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
        Vector3 checkPosition = floorController.transform.position + new Vector3(1, 0, 1);

        if(floorController.transform.position.y  != LevelManager.Instance.currentPlayerFloor * 3)
        {
            return;
        }    

        if (Vector3.Distance(checkPosition, playerController.transform.position) < minDistanceFromPlayer)
        {
            return;
        }
        if (CheckGround(checkPosition))
        {
            canSpawn = true;
            newEnemyPosition = checkPosition;
        }
    }


    public bool CheckGround(Vector3 posToCheck)
    {
        RaycastHit groundHit;
        if (Physics.Raycast(posToCheck + new Vector3(0, 1, 0), Vector3.down, out groundHit, Mathf.Infinity, floorLayermask))
        {
            newEnemyPosition = groundHit.transform.position;

            return true;
        }
        else return false;
    }
    void OnDrawGizmosSelected()
    {
        if (debugSpawnArea)
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawCube(middleOfMap.position, new Vector3(maxSpawnPointFromMiddleX * 2, 1, maxSpawnPointFromMiddleZ * 2));
        }

    }
}
