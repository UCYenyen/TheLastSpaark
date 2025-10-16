using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class BossHealthController : MonoBehaviour
{
    [SerializeField] private BossController theBoss;
    public bool isDead = false;
    public Image[] bossHealthImages;
    public Sprite fullHealthSprite;
    public Sprite emptyHealthSprite;
    public int currentHealth;
    public int maxHealth = 3;

    [Header("Immunity settings")]
    public float startImmunityTime = 0.2f;
    public float currentImunityTime;
    public SpriteRenderer sr;
    public Material normalMaterial;
    public Material takeDamageMaterial;
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        CalculateImmunityTime();
    }
    IEnumerator ChangeMaterial()
    {
        sr.material = takeDamageMaterial;
        yield return new WaitForSeconds(0.1f);
        sr.material = normalMaterial;
    }
    public void TakeDamage(int damage)
    {
        if (currentImunityTime <= 0)
        {
            currentImunityTime = startImmunityTime;
            currentHealth -= damage;
        }
        
        StartCoroutine(ChangeMaterial());
        
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthUI(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI(currentHealth, maxHealth);
    }
    private void CalculateImmunityTime()
    {
        if (currentImunityTime > 0)
        {
            currentImunityTime -= Time.deltaTime;
        }
    }
    private void Die()
    {
        // Handle player death logic here
        theBoss.anim.SetTrigger("death");
        isDead = true;
        theBoss.canMove = false;
        theBoss.shouldAttack = false;
        theBoss.shouldChasePlayer = false;
        gameObject.SetActive(false);
        foreach (GameObject obj in theBoss.gamobjectToDeactivateOnDeath)
        {
            obj.SetActive(false);
        }
    }
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < bossHealthImages.Length; i++)
        {
            if (i < currentHealth)
            {
                bossHealthImages[i].sprite = fullHealthSprite;
            }
            else
            {
                bossHealthImages[i].sprite = emptyHealthSprite;
            }
        }
    }
}
