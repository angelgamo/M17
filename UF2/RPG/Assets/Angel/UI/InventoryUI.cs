using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance;

    private void Awake()
    {
        instance = this;
        inventory = Inventory.instance;
        inventorySlots = inventoryItemsParent.GetComponentsInChildren<InventorySlot>();
    }
    #endregion
    
    Inventory inventory;
    [SerializeField] Transform inventoryItemsParent;
    InventorySlot[] inventorySlots;

    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            animator.SetBool("isOpen", !animator.GetBool("isOpen"));
        else if (Input.GetKeyDown(KeyCode.Escape))
            animator.SetBool("isOpen", false);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.items.Count)
                inventorySlots[i].AddItem(inventory.items[i]);
            else
                inventorySlots[i].ClearSlot();
        }
    }

    public void CloseInventory()
    {
        animator.SetBool("isOpen", false);
    }
}
