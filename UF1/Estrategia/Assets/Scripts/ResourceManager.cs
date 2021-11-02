using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public BuildingController buildingController;

    private void Awake()
    {
        this.buildingController = (BuildingController)GameObject.Find("Player").GetComponent<BuildingController>();
        buildingController.ResourceHouses++;
    }

    private void OnDestroy()
    {
        buildingController.ResourceHouses--;
    }
}
