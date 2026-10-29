using System.Collections.Generic;
using UnityEngine;

public class MainSwordBehaviour : MeleeWeaponBehaviour
{
    [Header("Swipe")]
    [Tooltip("Total angle of the swipe arc in degrees, centered on the enemy direction.")]
    public float swipeArc = 120f;

    [Tooltip("Rotation offset if your sprite doesn't point to the right (+X) by default. " +
             "Use -90 if it points up, 90 if it points down, 180 if it points left.")]
    public float spriteAngleOffset = 0f;

    float baseAngle;
    float elapsed;
    float swipeDuration;
    List<GameObject> markedEnemies;

    public void SetDirection(Vector2 direction)
    {
        baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ApplyRotation(-swipeArc / 2f); // Start at the beginning of the arc right away
    }

    protected override void Start()
    {
        base.Start(); // Destroys the sword after destroyAfterSeconds
        markedEnemies = new List<GameObject>();
        swipeDuration = Mathf.Max(destroyAfterSeconds, 0.01f);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / swipeDuration);

        // Sweep from one side of the arc to the other
        float offset = Mathf.Lerp(-swipeArc / 2f, swipeArc / 2f, t);
        ApplyRotation(offset);
    }

    void ApplyRotation(float arcOffset)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, baseAngle + arcOffset + spriteAngleOffset);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (markedEnemies == null) markedEnemies = new List<GameObject>();

        // Each enemy/prop gets hit only once per swipe
        if (markedEnemies.Contains(collision.gameObject)) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent<BreakableProps>(out BreakableProps breakable))
            {
                breakable.TakeDamage(currentDamage);
                markedEnemies.Add(collision.gameObject);
            }
        }
    }
}