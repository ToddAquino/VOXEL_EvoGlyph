using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public void StackInInventory(UISlot currentSlot, Item item)
    {
        currentSlot.Item.ItemCount += item.ItemCount;
        currentSlot.ItemCountText.text = currentSlot.Item.ItemCount.ToString();
    }

    public void PlaceInInventory(UISlot currentSlot, Item item)
    {
        currentSlot.Item = item;
        currentSlot.Icon.sprite = item.ItemIcon;
        currentSlot.ItemCountText.text = item.ItemCount.ToString();
        currentSlot.Icon.gameObject.SetActive(true);
    }

    public void ClearItemSlot(UISlot currentSlot)
    {
        currentSlot.Item = null;
        currentSlot.Icon.sprite = null;
        currentSlot.ItemCountText.text = string.Empty;
        currentSlot.Icon.gameObject.SetActive(false);
    }
}
