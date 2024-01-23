using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles object activation/deactivation
/// </summary>
public class Activator : MonoBehaviour
{
    [Tooltip("Objects to be affected")]
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    private enum ObjectsAction
    {
        activateObjects,
        deactivateObjects
    }
    [Tooltip("How to affect the objects")]
    [SerializeField] private ObjectsAction objectsAction;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (targetObjects.Count == 0)
        {
            return;
        }

        if (!collider.gameObject.GetComponent<PlayerController>())
        {
            return;
        }

        AffectObjects();
    }
    
    /// <summary>
    /// Activates/deactivates the given object
    /// </summary>
    private void AffectObjects()
    {
        if (objectsAction == ObjectsAction.activateObjects)
        {
            foreach(GameObject targetObject in targetObjects)
            {
                targetObject.SetActive(true);
            }
        }

        if (objectsAction == ObjectsAction.deactivateObjects)
        {
            foreach (GameObject targetObject in targetObjects)
            {
                targetObject.SetActive(false);
            }
        }
    }
}
