using UnityEngine;

[System.Serializable]
public class Resource
{
    public float Value { get { return finalValue; } }
    protected float finalValue;
    public Stat MaxValue;

    public void ModifyValue(float value)
    {
        finalValue = Mathf.Clamp(finalValue + value, 0, MaxValue.Value);
    }

    public virtual float GetPercent(out float value, out float maxValue)
    {
        value = Value;
        maxValue = MaxValue.Value;
        return Value / MaxValue.Value;
    }
}
