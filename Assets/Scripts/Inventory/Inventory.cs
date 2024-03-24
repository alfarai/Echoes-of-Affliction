using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private int pointer = 0;
    public int maxSize = 2;

    void Awake()
    {
        DataHub.PlayerStatus.invSlots = maxSize;
    }
    void Start()
    {
        //Debug.Log(inventory[0].itemData.displayName);
    }
    public bool Add(ItemData itemData)
    {
        
        bool flag = false;
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            /*item.AddToInventory();
            Debug.Log($"{item.itemData.displayName}'s count is now: {item.itemSize}");*/
            
            Debug.Log($"{ item.itemData.displayName} is already in inventory!");

        }
        else
        {
            if(inventory.Count < maxSize)
            {
                InventoryItem newItem = new InventoryItem(itemData);

                //it's me hi im the problem its me
                inventory.Add(newItem);
                pointer = inventory.Count - 1; //object held is set automatically once item is added

                itemDictionary.Add(itemData, newItem);
                Debug.Log($"Added {itemData.displayName} to the inventory!");
                flag = true;
            }
            else
            {
                DataHub.PlayerStatus.isInventoryFull = true;
                Debug.Log("Inventory is full!");
            }
            
        }
        return flag;
    }

    public void Remove(ItemData itemData)
    {

        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RmvFrInventory();
            if (item.itemSize == 0)
            {
                DataHub.PlayerStatus.isInventoryFull = false;
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }
    //call this to know current object being held
    public string GetActiveItem()
    {
        //check first if pointer is pointing on empty slot, or inventory is empty. If so, return EmptyObj
        if(inventory.Count == 0 || pointer >= inventory.Count)
        {
            return "EmptyObj";
        }
        
        return inventory[pointer].GetItemDataName();
    }
    public void MovePointerForward()
    {
        ++pointer;
        pointer %= maxSize;
        Debug.Log("Pointer is at " + pointer);
    }
    public void MovePointerBackward()
    {
        --pointer;
        pointer = (pointer + maxSize) % maxSize; //c# modulo with negative number doesn't work as expected
        Debug.Log("Pointer is at " + pointer);
    }

    public InventoryItem GetInventoryItem(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            return item;
        }

        return null;
    }

    public int GetItemSize(ItemData itemData)
    {
        int size = 0;

        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            size = item.itemSize;
        }

        return size;
    }
    public List<InventoryItem> GetInventory()
    {
        return inventory;
    }

    void Update()
    {

    }
}
