using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DungeonUtils
{   public enum Expansion
    {
        Random,
        DistanceFromCenter,
        ChildIndex
    }

    [System.Serializable]
    public class Placeable
    {
        [ShowAssetPreview] public GameObject prefab;
        public bool top;
        public bool bot;
        public bool rig;
        public bool lef;
    }

}
