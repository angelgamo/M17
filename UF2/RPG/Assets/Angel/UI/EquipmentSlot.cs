using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TooltipTrigger))]
public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Equipment item;
    TooltipTrigger tooltip;

    private void Awake()
    {
        tooltip = GetComponent<TooltipTrigger>();
        tooltip.enabled = false;
    }

    public void AddItem(Equipment newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        tooltip.header = item.name;
        tooltip.content = item.description;
        tooltip.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        tooltip.header = null;
        tooltip.content = null;
        tooltip.enabled = false;

        TooltipSystem.Hide();
    }

    public void Unequip()
    {
        if (item != null)
        {
            bool wasUnequip = EquipmentManager.instance.Unequip((int)item.equipSlot);
            if (wasUnequip)
                ClearSlot();
        }
    }
}
