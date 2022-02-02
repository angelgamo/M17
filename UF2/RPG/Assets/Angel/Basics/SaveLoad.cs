using System;
using System.Text;
using System.IO;
using UnityEngine;
using System.Linq;

public class SaveLoad : MonoBehaviour
{
    #region Singleton
    public static SaveLoad instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] Layout layout;

    public void ResetGame()
	{
        layout.coins = 0;
        layout.level = 1;
        layout.currentXp = 0;
        layout.requiredXp = 0;

        layout.inventory = new Item[0];
        layout.equipment = new Equipment[7];

        layout.skills = new int[7];

        string jsonStr = JsonUtility.ToJson(layout);
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonStr));
        File.WriteAllText("savegame.json", base64);
    }

	public void SaveGame()
	{
        layout.coins = LevelsSystem.instance.coins;
        layout.level = LevelsSystem.instance.level;
        layout.currentXp = LevelsSystem.instance.currentXp;
        layout.requiredXp = LevelsSystem.instance.requiredXp;

        layout.inventory = Inventory.instance.items.ToArray();
        layout.equipment = EquipmentManager.instance.currentEquipment;

        layout.skills = SkillsManager.instance.lastBarValues;

        string jsonStr = JsonUtility.ToJson(layout);
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonStr));
        File.WriteAllText("savegame.json", base64);
    }

    public void LoadGame()
	{
        string base64 = File.ReadAllText("savegame.json");
        string jsonStr = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        JsonUtility.FromJsonOverwrite(jsonStr, layout);

        LevelsSystem.instance.coins = layout.coins;
        LevelsSystem.instance.level = layout.level;
        LevelsSystem.instance.currentXp = layout.currentXp;
        LevelsSystem.instance.requiredXp = layout.requiredXp;
        Inventory.instance.items = layout.inventory.Cast<Item>().ToList();
        EquipmentManager.instance.SetEquipment(layout.equipment);
        SkillsManager.instance.SetVales(layout.skills);
        LevelsSystem.instance.UpdateUI();
        InventoryUI.instance.UpdateUI();
    }
}
