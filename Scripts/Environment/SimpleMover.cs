using System.Collections;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    [Header("X axis")]
    [Tooltip("Whether the object should move in the X axis")]
    [SerializeField] private bool moveX = false;
    [Tooltip("Movement speed on the X axis")]
    [SerializeField] private float speedX = 0.0f;
    [Tooltip("Whether speed on the X axis should be randomly set between 0.1 and speedX")]
    [SerializeField] private bool randomSpeedX = false;
    [Tooltip("How far should the object move on the X axis")]
    [SerializeField] private float distanceX = 0.0f;
    [Tooltip("Whether distance on the X axis should be randomly set between 0.1 and distanceX")]
    [SerializeField] private bool randomDistanceX = false;
    [Tooltip("Whether the object should go back after reaching distanceX")]
    [SerializeField] private bool boomerangX = false;
    [Tooltip("How long should the object wait before going back after reaching distanceX")]
    [SerializeField] private float intervalX = 0.0f;
    [Tooltip("Whether interval should be randomly set between 0.1 and intervalX")]
    [SerializeField] private bool randomIntervalX = false;
    [Tooltip("Delay before movement on the X axis starts")]
    [SerializeField] private float delayX = 0.0f;
    [Tooltip("Whether delay should be randomly set between 0.1 and delayX")]
    [SerializeField] private bool randomDelayX = false;

    [Header("Y axis")]
    [Tooltip("Whether the object should move in the Y axis")]
    [SerializeField] private bool moveY = false;
    [Tooltip("Movement speed on the Y axis")]
    [SerializeField] private float speedY = 0.0f;
    [Tooltip("Whether speed on the Y axis should be randomly set between 0.1 and speedY")]
    [SerializeField] private bool randomSpeedY = false;
    [Tooltip("How far should the object move on the Y axis")]
    [SerializeField] private float distanceY = 0.0f;
    [Tooltip("Whether distance on the Y axis should be randomly set between 0.1 and distanceY")]
    [SerializeField] private bool randomDistanceY = false;
    [Tooltip("Whether the object should go back after reaching distanceY")]
    [SerializeField] private bool boomerangY = false;
    [Tooltip("How long should the object wait before going back after reaching distanceY")]
    [SerializeField] private float intervalY = 0.0f;
    [Tooltip("Whether interval should be randomly set between 0.1 and intervalY")]
    [SerializeField] private bool randomIntervalY = false;
    [Tooltip("Delay before movement on the Y axis starts")]
    [SerializeField] private float delayY = 0.0f;
    [Tooltip("Whether delay should be randomly set between 0.1 and delayY")]
    [SerializeField] private bool randomDelayY = false;

    [Header("Z axis")]
    [Tooltip("Whether the object should move in the Z axis")]
    [SerializeField] private bool moveZ = false;
    [Tooltip("Movement speed on the Z axis")]
    [SerializeField] private float speedZ = 0.0f;
    [Tooltip("Whether speed on the Z axis should be randomly set between 0.1 and speedZ")]
    [SerializeField] private bool randomSpeedZ = false;
    [Tooltip("How far should the object move on the Z axis")]
    [SerializeField] private float distanceZ = 0.0f;
    [Tooltip("Whether distance on the Z axis should be randomly set between 0.1 and distanceZ")]
    [SerializeField] private bool randomDistanceZ = false;
    [Tooltip("Whether the object should go back after reaching distanceZ")]
    [SerializeField] private bool boomerangZ = false;
    [Tooltip("How long should the object wait before going back after reaching distanceZ")]
    [SerializeField] private float intervalZ = 0.0f;
    [Tooltip("Whether interval should be randomly set between 0.1 and intervalZ")]
    [SerializeField] private bool randomIntervalZ = false;
    [Tooltip("Delay before movement on the Z axis starts")]
    [SerializeField] private float delayZ = 0.0f;
    [Tooltip("Whether delay should be randomly set between 0.1 and delayZ")]
    [SerializeField] private bool randomDelayZ = false;

    public bool GetMoveX()
    {
        return moveX;
    }

    public void SetMoveX(bool value)
    {
        moveX = value;
    }

    public bool GetMoveY()
    {
        return moveY;
    }

    public void SetMoveY(bool value)
    {
        moveY = value;
    }

    public bool GetMoveZ()
    {
        return moveZ;
    }

    public void SetMoveZ(bool value)
    {
        moveZ = value;
    }

    private AxisMover axisX;
    private AxisMover axisY;
    private AxisMover axisZ;

    private bool startX = false;
    private bool startY = false;
    private bool startZ = false;

    private void Start()
    {
        SetInitialValues();
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// Sets initial movement values.
    /// </summary>
    private void SetInitialValues()
    {
        // X settings
        if (moveX)
        {
            // Speed X
            speedX = Mathf.Abs(speedX);
            if (randomSpeedX)
            {
                speedX = Random.Range(0.1f, speedX);
            }
            // Distance X
            if (randomDistanceX)
            {
                distanceX = Random.Range(0.1f, distanceX);
            }
            // Interval X
            intervalX = Mathf.Abs(intervalX);
            if (randomIntervalX)
            {
                intervalX = Random.Range(0.1f, intervalX);
            }
            // Delay X
            delayX = Mathf.Abs(delayX);
            if (randomDelayX)
            {
                delayX = Random.Range(0.1f, delayX);
            }
            axisX = new AxisMover(this.gameObject, this.gameObject.transform.position.x, new Vector3(speedX, 0.0f, 0.0f), distanceX, boomerangX, intervalX);
        }

        // Y settings
        if (moveY)
        {
            // Speed Y
            speedY = Mathf.Abs(speedY);
            if (randomSpeedY)
            {
                speedY = Random.Range(0.1f, speedY);
            }
            // Distance Y
            if (randomDistanceY)
            {
                distanceY = Random.Range(0.1f, distanceY);
            }
            // Interval Y
            intervalY = Mathf.Abs(intervalY);
            if (randomIntervalY)
            {
                intervalY = Random.Range(0.1f, intervalY);
            }
            // Delay Y
            delayY = Mathf.Abs(delayY);
            if (randomDelayY)
            {
                delayY = Random.Range(0.1f, delayY);
            }
            axisY = new AxisMover(this.gameObject, this.gameObject.transform.position.y, new Vector3(0.0f, speedY, 0.0f), distanceY, boomerangY, intervalY);
        }

        // Z settings
        if (moveZ)
        {
            // Speed Z
            speedZ = Mathf.Abs(speedZ);
            if (randomSpeedZ)
            {
                speedZ = Random.Range(0.1f, speedZ);
            }
            // Distance Z
            if (randomDistanceZ)
            {
                distanceZ = Random.Range(0.1f, distanceZ);
            }
            // Interval Z
            intervalZ = Mathf.Abs(intervalZ);
            if (randomIntervalZ)
            {
                intervalZ = Random.Range(0.1f, intervalZ);
            }
            // Delay 
            delayZ = Mathf.Abs(delayZ);
            if (randomDelayZ)
            {
                delayZ = Random.Range(0.1f, delayZ);
            }
            axisZ = new AxisMover(this.gameObject, this.gameObject.transform.position.z, new Vector3(0.0f, 0.0f, speedZ), distanceZ, boomerangZ, intervalZ);
        }
    }

    /// <summary>
    /// Handles movement on axes.
    /// </summary>
    private void Move()
    {
        if (moveX)
        {
            StartCoroutine(WaitBeforeMove(delayX, "X"));
            if (startX)
            {
                axisX.MoveOnAxis(this.gameObject.transform.position.x);
            }
        }

        if (moveY)
        {
            StartCoroutine(WaitBeforeMove(delayY, "Y"));
            if (startY)
            {
                axisY.MoveOnAxis(this.gameObject.transform.position.y);
            }
        }

        if (moveZ)
        {
            StartCoroutine(WaitBeforeMove(delayZ, "Z"));
            if (startZ)
            {
                axisZ.MoveOnAxis(this.gameObject.transform.position.z);
            }
        }
    }

    /// <summary>
    /// Delays the start of the movement.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    private IEnumerator WaitBeforeMove(float delay, string axis)
    {
        yield return new WaitForSeconds(delay);

        switch (axis)
        {
            case "X":
                startX = true;
                break;
            case "Y":
                startY = true;
                break;
            case "Z":
                startZ = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Represents movement on an axis.
    /// </summary>
    class AxisMover
    {
        private GameObject targetObject;
        private float initialPosition;
        private float targetPosition;
        private Vector3 moveVector;
        private float distance;
        private bool boomerang;
        private float interval;

        private bool targetPositionSet = false;
        private bool directionChanged = false;

        private bool wait = false;
        private bool timerSet = false;
        private float waitTimer = 0.0f;
        private bool stop = false;

        public AxisMover(GameObject targetObject, float initialPosition, Vector3 moveVector, float distance, bool boomerang, float interval)
        {
            this.targetObject = targetObject;
            this.initialPosition = initialPosition;
            this.moveVector = moveVector;
            this.distance = distance;
            this.boomerang = boomerang;
            this.interval = interval;
        }

        /// <summary>
        /// Moves on the axis.
        /// </summary>
        /// <param name="currentPosition"></param>
        public void MoveOnAxis(float currentPosition)
        {
            // Set target position to move to
            if (!targetPositionSet)
            {
                targetPosition = initialPosition + distance;
                targetPositionSet = true;
            }

            // Positive target position relative to the initial position
            if (targetPosition >= initialPosition)
            {
                HandlePositivePosition(currentPosition, targetPosition);

                if (boomerang)
                {
                    HandleNegativePosition(currentPosition, initialPosition);
                }

                if (currentPosition < targetPosition && currentPosition > initialPosition)
                {
                    directionChanged = false; // Unlock direction change
                }
            }
            // Negative target position relative to the initial position
            else
            {
                HandleNegativePosition(currentPosition, targetPosition);

                if (boomerang)
                {
                    HandlePositivePosition(currentPosition, initialPosition);

                    if (currentPosition > targetPosition && currentPosition < initialPosition)
                    {
                        directionChanged = false; // Unlock direction change
                    }
                }
                else
                {
                    ChangeDirection();
                }
            }

            if (!wait && !stop)
            {
                targetObject.transform.position += moveVector * Time.deltaTime; // Move
            }
        }

        /// <summary>
        /// Handles direction change if current position is greater or equal to the end positon.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="endPosition"></param>
        private void HandlePositivePosition(float currentPosition, float endPosition)
        {
            if (currentPosition >= endPosition)
            {
                if (boomerang)
                {
                    ChangeDirection();
                }
                else
                {
                    stop = true; // Stop moving
                    return;
                }
            }
        }

        /// <summary>
        /// Handles direction change if current position is smaller than end positon.
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="endPosition"></param>
        private void HandleNegativePosition(float currentPosition, float endPosition)
        {
            if (currentPosition <= endPosition)
            {
                if (boomerang)
                {
                    ChangeDirection();
                }
                else
                {
                    stop = true; // Stop moving
                    return;
                }
            }
        }

        /// <summary>
        /// Changes movement direction.
        /// </summary>
        private void ChangeDirection()
        {
            if (directionChanged)
            {
                return;
            }

            wait = IntervalTimer();

            if (!wait)
            {
                moveVector *= -1;
                directionChanged = true; // Mark direction as changed
            }
        }

        /// <summary>
        /// Stops movement for the given time interval.
        /// </summary>
        /// <returns></returns>
        private bool IntervalTimer()
        {
            wait = true;

            if (!timerSet)
            {
                waitTimer = Time.time;
                timerSet = true; // Mark timer as set
            }

            if (Time.time - waitTimer >= interval)
            {
                waitTimer = 0.0f;
                timerSet = false;
                wait = false;
            }

            return wait;
        }
    }
}
