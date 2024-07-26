using System.Collections.Generic;
using UnityEngine;

public class BossController : Health
{
    [Header("Boss-specific")]
    [Tooltip("Boss auras")]
    public List<GameObject> bossAuras;
    [Tooltip("Level finish script")]
    [SerializeField] GoalPickup levelFinishScript;
    [Tooltip("No key text display object")]
    [SerializeField] ColliderTextDisplay noKeyTextScript;
    [Tooltip("Key found text display object")]
    [SerializeField] GameObject keyFoundObject;
    [Tooltip("Key found text")]
    [SerializeField] GameObject keyFoundText;
    [Tooltip("Player health script")]
    [SerializeField] Health playerHealthScript;
    [Tooltip("UI Manager script")]
    [SerializeField] UIManager interfaceManagerScript;

    /// <summary>
    /// Makes sure the loot is always dropped.
    /// </summary>
    public override void DropLoot()
    {
        if (basicLoot == null)
        {
            return;
        }

        // Drop key
        GameObject bossKey = Instantiate(basicLoot, transform.position, transform.rotation, null);
        bossKey.GetComponent<KeyPickup>().goalScript = levelFinishScript;

        // Key found info
        GameObject bossKeyInfo = Instantiate(keyFoundObject, transform.position, transform.rotation, null);
        bossKeyInfo.GetComponent<ColliderTextDisplay>().text = keyFoundText;
        bossKeyInfo.GetComponent<ColliderTextDisplay>().playerHealth = playerHealthScript;
        bossKeyInfo.GetComponent<ColliderTextDisplay>().interfaceManagerScript = interfaceManagerScript;
        bossKeyInfo.GetComponent<ColliderTextDisplay>().displayTime = 4.0f;

        // Get rid of the key not found info
        if (noKeyTextScript != null)
        {
            noKeyTextScript.GetComponent<ColliderTextDisplay>().dependsOnTargetObject = true;
            noKeyTextScript.GetComponent<ColliderTextDisplay>().targetObject = bossKey;
        }
    }
}