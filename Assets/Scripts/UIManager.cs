using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealthBar()
    {
        
        healthBar.value = DataHub.PlayerStatus.health / (float)DataHub.PlayerStatus.maxHealth;
        healthBar.interactable = false; // Disable interactability
    }
}
