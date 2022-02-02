using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Equipment/Consumibles")]
public class Consumibles : Equipment
{
    public List<ConsumibleEffect> effect;
}
