using UnityEngine;

[CreateAssetMenu(fileName = "New HealthPotion", menuName = "Objects/HealthPotion")]
public class ItemHeath : Item
{
	[SerializeField] float health;

	public override void Use()
	{
		base.Use();
		RemoveFromInventory();
		PlayerCharacterStats.instance.RestoreHealth(health);
	}
}
