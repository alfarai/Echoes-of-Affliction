﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;
    public bool isDuplicable;
    
}
