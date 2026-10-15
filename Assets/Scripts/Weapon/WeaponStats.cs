using UnityEngine;

public class WeaponStats
{
    public float Damage { get; set; }
    public float AttackSpeed { get; set; }
    public float Range { get; set; }
    public float SwipeDistance { get; set; }

    public WeaponStats(float damage = 10f, float attackSpeed = 1f, float range = 5f, float swipeDistance = 3f)
    {
        Damage = damage;
        AttackSpeed = attackSpeed;
        Range = range;
        SwipeDistance = swipeDistance;
    }

    public void UpgradeStat(string statName, float value)
    {
        switch (statName.ToLower())
        {
            case "damage":
                Damage = value;
                break;
            case "attackspeed":
                AttackSpeed = value;
                break;
            case "range":
                Range = value;
                break;
            case "swipedistance":
                SwipeDistance = value;
                break;
            default:
                Debug.LogWarning($"Unknown stat: {statName}");
                break;
        }
    }

    public float GetStat(string statName)
    {
        return statName.ToLower() switch
        {
            "damage" => Damage,
            "attackspeed" => AttackSpeed,
            "range" => Range,
            "swipedistance" => SwipeDistance,
            _ => 0f
        };
    }

    public float GetAttackCooldown()
    {
        return 1f / (AttackSpeed > 0 ? AttackSpeed : 1f);
    }
}
