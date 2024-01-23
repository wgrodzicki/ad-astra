using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Applies extra gravity to the player and changes their color upon collission
/// </summary>
public class ExtraGravity : MonoBehaviour
{
    [Tooltip("Player Controller script")]
    public PlayerController playerController;
    [Tooltip("Text to be displayed when entering the zone")]
    public GameObject textEnter;
    [Tooltip("Player movement speed modifier when in the zone")]
    public float speedModifier = 0.5f;

    [Header("Player color when in the zone")]
    [Range(0,255)]
    public int red;
    [Range(0,255)]
    public int green;
    [Range(0,255)]
    public int blue;
    [Range(0,255)]
    public int alpha;

    [Tooltip("UI Manager script")]
    [SerializeField] UIManager interfaceManagerScript;

    // Color values converted to 0-1 scale
    private float redPercent = 0;
    private float greenPercent = 0;
    private float bluePercent = 0;
    private float alphaPercent = 0;

    void Start()
    {
        // Convert RGB values to 0-1 scale
        if (red != 0)
        {
            redPercent = red / 255.0f;
        }
        if (green != 0)
        {
            greenPercent = green / 255.0f;
        }
        if (blue != 0)
        {
            bluePercent = blue / 255.0f;
        }
        if (alpha != 0)
        {
            alphaPercent = alpha / 255.0f;
        }
    }

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
        if (!playerController.affectedByExtraGravity)
        {
            ApplyExtraGravity(collission);

            // Display text if one exists
            if (textEnter != null)
            {
                textEnter.SetActive(true);
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
        if (textEnter != null)
        {
            textEnter.SetActive(false);
        }
    }

    /// <summary>
    /// Turns the text off if game is paused
    /// </summary>
    private void CheckPause()
    {
        if (interfaceManagerScript.isPaused)
        {
            textEnter.SetActive(false);
        }
    }

    private void ApplyExtraGravity(Collider2D collission)
    {
        // Save jump value and block jumping
        playerController.defaultJumpsAllowed = playerController.allowedJumps;
        playerController.allowedJumps = 0;

        // Save speed value and modify speed
        playerController.defaultSpeed = playerController.movementSpeed;
        playerController.movementSpeed *= speedModifier;

        // Get the sprite renderer
        Component[] spriteRenderers = collission.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Save original color
            playerController.defaultColor = spriteRenderer.color;
            // Change player's color
            spriteRenderer.color = new Color(redPercent, greenPercent, bluePercent, alphaPercent);
        }

        playerController.affectedByExtraGravity = true; // Change gravity state
    }
}
