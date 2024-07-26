using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Tooltip("Objects that should be detected by the collider")]
    [SerializeField] private string tagToLookFor;
    [HideInInspector] public bool isColliding = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == tagToLookFor)
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == tagToLookFor)
        {
            isColliding = false;
        }
    }
}
