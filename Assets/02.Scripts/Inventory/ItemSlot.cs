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
        private Text itemNameText;

        [SerializeField]
        private Text itemCountText;

        [SerializeField]
        private GameObject lockImage;

        protected Button button;
        protected IInventoryUIController uiController;



        public Sprite MyItemSprite { get; private set; }
        public string MyItemName { get; private set; }
        public string MyItemCount { get; private set; }



        protected void Awake()
        {
            button = GetComponentInChildren<Button>();           
            button.onClick.AddListener(OnClickButton);

            MyItemSprite = slotImage.sprite;
            MyItemName = itemNameText.text;
            MyItemCount = itemCountText.text;
        }


        public void Initialize(IInventoryUIController uiController)
        {
            this.uiController = uiController;
        }


        private void OnClickButton()
        {
            uiController.ClickedItemSlot(this);
        }


        // ������ �̹��� ����
        public void UpdateSlotImage(Sprite sprite)
        {
            if (slotImage.sprite.Equals(sprite))
                return;

            slotImage.sprite = sprite;
            MyItemSprite = sprite;
        }


        // ������ ������ �̸� ����
        public virtual void UpdateItemName(string name)
        {
            itemNameText.text = name;
            MyItemName = name;
        }


        // ������ ������ ���� ����
        public void UpdateSlotCount(int count)
        {
            itemCountText.text = count.ToString();
            MyItemCount = count.ToString();
        }


        // ���� ����
        public virtual void ClearSlot()
        {
            slotImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.Empty);
            itemNameText.text = string.Empty;
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
