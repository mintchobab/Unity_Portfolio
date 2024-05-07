using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class EquipSlot : MonoBehaviour
    {
        [field: SerializeField]
        public EquipType EquipSlotType { get; private set; }

        [field: SerializeField]
        public Image ItemImage { get; private set; }

        [SerializeField]
        private EquipInventoryPopup equipInventoryPopup;

        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;

        private Button slotButton;


        private void Awake()
        {
            slotButton = GetComponent<Button>();
            slotButton.onClick.AddListener(OnClickSlotButton);
        }


        public void UpdateSlotImage(Sprite sprite)
        {
            if (ItemImage.sprite.Equals(sprite))
                return;

            ItemImage.sprite = sprite;
        }


        public void ClearSlot()
        {
            ItemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.Empty);
        }


        private void OnClickSlotButton()
        {
            int itemId = inventoryManager.EquipedItemDic[EquipSlotType];

            if (itemId <= 0)
                return;

            equipInventoryPopup.Show(false, itemId);
        }
    }
}
