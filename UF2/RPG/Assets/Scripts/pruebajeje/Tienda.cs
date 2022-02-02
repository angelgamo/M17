using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] Transform inventoryItemsParent;
    InventorySlot[] inventorySlots;

    [SerializeField] Animator animator;

    private void Start()
    {
        inventory = Inventory.instance;
        inventorySlots = inventoryItemsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            animator.SetBool("isOpen", !animator.GetBool("isOpen"));
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


    public void OpenInventory()
    {
        animator.SetBool("isOpen", true);
    }
    public void CloseInventory()
    {
        animator.SetBool("isOpen", false);
    }
}
