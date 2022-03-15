using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using static DungeonUtils;

public class DungeonGeneratorMultiLayerV2 : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Expandable] public DungeonSettings settings;

    System.Random rand = new System.Random();

    int[,] map;

    public int plantas;
    bool plant;
    DungeonGeneratorMultiLayerV2 plantaGen;

    [Button]
    public void GeneratePlants()
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        Clear();

        GameObject newPlanta = new GameObject("Planta " + plantas);
        newPlanta.transform.parent = transform.root;

        DungeonGeneratorMultiLayerV2 gen = (DungeonGeneratorMultiLayerV2)newPlanta.AddComponent(typeof(DungeonGeneratorMultiLayerV2));
        gen.settings = settings;
        gen.plantas = plantas - 1;
        gen.Generate();

        watch.Stop();
        print(this.GetType() + " Execution time: " + watch.ElapsedMilliseconds);
    }

    public void Generate()
    {
        settings.seed = settings.manualSeed ? settings._customSeed : rand.GetHashCode();
        rand = new System.Random(settings.seed);

        plant = plantas > 0;

        Clear();
        // Inicialize
        map = new int[settings.plantaSize, settings.plantaSize];
        for (int y = 0; y < settings.plantaSize; y++)
            for (int x = 0; x < settings.plantaSize; x++)
                map[y, x] = -1;

        Vector2Int center = new Vector2Int(settings.plantaSize / 2, settings.plantaSize / 2);
        SpawnPrefab(center, 0, settings.placeables[settings.startPrefabIndex]);

        Achus(center, settings.startPrefabIndex, 0);

        if (plant)
        {
            plant = false;

            Vector3 pos = transform.GetChild(rand.Next(1, transform.childCount)).position;

            GameObject stairs = (GameObject)PrefabUtility.InstantiatePrefab(this.settings.stairs, transform);
            PrefabUtility.UnpackPrefabInstance(stairs, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            stairs.transform.position = new Vector3(pos.x, transform.position.y,pos.z);

            GameObject newPlanta2 = new GameObject("Planta " + plantas);
            newPlanta2.transform.parent = transform.root;
            newPlanta2.transform.position = new Vector3(stairs.transform.position.x - (settings.plantaSize - 1) * settings.prefabSize * .5f, transform.position.y + 10, stairs.transform.position.z - (settings.plantaSize - 1) * settings.prefabSize * .5f);

            plantaGen = (DungeonGeneratorMultiLayerV2)newPlanta2.AddComponent(typeof(DungeonGeneratorMultiLayerV2));
            plantaGen.settings = settings;
            plantaGen.plantas = plantas - 1;
            plantaGen.Generate();
        }
    }

    void Achus(Vector2Int pos, int index, int childIndex)
    {

        if (settings.placeables[index].top)
            SpawnPrefab(pos + new Vector2Int(1, 0), childIndex + 1);

        if (settings.placeables[index].rig)
            SpawnPrefab(pos + new Vector2Int(0, 1), childIndex + 1);

        if (settings.placeables[index].bot)
            SpawnPrefab(pos + new Vector2Int(-1, 0), childIndex + 1);

        if (settings.placeables[index].lef)
            SpawnPrefab(pos + new Vector2Int(0, -1), childIndex + 1);
    }

    bool CheckPosition(Vector2Int pos, int side, int childIndex)
    {
        // check bounds
        if (pos.y < 0 || pos.x < 0 || pos.y >= settings.plantaSize || pos.x >= settings.plantaSize)
            return false;

        // check empty
        if (map[pos.y, pos.x] == -1)
            return RandomBool(pos, childIndex);

        switch (side) // return value of oposite side
        {
            case 0:
                return settings.placeables[map[pos.y, pos.x]].bot;
            case 1:
                return settings.placeables[map[pos.y, pos.x]].lef;
            case 2:
                return settings.placeables[map[pos.y, pos.x]].top;
            case 3:
                return settings.placeables[map[pos.y, pos.x]].rig;
        }

        return false;
    }

    bool RandomBool(Vector2Int pos, int childIndex)
    {
        if (settings.expansionType == Expansion.Random)
        {
            return rand.NextDouble() > 0.5f;
        }
        else if (settings.expansionType == Expansion.DistanceFromCenter)
        {
            float x = (float)pos.x / settings.plantaSize * 2 - 1;
            float y = (float)pos.y / settings.plantaSize * 2 - 1;
            float t = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
            return rand.NextDouble() < settings.expansionCenterToBorder.Evaluate(t);
        }
        else
        {
            float t = Mathf.Clamp01((float)childIndex / (settings.plantaSize * 2));
            return rand.NextDouble() < settings.expansionIndex.Evaluate(t);
        }
    }

    void SpawnPrefab(Vector2Int pos, int childIndex, Placeable placeable = null)
    {
        // Check bounds
        if (pos.y < 0 || pos.x < 0 || pos.y >= settings.plantaSize || pos.x >= settings.plantaSize)
            return;

        // Chech empty
        if (map[pos.y, pos.x] != -1)
            return;

        if (placeable == null)
        {
            // Select prefab
            bool top = CheckPosition(pos + new Vector2Int(1, 0), 0, childIndex);
            bool rig = CheckPosition(pos + new Vector2Int(0, 1), 1, childIndex);
            bool bot = CheckPosition(pos + new Vector2Int(-1, 0), 2, childIndex);
            bool lef = CheckPosition(pos + new Vector2Int(0, -1), 3, childIndex);

            placeable = settings.placeables.Where(p => p.top == top && p.rig == rig && p.bot == bot && p.lef == lef).First();
        }

        int index = settings.placeables.IndexOf(placeable);
        map[pos.y, pos.x] = index;

        Achus(pos, index, childIndex);

        // Ground
        GameObject ground = (GameObject)PrefabUtility.InstantiatePrefab(this.settings.ground, transform);
        PrefabUtility.UnpackPrefabInstance(ground, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        ground.transform.position = new Vector3(transform.position.x + pos.y * settings.prefabSize, transform.position.y, transform.position.z + pos.x * settings.prefabSize);
        if (settings.gradientGroundColor)
            ground.GetComponent<MeshRenderer>().material.color = settings.spawnGround.Evaluate((float)childIndex / (settings.plantaSize * 2));

        // Room
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(placeable.prefab, transform);
        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        go.transform.position = new Vector3(transform.position.x + pos.y * settings.prefabSize, transform.position.y, transform.position.z + pos.x * settings.prefabSize);
    }

    [Button]
    public void Clear()
    {
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);
    }
}