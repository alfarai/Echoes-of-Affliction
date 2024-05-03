﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    public List<InventoryItem> playerInventory;
    public bool isInventoryFull;
    public int invSlots;
    public int focusedSlot = 0; //default is first slot
   

    public int health;
    public int maxHealth = 100;
    public int damageTaken;

    public bool isAlive;
    public bool isTalking;
    public bool isCutscenePlaying;
}
