using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Restores normal gravity and color to the player
/// </summary>
public class NormalGravity : MonoBehaviour
{
    [Tooltip("Player Controller script")]
    public PlayerController playerController;
    [Tooltip("Text to be displayed when exiting the zone")]
    public GameObject textExit;
    [Tooltip("UI Manager script")]
    [SerializeField] UIManager interfaceManagerScript;

    private void Update()
    {
        CheckPause();
    }

    void OnTriggerEnter2D(Collider2D collission)
    {
        if (collission.tag != "Player")
        {
            return;
        }

        // Check if the player was in the zone
        if (playerController.affectedByExtraGravity)
        {
            RestoreNormalGravity(collission);
            
            // Display text if one exists
            if (textExit != null)
            {
                textExit.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collission)
    {   
        if (collission.tag != "Player")
        {
            return;
        }
        
        // Hide text if one exists
        if (textExit != null)
        {
            textExit.SetActive(false);
        }
    }

    /// <summary>
    /// Turns the text off if game is paused
    /// </summary>
    private void CheckPause()
    {
        if (interfaceManagerScript.isPaused)
        {
            textExit.SetActive(false);
        }
    }

    public void RestoreNormalGravity(Collider2D collission)
    {
        // Enable jumping
        playerController.allowedJumps = playerController.defaultJumpsAllowed;

        // Restore speed
        playerController.movementSpeed = playerController.defaultSpeed;
        
        // Get the sprite renderer
        Component[] spriteRenderers = collission.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Restore player's color
            spriteRenderer.color = playerController.defaultColor;
        }

        playerController.affectedByExtraGravity = false; // Change gravity state
    }
}
