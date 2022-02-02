public enum StatModType
{
    Flat = 100,
    Percent = 200,
    PercentAcc = 300,
    PercentMul = 400,
}

public enum StatModCalc
{
    AccumulatedValue,
    BaseValue,
}

[System.Serializable]
public class StatModifier
{
    public float Value;
    public StatModType Type;
    public StatModCalc Calc;
    public int Order = -1;
    public object Source = null;

    public StatModifier() { }

    public StatModifier(float value, StatModType type, StatModCalc calc, int order, object source) : this()
    {
        Value = value;
        Type = type;
        Calc = calc;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, StatModType type, StatModCalc calc, object source) : this(value, type, calc, (int)type, source) { }

    public StatModifier(float value, StatModType type, StatModCalc calc, int order) : this(value, type, calc, order, null) { }

    public StatModifier(float value, StatModType type, StatModCalc calc) : this(value, type, calc, (int)type, null) { }
}
