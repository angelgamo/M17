using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Damage", order = 1)]
public class Damage : ScriptableObject
{
    public int damage;
    public int enemyLayer;
}
