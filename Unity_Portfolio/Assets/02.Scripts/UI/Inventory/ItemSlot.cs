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

        // TODO : 왜 있는건지 확인해보기
        public Sprite MyItemSprite { get; private set; }
        public string MyItemName { get; private set; }
        public string MyItemCount { get; private set; }

        public bool IsLocked { get; private set; }


        protected void Awake()
        {
            button = GetComponentInChildren<Button>();           
            button.onClick.AddListener(() => uiController.ClickedItemSlot(this));

            MyItemSprite = slotImage.sprite;
            MyItemName = itemNameText.text;
            MyItemCount = itemCountText.text;
        }


        // TODO : 필요한가 확인
        public void Initialize(IInventoryUIController uiController)
        {
            this.uiController = uiController;
        }


        public void UpdateSlotImage(Sprite sprite)
        {
            if (slotImage.sprite.Equals(sprite))
                return;

            slotImage.sprite = sprite;
            MyItemSprite = sprite;
        }


        // NOTE : 장비일 떄 이름
        public virtual void UpdateItemName(string name)
        {
            itemNameText.text = name;
            MyItemName = name;
        }


        // NOTE : 소모품일 떄 개수
        public void UpdateSlotCount(int count)
        {
            itemCountText.text = count.ToString();
            MyItemCount = count.ToString();
        }


        public virtual void ClearSlot()
        {
            slotImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.Empty);
            itemNameText.text = string.Empty;
            itemCountText.text = string.Empty;
        }


        public void Lock()
        {
            IsLocked = true;

            lockImage.SetActive(true);

            Color newAlpha = backgorundImage.color;
            newAlpha.a = 200 / 256f;
            backgorundImage.color = newAlpha;
        }


        public void UnLock()
        {
            IsLocked = false;

            lockImage.SetActive(false);

            Color newAlpha = backgorundImage.color;
            newAlpha.a = 128 / 256f;
            backgorundImage.color = newAlpha;
        }
    }
}
