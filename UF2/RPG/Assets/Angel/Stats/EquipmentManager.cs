using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    Inventory inventory;
    public Equipment[] currentEquipment;
    PlayerCharacterStats playerCharacterStats;
    PlayerController playerController;

    [SerializeField] SpriteRenderer headEquipment;
    [SerializeField] SpriteRenderer chestEquipment;
    [SerializeField] SpriteRenderer legsEquipment;

    [SerializeField] GameEvent onItemChanged;

    [SerializeField] Weapon2 weapon;
    [SerializeField] SpriteRenderer weaponImage;
    [SerializeField] Shoot shoot;
    [SerializeField] Animator weaponAnim;
    [SerializeField] RuntimeAnimatorController mele;
    [SerializeField] RuntimeAnimatorController range;

    public delegate void EquipmentChanged(Equipment newItem, Equipment oldItem);
    public EquipmentChanged onEquipmentChanged;

    private void Start()
    {
        inventory = Inventory.instance;
        playerCharacterStats = PlayerCharacterStats.instance;
        playerController = PlayerController.current;

        int numSlots = System.Enum.GetNames(typeof(EquipSlot)).Length;
        currentEquipment = new Equipment[numSlots];

        playerController.UpdateMovement(playerCharacterStats.MoveSpeed.Value);
    }

    public void SetEquipment(Equipment[] newEquipment)
	{
		for (int slotIndex = 0; slotIndex < currentEquipment.Length; slotIndex++)
		{
            if (currentEquipment[slotIndex] != null)
			{
                playerCharacterStats.Strength.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
                playerCharacterStats.Agility.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
                playerCharacterStats.Intelligence.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
                playerCharacterStats.PhysicResist.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
                playerCharacterStats.MagicResist.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
                playerCharacterStats.MoveSpeed.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);

                if (onEquipmentChanged != null)
                    onEquipmentChanged.Invoke(null, currentEquipment[slotIndex]);

                currentEquipment[slotIndex] = null;
            }

            if (newEquipment[slotIndex] == null)
                continue;

            Equipment newItem = newEquipment[slotIndex];

            if (newItem.equipSlot == EquipSlot.Head)
                headEquipment.sprite = newItem.icon;
            else if (newItem.equipSlot == EquipSlot.Chest)
                chestEquipment.sprite = newItem.icon;
            else if (newItem.equipSlot == EquipSlot.Legs)
                legsEquipment.sprite = newItem.icon;

            if (newItem.equipSlot == EquipSlot.Weapon)
            {
                weaponImage.sprite = newItem.icon;

                if (newItem.GetType() == typeof(WeaponMagic))
                {
                    playerCharacterStats.isWeaponMagic = true;
                    shoot.pro = ((WeaponMagic)newItem).castAttack;
                    shoot.Speed = ((WeaponMagic)newItem).proyectileSpeed;
                    weaponAnim.runtimeAnimatorController = range;
                }
                else
                {
                    playerCharacterStats.isWeaponMagic = false;
                    weaponAnim.runtimeAnimatorController = mele;
                }

                Weapon weapon = (Weapon)newItem;
                this.weapon.ChangeAttackSpeed(weapon.AttackSpeed + playerCharacterStats.Agility.Value);
            }

            playerCharacterStats.Strength.AddModifier(newItem.Strength);
            playerCharacterStats.Agility.AddModifier(newItem.Agility);
            playerCharacterStats.Intelligence.AddModifier(newItem.Intelligence);
            playerCharacterStats.PhysicResist.AddModifier(newItem.PhysicResist);
            playerCharacterStats.MagicResist.AddModifier(newItem.MagicResist);
            playerCharacterStats.MoveSpeed.AddModifier(newItem.MoveSpeed);

            currentEquipment[slotIndex] = newItem;
            onItemChanged.Raise();
        }
	}

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = null;
        
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            bool wasUnequip = Unequip(slotIndex);
            if (!wasUnequip)
                return;
        }

        if (newItem.equipSlot == EquipSlot.Head)
            headEquipment.sprite = newItem.icon;
        else if (newItem.equipSlot == EquipSlot.Chest)
            chestEquipment.sprite = newItem.icon;
        else if (newItem.equipSlot == EquipSlot.Legs)
            legsEquipment.sprite = newItem.icon;

        if (newItem.equipSlot == EquipSlot.Weapon)
        {
            weaponImage.sprite = newItem.icon;

            if (newItem.GetType() == typeof(WeaponMagic))
            {
                playerCharacterStats.isWeaponMagic = true;
                shoot.pro = ((WeaponMagic)newItem).castAttack;
                shoot.Speed = ((WeaponMagic)newItem).proyectileSpeed;
                weaponAnim.runtimeAnimatorController = range;
            }
            else
            {
                playerCharacterStats.isWeaponMagic = false;
                weaponAnim.runtimeAnimatorController = mele;
            }

            Weapon weapon = (Weapon)newItem;
            this.weapon.ChangeAttackSpeed(weapon.AttackSpeed + playerCharacterStats.Agility.Value);

        }

        playerCharacterStats.Strength.AddModifier(newItem.Strength);
        playerCharacterStats.Agility.AddModifier(newItem.Agility);
        playerCharacterStats.Intelligence.AddModifier(newItem.Intelligence);
        playerCharacterStats.PhysicResist.AddModifier(newItem.PhysicResist);
        playerCharacterStats.MagicResist.AddModifier(newItem.MagicResist);
        playerCharacterStats.MoveSpeed.AddModifier(newItem.MoveSpeed);

        playerController.UpdateMovement(playerCharacterStats.MoveSpeed.Value);

        if (onEquipmentChanged != null) 
            onEquipmentChanged.Invoke(newItem, oldItem);

        currentEquipment[slotIndex] = newItem;
        onItemChanged.Raise();
    }

    public bool Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            bool wasAdded = inventory.Add(oldItem);

            if (!wasAdded)
                return false;

            if (slotIndex == (int)EquipSlot.Head)
                headEquipment.sprite = null;
            else if (slotIndex == (int)EquipSlot.Chest)
                chestEquipment.sprite = null;
            else if (slotIndex == (int)EquipSlot.Legs)
                legsEquipment.sprite = null;

            if (slotIndex == (int)EquipSlot.Weapon)
            {
                weaponImage.sprite = null;
            }

            playerCharacterStats.Strength.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
            playerCharacterStats.Agility.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
            playerCharacterStats.Intelligence.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
            playerCharacterStats.PhysicResist.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
            playerCharacterStats.MagicResist.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);
            playerCharacterStats.MoveSpeed.RemoveModifierAllModifiersFromSource(currentEquipment[slotIndex]);

            playerController.UpdateMovement(playerCharacterStats.MoveSpeed.Value);

            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);

            currentEquipment[slotIndex] = null;
            onItemChanged.Raise();
            return true;
        }
        return false;
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}
