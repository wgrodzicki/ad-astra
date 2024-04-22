using UnityEngine;

public class MovementActivator : MonoBehaviour
{
    [Tooltip("Simple mover script attached to the target object")]
    [SerializeField] SimpleMover moverScript;
    [Tooltip("Movement on which axis should be affected")]
    [SerializeField] string axisToAffect = null;
    [Tooltip("How to affect movement on the given axis")]
    [SerializeField] MovementAction movementAction;
    [Tooltip("Tag of the object that will trigger the action on contact")]
    [SerializeField] string tagToLookFor;

    private enum MovementAction
    {
        activateMovement,
        deactivateMovement
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (moverScript == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(tagToLookFor))
        {
            return;
        }

        if (collider.tag != tagToLookFor)
        {
            return;
        }

        AffectMovement(axisToAffect);
    }

    /// <summary>
    /// Activates/deactivates movement on the given axis.
    /// </summary>
    private void AffectMovement(string axis)
    {
        moverScript.enabled = true;

        switch (axis)
        {
            case "X":
                if (movementAction == MovementAction.activateMovement)
                {
                    moverScript.SetMoveX(true);
                }
                else
                {
                    moverScript.SetMoveX(false);
                }
                break;
            case "Y":
                if (movementAction == MovementAction.activateMovement)
                {
                    moverScript.SetMoveY(true);
                }
                else
                {
                    moverScript.SetMoveY(false);
                }
                break;
            case "Z":
                if (movementAction == MovementAction.activateMovement)
                {
                    moverScript.SetMoveZ(true);
                }
                else
                {
                    moverScript.SetMoveZ(false);
                }
                break;
            default:
                break;
        }
    }
}
