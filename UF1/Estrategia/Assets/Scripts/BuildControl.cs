using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildControl : MonoBehaviour
{
    public BuildingController.Size size;
    Vector2Int buildSize;
    Vector3Int builtAt;
    bool created = false;

    public enum BuildType { House, Resource, Villager, Enemy }
    public BuildType buildType;

    public int healtPoints;
    public int logsCost;
    public int rocksCost;
    public int goldCost;

    public GameObject villager;

    BuildingController buildingController;

    private void Awake()
    {
        if (size == BuildingController.Size._2x2) buildSize = new Vector2Int(2, 2);
        else if (size == BuildingController.Size._3x3) buildSize = new Vector2Int(3, 3);
        else if (size == BuildingController.Size._3x4) buildSize = new Vector2Int(3, 4);
        else if (size == BuildingController.Size._4x4) buildSize = new Vector2Int(4, 4);
    }

    public void DestroyBuild()
    {
        if (this.created)
        {
            buildingController.DestroyBuild(this);
            Destroy(gameObject);
        }
    }

    public Vector3Int getBuiltAt()
    {
        return this.builtAt;
    }

    public Vector2Int getBuildSize()
    {
        return this.buildSize;
    }

    public void Create(Vector3Int builtAt, BuildingController buildingController)
    {
        this.builtAt = builtAt;
        this.buildingController = buildingController;
        this.created = true;

        if (buildType == BuildType.House) gameObject.AddComponent<HouseManager>();
        else if (buildType == BuildType.Resource) gameObject.AddComponent<ResourceManager>();
        else if (buildType == BuildType.Villager)
        {
            gameObject.AddComponent<VillagerManager>();
            gameObject.GetComponent<VillagerManager>().villager = this.villager;
        }

        gameObject.GetComponent<CombatManager>().healtPoints = healtPoints;

        gameObject.AddComponent<PolygonCollider2D>();
    }
}
