using System.Collections.Generic;
using UnityEngine;

public class InventoryContainer : MonoBehaviour
{
    public List<InventorySlot> slots;

    private void OnEnable()
    {
        BattleManager.Instance.OnVictory.AddListener(ClearAllSlots);
        BattleManager.Instance.OnGameOver.AddListener(ClearAllSlots);

        foreach (InventorySlot slot in slots)
        {
            slot.Refresh();
        }
    }

    private void OnDisable()
    {
        BattleManager.Instance.OnVictory.RemoveListener(ClearAllSlots);
        BattleManager.Instance.OnGameOver.RemoveListener(ClearAllSlots);
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
            SortInventorySlots();
        }
       
    }

    public void SortInventorySlots()
    {
        int indexToCheck = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty)
            {
                if(i != indexToCheck)
                {
                    slots[indexToCheck].Set(slots[i].Item);
                    slots[i].Set(null);
                }
                indexToCheck++;
                
            }
        }
    }
    public void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.Set(null);
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
