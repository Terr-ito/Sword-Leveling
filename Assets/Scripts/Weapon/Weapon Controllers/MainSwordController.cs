using UnityEngine;

public class MainSwordController : WeaponController
{
    [Header("Targeting")]
    [Tooltip("Enemies farther than this are ignored. Set to 0 for unlimited range.")]
    public float maxRange = 0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        EnemyStats target = FindClosestEnemy();

        // No enemy in range: don't reset the cooldown, so we swing the moment one appears
        if (target == null) return;

        base.Attack(); // Resets the cooldown

        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Spawn on the enemy (not parented, so it doesn't drift with the player)
        GameObject spawnedSword = Instantiate(weaponData.Prefab, target.transform.position, Quaternion.identity);

        spawnedSword.GetComponent<MainSwordBehaviour>().SetDirection(direction);
    }

    EnemyStats FindClosestEnemy()
    {
        EnemyStats[] enemies = Object.FindObjectsByType<EnemyStats>();

        EnemyStats closest = null;
        float closestSqrDist = float.MaxValue;
        float maxSqrRange = maxRange * maxRange;

        foreach (EnemyStats enemy in enemies)
        {
            float sqrDist = ((Vector2)enemy.transform.position - (Vector2)transform.position).sqrMagnitude;

            if (maxRange > 0f && sqrDist > maxSqrRange) continue;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closest = enemy;
            }
        }

        return closest;
    }
}