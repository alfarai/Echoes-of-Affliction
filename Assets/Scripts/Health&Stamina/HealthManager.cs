using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;


public class HealthManager: MonoBehaviour
{
    public CanvasGroup deathScreen;
    public float maxHealth = 100;
    private float currentHealth;
    private bool isDeathScreenEnabled, isDeathScreenShowing, isPlayerRevived;
    private Character player;

    
    public UnityEvent HealthUIChangeEvent;
    


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Character>();
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
            Die();
            //Debug.Log("player died");
            //invoke death event
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    void Die()
    {
        player.DisableCameraRotation();
        Cursor.lockState = CursorLockMode.Confined;
        player.SetIsAllowedMovement(false);
        isDeathScreenEnabled = true;
        // Handle player death here (e.g., restart level, game over screen, etc.)
        Debug.Log("Player died.");
    }
    public void Revive()
    {
        player.EnableCameraRotation();
        Cursor.lockState = CursorLockMode.Locked;
        isPlayerRevived = true;
        DataHub.PlayerStatus.health = DataHub.PlayerStatus.maxHealth;
        //set local current health
        currentHealth = DataHub.PlayerStatus.health;

        //init health
        HealthUIChangeEvent.Invoke();
        player.SetIsAllowedMovement(true);
    }
    private float GetCurrentHealth()
    {
        return DataHub.PlayerStatus.health;
    }
    void Update()
    {
        if (isDeathScreenEnabled && !isDeathScreenShowing)
        {
            if (!deathScreen.gameObject.activeInHierarchy)
            {
                deathScreen.gameObject.SetActive(true);
            }
            deathScreen.alpha = Mathf.MoveTowards(deathScreen.alpha, 0.6f, 1.0f * Time.deltaTime);
        }
        if(deathScreen.alpha == 0.6f)
        {
            isDeathScreenShowing = true;
        }
        if (isPlayerRevived)
        {
            //off variables for showing death screen
            isDeathScreenEnabled = false;
            isDeathScreenShowing = false;

            deathScreen.alpha = 0f; // disable death screen
            //load respawn point
            Debug.Log("respawn player");
            isPlayerRevived = false;
        }
    }

}
