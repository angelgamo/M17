using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Singleton
    private static EnemySpawner instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Scene")]
    [SerializeField] SceneController sceneController;

    [Header("Enemy")]
    [SerializeField] List<Vector3> spanwpoints;
    [SerializeField] GameObject enemy;
    [SerializeField] Animator anim;
    [SerializeField] RuntimeAnimatorController mele;
    [SerializeField] RuntimeAnimatorController range;

    [Header("Items")]
    [SerializeField] Weapon[] weapons;
    [SerializeField] Equipment[] equipments;
    [SerializeField] Item[] objects;

    [Header("Oleada")]
    [SerializeField] int oleadasRestantes;
    [SerializeField] int enemyRestantes;

    LevelsSystem levelsSystem;

	private void Start()
	{
        levelsSystem = LevelsSystem.instance;

        oleadasRestantes = Random.Range(3, 10);
        Oleada();
    }


	void Oleada()
	{
        // If no more oleadas, change scene to openworld
        if (oleadasRestantes-- <= 0)
            sceneController.LoadOpenWorld();

        // Create Random number of enemies
        int level = levelsSystem.level;
        enemyRestantes = Random.Range(5 + level, 10 + level * 2);

        for (int i = 0; i < enemyRestantes; i++)
		{
            try
			{
                // Create enemy
                GameObject enemyClone = Instantiate(enemy);
                // Set spawnpoint
                int random = Random.Range(0, spanwpoints.Count);
                enemyClone.transform.position = spanwpoints[random];

                // Random items
                List<Equipment> equipment = new List<Equipment>();
                int itemsCount = Random.Range(0, 3);
                if (itemsCount > 0)
                    for (int j = 0; j < itemsCount; j++)
                        equipment.Add(equipments[Random.Range(0, equipments.Length)]);
                Weapon weapon = (weapons[Random.Range(0, weapons.Length)]);

                // Random drops
                bool dropWeapon = Random.Range(0, 20) == 0;
                bool drowEquipment = Random.Range(0, 10) == 0;
                bool dropObject = Random.Range(0, 15) == 0;

                if (dropWeapon)
                    enemyClone.GetComponent<CharacterStats>().items.Add(weapon);
                if (drowEquipment)
                    enemyClone.GetComponent<CharacterStats>().items.Add(equipment[0]);
                if (dropObject)
                    enemyClone.GetComponent<CharacterStats>().items.Add(objects[Random.Range(0, objects.Length)]);

                // Add Modifier
                float percent1 = 2f * level;
                float percent2 = 2f * level;
                float percent3 = 2f * level;

                CharacterStats characterStats = enemyClone.GetComponent<CharacterStats>();

                StatModifier mod1 = new StatModifier(percent1, StatModType.Percent, StatModCalc.AccumulatedValue);
                characterStats.Strength.AddModifier(mod1);
                characterStats.Intelligence.AddModifier(mod1);
                StatModifier mod2 = new StatModifier(percent2, StatModType.Percent, StatModCalc.AccumulatedValue);
                characterStats.Agility.AddModifier(mod2);
                characterStats.MoveSpeed.AddModifier(mod2);
                StatModifier mod3 = new StatModifier(percent3, StatModType.Percent, StatModCalc.AccumulatedValue);
                characterStats.Agility.AddModifier(mod3);
                characterStats.MoveSpeed.AddModifier(mod3);
                StatModifier mod4 = new StatModifier(percent1 + percent3, StatModType.Percent, StatModCalc.AccumulatedValue);
                characterStats.Health.MaxValue.AddModifier(mod4);

                characterStats.Health.ModifyValue(characterStats.Health.MaxValue.Value);

                EnemyIA enemyIA = enemyClone.GetComponent<EnemyIA>();

                if (weapon.GetType() == typeof(WeaponMagic))
                {
                    enemyIA.isMelee = false;
                    enemyClone.transform.GetChild(0).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = range;
                    enemyIA.range = 4;
                    enemyIA.visionRange = 5;
                    enemyIA.attackDistance = 3;
                }
                else
                {
                    enemyClone.transform.GetChild(0).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = mele;
                    enemyIA.range = 1.5f;
                    enemyIA.visionRange = 5;
                    enemyIA.attackDistance = .7f;
                }

                // Add equipment to enemy
                equipment.Add(weapon);
                enemyClone.GetComponent<EnemyEquipmentManager>().Equipment(equipment.ToArray());
            }
            catch
			{
                continue;
			}
        }
    }

    public void EnemyDie()
	{
        if (--enemyRestantes <= 0)
            Oleada();

        float requiredXp = levelsSystem.requiredXp;
        int level = levelsSystem.level;

        int xp = (int)(requiredXp / (level + (Random.Range(0, level) - level / 2)));
        levelsSystem.GainExperience(xp);
    }
}
