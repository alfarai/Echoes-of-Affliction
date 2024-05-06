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

    public Canvas barCanvas;
    public CanvasGroup barCanvasGroup;
    bool canvasActive = true;
    public Image progressStamina;
    public Image usedStamina;
    public Camera _cam;


    public bool isRegenStamina = true;

    private bool isShiftHeld, isValueChanged;
    private Color originalRedColor;
    private float elapsed;
    private IEnumerator coroutine,coroutine2;
    private void Start()
    {
        originalRedColor = usedStamina.color;
        barCanvasGroup.alpha = 0;
        _cam = Camera.main;
        currentStamina = maxStamina;
        //progressStamina.interactable = false; // Disable interactability
    }

    
    private void FixedUpdate()
    {
        if (progressStamina.fillAmount == 0)
        {
            usedStamina.color = Color.Lerp(usedStamina.color, Color.red, Mathf.PingPong(Time.time, 0.05f));
            
        }
        else
        {
            usedStamina.color = Color.Lerp(usedStamina.color, originalRedColor, Mathf.PingPong(Time.time, 0.15f));
        }

    }

    void Update()
    {

        if (currentStamina < maxStamina && !isShiftHeld)
        {
            RegenStamina();
        }

        if (CanSprint() && isShiftHeld)
        {
            UseStamina();
        }

        //if stamina is full, hide it
        if (progressStamina.fillAmount == 1)
        {
            barCanvasGroup.alpha = Mathf.MoveTowards(barCanvasGroup.alpha, 0, 2.0f * Time.deltaTime);
        }
        //if stamina isnt full, show it
        if(progressStamina.fillAmount < 1)
        {
            barCanvasGroup.alpha = Mathf.MoveTowards(barCanvasGroup.alpha, 1, 2.0f * Time.deltaTime);
        }

        
        



    }

    private void LateUpdate()
    {
        if (isValueChanged)
        {
            UpdateStaminaBar();
            isValueChanged = false;
        }
        barCanvas.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }

    public void ShiftHeld(bool flag)
    {
        isShiftHeld = flag;
    }
    public bool CanSprint()
    { 
        return (currentStamina > 0);
    }

    public void UseStamina()
    {
        isValueChanged = true;
        currentStamina -= runStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

    }
    private void RegenStamina()
    {
        isValueChanged = true;
        currentStamina += staminaRegenRate;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

    }
    public bool isStaminaHalfway()
    {
        return currentStamina <= 50;
    }
    public bool isStaminaMoreThanHalf()
    {
        return currentStamina > 50;
    }
    private void UpdateStaminaBar()
    {
        progressStamina.fillAmount = currentStamina / 100f;
    }
    public void ShowStaminaBar()
    {
        canvasActive = true;
    }
}
