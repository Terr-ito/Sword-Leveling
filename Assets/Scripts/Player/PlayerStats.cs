using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    // Current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    #region Current Stats Properties

    public float CurrentHealth 
    { 
        get { return currentHealth; } 
        set 
        { 
            if(currentHealth != value) 
            { 
                currentHealth = value;

                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }   
        } 
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;

                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
                // Update the real time value of the stat
                // Add any additional logic here
            }
        }
    }
    #endregion

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

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItem, secondPassiveItem;

    void Awake()
    {
        inventory = GetComponent<InventoryManager>();

        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        // Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        // Spawn the weapons
        SpawnWeapon(characterData.StartingWeapon);

        // Spawn the passive items
        SpawnPassiveItem(firstPassiveItem);
        //SpawnPassiveItem(secondPassiveItem);
    }

    void Start()
    {
        GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + CurrentMagnet;

        //Moved it to Die() function because it isn't working here for wtf reason
        //GameManager.instance.AssignChosenCharacterUI(characterData.Icon, characterData.Name);
    }
    private void Update()
    {
        passiveRecovery();

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
            
            GameManager.instance.StartLevelUp();
        }
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            CurrentHealth -= dmg;

            invicibilityTimer = invicibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }   
    }

    // for button to call this function
    public void ButtonDie()
    {
        Die();
    }

    public void Die()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignChosenCharacterUI(characterData.Icon, characterData.Name);
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        // Cap health restoration to not exceed max health
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void passiveRecovery()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        // Checking if the slost are full, and returning if it is
        if(weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }

        // Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); // Set the weapon to be a child of the player
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        // Checking if the slost are full, and returning if it is
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }

        // Spawn the starting passive item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); // Set the passiveItem to be a child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
