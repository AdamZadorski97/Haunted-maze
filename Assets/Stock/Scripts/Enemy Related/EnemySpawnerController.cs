using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    public List<EnemyController> enemiesPrefabs = new List<EnemyController>();

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
                delayAndSpawnRate -= 0.1f;
            }
        }
    }





    [Button]
    public void SpawnEnemy()
    {
        int counter = 0;
        for (int i = 0; i < 100; i++)
        {
            counter++;
            CheckCanSpawnEnemy();
            if (canSpawn == true)
            {
                EnemyController spawnedEnemy = Instantiate(enemiesPrefabs[0], enemiesParrent);
                spawnedEnemies.Add(spawnedEnemy);
                spawnedEnemy.transform.position = newEnemyPosition;
                canSpawn = false;
                break;
            }
        }
    }

    private void CheckCanSpawnEnemy()
    {
        Vector3 middleOfMapPosition = middleOfMap.position;
        Vector3 checkPosition = middleOfMapPosition +
            new Vector3(Random.Range(-maxSpawnPointFromMiddleX, maxSpawnPointFromMiddleX),
            0, Random.Range(-maxSpawnPointFromMiddleZ, maxSpawnPointFromMiddleZ));

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
