using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int itemSize;
    
    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddToInventory();
    }

    public void AddToInventory()
    {
        itemSize++;
    }

    public void RmvFrInventory()
    {
        itemSize--;
    }
}
