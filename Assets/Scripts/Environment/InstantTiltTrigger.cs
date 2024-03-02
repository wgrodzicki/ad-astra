using UnityEngine;

public class InstantTiltTrigger : MonoBehaviour
{
    [Tooltip("Associated waypoint mover, e.g. platform")]
    [SerializeField] private WaypointMover waypointMoverScript;
    [Tooltip("Objects that should be detected by the collider")]
    [SerializeField] private string tagToLookFor;

    private Collider2D targetCollider = null;

    private void Update()
    {
        StraightenObjectOnFullTilt();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == tagToLookFor)
        {
            targetCollider = collider;
            // Tilt immediately
            waypointMoverScript.shouldTilt = true;
        }
    }

    /// <summary>
    /// Straightens the object when tilting is finished.
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
