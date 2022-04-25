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

    private bool isDraging;

    private int slotNumber;
    public int SlotNumber
    {
        get => slotNumber;
        set
        {
            slotNumber = value;
        }
    }



    private void Awake()
    {
        defaultSprite = ItemImage.sprite;
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
            InventorySystem.Instance.RightClickedSlot(slotNumber);
        }
    }


    // 마우스 왼쪽버튼 드래그 시작
    // 여기에 아이템이 있는지 확인해야됨 (빈 슬롯인지 아닌지 구분)
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!InventorySystem.Instance.CheckEmptySlot(slotNumber))
            return;

        isDraging = true;

        InventorySystem.Instance.DragSlot.StartDrag(ItemImage.sprite);
        dragObj = InventorySystem.Instance.DragSlot.gameObject;
        dragObj.gameObject.SetActive(true);
    }


    // 마우스 왼쪽버튼 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isDraging)
            return;

        dragObj.transform.position = eventData.position;
    }


    // 마우스 왼쪽버튼 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isDraging)
            return;

        isDraging = false;

        if (eventData.pointerCurrentRaycast.gameObject)
        {
            InventorySlot slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

            if (slot != null)
            {
                InventorySystem.Instance.ExchangeItem(slotNumber, slot.slotNumber);
            }

            dragObj.gameObject.SetActive(false);
        }        
    }
}
