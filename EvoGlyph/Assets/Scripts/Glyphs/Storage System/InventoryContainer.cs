using System.Collections.Generic;
using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    public List<InventorySlot> slots;

    private void OnEnable()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.Refresh();
        }
    }
    public void AddItem(Glyph item)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.Set(item);
                return;
            }
        }
        //HandleInventoryFull(item);
    }

    public void ReplaceItemFromSlot(Glyph replacementItem, InventorySlot slotToReplace)
    {
        slotToReplace.Set(replacementItem);
    }
    public void RemoveItemFromSlot(InventorySlot slotToClear)
    {
        if(slotToClear.Item != null)
        {
            slotToClear.Set(null);
        }
    }
    public Glyph TakeItemFromSlot(InventorySlot slotToTake)
    {
        if(slotToTake.Item != null)
        {
            return slotToTake.Item;
        }
        return null;
    }

    void HandleInventoryFull(Glyph item)
    {
        //All inventory slot shakes with red border
        // when slot ckicled RemoveIemFromSlot
        //toggle off inventoryfull
        //Add new item
    }
}
