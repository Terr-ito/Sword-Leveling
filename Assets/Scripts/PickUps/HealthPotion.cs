using UnityEditor.Build;
using UnityEngine;

public class HealthPotion : Pickup, ICollectible
{
    public int healthToRestore;

    public void Collect()
    {
        PlayerStats player = Object.FindAnyObjectByType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
    }
}
