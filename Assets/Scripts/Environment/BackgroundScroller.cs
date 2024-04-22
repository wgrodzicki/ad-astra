using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("How fast should the background scroll")]
    [SerializeField] private float speed = 0.0f;

    private Vector3 startingPosition;
    private Vector3 size;
    private float distanceCovered = 0.0f;

    private void Start()
    {
        startingPosition = this.gameObject.transform.position;
        size = this.gameObject.GetComponent<BoxCollider2D>().bounds.size; // Size of the attached collider
    }

    private void Update()
    {
        Scroll();
    }

    /// <summary>
    /// Scrolls the background indefinitely.
    /// </summary>
    private void Scroll()
    {
        if (distanceCovered >= ((size.x / 2) - speed * Time.deltaTime)) // Adjust by speed and deltaTime to avoid breaches
        {
            // Immediately move the whole background to the starting position
            this.gameObject.transform.position = startingPosition;
            distanceCovered = 0.0f;
        }

        // Keep moving the background
        this.gameObject.transform.position -= new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);

        distanceCovered += speed * Time.deltaTime;
    }
}
