using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayerController.instance.ResetVelocity();
        if (currentHealth <= 0)
        {
            Die();
        }
        UIController.instance.UpdateHealthUI(currentHealth, maxHealth);
        UIController.instance.TakeDamageEffect();
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
}
