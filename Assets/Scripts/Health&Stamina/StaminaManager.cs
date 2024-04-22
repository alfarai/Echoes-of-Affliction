using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaManager : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;
    public float staminaRegenRate; // Stamina regenerated per second
    public float runStaminaCost;

    public Slider staminaBar;

    public GameObject barCanvas;
    bool canvasActive = true;
    public Image progressStamina;
    public Image usedStamina;
    public Camera _cam;

    public bool isRegenStamina = true;
    private void Start()
    {
        _cam = Camera.main;
        currentStamina = maxStamina;
        UpdateStaminaBar();
        barCanvas.SetActive(canvasActive);
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
            usedStamina.fillAmount = (currentStamina / maxStamina) + 0.07f;
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);

        if (currentStamina >= maxStamina)
        {
            StartCoroutine(hideStaminaWheel());
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);
    }

    IEnumerator hideStaminaWheel()
    {
        yield return new WaitForSeconds(0.05f);
        //canvasActive = false;
        barCanvas.SetActive(canvasActive);
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
