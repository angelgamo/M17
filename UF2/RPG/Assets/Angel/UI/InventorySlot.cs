using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TooltipTrigger))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button removeButton;
    [SerializeField] Item item;
    TooltipTrigger tooltip;

    private void Awake()
    {
        tooltip = GetComponent<TooltipTrigger>();
        tooltip.enabled = false;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;

        tooltip.header = item.name;
        tooltip.content = item.description;
        tooltip.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;

        tooltip.header = null;
        tooltip.content = null;
        tooltip.enabled = false;

        TooltipSystem.Hide();
    }

    public void OnRemoveButton()
    {
        Inventory.instance.RemoveAndSpawn(item);
    }

    public void UseItem()
    {
        if (item != null)
            item.Use();
    }
}
