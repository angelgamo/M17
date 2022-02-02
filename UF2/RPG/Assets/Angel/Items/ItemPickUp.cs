using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;

    private void OnValidate()
	{
        UpdateEditor();
    }

    public void UpdateEditor()
	{
        if (item != null)
        {
            name = item.name;

            if (GetComponent<SpriteRenderer>())
                GetComponent<SpriteRenderer>().sprite = item.icon;
        }
    }

	protected override void Interact()
    {
        PickUp();
    }

    public void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            hasInteracted = false;
            Destroy(gameObject);
        }

        if (item.GetType() == typeof(Equipment) || item.GetType().IsSubclassOf(typeof(Equipment)))
            if (EquipmentManager.instance.currentEquipment[(int)((Equipment)item).equipSlot] == null)
                EquipmentManager.instance.Equip((Equipment)item);
    }

    public void SpawnItem(Vector2 position)
    {
        GameObject item = new GameObject("Item " + this.item.name);
        item.transform.position = position;
        ItemPickUp itemScript = item.AddComponent<ItemPickUp>();
        itemScript.item = this.item;
    }
}