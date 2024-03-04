using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;
    public float staminaRegenRate; // Stamina regenerated per second
    public float runStaminaCost;

    public Slider staminaBar;

    public bool isRegenStamina = true;
    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
        staminaBar.interactable = false; // Disable interactability
    }

    private void FixedUpdate()
    {
   
        // Regenerate stamina over time
        if (currentStamina < maxStamina)
        {
            UpdateStaminaBar();
        }
        if (isRegenStamina && currentStamina<maxStamina) {
            
            currentStamina += staminaRegenRate;
        
        }
        if (!isRegenStamina && CanPerformAction(runStaminaCost))
        {
            UseStamina(runStaminaCost);
        }
    }

    public void RegenStamina(bool boolean)
    {
        isRegenStamina = boolean;
        
    }
    public bool CanPerformAction(float actionCost)
    {
        // Check if player has enough stamina to perform an action
        return currentStamina >= actionCost;
    }

    public void UseStamina(float actionCost)
    {
        // Use stamina when performing an action
        currentStamina -= actionCost;
     
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina / maxStamina;
    }

}
