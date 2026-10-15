using UnityEngine;

public class SwordSwipe : MonoBehaviour
{
    private float damage;
    private float range;
    private Collider2D damageZone;
    private float lifetime = 0.3f;
    private float elapsedTime = 0f;

    // Visual Effects
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private ParticleSystem particleSystem;
    private Vector3 spawnScale;

    private void Start()
    {
        damageZone = GetComponent<Collider2D>();
        if (damageZone == null)
        {
            Debug.LogError("SwordSwipe requires a Collider2D component");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

        InitializeVisualEffects();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / lifetime;

        // Animate visual scale (grow then shrink)
        float scaleAnimation = Mathf.Sin(progress * Mathf.PI);
        transform.localScale = spawnScale * (0.5f + scaleAnimation * 0.5f);

        // Fade out alpha
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, progress);
            spriteRenderer.color = color;
        }

        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float damageValue, float rangeValue, Vector3 spawnPosition, Vector3 direction)
    {
        damage = damageValue;
        range = rangeValue;
        spawnScale = new Vector3(range, range, 1f);

        transform.position = spawnPosition;
        gameObject.transform.localScale = spawnScale;

        // Rotate the swipe towards the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void InitializeVisualEffects()
    {
        // Setup sprite renderer if not already configured
        if (spriteRenderer != null)
        {
            // Use a semi-transparent white color for visual feedback
            Color color = new Color(1f, 0.8f, 0.3f, 0.6f);
            spriteRenderer.color = color;
        }

        // Setup trail renderer for motion effect
        if (trailRenderer != null)
        {
            trailRenderer.time = 0.15f;
            trailRenderer.startWidth = 0.5f;
            trailRenderer.endWidth = 0.1f;
            trailRenderer.startColor = new Color(1f, 0.8f, 0.3f, 0.8f);
            trailRenderer.endColor = new Color(1f, 0.8f, 0.3f, 0f);
        }

        // Create particle system if available
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            // Customize particle system settings for sword swipe effect
            var main = particleSystem.main;
            main.duration = lifetime;
            main.loop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is an enemy (you can customize this based on your enemy setup)
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            PlayImpactEffect(collision.transform.position);
        }
        else if (collision.CompareTag("Enemy"))
        {
            // Fallback for enemies without IDamageable interface
            Debug.LogWarning($"Enemy '{collision.gameObject.name}' hit but has no IDamageable interface");
            PlayImpactEffect(collision.transform.position);
        }
    }

    private void PlayImpactEffect(Vector3 impactPosition)
    {
        // Create particle effect at impact
        SwordVisualEffects.CreateHitParticles(impactPosition, 6);

        // Create a brief yellow flash at impact point
        GameObject impactEffect = new GameObject("ImpactEffect");
        impactEffect.transform.position = impactPosition;

        SpriteRenderer impactRenderer = impactEffect.AddComponent<SpriteRenderer>();
        impactRenderer.color = new Color(1f, 1f, 0f, 0.8f);

        CircleCollider2D impactCollider = impactEffect.AddComponent<CircleCollider2D>();
        impactCollider.radius = 0.2f;

        // Destroy after a short time
        Destroy(impactEffect, 0.15f);
    }
}
