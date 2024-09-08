using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [Tooltip("Objects to be affected")]
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    [Tooltip("How to affect the objects")]
    [SerializeField] private ObjectsAction objectsAction;

    private enum ObjectsAction
    {
        activateObjects,
        deactivateObjects
    }

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
    /// Activates/deactivates the given object.
    /// </summary>
    private void AffectObjects()
    {
        if (objectsAction == ObjectsAction.activateObjects)
        {
            foreach (GameObject targetObject in targetObjects)
            {
                if (targetObject != null)
                    targetObject.SetActive(true);
            }
        }

        if (objectsAction == ObjectsAction.deactivateObjects)
        {
            foreach (GameObject targetObject in targetObjects)
            {
                if (targetObject != null)
                    targetObject.SetActive(false);
            }
        }
    }
}
