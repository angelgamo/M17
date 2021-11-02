using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    Player player;

    public GameObject villager;

    int villagerFoodCost = 4;
    float villagerTime = 3f;

    void Start()
    {
        this.player = (Player)GameObject.Find("Player").GetComponent<PlayerManager>().player;
        StartCoroutine(GenerateLogs());
    }

    IEnumerator GenerateLogs()
    {
        while (true)
        {
            if (player.villagersCount < player.villagersCapacityCount && player.foodCount > villagerFoodCost)
            {
                player.villagersCount++;
                player.foodCount -= villagerFoodCost;
                // SPAWN VILLAGER
                Instantiate(villager, transform.position + new Vector3(-.5f, -.5f, 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(villagerTime);
        }
    }
}
