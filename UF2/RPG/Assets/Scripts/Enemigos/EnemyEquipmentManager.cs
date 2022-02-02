using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEquipmentManager : MonoBehaviour
{
    public List<Equipment> inventory;
    public CharacterStats EnemyCS;
    public EnemyIA EnemyIA;
    [SerializeField] SpriteRenderer suit;
    [SerializeField] SpriteRenderer headEquipment;
    [SerializeField] SpriteRenderer chestEquipment;
    [SerializeField] SpriteRenderer legsEquipment;
    [SerializeField] SpriteRenderer weaponImage;
    Animator anim;

	private void Awake()
	{
        EnemyCS = this.GetComponent<CharacterStats>();
        EnemyIA = this.GetComponent<EnemyIA>();
        anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

	void Start()
    {
        EnemyCS = this.GetComponent<CharacterStats>();
        anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        /*
        foreach (var item in inventory)
        {
            Equip(item);
        }*/
        suit.color = Random.ColorHSV();
    }

    public void Equipment(Equipment[] equipments)
	{
        foreach (var item in equipments)
            Equip(item);
    }

    public void Equip(Equipment newItem)
    {
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
                EnemyCS.isWeaponMagic = true;
            else
                EnemyCS.isWeaponMagic = false;

            Weapon weapon = (Weapon)newItem;
            if (newItem.GetType() == typeof(WeaponMagic)) {
                Shoot s = this.gameObject.transform.Find("Hand/Weapon").gameObject.GetComponent<Shoot>();
                WeaponMagic m = (WeaponMagic)weapon;
                s.pro = m.castAttack;
                s.offset = m.AttackOffset;
            }
            anim.speed = weapon.AttackSpeed;
        }

        EnemyCS.Strength.AddModifier(newItem.Strength);
        EnemyCS.Agility.AddModifier(newItem.Agility);
        EnemyCS.Intelligence.AddModifier(newItem.Intelligence);
        EnemyCS.PhysicResist.AddModifier(newItem.PhysicResist);
        EnemyCS.MagicResist.AddModifier(newItem.MagicResist);
        EnemyCS.MoveSpeed.AddModifier(newItem.MoveSpeed);

        EnemyIA.UpdateStats();
    }

}
