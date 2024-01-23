
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns object at the given position when triggered by another object
/// </summary>
public class ObjectSpawnerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Objects to spawn, picked randomly from the list")]
    public List<GameObject> objectsToSpawn = new List<GameObject>();
    public GameObject spawnPosition;
    public string spawnActivatorTag;
    public bool deactivateOnExit = false;
    private enum SpawnMode
    {
        spawnEndless,
        checkForCollisionsWithTarget
    }
    [SerializeField] private SpawnMode spawnMode;

    [Header("Follow")]
    [Tooltip("Whether the spawner should follow a target")]
    [SerializeField] private bool followTarget = false;
    [Tooltip("Target object to follow")]
    [SerializeField] private GameObject targetToFollow;
    private enum FollowMode
    {
        xAxis,
        yAxis,
        zAxis
    }
    [Tooltip("Which axis position of the target should this spawner follow")]
    [SerializeField] private FollowMode followMode;

    [Header("Endless spawn")]
    [SerializeField] private bool spawnEndless = false;
    [SerializeField] private float spawnInterval = 0.0f;
    [HideInInspector] private bool shouldSpawn = false;
    [HideInInspector] private bool timeToSpawn = false;

    [Header("Spawn on contact")]
    public GameObject targetSpawnControlCollider;
    public bool limitSpawn = false;
    [Tooltip("Max number of objects to spawn in a row")]
    public int maxObjects = 0;

    [HideInInspector] public bool targetCollisionInTrigger = false;
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
    /// Follows the target
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
    /// Spawns the given object
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
