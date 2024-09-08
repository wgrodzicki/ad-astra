using UnityEngine;

public class NormalGravity : MonoBehaviour
{
    [Tooltip("Player Controller script")]
    [SerializeField] private PlayerController playerController;
    [Tooltip("Text to be displayed when exiting the zone")]
    [SerializeField] private GameObject textExit;
    [Tooltip("UI Manager script")]
    [SerializeField] private UIManager interfaceManagerScript;

    private void Update()
    {
        CheckPause();
    }

    private void OnTriggerEnter2D(Collider2D collission)
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

    private void OnTriggerExit2D(Collider2D collission)
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
    /// Turns the text off if game is paused.
    /// </summary>
    private void CheckPause()
    {
        if (interfaceManagerScript.isPaused)
        {
            textExit.SetActive(false);
        }
    }

    /// <summary>
    /// Cancels the extra gravity effect.
    /// </summary>
    /// <param name="collission"></param>
    public void RestoreNormalGravity(Collider2D collission)
    {
        playerController.allowedJumps = playerController.defaultJumpsAllowed;
        playerController.movementSpeed = playerController.defaultSpeed;

        Component[] spriteRenderers = collission.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Restore player's color
            spriteRenderer.color = playerController.defaultColor;
        }

        playerController.affectedByExtraGravity = false; // Change gravity state
    }
}
