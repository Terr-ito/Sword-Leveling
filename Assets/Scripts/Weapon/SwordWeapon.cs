using UnityEngine;

public class SwordWeapon : MonoBehaviour, IUpgradeable
{
    [SerializeField] private GameObject swipePrefab;
    [SerializeField] private Transform swipeSpawnPoint;

    private WeaponStats stats;
    private float lastAttackTime;
    private PlayerMovement playerMovement;

    private void Start()
    {
        // Initialize weapon stats with default values
        stats = new WeaponStats(
            damage: 10f,
            attackSpeed: 1.5f,
            range: 5f,
            swipeDistance: 3f
        );

        lastAttackTime = -stats.GetAttackCooldown();
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            playerMovement = Object.FindAnyObjectByType<PlayerMovement>();
        }

        if (swipeSpawnPoint == null)
        {
            swipeSpawnPoint = transform;
        }
    }

    private void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        // Attack with left mouse button or space
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && CanAttack())
        {
            PerformSwipe();
            lastAttackTime = Time.time;
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= stats.GetAttackCooldown();
    }

    private void PerformSwipe()
    {
        // Determine swipe direction based on player movement direction
        Vector3 swipeDirection = GetSwipeDirection();

        // Spawn swipe prefab or create it dynamically
        if (swipePrefab != null)
        {
            GameObject swipeInstance = Instantiate(
                swipePrefab,
                swipeSpawnPoint.position + swipeDirection.normalized * stats.SwipeDistance,
                Quaternion.identity
            );

            SwordSwipe swipeComponent = swipeInstance.GetComponent<SwordSwipe>();
            if (swipeComponent != null)
            {
                swipeComponent.Initialize(stats.Damage, stats.Range, swipeInstance.transform.position, swipeDirection);
            }
        }
        else
        {
            // Fallback: create a simple circle swipe dynamically
            SpawnDynamicSwipe(swipeDirection);
        }
    }

    private Vector3 GetSwipeDirection()
    {
        // Use player movement direction if available
        if (playerMovement != null && playerMovement.moveDir != Vector2.zero)
        {
            return new Vector3(playerMovement.moveDir.x, playerMovement.moveDir.y, 0f);
        }

        // Fallback to mouse direction relative to player
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 directionToMouse = (worldMousePos - transform.position).normalized;
        return directionToMouse;
    }

    private void SpawnDynamicSwipe(Vector3 direction)
    {
        GameObject swipeGameObject = new GameObject("SwordSwipe");
        swipeGameObject.transform.position = swipeSpawnPoint.position + direction.normalized * stats.SwipeDistance;

        // Add circle collider for the swipe area
        CircleCollider2D circleCollider = swipeGameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = stats.Range;
        circleCollider.isTrigger = true;

        // Add sprite renderer for visual feedback with enhanced colors
        SpriteRenderer spriteRenderer = swipeGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 0.8f, 0.3f, 0.6f);
        spriteRenderer.sortingOrder = 0;

        // Add trail renderer for swipe motion effect
        TrailRenderer trailRenderer = swipeGameObject.AddComponent<TrailRenderer>();
        trailRenderer.time = 0.15f;
        trailRenderer.startWidth = 0.5f;
        trailRenderer.endWidth = 0.1f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = new Color(1f, 0.8f, 0.3f, 0.8f);
        trailRenderer.endColor = new Color(1f, 0.8f, 0.3f, 0f);

        // Add audio source for attack sound (optional)
        AudioSource audioSource = swipeGameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Add SwordSwipe component
        SwordSwipe swipeComponent = swipeGameObject.AddComponent<SwordSwipe>();
        swipeComponent.Initialize(stats.Damage, stats.Range, swipeGameObject.transform.position, direction);

        // Play attack sound if one is assigned
        PlayAttackSound(audioSource);
    }

    private void PlayAttackSound(AudioSource audioSource)
    {
        // Placeholder for sword attack sound
        // You can add sound clips to the weapon later
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Slight pitch variation
        }
    }

    // IUpgradeable implementation
    public void UpgradeStat(string statName, float value)
    {
        stats.UpgradeStat(statName, value);
        Debug.Log($"Upgraded {statName} to {value}");
    }

    public float GetStat(string statName)
    {
        return stats.GetStat(statName);
    }

    // Public methods for external stat access
    public WeaponStats GetStats()
    {
        return stats;
    }

    public void SetStats(WeaponStats newStats)
    {
        if (newStats != null)
        {
            stats = newStats;
        }
    }
}
