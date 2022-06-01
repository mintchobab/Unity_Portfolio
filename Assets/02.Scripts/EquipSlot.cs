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


        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        private Button slotButton;

        private bool isEmpty = true;

        private void Awake()
        {
            slotButton = GetComponent<Button>();
            slotButton.onClick.AddListener(OnClickSlotButton);
        }


        // 슬롯의 이미지 변경
        public void UpdateSlotImage(Sprite sprite)
        {
            if (ItemImage.sprite.Equals(sprite))
                return;

            isEmpty = false;
            ItemImage.sprite = sprite;
        }


        public void ClearSlot()
        {
            isEmpty = true;
            ItemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.Empty);
        }


        private void OnClickSlotButton()
        {
            if (isEmpty)
                return;

            // 1. 내 타입의 아이템의 장착된 아이템을 찾아서
            EquipItem item = equipInventoryManager.EquipedItemDic[EquipSlotType];

            // 2. 팝업 띄우고, 정보 표시
            equipInventoryPopup.Show(item, false);
        }

    }
}
