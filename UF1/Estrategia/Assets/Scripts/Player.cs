using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Player")]
public class Player : ScriptableObject
{
    public int logsCount = 200;
    public int foodCount = 100;
    public int rocksCount = 50;
    public int goldCount = 25;
    public int villagersCapacityCount = 15;
    public int villagersCount = 0;
    public bool mouseOverUI = false;
}
