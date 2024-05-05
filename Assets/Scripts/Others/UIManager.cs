using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject ui;
    public GameObject[] inventorySlots;
    public GameObject[] inventoryBorderSlots;
    public TextMeshProUGUI itemText;
    public GameObject playerObj;
    public Sprite inactive, active; //png 
    private Inventory inv;
    private bool isUIShown = true;

    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        inv = playerObj.GetComponent<Inventory>();
        //UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (DataHub.PlayerStatus.isTalking || DataHub.PlayerStatus.isCutscenePlaying && isUIShown)
        {
            isUIShown = false;
            ui.SetActive(false);
        }
        else if(DataHub.PlayerStatus.isTalking || !DataHub.PlayerStatus.isCutscenePlaying && !isUIShown)
        {
            isUIShown = true;
            ui.SetActive(true);
        }
    }
    public void UpdateHealthBar()
    {

        healthBar.value = DataHub.PlayerStatus.health / (float)DataHub.PlayerStatus.maxHealth;
        healthBar.interactable = false; // Disable interactability
    }
    public void UpdateInventoryUI()
    {
        string display = "";
        List<InventoryItem> inventory = inv.GetInventory();
        if (inventory.Count == 0)
        {
            for (int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
            {
                inventorySlots[i].GetComponent<Image>().sprite = null;
                inventorySlots[i].GetComponent<Image>().color = new Color(0,0,0,0); //make it invisible
            }
        }
        for (int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
        {
            //inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>().text = display;
            try
            {
                inventorySlots[i].GetComponent<Image>().sprite = inventory[i].GetIcon();
                inventorySlots[i].GetComponent<Image>().color = new Color(0, 0, 0, 255);
            }
            catch
            {
                inventorySlots[i].GetComponent<Image>().sprite = null;
                inventorySlots[i].GetComponent<Image>().color = new Color(0, 0, 0, 0); //make it invisible
            }

        }


    }
    //focused events are triggered through scrolling or when adding to inv
    public void UpdateFocusedInventoryUI()
    {
        List<InventoryItem> inventory = inv.GetInventory();

        for (int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
        {
            inventoryBorderSlots[i].GetComponent<Image>().sprite = inactive;
        }
        //update border of focused slot
        inventoryBorderSlots[DataHub.PlayerStatus.focusedSlot].GetComponent<Image>().sprite = active;

        //check if there is an index out of range exception (meaning that the focused slot is empty)
        try
        {
            //show display name of focused item
            itemText.text = inventory[DataHub.PlayerStatus.focusedSlot].GetItemDataName();
        }
        catch
        {
            itemText.text = "";
        }
        
        //itemText.text = inventory
    }
}
