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

        // �ƿ����� �̹��� Ȱ��ȭ
        public void ActivateOutline()
        {
            selectedOutlineObj.SetActive(true);
        }


        // �ƿ����� �̹��� ��Ȱ��ȭ
        public void DeactivateOutline()
        {
            selectedOutlineObj.SetActive(false);
        }


        // ������ �̹��� ����
        public virtual void UpdateSlotImage(Sprite sprite)
        {
            if (SlotImage.sprite.Equals(sprite))
                return;

            isEmpty = false;

            SlotImage.sprite = sprite;
        }


        // ������ ������ ���� ����
        public virtual void UpdateSlotCount(int count)
        {
            ItemCountText.text = count.ToString();
        }


        // ���� ����
        public virtual void ClearSlot()
        {
            isEmpty = true;
            SlotImage.sprite = emptySprite;
            ItemCountText.text = string.Empty;
        }
    }
}
