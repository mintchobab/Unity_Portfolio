using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField]
        private Image backgorundImage;

        [SerializeField]
        private Image slotImage;

        [SerializeField]
        private GameObject lockImage;

        [SerializeField]
        private Text itemCountText;


        protected Button button;
        protected InventoryUIController uiController;



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


        // ������ �̹��� ����
        public virtual void UpdateSlotImage(Sprite sprite)
        {
            if (slotImage.sprite.Equals(sprite))
                return;

            slotImage.sprite = sprite;
        }


        // ������ ������ ���� ����
        public virtual void UpdateSlotCount(int count)
        {
            itemCountText.text = count.ToString();
        }


        // ���� ����
        public virtual void ClearSlot()
        {
            slotImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.Empty);
            itemCountText.text = string.Empty;
        }


        public void ActivateLock()
        {
            lockImage.SetActive(true);

            Color newAlpha = backgorundImage.color;
            newAlpha.a = 200 / 256f;
            backgorundImage.color = newAlpha;
        }


        public void DeactivateLock()
        {
            lockImage.SetActive(false);

            Color newAlpha = backgorundImage.color;
            newAlpha.a = 128 / 256f;
            backgorundImage.color = newAlpha;
        }
    }
}
