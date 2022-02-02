using System.Collections;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    PlayerCharacterStats playerCharacterStats;

    EquipmentManager equipment;
    [SerializeField] Transform equipmentItemsParent;
    EquipmentSlot[] equipmentSlots;

    [SerializeField] TMPro.TextMeshProUGUI strength;
    [SerializeField] TMPro.TextMeshProUGUI agility;
    [SerializeField] TMPro.TextMeshProUGUI intelligence;
    [SerializeField] TMPro.TextMeshProUGUI physicResist;
    [SerializeField] TMPro.TextMeshProUGUI magicResist;
    [SerializeField] TMPro.TextMeshProUGUI moveSpeed;

	private void Awake()
	{
        playerCharacterStats = PlayerCharacterStats.instance;

        equipment = EquipmentManager.instance;
        equipmentSlots = equipmentItemsParent.GetComponentsInChildren<EquipmentSlot>();
    }

	private void Start()
    {
        StartCoroutine(StartDelayed());
    }

    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(0.3f);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (equipment.currentEquipment.Length == 0)
            return;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipment.currentEquipment[i] != null)
                equipmentSlots[i].AddItem(equipment.currentEquipment[i]);
            else
                equipmentSlots[i].ClearSlot();
        }

        strength.text = "Strength: " + playerCharacterStats.Strength.BaseValue + " > " + playerCharacterStats.Strength.Value;
        agility.text = "Agility: " + playerCharacterStats.Agility.BaseValue + " > " + playerCharacterStats.Agility.Value;
        intelligence.text = "Intelligence: " + playerCharacterStats.Intelligence.BaseValue + " > " + playerCharacterStats.Intelligence.Value;
        physicResist.text = "PhysicResist: " + playerCharacterStats.PhysicResist.BaseValue + " > " + playerCharacterStats.PhysicResist.Value;
        magicResist.text = "MagicResist: " + playerCharacterStats.MagicResist.BaseValue + " > " + playerCharacterStats.MagicResist.Value;
        moveSpeed.text = "MoveSpeed: " + playerCharacterStats.MoveSpeed.BaseValue + " > " + playerCharacterStats.MoveSpeed.Value;
    }
}
