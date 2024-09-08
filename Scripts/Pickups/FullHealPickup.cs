using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collission)
    {
        // Only heal if colliding with the player
        if (collission.tag == "Player")
        {
            Health playerHealth = collission.gameObject.GetComponent<Health>();
            playerHealth.ReceiveHealing(playerHealth.maximumHealth);
            Destroy(this.gameObject);
        }
    }
}
