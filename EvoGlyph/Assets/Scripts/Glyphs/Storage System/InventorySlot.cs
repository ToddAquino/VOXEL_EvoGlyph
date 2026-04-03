using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour/*,IBeginDragHandler,IDragHandler, IEndDragHandler, IDropHandler*/
{
    public Glyph Item;
    public Image Icon;
    public bool IsEmpty => Item == null;

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log("IsDragging");
    //    InventoryDragHandler.Instance.BeginDrag(this);
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    InventoryDragHandler.Instance.Drag(eventData.position);
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("IsDragging Ended");
    //    if(eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<InventorySlot>() == null)
    //    {
    //        InventorySlot slotToClear = InventoryDragHandler.Instance.originSlot;
    //        InventoryContainer container = slotToClear.GetComponentInParent<InventoryContainer>();
    //        if (container != null)
    //        {
    //            container.RemoveItemFromSlot(slotToClear);
    //        }
    //    }
    //    InventoryDragHandler.Instance.EndDrag();
    //}
    //public void OnDrop(PointerEventData eventData)
    //{
    //    InventoryDragHandler.Instance.HandleDrop(this);
    //}

    public void Set(Glyph item)
    {
        if (item == null)
        {
            Item = null;
            Icon.enabled = false;
        }
        else
        {
            Icon.enabled = true;
            Item = item;
            Icon.sprite = item.GlyphIcon;
        }
    }

    public void Refresh()
    {
        if (Item == null)
        {
            Icon.enabled = false;
        }
        else
        {
            Icon.enabled = true;
            Icon.sprite = Item.GlyphIcon;
        }

    }

}
