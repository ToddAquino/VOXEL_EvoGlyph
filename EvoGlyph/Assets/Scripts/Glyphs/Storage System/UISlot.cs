using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public Item Item;
    public Image Icon;
    public TextMeshProUGUI ItemCountText;
    public InventoryManager InventoryManager;
    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item == null) return;

            InventoryCursorManager.Instance.PickupFromStack(this);
            return;
        }
        InventoryCursorManager.Instance.UpdateHeldItem(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Item != null)
        {
            Item = Item.Clone();
            Icon.sprite = Item.ItemIcon;
            ItemCountText.text = Item.ItemCount.ToString();
        }

        else
        {
            Icon.gameObject.SetActive(false);
            ItemCountText.text = string.Empty;
        }
    }
}
