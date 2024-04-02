using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class HealthManager: MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    
    public UnityEvent HealthUIChangeEvent;
    


    private void Start()
    {
        //set max health
        DataHub.PlayerStatus.maxHealth = maxHealth;

        //set current health
        DataHub.PlayerStatus.health = DataHub.PlayerStatus.maxHealth;

        //set local current health
        currentHealth = DataHub.PlayerStatus.health;

        //init health
        HealthUIChangeEvent.Invoke();
    }

    public void TakeDamage()
    {
        //Debug.Log("ouch");
        currentHealth -= DataHub.PlayerStatus.damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        DataHub.PlayerStatus.health = currentHealth;
        //UpdateHealthBar();
        HealthUIChangeEvent.Invoke();

        if (currentHealth <= 0)
        {
            Debug.Log("player died");
            //invoke death event
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    void Die()
    {
        // Handle player death here (e.g., restart level, game over screen, etc.)
        Debug.Log("Player died.");
    }
    private int GetCurrentHealth()
    {
        return DataHub.PlayerStatus.health;
    }
    
}
