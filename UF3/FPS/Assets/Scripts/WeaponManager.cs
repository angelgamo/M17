using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
	public TMPro.TextMeshProUGUI weaponName;
	public TMPro.TextMeshProUGUI weaponAmmo;

    public Weapon[] weapons;
    public List<MeshRenderer> meshRenderers;

	public int index;

	private void OnValidate()
	{
		weapons = GetComponentsInChildren<Weapon>();
		meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
	}

	private void OnEnable()
	{
		foreach (Weapon weapon in weapons)
			weapon.reload.AddListener(UpdateWeaponAmmo);
	}

	private void OnDisable()
	{
		foreach (Weapon weapon in weapons)
			weapon.reload.RemoveListener(UpdateWeaponAmmo);
	}

	private void Start()
	{
		index = -1;
		ChangeWeapon(0);
	}

	private void Update()
	{
		HandleChangeWeapon();

		if (Input.GetMouseButton(0))
		{
			weapons[index].StartFiring();
			UpdateWeaponAmmo();
		}

		if (Input.GetKeyDown(KeyCode.R))
			StartCoroutine(weapons[index].Reload());
	}

	void HandleChangeWeapon()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			ChangeWeapon(0);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			ChangeWeapon(1);
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			ChangeWeapon(2);
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			ChangeWeapon(3);
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			ChangeWeapon(4);
		else if (Input.GetKeyDown(KeyCode.Alpha6))
			ChangeWeapon(5);
		else if (Input.GetKeyDown(KeyCode.Alpha7))
			ChangeWeapon(6);
		else if (Input.GetKeyDown(KeyCode.Alpha8))
			ChangeWeapon(7);
		else if (Input.GetKeyDown(KeyCode.Alpha9))
			ChangeWeapon(8);
	}

	void ChangeWeapon(int newIndex)
	{
		if (index == newIndex)
			return;
		index = newIndex;

		meshRenderers.ForEach(mesh => mesh.enabled = false);
		meshRenderers[index].enabled = true;

		UpdateWeaponName();
		UpdateWeaponAmmo();
	}

	void UpdateWeaponName()
	{
		weaponName.text = weapons[index].name;
	}

	void UpdateWeaponAmmo()
	{
		int ammo;
		var magazine = weapons[index].weaponPreset.magazineCapacity;

		if (weapons[index].weaponPreset.burst)
			if (weapons[index].weaponPreset.shootDelay == 0f)
			{
				magazine = magazine / weapons[index].weaponPreset.burstQuantity;
				ammo = weapons[index].ammoMagazine / weapons[index].weaponPreset.burstQuantity;
			}
			else
				ammo = weapons[index].ammoMagazine;
		else
			ammo = weapons[index].ammoMagazine;

		weaponAmmo.text = ammo.ToString() + "/" + magazine.ToString();
	}
}
