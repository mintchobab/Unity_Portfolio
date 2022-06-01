using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class EquipUIController : SceneUI, IInventoryUIController
    {
        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private EquipInventoryPopup equipInventoryPopup;

        [SerializeField]
        private List<ItemSlot> slotList;

        [SerializeField]
        private Button[] tabButtons;

        [SerializeField]
        private EquipSlot[] equipSlots;


        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        protected override void Awake()
        {
            base.Awake();
            equipInventoryManager.onItemAdded += OnItemAdded;
            //equipInventoryManager.onItemRemoved += OnItemRemoved;
            equipInventoryManager.onItemExchanged += OnItemExchanged;
            equipInventoryManager.onItemMovedTo += OnItemMovedTo;
            equipInventoryManager.onItemEquiped += OnItemEquiped;
            equipInventoryManager.onItemUnEquiped += OnItemUnEquiped;

            exitButton.onClick.AddListener(Hide);

            AddTabButtonEvent();
            SetInitalizeSlots();
        }


        private void AddTabButtonEvent()
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                int j = i;
                tabButtons[j].onClick.AddListener(() => OnClickTabButton(j));
            }
        }


        private void SetInitalizeSlots()
        {
            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].Initialize(this);
            }
        }
  


        public override void Show()
        {
            base.Show();            
            scrollRect.verticalNormalizedPosition = 1f;
        }

        public override void Hide()
        {
            base.Hide();
            equipInventoryPopup.gameObject.SetActive(false);
            Managers.Instance.UIManager.InputController.Show();
        }


        private void OnItemAdded(int itemIndex)
        {
            EquipInventoryItem equipInventoryItem = equipInventoryManager.ItemList[itemIndex];

            // 이미지 변경
            string path = $"{ResourcePath.EquipItem}/{equipInventoryItem.item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slotList[itemIndex].UpdateSlotImage(sprite);

            // 이름 변경
            string name = StringManager.GetLocalizedItemName(equipInventoryItem.item.name);
            slotList[itemIndex].UpdateItemName(name);
        }


        //private void OnItemRemoved(int itemIndex)
        //{
        //    slotList[itemIndex].ClearSlot();
        //}


        private void OnItemExchanged(int prev, int next)
        {
            Sprite prevSprite;
            string prevName;

            prevSprite = slotList[prev].MyItemSprite;
            prevName = slotList[prev].MyItemName;

            slotList[prev].UpdateSlotImage(slotList[next].MyItemSprite);
            slotList[prev].UpdateItemName(slotList[next].MyItemName);

            slotList[next].UpdateSlotImage(prevSprite);
            slotList[next].UpdateItemName(prevName);
        }


        private void OnItemMovedTo(int self, int target)
        {
            slotList[target].UpdateSlotImage(slotList[self].MyItemSprite);
            slotList[target].UpdateItemName(slotList[self].MyItemName);
        }


        // 아이템 장착 이벤트
        private void OnItemEquiped(EquipType equipType, EquipItem item)
        {
            // 해당 타입의 슬롯에 아이템 들어감...
            EquipSlot slot = FindEquipSlot(equipType);

            // 로드로 할 필요가 없을거 같은데....
            string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slot.UpdateSlotImage(sprite);
        }


        // 아이템 장착 해제 이벤트
        private void OnItemUnEquiped(EquipType equipType, EquipItem item)
        {
            EquipSlot slot = FindEquipSlot(equipType);
            slot.ClearSlot();
        }



        // 인벤토리 슬롯 버튼들이 클릭됐을 때
        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (equipInventoryManager.ItemList[index].item == null)
                return;

            // 팝업 띄우기
            EquipItem item = equipInventoryManager.ItemList[index].item;
            equipInventoryPopup.Show(item, true, index);
        }


        // 장비슬롯 찾기
        private EquipSlot FindEquipSlot(EquipType equipType)
        {
            for (int i = 0; i < equipSlots.Length; i++)
            {
                if (equipSlots[i].EquipSlotType == equipType)
                {
                    return equipSlots[i];
                }
            }

            return null;            
        }



        private void OnClickTabButton(int index)
        {
            switch (index)
            {
                // All
                case 0:
                    break;

                // 무기
                case 1:
                    break;

                // 방패
                case 2:
                    break;

                // 투구
                case 3:
                    break;

                // 갑옷
                case 4:
                    break;

                // 신발
                case 5:
                    break;
            }
        }

    }
}
