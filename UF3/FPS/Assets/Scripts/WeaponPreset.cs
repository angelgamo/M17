using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Weapon", order = 1)]
[Serializable]
public class WeaponPreset : ScriptableObject
{
    [Header("Weapon Settings")]
    [Range(0, 250)] public int damage;
    [Range(0f, 2000f), AllowNesting] public float shootVelocity;
    [Range(0f,0.1f)]
    public float dispersion;
    [Space]
    [Range(0, 100)] public int magazineCapacity;
    [Space]
    [Range(0f, 100f)] public float shootDelay;
    [Range(0f, 100f)] public float reloadDelay;
    [Space]
    public bool burst;
    [ShowIf("burst"), Range(0f, 100f)] public float burstDelay;
    [ShowIf("burst"), Range(0, 100)] public int burstQuantity;

    [Header("Bullet Settings")]
    [OnValueChanged("UpdatePreset")] public BulletDiameter preset = BulletDiameter.custom;
    [Range(0.1f, 0.6f)] public float bulletCoefficient = 0.47f;
    [EnableIf("bulletCustom"), Label("Diameter (mm)")] public float diameter;
    [EnableIf("bulletCustom"), Label("Weight (gr)")] public float weight;

    // Extra
    bool bulletCustom = true;

    float[] diameterVal = new float[] { 10, 12, 14, 16, 18, 20, 22, 24, 9.14f, 9.65f, 10, 10.41f, 11.18f, 12.70f };
    float[] weightVal = new float[] { 1.2f, 2, 3.2f, 4.7f, 6.7f, 9.2f, 12.3f, 15.9f, 4.5f, 5.3f, 5.9f, 6.7f, 8.3f, 12.2f };
    void UpdatePreset()
	{
        bulletCustom = preset == BulletDiameter.custom;
        if (bulletCustom)
            return;
        diameter = diameterVal[(int)preset];
        weight = weightVal[(int)preset];
	}
}

public enum BulletDiameter
{
    _10mm, _12mm, _14mm, _16mm, _18mm, _20mm, _22mm, _24mm, _36cal, _38cal, _40cal, _41cal, _44cal, _50cal, custom
}