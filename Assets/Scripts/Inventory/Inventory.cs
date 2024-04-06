using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Inventory : MonoBehaviour
{

    public static List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private int pointer = 0;
    public int maxSize = 2;

    private List<string> duplicateTracker = new List<string>();

    public UnityEvent InventoryChangeEvent, ChangeInventoryFocusEvent;

    void Awake()
    {
        DataHub.PlayerStatus.invSlots = maxSize;
    }
    void Start()
    {
        ChangeInventoryFocusEvent.Invoke(); //set first slot as focused on start
        //Debug.Log(inventory[0].itemData.displayName);
    }
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            //Debug.Log("scrolling up");
            MovePointerBackward();
            ChangeInventoryFocusEvent.Invoke();

        }
        if (Input.mouseScrollDelta.y < 0)
        {
            //Debug.Log("scrolling down");
            MovePointerForward();
            ChangeInventoryFocusEvent.Invoke();



        }

    }
    public bool Add(ItemData itemData)
    {

        bool flag = false;
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (itemData.isDuplicable)
            {
                if (inventory.Count < maxSize)
                {
                    InventoryItem newItem = new InventoryItem(itemData);

                    //it's me hi im the problem its me
                    inventory.Add(newItem);
                    duplicateTracker.Add(newItem.GetItemDataName());
                    pointer = inventory.Count - 1; //object held is set automatically once item is added

                    //itemDictionary.Add(itemData, newItem);
                    //Debug.Log($"Added {itemData.displayName} to the inventory!");
                    flag = true;

                    DataHub.PlayerStatus.focusedSlot = pointer;
                    DataHub.PlayerStatus.playerInventory = inventory;
                    InventoryChangeEvent.Invoke();
                    ChangeInventoryFocusEvent.Invoke();
                }
                else
                {
                    DataHub.PlayerStatus.isInventoryFull = true;
                    Debug.Log("Inventory is full!");
                }
            }
            else
            {
                Debug.Log($"{ item.itemData.displayName} is already in inventory!");
            }
            /*item.AddToInventory();
            Debug.Log($"{item.itemData.displayName}'s count is now: {item.itemSize}");*/



        }
        else
        {
            if (inventory.Count < maxSize)
            {
                InventoryItem newItem = new InventoryItem(itemData);

                //it's me hi im the problem its me
                inventory.Add(newItem);
                pointer = inventory.Count - 1; //object held is set automatically once item is added

                itemDictionary.Add(itemData, newItem);
                //Debug.Log($"Added {itemData.displayName} to the inventory!");
                flag = true;

                DataHub.PlayerStatus.focusedSlot = pointer;
                DataHub.PlayerStatus.playerInventory = inventory;
                InventoryChangeEvent.Invoke();
                ChangeInventoryFocusEvent.Invoke();
            }
            else
            {
                DataHub.PlayerStatus.isInventoryFull = true;
                Debug.Log("Inventory is full!");
            }

        }
        //Debug.Log(inventory.Count);
        return flag;
    }

    public void Remove(ItemData itemData)
    {


        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            //item.RmvFrInventory();



            Debug.Log("Dropping item");
            DataHub.PlayerStatus.isInventoryFull = false;
            Debug.Log(item.itemData.displayName);

            //find specific item to remove from inventory that player wishes to remove
            //if we directly do inventory.remove, it will return false on the 2nd drop if it had duplicates because we're removing a copy, not the same object: https://stackoverflow.com/questions/10971167/list-remove-in-c-sharp-does-not-remove-item
            
            var itemToRemove = inventory.FirstOrDefault<InventoryItem>(x => x.itemData.id == item.itemData.id);
            if(itemToRemove != null)
            {
                //remove that item
                inventory.Remove(itemToRemove);
            }
            
            //Debug.Log(x);


            if (!itemData.isDuplicable)
            {
                Debug.Log("Deleting dict not dupe");
                //if item is not duplicable, remove from dictionary
                itemDictionary.Remove(itemData);
            }
            else //if item is duplicable
            {

                if (!duplicateTracker.Remove(item.GetItemDataName()))
                {
                    Debug.Log("Deleting dict");
                    itemDictionary.Remove(itemData);

                }







            }




            DataHub.PlayerStatus.playerInventory = inventory;
            InventoryChangeEvent.Invoke();
        }
    }
    //call this to know current object being held
    public string GetActiveItem()
    {
        //check first if pointer is pointing on empty slot, or inventory is empty. If so, return EmptyObj
        if (inventory.Count == 0 || pointer >= inventory.Count)
        {
            return "EmptyObj";
        }

        return inventory[pointer].GetItemDataName();
    }
    public void MovePointerForward()
    {
        ++pointer;
        pointer %= maxSize;
        DataHub.PlayerStatus.focusedSlot = pointer;


    }
    public void MovePointerBackward()
    {
        --pointer;
        pointer = (pointer + maxSize) % maxSize; //c# modulo with negative number doesn't work as expected
        DataHub.PlayerStatus.focusedSlot = pointer;

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


}
