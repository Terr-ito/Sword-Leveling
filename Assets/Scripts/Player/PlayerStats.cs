using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    // Current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

    // Experience and level of the player
    [UnitHeaderInspectable("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;
    public int experienceCapIncrease;

    // I-Frames
    [Header("I-Frames")]
    public float invicibilityDuration;
    float invicibilityTimer;
    bool isInvincible;

    void Awake()
    {
        // Assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
    }

    private void Update()
    {
        if (invicibilityTimer > 0)
        {
            invicibilityTimer -= Time.deltaTime;

        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        levelUpChecker();
    }

    void levelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            experienceCap += experienceCapIncrease;
            // You can add additional logic here for what happens when the player levels up
            // For example, you might want to increase the player's stats or unlock new abilities
        }
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            currentHealth -= dmg;

            invicibilityTimer = invicibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Die();
            }
        }   
    }

    public void Die()
    {
        // Logic for player death (e.g., respawn, game over screen, etc.)
        Debug.Log("Player has died.");
    }

    public void RestoreHealth(float amount)
    {
        // Cap health restoration to not exceed max health
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
}
