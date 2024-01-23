using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects collision with the specified type of objects
/// </summary>
public class CollisionDetector : MonoBehaviour
{
    [Tooltip("Objects that should be detected by the collider")]
    public string tagToLookFor;
    [HideInInspector] public bool isColliding = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object is a match
        if (collision.tag == tagToLookFor)
        {
            // Detect collision if so
            isColliding = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the exiting object is a match
        if (collision.tag == tagToLookFor)
        {
            // Register lack of collision if so
            isColliding = false;
        } 
    }
}
