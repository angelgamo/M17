using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout : ScriptableObject
{
    // Levels System
    public int coins;
    public int level;
    public float currentXp;
    public float requiredXp;

    // Inventory
    public Item[] inventory;
    public Equipment[] equipment;

    // Skills
    public int[] skills;
}
