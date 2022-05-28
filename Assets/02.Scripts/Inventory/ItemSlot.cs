using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField]
        private SlotType slotType;

        [field: SerializeField]
        public Image SlotImage { get; private set; }

        [SerializeField]
        protected Sprite emptySprite;

        [SerializeField]
        protected GameObject selectedOutlineObj;

        [field: SerializeField]
        public Text ItemCountText { get; private set; }

        protected Button button;

        [SerializeField]
        protected InventoryUIController uiController;


        protected bool isClicked;
        protected bool isEmpty = true;


        protected void Awake()
        {
            button = GetComponentInChildren<Button>();           
            button.onClick.AddListener(OnClickButton);
        }


        public void Initialize(InventoryUIController uiController)
        {
            this.uiController = uiController;
        }


        private void OnClickButton()
        {
            uiController.ClickedItemSlot(this);
        }

        // 아웃라인 이미지 활성화
        public void ActivateOutline()
        {
            selectedOutlineObj.SetActive(true);
        }


        // 아웃라인 이미지 비활성화
        public void DeactivateOutline()
        {
            selectedOutlineObj.SetActive(false);
        }


        // 슬롯의 이미지 변경
        public virtual void UpdateSlotImage(Sprite sprite)
        {
            if (SlotImage.sprite.Equals(sprite))
                return;

            isEmpty = false;

            SlotImage.sprite = sprite;
        }


        // 슬롯의 아이템 개수 변경
        public virtual void UpdateSlotCount(int count)
        {
            ItemCountText.text = count.ToString();
        }


        // 슬롯 비우기
        public virtual void ClearSlot()
        {
            isEmpty = true;
            SlotImage.sprite = emptySprite;
            ItemCountText.text = string.Empty;
        }
    }
}
