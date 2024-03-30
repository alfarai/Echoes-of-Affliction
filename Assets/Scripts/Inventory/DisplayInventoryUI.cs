using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DisplayInventoryUI : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public GameObject playerObj;
    private Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = playerObj.GetComponent<Inventory>();
        UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateInventoryUI()
    {
        string display = "";
        List<InventoryItem> inventory = inv.GetInventory();
        for(int i = 0; i < DataHub.PlayerStatus.invSlots; i++)
        {
            //Debug.Log(inventory[i].GetItemDataName());
            display = inventory[i].GetItemDataName();
            if (display == null)
            {
                display = "Empty";
            }
            
            inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>().text = display;
            inventorySlots[i].GetComponent<Image>().sprite = inventory[i].GetIcon();
        }
    }
}
