using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    [field: SerializeField]
    public Text ItemCountText { get; private set; }



    public void SetItemCount(int count)
    {
        ItemCountText.text = count.ToString();
    }


    public override void SetSlotEmpty()
    {
        base.SetSlotEmpty();
        SetItemCount(0);
        ItemCountText.gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventorySystem.Instance.RightClickedSlot(slotNumber);
        }
    }


    // ���콺 ���ʹ�ư �巡�� ����
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!InventorySystem.Instance.CheckEmptySlot(slotNumber))
            return;

        isDraging = true;

        InventorySystem.Instance.DragSlot.StartDrag(SlotImage.sprite);
        dragObj = InventorySystem.Instance.DragSlot.gameObject;
        dragObj.gameObject.SetActive(true);
    }


    // ���콺 ���ʹ�ư �巡�� ��
    public override void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if (!isDraging)
            return;

        dragObj.transform.position = eventData.position;
    }


    // ���콺 ���ʹ�ư �巡�� ����
    public override void OnEndDrag(PointerEventData eventData)
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
