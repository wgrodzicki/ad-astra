using System.Collections.Generic;
using UnityEngine;

public class BossHeartController : MonoBehaviour
{
    [Tooltip("Input manager script")]
    [SerializeField] private InputManager inputManager;
    [Tooltip("Main boss object")]
    [SerializeField] private GameObject boss;
    [Tooltip("Boss hit effect")]
    [SerializeField] private GameObject bossHitEffect;
    [Tooltip("Heart color after deactivation")]
    [SerializeField] private Color deactivationColor = Color.white;

    private List<GameObject> bossAuras;
    private bool heartDeactivated = false;
    private bool heartInteractible = false;

    private void Start()
    {
        bossAuras = boss.GetComponent<BossController>().bossAuras;
    }

    private void Update()
    {
        ActivateHeart();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.GetComponent<PlayerController>())
        {
            return;
        }

        if (heartDeactivated)
        {
            return;
        }

        heartInteractible = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.gameObject.GetComponent<PlayerController>())
        {
            return;
        }

        if (heartDeactivated)
        {
            return;
        }

        heartInteractible = false;
    }

    /// <summary>
    /// Activates the heart.
    /// </summary>
    private void ActivateHeart()
    {
        if (heartDeactivated)
        {
            return;
        }

        if (!heartInteractible)
        {
            return;
        }

        if (inputManager.specialActionButton == 1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = deactivationColor;
            this.gameObject.GetComponent<Animator>().enabled = false;

            if (bossAuras.Count != 0)
            {
                for (int i = bossAuras.Count - 1; i >= 0; i--)
                {
                    if (bossAuras[i] != null)
                    {
                        bossAuras[i].SetActive(false);
                        bossAuras.Remove(bossAuras[i]);
                        break;
                    }
                }
            }

            Instantiate(bossHitEffect, boss.transform.position, boss.transform.rotation);
            heartDeactivated = true;
        }
    }
}
