using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // List of enemy groups to spawn in this wave
        public int waveQuota; // Total number of enemies to spawn in this wave
        public float spawnInterval; // The interval at which to spawn enemies
        public int spawnCount; // Number of enemies spawned so far in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // Number of enemies to spawn in this group
        public int spawnCount; // Number of enemies spawned so far in this group
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // List of waves in the game
    public int currentWaveIndex; // Index of the current wave

    [Header("Spawn Attributes")]
    float spawnTimer; // Timer used to track when to spawn the next enemy
    public int enemiesAlive;
    public int maxEnemiesAlive;
    public bool maxEnemiesAliveReached = false; // Flag to indicate if the max number of enemies alive has been reached
    public float waveInterval; // Interval between waves

    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints; // List of spawn points for enemies

    Transform player;

    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {
        // Check if the current wave has ended and if there are more waves to spawn
        if (currentWaveIndex < waves.Count && waves[currentWaveIndex].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn the next enemy
        if (spawnTimer >= waves[currentWaveIndex].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);
        
        if(currentWaveIndex < waves.Count - 1)
        {
            currentWaveIndex++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        
        foreach (var enemyGroup in waves[currentWaveIndex].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveIndex].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        // Check if the current wave has spawned all its enemies
        if (waves[currentWaveIndex].spawnCount < waves[currentWaveIndex].waveQuota && !maxEnemiesAliveReached)
        {
            // Spawn each type of enemy in the current wave
            foreach (var enemyGroup in waves[currentWaveIndex].enemyGroups)
            {
                // Check if the enemy group has spawned all its enemies
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Limit the number of enemies alive to the maximum allowed
                    if (enemiesAlive >= maxEnemiesAlive)
                    {
                        maxEnemiesAliveReached = true;
                        return;
                    }

                    // Spawn the enemy at a random spawn point relative to the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveIndex].spawnCount ++;
                    enemiesAlive++;
                }
            }
        }

        if(enemiesAlive < maxEnemiesAlive)
        {
            maxEnemiesAliveReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
