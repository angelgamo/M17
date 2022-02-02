using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacterStats : CharacterStats
{
    #region Singleton
    public static PlayerCharacterStats instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Resource Stamina;

    [Header("Stamina Settings")]
    [SerializeField] float staminaStartDelay;
    [SerializeField] float staminaDelay;
    [SerializeField] float staminaQuantity;

    [Header("UI Variables")]
    [SerializeField] ResourceUI HealthUI;
    [SerializeField] ResourceUI StaminaUI;
    float healthValue, healthMaxValue, staminaValue, staminaMaxValue;

    private void Start()
    {
        Health.ModifyValue(Health.MaxValue.Value);
        Stamina.ModifyValue(Stamina.MaxValue.Value);
        UpdateHealthBar();
        UpdateStaminaBar();

        StartCoroutine(DelayLoad());
    }

    IEnumerator DelayLoad()
	{
        yield return new WaitForSeconds(.4f);
        SaveLoad.instance.LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveLoad.instance.SaveGame();
        }
    }

    public override void RecieveDamage(float damage, AttackType attackType)
    {
        base.RecieveDamage(damage, attackType);

        UpdateHealthBar();
    }

	public override void RestoreHealth(float health)
	{
		base.RestoreHealth(health);

        UpdateHealthBar();
	}

	public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool UseStamina(float amount)
    {
        if (Stamina.Value >= amount)
        {
            Stamina.ModifyValue(-amount);
            UpdateStaminaBar();
            StopCoroutine("RestoreStamina");
            StartCoroutine("RestoreStamina");
            return true;
        }
        return false;
    }

    IEnumerator RestoreStamina()
    {
        yield return new WaitForSeconds(staminaStartDelay);

        while (Stamina.Value < Stamina.MaxValue.Value)
        {
            Stamina.ModifyValue(staminaQuantity);
            UpdateStaminaBar();
            yield return new WaitForSeconds(staminaDelay);
        }
    }

    public void UpdateHealthBar()
    {
        HealthUI.ModifyValue(Health.GetPercent(out healthValue, out healthMaxValue), healthValue, healthMaxValue);
    }

    void UpdateStaminaBar()
    {
        StaminaUI.ModifyValue(Stamina.GetPercent(out staminaValue, out staminaMaxValue), staminaValue, staminaMaxValue);
    }
}