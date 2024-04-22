using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Objects to spawn, picked randomly from the list")]
    [SerializeField] private List<GameObject> objectsToSpawn = new List<GameObject>();
    [Tooltip("How should the objects be spawned")]
    [SerializeField] private SpawnMode spawnMode;
    [Tooltip("Position to spawn the objects at")]
    [SerializeField] private GameObject spawnPosition;
    [Tooltip("What should trigger the spawn")]
    [SerializeField] private string spawnActivatorTag;
    [Tooltip("Whether the spawning should be stopped when the activator exits the trigger")]
    [SerializeField] private bool deactivateOnExit = false;

    [Header("Follow")]
    [Tooltip("Whether the spawner should follow a target")]
    [SerializeField] private bool followTarget = false;
    [Tooltip("Target object to follow")]
    [SerializeField] private GameObject targetToFollow;
    [Tooltip("Which axis position of the target should this spawner follow")]
    [SerializeField] private FollowMode followMode;

    [Header("Endless spawn")]
    [Tooltip("Whether the spawn should never end")]
    [SerializeField] private bool spawnEndless = false;
    [Tooltip("Time interval between spawns")]
    [SerializeField] private float spawnInterval = 0.0f;

    [Header("Spawn on contact")]
    [Tooltip("Spawn trigger collider")]
    [SerializeField] private GameObject targetSpawnControlCollider;
    [Tooltip("Whether the number of spawns should be limited")]
    [SerializeField] private bool limitSpawn = false;
    [Tooltip("Max number of objects to spawn in a row")]
    [SerializeField] private int maxObjects = 0;

    [HideInInspector] public bool targetCollisionInTrigger = false;

    private enum SpawnMode
    {
        spawnEndless,
        checkForCollisionsWithTarget
    }

    private enum FollowMode
    {
        xAxis,
        yAxis,
        zAxis
    }

    private bool shouldSpawn = false;
    private bool timeToSpawn = false;
    private bool spawned = false;

    private void Update()
    {
        if (spawnEndless)
        {
            SpawnObject();
        }
        if (followTarget)
        {
            Follow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != spawnActivatorTag)
        {
            return;
        }

        if (spawnMode == SpawnMode.spawnEndless)
        {
            shouldSpawn = true;
            timeToSpawn = true;
            return;
        }
        else
        {
            targetCollisionInTrigger = true;
            spawned = false;
        }

        if (limitSpawn)
        {
            if (this.gameObject.transform.childCount >= maxObjects)
            {
                return;
            }
        }

        SpawnObject();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != spawnActivatorTag)
        {
            return;
        }

        if (!deactivateOnExit)
        {
            return;
        }

        if (spawnMode == SpawnMode.spawnEndless)
        {
            shouldSpawn = false;
            return;
        }
        else
        {
            targetCollisionInTrigger = false;
        }
    }

    /// <summary>
    /// Follows the target.
    /// </summary>
    private void Follow()
    {
        if (targetToFollow == null)
        {
            return;
        }

        switch (followMode)
        {
            case FollowMode.xAxis:
                this.gameObject.transform.position = new Vector3(targetToFollow.transform.position.x, this.gameObject.transform.position.y,
                                                                 this.gameObject.transform.position.z);
                break;
            case FollowMode.yAxis:
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, targetToFollow.transform.position.y,
                                                                 this.gameObject.transform.position.z);
                break;
            case FollowMode.zAxis:
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
                                                                 targetToFollow.transform.position.z);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Spawns the object.
    /// </summary>
    private void SpawnObject()
    {
        int index = Random.Range(0, objectsToSpawn.Count);

        if (spawnMode == SpawnMode.spawnEndless)
        {
            if (!shouldSpawn)
            {
                return;
            }

            if (!timeToSpawn)
            {
                return;
            }

            Instantiate(objectsToSpawn[index], spawnPosition.transform.position, spawnPosition.transform.rotation, null);
            timeToSpawn = false;
            StartCoroutine(WaitBeforeSpawn(spawnInterval));
        }
        else
        {
            // Check if any spawned objects collide with the target object
            if (targetSpawnControlCollider.GetComponent<CollisionDetector>().isColliding)
            {
                return;
            }

            if (!spawned)
            {
                // Spawn object at the spawn position if no spawned object collide with the target object and nothing has spawned yet
                Instantiate(objectsToSpawn[index], spawnPosition.transform.position, spawnPosition.transform.rotation, null);
                spawned = true;
            }
        }
    }

    private IEnumerator WaitBeforeSpawn(float spawnInterval)
    {
        yield return new WaitForSeconds(spawnInterval);
        timeToSpawn = true;
    }
}
