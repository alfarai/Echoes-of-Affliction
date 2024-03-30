using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public GameObject itemText;
    public GameObject playerObj;
    private Inventory inv;

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
            }
        }
        for (int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
        {

            //Debug.Log(inventory[i].GetItemDataName());
            try
            {
                display = inventory[i].GetItemDataName();
                if (display == null)
                {
                    display = "";
                }
                //itemText.GetComponent<TextMeshProUGUI>().text = display;
            }
            catch //if inventory slot is empty
            {
                display = "";
                // itemText.GetComponent<TextMeshProUGUI>().text = display;
            }



            //inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>().text = display;
            try
            {
                inventorySlots[i].GetComponent<Image>().sprite = inventory[i].GetIcon();
            }
            catch
            {
                inventorySlots[i].GetComponent<Image>().sprite = null;
            }

        }


    }
    //focused events are triggered through scrolling or when adding to inv
    public void UpdateFocusedInventoryUI()
    {
        for (int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
        {
            inventorySlots[i].GetComponent<Image>().color = new Color32(255, 255, 255,100);
        }

        inventorySlots[DataHub.PlayerStatus.focusedSlot].GetComponent<Image>().color = new Color32(69, 69, 69,50);
    }
}
