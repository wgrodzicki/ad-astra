using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTextDisplay : MonoBehaviour
{
    [Tooltip("Text to be displayed in the UI on collision with this collider")]
    public GameObject text;
    [Tooltip("Whether the display of the text should depend on a presence/lack thereof of a specific object")]
    public bool dependsOnTargetObject = false;
    [Tooltip("Object whose disappearance deactivates the text")]
    public GameObject targetObject;
    [Tooltip("Player's health script")]
    public Health playerHealth;
    [Tooltip("UI Manager script")]
    public UIManager interfaceManagerScript;
    [Tooltip("How long should the text be displayed")]
    public float displayTime = 0.0f;
    [Tooltip("Objects to be destroyed when this text has been displayed")]
    [SerializeField] private List<GameObject> objectsToDestroy = new List<GameObject>();
    [Tooltip("Whether this text should be deactivated after being displayed; otherwise will be destroyed")]
    [SerializeField] private bool deactivateAfterDisplay = false;

    private bool textWasDisplayed = false;

    private void Update()
    {
        CheckPlayerLives();
        CheckPause();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (text == null)
        {
            return;
        }

        if (playerHealth == null)
        {
            return;
        }

        if (collider.tag != "Player")
        {
            return;
        }

        if (dependsOnTargetObject)
        {
            if (targetObject == null)
            {
                return;
            }
        }

        if (!textWasDisplayed)
        {
            text.SetActive(true);
            StartCoroutine(DisplayText());
        }
    }

    /// <summary>
    /// Makes sure player is still alive.
    /// </summary>
    private void CheckPlayerLives()
    {
        // Turn the text off if game over (player is dead)
        if (playerHealth.useLives)
        {
            if (playerHealth.currentLives <= 0 || playerHealth.currentHealth <= 0)
            {
                StopCoroutine(DisplayText());
                text.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Turns the text off if game is paused.
    /// </summary>
    private void CheckPause()
    {
        if (interfaceManagerScript.isPaused)
        {
            StopCoroutine(DisplayText());
            text.SetActive(false);
        }
    }

    /// <summary>
    /// Displays the text for a specific period of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayText()
    {
        yield return new WaitForSeconds(displayTime);
        text.SetActive(false);

        if (objectsToDestroy.Count > 0)
        {
            foreach (GameObject objectToDestroy in objectsToDestroy)
            {
                Destroy(objectToDestroy);
            }
        }

        if (deactivateAfterDisplay)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
