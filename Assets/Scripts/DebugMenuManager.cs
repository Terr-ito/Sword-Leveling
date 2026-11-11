using UnityEngine;
using UnityEngine.UI;

public class DebugMenuManager : MonoBehaviour
{
    [SerializeField] 
    public GameObject debugMenuCanvas;

    // Stat increase amounts
    [SerializeField] private float mightIncreaseAmount = 10f;
    [SerializeField] private float moveSpeedIncreaseAmount = 2f;

    // Enemy spawning
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int enemyGroupIndexToSpawn = 0; // Index of enemy to spawn (usually Bat is 0)

    private bool menuOpen = false;
    private bool isGodMode = false;
    private PlayerStats playerStats;

    private void Start()
    {
        // Get the player stats from the player object in GameManager
        if (GameManager.instance != null && GameManager.instance.playerObject != null)
        {
            playerStats = GameManager.instance.playerObject.GetComponent<PlayerStats>();
        }

        // Initialize the debug menu as closed
        if (debugMenuCanvas != null)
        {
            debugMenuCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleDebugMenu();
        }
    }

    private void ToggleDebugMenu()
    {
        menuOpen = !menuOpen;

        if (debugMenuCanvas != null)
        {
            debugMenuCanvas.gameObject.SetActive(menuOpen);
        }

        // Pause the game while menu is open
        //Time.timeScale = menuOpen ? 0f : 1f;

        //Debug.Log($"Debug Menu: {(menuOpen ? "OPEN" : "CLOSED")}");
    }

    public void IncreaseMight()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats not found!");
            return;
        }

        playerStats.CurrentMight += mightIncreaseAmount;
        Debug.Log($"Might increased to {playerStats.CurrentMight}");
    }

    public void IncreaseSpeed()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats not found!");
            return;
        }

        playerStats.CurrentMoveSpeed += moveSpeedIncreaseAmount;
        Debug.Log($"Move Speed increased to {playerStats.CurrentMoveSpeed}");
    }

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
        Debug.Log($"God Mode: {(isGodMode ? "ON" : "OFF")}");
    }

    public void SpawnEnemy()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats not found!");
            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogWarning("EnemySpawner not found! Make sure it's assigned in the inspector.");
            return;
        }

        // Get the current wave and enemy group
        if (enemySpawner.waves.Count > 0)
        {
            var wave = enemySpawner.waves[enemySpawner.currentWaveIndex];

            if (wave.enemyGroups.Count > enemyGroupIndexToSpawn)
            {
                var enemyGroup = wave.enemyGroups[enemyGroupIndexToSpawn];

                // Spawn enemy at random spawn point relative to player
                if (enemySpawner.relativeSpawnPoints.Count > 0)
                {
                    Vector3 spawnPos = playerStats.transform.position + 
                        enemySpawner.relativeSpawnPoints[Random.Range(0, enemySpawner.relativeSpawnPoints.Count)].position;

                    GameObject spawnedEnemy = Instantiate(enemyGroup.enemyPrefab, spawnPos, Quaternion.identity);
                    enemySpawner.enemiesAlive++;

                    Debug.Log($"Spawned {enemyGroup.enemyName} at {spawnPos}");
                }
                else
                {
                    Debug.LogWarning("No spawn points defined in EnemySpawner!");
                }
            }
            else
            {
                Debug.LogWarning($"Enemy group index {enemyGroupIndexToSpawn} not found in current wave!");
            }
        }
        else
        {
            Debug.LogWarning("No waves defined in EnemySpawner!");
        }
    }

    /// <summary>
    /// Returns whether god mode is currently active. 
    /// Call this from PlayerStats.TakeDamage() to prevent damage when enabled.
    /// </summary>
    public bool IsGodMode
    {
        get { return isGodMode; }
    }
}