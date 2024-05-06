using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    public CanvasGroup menuCanvas;
    public Toggle lowToggle, medToggle, highToggle;
    private bool isStarted;

    void Awake()
    {
        DataHub.PlayerStatus.GameNotLoaded = true;
        Cursor.lockState = CursorLockMode.Confined;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            menuCanvas.alpha = Mathf.MoveTowards(menuCanvas.alpha, 0, 0.8f * Time.deltaTime);
        }
        if(menuCanvas.alpha == 0)
        {
            isStarted = false;
        }
        
    }
    public void StartGame()
    {
        isStarted = true;
        SceneManager.LoadSceneAsync("Main");
    }

   

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ApplyQuality()
    {
        if (lowToggle.isOn)
        {
            QualitySettings.SetQualityLevel(1, true);
            
        }
        if (medToggle.isOn)
        {
            QualitySettings.SetQualityLevel(3, true);
            
        }
        if (highToggle.isOn)
        {
            QualitySettings.SetQualityLevel(5, true);
        }

    }


}
