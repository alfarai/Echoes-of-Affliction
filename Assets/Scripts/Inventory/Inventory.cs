using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    public void Add(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToInventory();
            Debug.Log($"{item.itemData.displayName}'s count is now: {item.itemSize}");

        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);

            //it's me hi im the problem its me
            inventory.Add(newItem);

            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added new {itemData.displayName} x1 to the inventory!");
        }
    }

    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RmvFrInventory();
            if (item.itemSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
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

    void Update()
    {

    }
}
