using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [field: SerializeField]
    public Image SlotImage { get; private set; }

    [SerializeField]
    protected Sprite emptySprite;

    protected GameObject dragObj;

    protected bool isDraging;


    protected int slotNumber;
    public int SlotNumber
    {
        get => slotNumber;
        set
        {
            slotNumber = value;
        }
    }


    public void SetItemImage(Sprite sprite)
    {
        SlotImage.sprite = sprite;
    }

    // 이 함수 자체가 필요없을듯...???
    public virtual void SetSlotEmpty()
    {
        SetItemImage(emptySprite);        
    }


    public virtual void OnPointerClick(PointerEventData eventData) { }

    public virtual void OnBeginDrag(PointerEventData eventData) { }

    public virtual void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData) { }
}
