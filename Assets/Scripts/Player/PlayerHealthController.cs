using System.Collections;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    [Header("Immunity settings")]
    public float startImmunityTime = 0.2f;
    public float currentImunityTime;
    public SpriteRenderer sr;
    public Material normalMaterial;
    public Material takeDamageMaterial;

    IEnumerator ChangeMaterial()
    {
        sr.material = takeDamageMaterial;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        if (!UIController.instance.isPaused)
        {
            Time.timeScale = 1;
        }
        sr.material = normalMaterial;
    }
    public void TakeDamage(int damage)
    {
        if (currentImunityTime <= 0)
        {
            currentImunityTime = startImmunityTime;
            currentHealth -= damage;
            UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
            UIController.instance.TakeDamageEffect();
            PlayerController.instance.playerAudio.PlaySFX(Random.Range(49, 55));
            StartCoroutine(ChangeMaterial());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        // Handle player death logic here
        PlayerController.instance.anim.SetTrigger("death");
        PlayerController.instance.isDead = true;
        UIController.instance.ShowDeathScreen();
    }
    private void Revive()
    {
        PlayerController.instance.isDead = false;
        PlayerController.instance.anim.SetTrigger("revive");
        currentHealth = maxHealth;
        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
    }
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (currentImunityTime > 0)
        {
            currentImunityTime -= Time.deltaTime;
        }
    }
}
