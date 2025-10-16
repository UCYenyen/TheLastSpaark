using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageGiver : MonoBehaviour
{
    [Header("Projectile Settings")]
    public bool shouldMove = false;
    public Rigidbody2D rb;
    public float speed;

    [Header("Freeze Settings")]
    public bool shouldFreezePlayer = false;
    public bool shouldDeactivateCampfire = false;
    public float freezeMeterIncreaseAmt;

    [Header("Deactivation Settings")]
    public bool shouldUseTimer = false;
    public bool shouldDeactivateOnContact = true;
    public float startDeactivateTimer = 5f;
    private float deactivateTimer = 5f;
    void OnEnable()
    {
        deactivateTimer = startDeactivateTimer;
    }
    void FixedUpdate()
    {
        if (shouldMove)
        {
            rb.velocity = transform.right * speed;
        }
        if(shouldUseTimer)
        {
            deactivateTimer -= Time.fixedDeltaTime;
            if (deactivateTimer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.healthController.TakeDamage(1);
            // Play the damage sound effect
            PlayerController.instance.playerAudio.PlaySFX(0);
            PlayerController.instance.playerTorch.gameObject.SetActive(false);
            PlayerController.instance.isFrozen = true;

            if (shouldFreezePlayer)
            {
                PlayerController.instance.IncrementFreezeMeter(freezeMeterIncreaseAmt);
            }
            // Deactivate this object after giving damage
            if (shouldDeactivateOnContact)
            {
                gameObject.SetActive(false);
            }
        }
        if (collision.CompareTag("Campfire"))
        {
            if (shouldDeactivateCampfire)
            {
                collision.GetComponent<Campfire>().DeactivateParent();
            }
        }
    }
}
