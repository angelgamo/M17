using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using static DungeonUtils;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] List<Placeable> placeables;
    [SerializeField] GameObject ground;

    [Header("Grid Settings")]
    [SerializeField] int size;
    [SerializeField] int cellSize;
    [SerializeField] int start;

    [Header("Customization")]
    [SerializeField] bool manualSeed;
    [SerializeField, HideIf("manualSeed"), ReadOnly] int seed;
    [SerializeField, ShowIf("manualSeed")] int _customSeed;
    [SerializeField] bool gradientGroundColor;
    [SerializeField, ShowIf("gradientGroundColor")] Gradient spawnGround;
    [SerializeField] Expansion expansionType;
    [SerializeField, CurveRange(EColor.Orange), ShowIf("_exp1")] AnimationCurve expansionCenterToBorder;
    [SerializeField, CurveRange(EColor.Orange), ShowIf("_exp2")] AnimationCurve expansionIndex;
    bool _exp1 => expansionType == Expansion.DistanceFromCenter;
    bool _exp2 => expansionType == Expansion.ChildIndex;

    System.Random rand = new System.Random();

    int[,] map;

    [Button]
    public void Generate()
	{
        float startTime = Time.time;

        seed = manualSeed ? _customSeed : rand.GetHashCode();
        rand = new System.Random(seed);


        Clear();
        // Inicialize
        map = new int[size, size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                map[y, x] = -1;

        Vector2Int center = new Vector2Int(size/2, size / 2);
        SpawnPrefab(center, 0, placeables[start]);

        Achus(center, start, 0);

        print("execution time: " + (Time.time - startTime));
    }

	void Achus(Vector2Int pos, int index, int childIndex)
	{

        if (placeables[index].top)
            SpawnPrefab(pos + new Vector2Int(1, 0), childIndex+1);

        if (placeables[index].rig)
            SpawnPrefab(pos + new Vector2Int(0, 1), childIndex + 1);

        if (placeables[index].bot)
            SpawnPrefab(pos + new Vector2Int(-1, 0), childIndex + 1);

        if (placeables[index].lef)
            SpawnPrefab(pos + new Vector2Int(0, -1), childIndex + 1);
    }

    public bool CheckPosition(Vector2Int pos, int side, int childIndex)
	{
        // check bounds
        if (pos.y < 0 || pos.x < 0 || pos.y >= size || pos.x >= size)
            return false;

        // check empty
        if (map[pos.y, pos.x] == -1)
            return RandomBool(pos, childIndex);

        switch (side) // return value of oposite side
        {
            case 0:
                return placeables[map[pos.y, pos.x]].bot;
            case 1:
                return placeables[map[pos.y, pos.x]].lef;
            case 2:
                return placeables[map[pos.y, pos.x]].top;
            case 3:
                return placeables[map[pos.y, pos.x]].rig;
        }
            
        return false;
	}

    bool RandomBool(Vector2Int pos, int childIndex)
	{
        if (expansionType == Expansion.Random)
		{
            return rand.NextDouble() > 0.5f;
        }
        else if (expansionType == Expansion.DistanceFromCenter)
		{
            float x = (float)pos.x / size * 2 - 1;
            float y = (float)pos.y / size * 2 - 1;
            float t = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
            return rand.NextDouble() < expansionCenterToBorder.Evaluate(t);
        }
        else
		{
            float t = Mathf.Clamp01((float)childIndex / (size * 2));
            return rand.NextDouble() < expansionIndex.Evaluate(t);
        }
    }

    void SpawnPrefab(Vector2Int pos, int childIndex, Placeable placeable = null)
	{
        // Check bounds
        if (pos.y < 0 || pos.x < 0 || pos.y >= size || pos.x >= size)
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

            placeable = placeables.Where(p => p.top == top && p.rig == rig && p.bot == bot && p.lef == lef).First();
        }
        
        int index = placeables.IndexOf(placeable);
        map[pos.y, pos.x] = index;

        // Ground
        GameObject ground = (GameObject)PrefabUtility.InstantiatePrefab(this.ground, transform);
        PrefabUtility.UnpackPrefabInstance(ground, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        ground.transform.position = new Vector3(pos.y * cellSize, 0, pos.x * cellSize);
        if (gradientGroundColor)
            ground.GetComponent<MeshRenderer>().material.color = spawnGround.Evaluate((float)childIndex / (size * 2));

        // Room
        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(placeable.prefab, transform);
        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        go.transform.position = new Vector3(pos.y * cellSize, 0, pos.x * cellSize);

        Achus(pos, index, childIndex);
    }

    [Button]
    public void Clear()
    {
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);
    }
}