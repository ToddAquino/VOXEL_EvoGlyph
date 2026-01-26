using Unity.VisualScripting;
using UnityEngine;

public class InventoryCursorManager : MonoBehaviour
{
    public static InventoryCursorManager Instance;
    public Item currentHeldItem;
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHeldItem(UISlot currentSlot)
    {
        Item currentActiveItem = currentSlot.Item;
        //Store item to slot with same item forming a stack
        if (currentHeldItem != null && currentActiveItem != null && currentHeldItem.ItemType == currentActiveItem.ItemType)
        {
            currentSlot.InventoryManager.StackInInventory(currentSlot, currentHeldItem);
            currentHeldItem = null;
            return;
        }

        //Pick up item from inventory
        if (currentSlot.Item != null)
        {
            currentSlot.InventoryManager.ClearItemSlot(currentSlot);
        }

        //place held item to inventory
        if (currentHeldItem != null)
        {
            currentSlot.InventoryManager.PlaceInInventory(currentSlot,currentHeldItem);
        }

        currentHeldItem = currentActiveItem;
    }

    public void PickupFromStack(UISlot currentSlot)
    {
        if (currentHeldItem != null && currentHeldItem.ItemType != currentSlot.Item.ItemType)
        {
            return;
        }

        if (currentHeldItem == null)
        {
            currentHeldItem = currentSlot.Item.Clone();
            currentHeldItem.ItemCount = 0;
        }

        currentHeldItem.ItemCount++;
        currentSlot.Item.ItemCount--;
        currentSlot.ItemCountText.text = currentSlot.Item.ItemCount.ToString();
        
        if (currentSlot.Item.ItemCount <= 0)
        {
            currentSlot.InventoryManager.ClearItemSlot(currentSlot);
        }
    }
}
