using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollectible
{

    public int experienceGranted;

    public void Collect()
    {
        PlayerStats player = Object.FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        Destroy(gameObject);
    }
}
