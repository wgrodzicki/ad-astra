using System.Collections;
using UnityEngine;

public class MovableSquareShifter : MonoBehaviour
{
    [Tooltip("Input manager script")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("How much should this object be shifted")]
    [SerializeField] private float shiftMagnitude = 0.0f;
    [Tooltip("Delay before the next shift in a row is allowed")]
    [SerializeField] private float timeToNextShift = 0.0f;
    [Tooltip("How many collisions should be allowed when raycasting to check if there is enough space for the shift")]
    [SerializeField] private int collisionsAllowed = 4;

    // Collider data from the trigger
    private Collider2D colliderToShift;

    // Whether shifting should be allowed at all (when triggered)
    private bool canShift = false;

    // Whether another shift in a row should be allowed (while still in the trigger)
    private bool canShiftAgain = false;

    private void Update()
    {
        Shift();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.GetComponent<PlayerController>())
        {
            return;
        }

        colliderToShift = collider;
        canShift = true;
        canShiftAgain = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.GetComponent<PlayerController>())
        {
            return;
        }

        colliderToShift = null;
        canShift = false;
        canShiftAgain = false;
    }

    /// <summary>
    /// Shifts the object on player input.
    /// </summary>
    private void Shift()
    {
        if (colliderToShift == null)
        {
            return;
        }

        if (!canShift)
        {
            return;
        }

        if (!canShiftAgain)
        {
            return;
        }

        // Check if player is on the left
        if (this.gameObject.transform.position.x > colliderToShift.gameObject.transform.position.x)
        {
            // Check if there is enough space behind the player
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.left, 2.0f);

            if (hit.Length <= collisionsAllowed)
            {
                if (inputManager.specialActionButton == 1)
                {
                    // Shift object's position to the left
                    this.gameObject.transform.position -= new Vector3(shiftMagnitude, 0.0f, 0.0f);

                    // Turn the player by 180 degrees
                    SpriteRenderer playerSprite = colliderToShift.gameObject.GetComponentInChildren<SpriteRenderer>();
                    playerSprite.flipX = true;

                    // Wait before another shift is allowed
                    canShiftAgain = false;
                    StartCoroutine(WaitForNextShift());
                }
            }
        }
        // Check if player is on the right
        else if (this.gameObject.transform.position.x < colliderToShift.gameObject.transform.position.x)
        {
            // Check if there is enough space behind the player
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.right, 2.0f);

            if (hit.Length <= collisionsAllowed)
            {
                if (inputManager.specialActionButton == 1)
                {
                    // Shift object's position to the right
                    this.gameObject.transform.position += new Vector3(shiftMagnitude, 0.0f, 0.0f);

                    // Turn the player by 180 degrees
                    SpriteRenderer playerSprite = colliderToShift.gameObject.GetComponentInChildren<SpriteRenderer>();
                    playerSprite.flipX = false;

                    // Wait before another shift is allowed
                    canShiftAgain = false;
                    StartCoroutine(WaitForNextShift());
                }
            }
        }
    }

    /// <summary>
    /// Handles the delay before the next shift in a row is allowed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForNextShift()
    {
        yield return new WaitForSeconds(timeToNextShift);
        canShiftAgain = true;
    }
}
