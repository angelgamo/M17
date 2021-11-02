using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    Player player;

    int villagersCapacityValue = 5;

    private void Start()
    {
        this.player = (Player)GameObject.Find("Player").GetComponent<PlayerManager>().player;
        AddVillagerCapacity2Player();
    }

    private void OnDestroy()
    {
        RemoveVillagerCapacity2Player();
    }

    private void AddVillagerCapacity2Player()
    {
        player.villagersCapacityCount += villagersCapacityValue;
    }

    private void RemoveVillagerCapacity2Player()
    {
        player.villagersCapacityCount -= villagersCapacityValue;
    }
}
