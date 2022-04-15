using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField]
    public Text ItemCountText { get; private set; }

    [field: SerializeField]
    public Image ItemImage { get; private set; }

    private GameObject dragObj;
    private Sprite defaultSprite;

    private int slotIndex;


    private void Awake()
    {
        defaultSprite = ItemImage.sprite;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetItemImage(Sprite sprite)
    {
        if (defaultSprite == null)
            defaultSprite = ItemImage.sprite;

        ItemImage.sprite = sprite;
    }

    public void SetItemCount(int count)
    {
        ItemCountText.text = count.ToString();
    }

    public void SetSlotEmpty()
    {
        SetItemImage(defaultSprite);
        SetItemCount(0);
        ItemCountText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventorySystem.Instance.RightClickedSlot(slotIndex);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventorySystem.Instance.DragSlot.StartDrag(ItemImage.sprite);
        dragObj = InventorySystem.Instance.DragSlot.gameObject;
        dragObj.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragObj.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject)
        {
            InventorySlot slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

            if (slot != null)
            {
                InventorySystem.Instance.ExchangeItem(slotIndex, slot.slotIndex);
            }

            dragObj.gameObject.SetActive(false);
        }
    }
}
