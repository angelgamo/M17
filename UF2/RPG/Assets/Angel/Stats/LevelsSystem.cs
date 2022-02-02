using UnityEngine;

public class LevelsSystem : MonoBehaviour
{
    #region Singleton
    public static LevelsSystem instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [HideInInspector] public int coins;
    [HideInInspector] public int level = 1;
    [HideInInspector] public float currentXp;
    [HideInInspector] public float requiredXp;

    [Header("Text UI")]
    [SerializeField] TMPro.TextMeshProUGUI coinsUI;
    [SerializeField] TMPro.TextMeshProUGUI levelUI;

    float AdditionalMultiplier = 300;
    float powerMultiplier = 2;
    float divisionMultiplier = 7;

	public void UpdateUI()
	{
        levelUI.text = level.ToString();
        coinsUI.text = coins.ToString();
        requiredXp = CalculateRequireXp();
    }

    public void AddCoin()
	{
        coins++;
        UpdateUI();
    }

    public void GainExperience(float xpGained)
	{
        currentXp += xpGained;
        if (currentXp > requiredXp)
            LevelUp();
	}

    void LevelUp()
	{
        level++;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        requiredXp = CalculateRequireXp();
        UpdateUI();

    }

    int CalculateRequireXp()
	{
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
		{
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + AdditionalMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
		}
        return solveForRequiredXp;
	}
}
