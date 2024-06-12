using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles moving the attached game object between waypoints
/// </summary>
public class WaypointMover : SwitchableObject
{
    [Header("Settings")]
    [Tooltip("A list of transforms to move between")]
    public List<Transform> waypoints;
    [Tooltip("How fast to move the platform")]
    public float moveSpeed = 1f;
    [Tooltip("How long to wait when arriving at a waypoint")]
    public float waitTime = 3f;
    [Tooltip("How fast to tilt the platform if triggered")]
    public float tiltSpeed = 0.1f;
    [Tooltip("Maximum tilt in degrees")]
    public float targetRotation = 0.0f;

    // The time at which movement is resumed
    private float timeToStartMovingAgain = 0f;
    // Whether or not the waypoint mover is stopped
    [HideInInspector]
    public bool stopped = false;

    // The previous waypoint or the starting position
    private Vector3 previousTarget;
    // The current waypoint being moved to
    private Vector3 currentTarget;
    // The index of the current Target ub tge waypoints list
    private int currentTargetIndex;
    // The current direction being travelled in
    [HideInInspector]
    public Vector3 travelDirection;

    // Tilting behaviour controllers
    [HideInInspector]
    public bool shouldTilt = false;
    [HideInInspector]
    public bool isTilted = false;
    private Quaternion initialRotation;
    private float degreesToTargetTilt = 0.0f;
    private float degreesToInitialRotation = 0.0f;

    public override void Switch(bool value)
    {
        base.Switch(value);
        shouldTilt = value;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        InitializeInformation();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    void Update()
    {
        ProcessMovementState();
    }

    /// <summary>
    /// Description:
    /// Processes current state and does movement accordingly
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void ProcessMovementState()
    {
        if (shouldTilt)
        {
            Tilt();
        }
        else if (stopped)
        {
            StartCheck();
        }
        else
        {
            Travel();
        }
    }


    /// <summary>
    /// Description:
    /// Checks to see if the waypoint mover can start movement again
    /// Input:
    /// none:
    /// return:
    /// void (no return)
    /// </summary>
    void StartCheck()
    {
        if (Time.time >= timeToStartMovingAgain)
        {
            stopped = false;
            previousTarget = currentTarget;
            currentTargetIndex += 1;
            if (currentTargetIndex >= waypoints.Count)
            {
                currentTargetIndex = 0;
            }
            currentTarget = waypoints[currentTargetIndex].position;
            CalculateTravelInformation();
        }
    }

    /// <summary>
    /// Description:
    /// Sets up the first previous target and current target
    /// then calls CalculateTravelInformation to initilize travel direction 
    /// Inuputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void InitializeInformation()
    {
        previousTarget = this.transform.position;
        currentTargetIndex = 0;
        if (waypoints.Count > 0)
        {
            currentTarget = waypoints[0].position;
        }
        else
        {
            waypoints.Add(this.transform);
            currentTarget = previousTarget;
        }

        CalculateTravelInformation();

        initialRotation = this.gameObject.transform.rotation;
    }

    /// <summary>
    /// Description:
    /// Calculates the current traveling direction using the previousTarget and the currentTarget
    /// Inuputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void CalculateTravelInformation()
    {
        travelDirection = (currentTarget - previousTarget).normalized;
    }

    /// <summary>
    /// Description:
    /// Translates the transform in the direction towards the next waypoint
    /// Input:
    /// none
    /// Returns:
    /// void
    /// </summary>
    void Travel()
    {
        transform.Translate(travelDirection * moveSpeed * Time.deltaTime);
        bool overX = false;
        bool overY = false;
        bool overZ = false;

        Vector3 directionFromCurrentPositionToTarget = currentTarget - transform.position;

        if (directionFromCurrentPositionToTarget.x == 0 || Mathf.Sign(directionFromCurrentPositionToTarget.x) != Mathf.Sign(travelDirection.x))
        {
            overX = true;
            transform.position = new Vector3(currentTarget.x, transform.position.y, transform.position.z);
        }
        if (directionFromCurrentPositionToTarget.y == 0 || Mathf.Sign(directionFromCurrentPositionToTarget.y) != Mathf.Sign(travelDirection.y))
        {
            overY = true;
            transform.position = new Vector3(transform.position.x, currentTarget.y, transform.position.z);
        }
        if (directionFromCurrentPositionToTarget.z == 0 || Mathf.Sign(directionFromCurrentPositionToTarget.z) != Mathf.Sign(travelDirection.z))
        {
            overZ = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, currentTarget.z);
        }

        // If we are over the x, y, and z of our target we need to stop
        if (overX && overY && overZ)
        {
            BeginWait();
        }
    }

    /// <summary>
    /// Description:
    /// Starts the waiting, sets up the needed variables for waiting
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void BeginWait()
    {
        stopped = true;
        timeToStartMovingAgain = Time.time + waitTime;
    }

    /// <summary>
    /// Handles tilting behaviour
    /// </summary>
    void Tilt()
    {
        // Update tilting degree
        float tiltDegrees = tiltSpeed * Time.deltaTime;

        // Check if marked as tilted
        if (isTilted)
        {
            if (degreesToInitialRotation >= targetRotation)
            {
                degreesToInitialRotation = 0;
                this.gameObject.transform.rotation = initialRotation;
                isTilted = false;
                shouldTilt = false;
                IsSwitching = false;
                return;
            }

            // Otherwise go to the horizontal position
            this.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, -1.0f) * tiltDegrees, Space.Self);
            degreesToInitialRotation += tiltDegrees;
        }

        // Check if not marked as tilted
        if (!isTilted)
        {
            if (degreesToTargetTilt >= targetRotation)
            {
                degreesToTargetTilt = 0;
                isTilted = true; // Mark as tilted to stop further tilting
                return;
            }

            // Otherwise go to the vertical position
            this.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * tiltDegrees, Space.Self);
            degreesToTargetTilt += tiltDegrees;
        }
    }
}
