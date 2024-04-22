using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This class handles the health state of a game object.
/// 
/// Implementation Notes: 2D Rigidbodies must be set to never sleep for this to interact with trigger stay damage
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Team Settings")]
    [Tooltip("The team associated with this damage")]
    public int teamId = 0;

    [Header("Health Settings")]
    [Tooltip("The default health value")]
    public int defaultHealth = 1;
    [Tooltip("The maximum health value")]
    public int maximumHealth = 1;
    [Tooltip("The current in game health value")]
    public int currentHealth = 1;
    [Tooltip("Invulnerability duration, in seconds, after taking damage")]
    public float invincibilityTime = 3f;

    [Header("Lives settings")]
    [Tooltip("Whether or not to use lives")]
    public bool useLives = false;
    [Tooltip("Current number of lives this health has")]
    public int currentLives = 3;
    [Tooltip("The maximum number of lives this health has")]
    public int maximumLives = 5;
    [Tooltip("The amount of time to wait before respawning")]
    public float respawnWaitTime = 3f;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetRespawnPoint(transform.position);
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        InvincibilityCheck();
        RespawnCheck();
        // Check if it's time to deactivate the player hurt effect (WG)
        DeactivatePlayerHurtEffect();
    }

    // The time to respawn at
    private float respawnTime;

    /// <summary>
    /// Description:
    /// Checks to see if the player should be respawned yet and only respawns them if the alloted time has passed
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void RespawnCheck()
    {
        if (respawnWaitTime != 0 && currentHealth <= 0 && currentLives > 0)
        {
            if (Time.time >= respawnTime)
            {
                Respawn();
            }
        }
    }

    // The specific game time when the health can be damaged again
    private float timeToBecomeDamagableAgain = 0;
    // Whether or not the health is invincible
    public bool isInvincible = false;

    /// <summary>
    /// Description:
    /// Checks against the current time and the time when the health can be damaged again.
    /// Removes invicibility if the time frame has passed
    /// Input:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    private void InvincibilityCheck()
    {
        if (timeToBecomeDamagableAgain <= Time.time)
        {
            isInvincible = false;
        }
    }

    // The position that the health's gameobject will respawn at
    private Vector3 respawnPosition;

    /// <summary>
    /// Description:
    /// Changes the respawn position to a new position
    /// Input:
    /// Vector3 newRespawnPosition
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="newRespawnPosition">The new position to respawn at</param>
    public void SetRespawnPoint(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    /// <summary>
    /// Description:
    /// Repositions the health's game object to the respawn position and resets the health to the default value
    /// Input:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void Respawn()
    {
        // Check if the respawning health is the player (WG)
        if (this.gameObject.GetComponent<PlayerController>() != null)
        {
            // Check if the player is currently affected by extra gravity
            if (this.gameObject.GetComponent<PlayerController>().affectedByExtraGravity)
            {
                // Restore player's jumping ability and color if so
                normalGravityScript.RestoreNormalGravity(this.gameObject.GetComponent<Collider2D>());
            }
        }

        transform.position = respawnPosition;
        currentHealth = defaultHealth;
        GameManager.UpdateUIElements();
    }

    // Variables to control the display of the player hurt effect (WG)
    [Tooltip("The effect to display when player is hurt")]
    public GameObject playerHurtEffect;

    [Tooltip("How long should the player hurt effect last")]
    public float playerHurtEffectDuration = 0;

    private float playerHurtEffectEnd = 0;
    private bool playerHurt = false;

    /// <summary>
    /// Description:
    /// Applies damage to the health unless the health is invincible.
    /// Input:
    /// int damageAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take</param>
    public void TakeDamage(int damageAmount)
    {
        if (isInvincible || currentHealth <= 0)
        {
            return;
        }
        else
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation, null);
                // Check if the health is the player (WG)
                if (this.gameObject.tag == "Player")
                {
                    // Display the hurt effect if so (WG)
                    playerHurtEffect.SetActive(true);
                    playerHurtEffectEnd = Time.time + playerHurtEffectDuration;
                    playerHurt = true;
                }
            }
            timeToBecomeDamagableAgain = Time.time + invincibilityTime;
            isInvincible = true;
            currentHealth -= damageAmount;
            CheckDeath();
        }
        GameManager.UpdateUIElements();
    }

    /// <summary>
    /// Handles the deactivation of the player hurt effect (WG)
    /// </summary>
    void DeactivatePlayerHurtEffect()
    {
        // Check if the player's hurt
        if (playerHurt)
        {
            // Check if enough time has passed
            if (playerHurtEffectEnd != 0 && playerHurtEffectEnd <= Time.time)
            {
                // Deactivate the effect if so
                playerHurtEffect.SetActive(false);
                playerHurt = false;
            }
        }
    }

    /// <summary>
    /// Description:
    /// Applies healing to the health, capped out at the maximum health.
    /// Input:
    /// int healingAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="healingAmount">How much healing to apply</param>
    public void ReceiveHealing(int healingAmount)
    {
        currentHealth += healingAmount;
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
        CheckDeath();
        GameManager.UpdateUIElements();
    }

    /// <summary>
    /// Description:
    /// Gives the health script more lives if the health is using lives
    /// Input:
    /// int bonusLives
    /// Return:
    /// void
    /// </summary>
    /// <param name="bonusLives">The number of lives to add</param>
    public void AddLives(int bonusLives)
    {
        if (useLives)
        {
            currentLives += bonusLives;
            if (currentLives > maximumLives)
            {
                currentLives = maximumLives;
            }
            GameManager.UpdateUIElements();
        }
    }

    [Header("Effects & Polish")]
    [Tooltip("The effect to create when this health dies")]
    public GameObject deathEffect;
    [Tooltip("The effect to create when this health is damaged (but does not die)")]
    public GameObject hitEffect;
    // Select loot to be dropped (WG)
    [Tooltip("Loot dropped by this health (only if tagged as enemy)")]
    public GameObject basicLoot;
    [Tooltip("More loot dropped by this health (only if tagged as enemy, requires Basic Loot)")]
    public GameObject extraLoot;
    // Any extra gravity script on the scene (WG)
    [Tooltip("Extra gravity script that can affect the player")]
    public NormalGravity normalGravityScript;

    /// <summary>
    /// Description:
    /// Checks if the health is dead or not. If it is, true is returned, false otherwise.
    /// Calls Die() if the health is dead.
    /// Input:
    /// None
    /// Return:
    /// bool
    /// </summary>
    /// <returns>bool: a boolean value representing if the health has died or not (true for dead)</returns>
    bool CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles loot dropping (WG)
    /// </summary>
    public virtual void DropLoot()
    {
        // Check if there is loot to be dropped
        if (basicLoot == null)
        {
            return;
        }

        int randomNumber = Random.Range(0, 5);

        // No loot condition
        if (randomNumber == 0)
        {
            return;
        }

        // Check if there is only basic loot to be dropped
        if (extraLoot == null)
        {
            Instantiate(basicLoot, transform.position, transform.rotation, null);
            return;
        }
        // Handle more loot dropping
        else
        {
            switch (randomNumber)
            {
                case 1:
                    Instantiate(basicLoot, transform.position, transform.rotation, null);
                    break;
                case 2:
                    Instantiate(extraLoot, transform.position, transform.rotation, null);
                    break;
                case 3:
                    Instantiate(basicLoot, new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z), transform.rotation, null);
                    Instantiate(basicLoot, new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.rotation, null);
                    break;
                case 4:
                    Instantiate(extraLoot, new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z), transform.rotation, null);
                    Instantiate(extraLoot, new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z), transform.rotation, null);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Description:
    /// Handles the death of the health. If a death effect is set, it is created. If lives are being used, the health is respawned.
    /// If lives are not being used or the lives are 0 then the health's game object is destroyed.
    /// Input:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation, null);
        }

        if (useLives)
        {
            currentLives -= 1;
            if (currentLives > 0)
            {
                if (respawnWaitTime == 0)
                {
                    Respawn();
                }
                else
                {
                    respawnTime = Time.time + respawnWaitTime;
                }
            }
            else
            {
                if (respawnWaitTime != 0)
                {
                    respawnTime = Time.time + respawnWaitTime;
                }
                else
                {
                    Destroy(this.gameObject);
                }
                GameOver();
            }

        }
        else
        {
            GameOver();

            // Drop loot if applicable (WG)
            if (this.gameObject.tag == "Enemy")
            {
                DropLoot();
            }

            Destroy(this.gameObject);
        }
        GameManager.UpdateUIElements();
    }

    /// <summary>
    /// Description:
    /// Tries to notify the game manager that the game is over
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    public void GameOver()
    {
        if (GameManager.instance != null && gameObject.tag == "Player")
        {
            GameManager.instance.GameOver();
        }
    }
}
