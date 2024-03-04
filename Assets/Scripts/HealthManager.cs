using UnityEngine;
using UnityEngine.UI;

public class HealthManager: MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("ouch");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death here (e.g., restart level, game over screen, etc.)
        Debug.Log("Player died.");
    }

    void UpdateHealthBar()
    {
        healthBar.value = currentHealth / (float)maxHealth;
        healthBar.interactable = false; // Disable interactability
    }
}
