using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player player;
    public Player playerPreset;

    BuildingController buildingController;

    private void Start()
    {
        Preset();
        tag = "Blue";
        buildingController = GetComponent<BuildingController>();
        buildingController.player = player;
        buildingController.tag = tag;
        StartCoroutine(DisableDelay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildingController.ChangeBuild(1);
        }
    }

    void Preset()
    {
        player.logsCount = playerPreset.logsCount;
        player.foodCount = playerPreset.foodCount;
        player.rocksCount = playerPreset.rocksCount;
        player.goldCount = playerPreset.goldCount;
        player.villagersCapacityCount = playerPreset.villagersCapacityCount;
        player.villagersCount = playerPreset.villagersCount;
        player.mouseOverUI = playerPreset.mouseOverUI;
    }
    IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(0.1f);
        buildingController.enabled = true;
        yield return new WaitForSeconds(0.1f);
        buildingController.enabled = false;
    }
}
