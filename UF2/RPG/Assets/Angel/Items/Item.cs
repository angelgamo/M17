using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Objects/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite icon = null;

    [Header("Tooltip")]
    [TextArea(3,8)]
    public string description;

	public virtual void Use()
    {
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
