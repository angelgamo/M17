using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Objects/Equipment")]
public class Equipment : Item
{
    public EquipSlot equipSlot;

    public StatModifier Strength;       // physical damage
    public StatModifier Agility;        // attack speed
    public StatModifier Intelligence;   // magic damage
    public StatModifier PhysicResist;   // physical damage resistance
    public StatModifier MagicResist;    // magical damage resistance
    public StatModifier MoveSpeed;      // character speed

    public override void Use()
    {
        base.Use();
        RemoveFromInventory();
        EquipmentManager.instance.Equip(this);
    }
    /*
    public StatModifier getStatModifier(StatModifierValues statModifierValues)
    {
        if (statModifierValues.Order <= 0)
            return new StatModifier(statModifierValues.Value, statModifierValues.Type, this);
        else
            return new StatModifier(statModifierValues.Value, statModifierValues.Type, statModifierValues.Order, this);
    }*/
}

[System.Serializable]
public class StatModifierValues
{
    public float Value;
    public StatModType Type;
    public int Order;
}

public enum EquipSlot
{
    Head, Chest, Legs, Weapon, Artifact, Ring, Book
}
