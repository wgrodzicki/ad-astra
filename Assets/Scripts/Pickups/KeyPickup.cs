﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pickup-derived class which handles a key collectable
/// </summary>
public class KeyPickup : Pickup
{
    [Header("Key Settings")]
    [Tooltip("The ID of the key used to determine which doors it unlocks (unlocks doors with matching IDs)\n" +
        "A key ID of 0 allows the player to open unlocked doors, and is therefore pointless.")]
    public int keyID = 0;

    // Goal pick-up script (WG)
    [Tooltip("Level finish pickup script")]
    public GoalPickup goalScript;

    /// <summary>
    /// Description:
    /// When picked up, adds the key id to the key ring
    /// Input: 
    /// Collider2D collision
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collider that picked up this key</param>
    public override void DoOnPickup(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Health>() != null)
        {
            // Mark the key as found (WG)
            goalScript.keyFound = true;

            KeyRing.AddKey(keyID);
        }
        base.DoOnPickup(collision);
    }
}
