using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDragHandler : MonoBehaviour
{
    public static InventoryDragHandler Instance;
    public GlyphSequencer GlyphSequencer;
    public Image dragIcon;

    public InventorySlot originSlot;
    public Glyph draggedItem;

    private void Awake()
    {
        Instance = this;
    }

    //private void Update()
    //{
    //    if (dragIcon.gameObject.activeSelf)
    //    {
    //        dragIcon.transform.position = Mouse.current.position.ReadValue();
    //    }
    //}

    public void BeginDrag(InventorySlot slot)
    {
        if (slot.IsEmpty) return;
        GlyphSequencer.gameObject.SetActive(true);
        originSlot = slot;
        draggedItem = slot.Item;

        dragIcon.sprite = draggedItem.GlyphIcon;
        dragIcon.gameObject.SetActive(true);
    }
    public void Drag(Vector2 position)
    {
        dragIcon.transform.position = position;
    }
    public void EndDrag()
    {
        dragIcon.gameObject.SetActive(false);
        originSlot = null;
        draggedItem = null;
    }
    public void HandleDrop(InventorySlot targetSlot)
    {
        if (originSlot == null)
            return;

        if (targetSlot.IsEmpty || targetSlot == originSlot)
        {
            EndDrag();
            return;
        }

        Glyph temp = targetSlot.Item;
        targetSlot.Set(draggedItem);
        originSlot.Set(temp);

        originSlot.Refresh();
        targetSlot.Refresh();
    }
}
