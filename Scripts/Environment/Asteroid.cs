using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Tooltip("Tags of objects to ignore when colliding")]
    [SerializeField] List<string> ignoredObjects = new List<string>();
    [Tooltip("Max asteroid scale")]
    [SerializeField] float maxScale = 2.0f;
    [Tooltip("Asteroid X axis speed range ")]
    [SerializeField] float rangeX = 5.0f;
    [Tooltip("Rotation script")]
    [SerializeField] RotateAround rotateScript;
    [Tooltip("Vfx to be spawned on asteroid hit")]
    [SerializeField] GameObject asteroidVfx;
    [Tooltip("Vfx scale modifier")]
    [SerializeField] float vfxScaleMod = 2.0f;

    private Vector3 horizontalShift;

    private void Awake()
    {
        SetRandomRotation();
        SetRandomScale();
        SetRandomHorizontalShift();
    }

    private void Update()
    {
        this.gameObject.transform.position += horizontalShift * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (ignoredObjects.Contains(collider.gameObject.tag))
        {
            return;
        }

        GameObject explosion = Instantiate(asteroidVfx, transform.position, transform.rotation, null);
        explosion.transform.localScale = this.gameObject.transform.localScale * vfxScaleMod;

        if (collider.tag != "Player")
        {
            Destroy(collider.gameObject);
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Sets rotation direction randomly.
    /// </summary>
    private void SetRandomRotation()
    {
        int randomDirection = Random.Range(-1, 1);
        if (randomDirection == 0)
        {
            randomDirection = 1;
        }
        rotateScript.speed *= randomDirection;
    }

    /// <summary>
    /// Sets scale randomly.
    /// </summary>
    private void SetRandomScale()
    {
        float randomScale = Random.Range(1.0f, maxScale);
        this.gameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    /// <summary>
    /// Sets horizontal shift randomly.
    /// </summary>
    private void SetRandomHorizontalShift()
    {
        float randomShift = Random.Range(-rangeX, rangeX);
        horizontalShift = new Vector3(randomShift, 0.0f, 0.0f);
    }
}
