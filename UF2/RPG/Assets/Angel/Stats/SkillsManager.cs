using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    #region Singleton
    public static SkillsManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    PlayerCharacterStats playerCharacterStats;
    PlayerController playerController;
    [SerializeField] GameEvent onItemChanged;

    [SerializeField] TMPro.TextMeshProUGUI pointsUI;
    int points = 15;
    [HideInInspector] public int[] lastBarValues = new int[7];

    [SerializeField] Slider HealthBar;
    [SerializeField] Slider StrengthBar;
    [SerializeField] Slider AgilityBar;
    [SerializeField] Slider IntelligenceBar;
    [SerializeField] Slider PhysicResistBar;
    [SerializeField] Slider MagicResistBar;
    [SerializeField] Slider MoveSpeedBar;

    [SerializeField] AnimationCurve HealthValue;
    [SerializeField] AnimationCurve StrengthValue;
    [SerializeField] AnimationCurve AgilityValue;
    [SerializeField] AnimationCurve IntelligenceValue;
    [SerializeField] AnimationCurve PhysicResistValue;
    [SerializeField] AnimationCurve MagicResistValue;
    [SerializeField] AnimationCurve MoveSpeedValue;

    StatModifier Health;
    StatModifier Strength;
    StatModifier Agility;
    StatModifier Intelligence;
    StatModifier PhysicResist;
    StatModifier MagicResist;
    StatModifier MoveSpeed;

    [SerializeField] Animator animator;

    private void Start()
    {
        playerCharacterStats = PlayerCharacterStats.instance;
        playerController = PlayerController.current;

        UpdateSkills();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            animator.SetBool("isOpen", !animator.GetBool("isOpen"));
        else if (Input.GetKeyDown(KeyCode.Escape))
            animator.SetBool("isOpen", false);
    }

    public void SetVales(int[] values)
	{
        lastBarValues = values;
        HealthBar.value = values[0];
        StrengthBar.value = values[1];
        AgilityBar.value = values[2];
        IntelligenceBar.value = values[3];
        PhysicResistBar.value = values[4];
        MagicResistBar.value = values[5];
        MoveSpeedBar.value = values[6];
        UpdateSkills();
    }

    public void UpdateValues()
	{
        int[] newValues = GetCurrentValues();

        if (newValues.Sum() > points)
		{
            HealthBar.value = lastBarValues[0];
            StrengthBar.value = lastBarValues[1];
            AgilityBar.value = lastBarValues[2];
            IntelligenceBar.value = lastBarValues[3];
            PhysicResistBar.value = lastBarValues[4];
            MagicResistBar.value = lastBarValues[5];
            MoveSpeedBar.value = lastBarValues[6];

            return;
        }

        lastBarValues = newValues;
        UpdateSkills();
    }

    int[] GetCurrentValues()
	{
        return new int[] { (int)HealthBar.value, (int)StrengthBar.value, (int)AgilityBar.value, (int)IntelligenceBar.value, (int)PhysicResistBar.value, (int)MagicResistBar.value, (int)MoveSpeedBar.value };
	}

    void UpdateSkills()
    {
        pointsUI.text = "Points: " + lastBarValues.Sum() + "/" + points;

		try
		{
            playerCharacterStats.Health.MaxValue.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.Strength.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.Agility.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.Intelligence.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.PhysicResist.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.MagicResist.RemoveModifierAllModifiersFromSource(this);
            playerCharacterStats.MoveSpeed.RemoveModifierAllModifiersFromSource(this);

        if (HealthBar.value + StrengthBar.value + AgilityBar.value + IntelligenceBar.value + PhysicResistBar.value + MagicResistBar.value + MoveSpeedBar.value == 0)
        {
            onItemChanged.Raise();
            playerController.UpdateMovement(playerCharacterStats.MoveSpeed.Value);
            return;
        }

        Health = new StatModifier(HealthValue.Evaluate(HealthBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        Strength = new StatModifier(StrengthValue.Evaluate(StrengthBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        Agility = new StatModifier(AgilityValue.Evaluate(AgilityBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        Intelligence = new StatModifier(IntelligenceValue.Evaluate(IntelligenceBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        PhysicResist = new StatModifier(PhysicResistValue.Evaluate(PhysicResistBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        MagicResist = new StatModifier(MagicResistValue.Evaluate(MagicResistBar.value), StatModType.Flat, StatModCalc.BaseValue, this);
        MoveSpeed = new StatModifier(MoveSpeedValue.Evaluate(MoveSpeedBar.value), StatModType.Flat, StatModCalc.BaseValue, this);

        playerCharacterStats.Health.MaxValue.AddModifier(Health);
        playerCharacterStats.Strength.AddModifier(Strength);
        playerCharacterStats.Agility.AddModifier(Agility);
        playerCharacterStats.Intelligence.AddModifier(Intelligence);
        playerCharacterStats.PhysicResist.AddModifier(PhysicResist);
        playerCharacterStats.MagicResist.AddModifier(MagicResist);
        playerCharacterStats.MoveSpeed.AddModifier(MoveSpeed);
        
        onItemChanged.Raise();
        playerController.UpdateMovement(playerCharacterStats.MoveSpeed.Value);

        }
        catch { }
    }

    public void Add(Slider bar)
    {
        bar.value++;
    }

    public void Remove(Slider bar)
    {
        bar.value--;
    }

    public void CloseSkillPanel()
    {
        animator.SetBool("isOpen", false);
    }
}
