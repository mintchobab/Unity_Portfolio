using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private SlotType slotType;

    [field: SerializeField]
    public Image SlotImage { get; private set; }

    [SerializeField]
    protected Sprite emptySprite;

    [SerializeField]
    protected GameObject selectedObj;

    [field: SerializeField]
    public Text ItemCountText { get; private set; }

    protected Button button;

    protected bool isClicked;

    [SerializeField]
    protected bool isEmpty = true;


    protected void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => ClickedSlot());
    }


    protected void ClickedSlot()
    {
        if (isEmpty)
            return;

        ItemSlot selectedSlot = CommonSystem.SelectedSlot;

        if (selectedSlot != null && this != selectedSlot)
        {
            selectedSlot.CancelClicked();
        }

        if (!isClicked)
            ClickedOnce();
        else
            ClickedDouble();
    }

    protected void ClickedOnce()
    {
        isClicked = true;
        selectedObj.SetActive(true);

        CommonSystem.SelectedSlot = this;
    }

    protected virtual void ClickedDouble()
    {
        isClicked = false;
        selectedObj.SetActive(false);

        InventorySystem.Instance.ClickedInventorySlot(this);
    }


    public void CancelClicked()
    {
        isClicked = false;
        selectedObj.SetActive(false);
    }


    public virtual void UpdateSlotImage(Item item)
    {
        isEmpty = item.CurrentCount > 0 ? false : true;

        SlotImage.sprite = item.ItemData.Image;
    }

    public virtual void UpdateSlotCount(Item item)
    {
        isEmpty = item.CurrentCount > 0 ? false : true;

        if (item.ItemData.ItemType != ItemType.Consumerable)
            return;

        ItemCountText.text = item.CurrentCount.ToString();
    }

    public virtual void ClearSlot()
    {
        isEmpty = true;
        SlotImage.sprite = emptySprite;
        ItemCountText.text = string.Empty;
    }
}
