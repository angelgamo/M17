using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[System.Serializable]
public class Stat
{
    public float BaseValue;
    protected float lastBaseValue = float.MinValue;

    public virtual float Value { 
        get {   
            if (!isValueUpdated || BaseValue != lastBaseValue) {
                lastBaseValue = BaseValue;
                finalValue = CalculateFinalValue();
                isValueUpdated = true;
            }
            return finalValue;
            }
        }

    protected float finalValue;
    protected bool isValueUpdated = false;

    protected List<StatModifier> statModifiers;
    public ReadOnlyCollection<StatModifier> StatModifiers;

    public Stat()
    {
        this.statModifiers = new List<StatModifier>();
        this.StatModifiers = statModifiers.AsReadOnly();
    }

    public Stat(float baseValue) : this()
    {
        this.BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isValueUpdated = false;

        if (mod.Order < 0)
            mod.Order = (int)mod.Type;

        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isValueUpdated = false;
            return true;
        }
        return false;
    }

    public virtual bool RemoveModifierAllModifiersFromSource(object source)
    {
        if (source == null)
            return false;

        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isValueUpdated = false;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        int type = 0;
        int calc = 0;

        if (a.Type < b.Type)
            type = -1;
        else if (a.Type > b.Type)
            type = 1;

        if (a.Calc < b.Calc)
            calc = -1;
        else if (a.Calc > b.Calc)
            calc = 1;

        if (type != 0)
            return type;
        else
            return calc;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        float sumPercentMul = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            // Flat Value
            if (mod.Type == StatModType.Flat)
                finalValue += mod.Value;

            // Percent
            else if (mod.Type == StatModType.Percent)
            {
                // Calculated with baseValue
                if (mod.Calc == StatModCalc.BaseValue)
                    finalValue += this.BaseValue * (mod.Value / 100);

                // Calculated with finalValue
                else if (mod.Calc == StatModCalc.AccumulatedValue)
                    finalValue *= 1 + (mod.Value / 100);
            }

            // Percent Accumulative
            else if (mod.Type == StatModType.PercentAcc)
            {
                // Calculated with baseValue
                if (mod.Calc == StatModCalc.BaseValue)
                {
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAcc || statModifiers[i + 1].Calc != StatModCalc.BaseValue)
                    {
                        finalValue += this.BaseValue * (1 + (sumPercentAdd / 100));
                        sumPercentAdd = 0;
                    }                    
                }

                // Calculated with finalValue
                else if (mod.Calc == StatModCalc.AccumulatedValue)
                {
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAcc || statModifiers[i + 1].Calc != StatModCalc.AccumulatedValue)
                    {
                        finalValue *= 1 + (sumPercentAdd / 100);
                        sumPercentAdd = 0;
                    }
                }
            }


            // Percent Multiplicative
            else if (mod.Type == StatModType.PercentMul)
            {
                // Calculated with baseValue
                if (mod.Calc == StatModCalc.BaseValue)
                {
                    sumPercentMul *= mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAcc || statModifiers[i + 1].Calc != StatModCalc.BaseValue)
                    {
                        finalValue += this.BaseValue * (1 + (sumPercentMul / 100));
                        sumPercentMul = 0;
                    }
                }

                // Calculated with finalValue
                else if (mod.Calc == StatModCalc.AccumulatedValue)
                {
                    sumPercentMul *= mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAcc || statModifiers[i + 1].Calc != StatModCalc.AccumulatedValue)
                    {
                        finalValue *= 1 + (sumPercentMul / 100);
                        sumPercentMul = 0;
                    }
                }
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}
