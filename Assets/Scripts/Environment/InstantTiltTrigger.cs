using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers instant tilting of the associated waypoint mover on contact with the specific object
/// </summary>
public class InstantTiltTrigger : MonoBehaviour
{
    [Tooltip("Associated waypoint mover, e.g. platform")]
    public WaypointMover waypointMoverScript;
    [Tooltip("Objects that should be detected by the collider")]
    public string tagToLookFor;

    private Collider2D targetCollider = null;

    void Update()
    {
        StraightenObjectOnFullTilt();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check for match
        if (collider.tag == tagToLookFor)
        {
            targetCollider = collider;
            // Tilt immediately
            waypointMoverScript.shouldTilt = true;
        }
    }

    /// <summary>
    /// Straightens the object when tilting is finished
    /// </summary>
    private void StraightenObjectOnFullTilt()
    {
        if (targetCollider == null)
        {
            return;
        }

        if (!waypointMoverScript.isTilted)
        {
            return;
        }

        // Restore rotation
        targetCollider.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
