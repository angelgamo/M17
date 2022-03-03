// https://frarees.github.io/default-gist-license

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MinMaxSliderAttribute : PropertyAttribute
{
    public float Min { get; set; }
    public float Max { get; set; }
    public bool DataFields { get; set; } = true;
    public bool FlexibleFields { get; set; } = true;
    public bool Bound { get; set; } = true;
    public bool Round { get; set; } = true;
	public bool V1 { get; }
	public bool V2 { get; }
	public bool V3 { get; }
	public float V4 { get; }
	public float V5 { get; }
	public int V6 { get; }
	public bool V7 { get; }

	public MinMaxSliderAttribute() : this(0, 1)
    {
    }

    public MinMaxSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }

	public MinMaxSliderAttribute(bool v1, bool v2, bool v3, float v4, float v5, int v6, bool v7)
	{
		V1 = v1;
		V2 = v2;
		V3 = v3;
		V4 = v4;
		V5 = v5;
		V6 = v6;
		V7 = v7;
	}
}
