using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scrolls the background by a given speed
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("How fast should the background scroll")]
    public float speed = 0.0f;

    private Vector3 startingPosition;
    private Vector3 size;
    private float distanceCovered = 0.0f;


    void Start()
    {
        startingPosition = this.gameObject.transform.position;
        size = this.gameObject.GetComponent<BoxCollider2D>().bounds.size; // Size of the attached collider
    }
    
    void Update()
    {
        Scroll();
    }

    /// <summary>
    /// Scroll the background indefinitely
    /// </summary>
    private void Scroll()
    {   
        // Check if enough distance has been covered
        if (distanceCovered >= ((size.x / 2) - speed * Time.deltaTime)) // Adjust by speed and deltaTime to avoid breaches
        {
            // Immediately move the whole background to the starting position
            this.gameObject.transform.position = startingPosition;
            // Reset the counter
            distanceCovered = 0.0f;
        }

        // Keep moving the background
        this.gameObject.transform.position -= new Vector3(speed * Time.deltaTime, 0.0f, 0.0f) ;

        // Update the counter
        distanceCovered += speed * Time.deltaTime;
    }
}
