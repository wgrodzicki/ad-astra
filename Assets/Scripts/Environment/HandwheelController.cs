using System.Collections.Generic;
using UnityEngine;

public class HandwheelController : MonoBehaviour
{
    [Tooltip("Input manager script")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("Handwheel waypoint script")]
    [SerializeField] private WaypointMover handwheelMover;
    [Tooltip("Objects to turn off when using the handwheel")]
    [SerializeField] private List<GameObject> objectsOff = new List<GameObject>();
    [Tooltip("Objects to turn on when using the handwheel")]
    [SerializeField] private List<GameObject> objectsOn = new List<GameObject>();
    [Tooltip("Handwheel sound effect")]
    [SerializeField] private GameObject handwheelEffect;

    private bool canMoveHandwheel = false;

    private void Update()
    {
        MoveHandwheel();
    }

    /// <summary>
    /// Handles handwheel movement.
    /// </summary>
    private void MoveHandwheel()
    {
        if (!canMoveHandwheel)
        {
            return;
        }

        // Wait for input
        if (inputManager.specialActionButton == 1)
        {
            handwheelMover.shouldTilt = true;
            Instantiate(handwheelEffect, transform.position, transform.rotation, null);
            AffectObjects(objectsOff, false);
            AffectObjects(objectsOn, true);
        }
    }

    /// <summary>
    /// Deactivates target objects if there are any.
    /// </summary>
    private void AffectObjects(List<GameObject> targetObjects, bool activate)
    {
        if (targetObjects.Count == 0)
        {
            return;
        }

        foreach (GameObject targetObject in targetObjects)
        {
            if (activate)
            {
                targetObject.SetActive(true);
            }
            else
            {
                targetObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (handwheelMover == null)
        {
            return;
        }

        if (inputManager == null)
        {
            return;
        }

        if (!collider.gameObject.GetComponent<PlayerController>())
        {
            return;
        }

        canMoveHandwheel = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (handwheelMover == null)
        {
            return;
        }

        if (inputManager == null)
        {
            return;
        }

        if (!collider.gameObject.GetComponent<PlayerController>())
        {
            return;
        }

        canMoveHandwheel = false;
    }
}
