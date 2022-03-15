using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using static DungeonUtils;

[CreateAssetMenu(fileName = "DungeonSettings", menuName = "Dungeon/Settings", order = 1)]
public class DungeonSettings : ScriptableObject
{
    [Header("Prefabs")]
    public List<Placeable> placeables;
    public GameObject ground;
    public GameObject stairs;

    [Header("Grid Settings")]
    public int plantaSize;
    public int prefabSize;
    public int startPrefabIndex;

    [Header("Customization")]
    public bool manualSeed;
    [HideIf("manualSeed"), ReadOnly] public int seed;
    [ShowIf("manualSeed")] public int _customSeed;
    public bool gradientGroundColor;
    [ShowIf("gradientGroundColor")] public Gradient spawnGround;
    public Expansion expansionType;
    [CurveRange(EColor.Orange), ShowIf("_exp1")] public AnimationCurve expansionCenterToBorder;
    [CurveRange(EColor.Orange), ShowIf("_exp2")] public AnimationCurve expansionIndex;
    bool _exp1 => expansionType == Expansion.DistanceFromCenter;
    bool _exp2 => expansionType == Expansion.ChildIndex;
}
