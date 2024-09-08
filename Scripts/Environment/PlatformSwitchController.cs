using UnityEngine;

public class PlatformSwitchController : MonoBehaviour
{
    [Tooltip("Input manager script")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("Script in the controlled platform to be accessed by this switch")]
    [SerializeField] private WaypointMover platformMoverScript;
    [Tooltip("Switch spatial orientation")]
    [SerializeField] private Orientation orientation;
    [Tooltip("How quickly the switch moves")]
    [SerializeField] private float movingSpeed = 0.0f;
    [Tooltip("How far should the switch hide relative to its length")]
    [SerializeField] private float hideDepth = 0.5f;
    [Tooltip("Whether the switch should return to the default position immediately after activation")]
    [SerializeField] private bool returns = false;
    [Tooltip("Switch sound effect")]
    [SerializeField] private GameObject switchEffect;

    private enum Orientation
    {
        verticalBottom,
        verticalTop,
        horizontalLeft,
        horizontalRight
    }

    // Switch length
    private float length = 0.0f;

    // How far has the switch moved
    private float distanceDown = 0.0f;
    private float distanceUp = 0.0f;

    // State controllers
    private bool inputDetected = false;
    private bool switchReady = false;
    private bool isMoving = false;
    private bool hidden = false;
    private bool visible = true;
    private bool shouldHide = false;
    private bool shouldShow = false;

    private void Start()
    {
        length = this.gameObject.transform.localScale.y;
    }

    private void Update()
    {
        CheckInput();
        CheckIfReady();
        HideSwitch();
        ShowSwitch();
    }

    /// <summary>
    /// Checks for user input (action key).
    /// </summary>
    private void CheckInput()
    {
        if (!switchReady)
        {
            return;
        }

        if (inputManager.specialActionButton == 1)
        {
            inputDetected = true;
        }
    }

    /// <summary>
    /// Checks if the switch is ready to be used.
    /// </summary>
    private void CheckIfReady()
    {
        if (!switchReady)
        {
            return;
        }

        if (!inputDetected)
        {
            return;
        }

        // Wait until the platform finishes tilting
        if (platformMoverScript.shouldTilt)
        {
            return;
        }

        if (!isMoving)
        {
            if (visible)
            {
                // Mark switch for hiding if still visible
                shouldHide = true;
                // Play sound effect
                Instantiate(switchEffect);
            }

            if (hidden)
            {
                // Mark switch for showing if still hidden
                shouldShow = true;
            }

            // Stop and tilt the platform
            platformMoverScript.shouldTilt = true;
            isMoving = true;
        }
    }

    /// <summary>
    /// Hides the switch.
    /// </summary>
    private void HideSwitch()
    {
        if (platformMoverScript == null)
        {
            return;
        }

        if (!shouldHide)
        {
            return;
        }

        // Check if moved down far enough
        if (distanceDown >= length * hideDepth)
        {
            hidden = true;
            visible = false;
            shouldHide = false;

            // Immediately return to the default position, otherwise wait for another trigger
            if (returns)
            {
                shouldShow = true;
            }

            distanceDown = 0.0f; // Reset distance counter
            return;
        }

        // Keep moving the switch down and updating the distance counter
        switch (orientation)
        {
            case Orientation.verticalBottom:
                this.gameObject.transform.position -= new Vector3(0.0f, movingSpeed);
                break;
            case Orientation.verticalTop:
                this.gameObject.transform.position += new Vector3(0.0f, movingSpeed);
                break;
            case Orientation.horizontalLeft:
                this.gameObject.transform.position -= new Vector3(movingSpeed, 0.0f);
                break;
            case Orientation.horizontalRight:
                this.gameObject.transform.position += new Vector3(movingSpeed, 0.0f);
                break;
        }
        distanceDown += movingSpeed;
    }

    /// <summary>
    /// Shows the switch.
    /// </summary>
    private void ShowSwitch()
    {
        if (platformMoverScript == null)
        {
            return;
        }

        if (!shouldShow)
        {
            return;
        }

        // Check if moved up far enough
        if (distanceUp >= length * hideDepth)
        {
            visible = true;
            hidden = false;
            shouldShow = false;
            distanceUp = 0.0f; // Reset distance counter
            inputDetected = false;
            isMoving = false;
            return;
        }

        // Keep moving the switch up and updating the distance counter
        switch (orientation)
        {
            case Orientation.verticalBottom:
                this.gameObject.transform.position += new Vector3(0.0f, movingSpeed);
                break;
            case Orientation.verticalTop:
                this.gameObject.transform.position -= new Vector3(0.0f, movingSpeed);
                break;
            case Orientation.horizontalLeft:
                this.gameObject.transform.position += new Vector3(movingSpeed, 0.0f);
                break;
            case Orientation.horizontalRight:
                this.gameObject.transform.position -= new Vector3(movingSpeed, 0.0f);
                break;
        }
        distanceUp += movingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Player")
        {
            return;
        }

        if (platformMoverScript == null)
        {
            return;
        }

        switchReady = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Player")
        {
            return;
        }

        if (platformMoverScript == null)
        {
            return;
        }

        switchReady = false;
    }
}
