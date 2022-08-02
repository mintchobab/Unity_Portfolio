using System;
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
        private EquipSlot[] equipSlots;

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text offensivePowerText;

        [SerializeField]
        private Text defensivePowerText;


        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        protected override void Awake()
        {
            base.Awake();

            equipInventoryManager.onExchangedAllItems += OnExchangedAllItems;
            equipInventoryManager.onAddedItem += OnAddedItem;
            //equipInventoryManager.onItemRemoved += OnItemRemoved;
            equipInventoryManager.onExchangedItem += OnExchangedItem;
            equipInventoryManager.onMovedItem += OnMovedItem;
            equipInventoryManager.onEquipedItem += OnEquipedItem;
            equipInventoryManager.onUnEquipedItem += OnUnEquipedItem;

            exitButton.onClick.AddListener(() => Hide());

            SetInitalizeSlots();
        }


        private void Start()
        {
            PlayerController.Instance.StatController.onChangedStat += ChangePlayerStatText;
        }




        private void SetInitalizeSlots()
        {
            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].Initialize(this);
            }
        }
  


        public override void Show(Action onShow = null)
        {
            base.Show(onShow);
            scrollRect.verticalNormalizedPosition = 1f;

            ChangePlayerStatText();

            // 카메라 줌 시작
            CameraController.Instance.LookPlayer();
        }



        public override void Hide(Action onHide = null)
        {
            base.Hide(onHide);
            equipInventoryPopup.gameObject.SetActive(false);
            Managers.Instance.UIManager.MainUIController.Show();

            // 카메라 줌 해제
            CameraController.Instance.RestoreCamera();
        }


        private void OnExchangedAllItems()
        {
            List<EquipInventoryItem> itemList = equipInventoryManager.ItemList;

            for (int i = 0; i < itemList.Count; i++)
            {
                EquipItem item = itemList[i].item;

                if (item == null)
                    continue;

                // 밑에꺼랑 비교
                string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
                Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

                slotList[i].UpdateSlotImage(sprite);

                string name = StringManager.GetLocalizedItemName(item.name);
                slotList[i].UpdateItemName(name);
            }
        }


        private void OnAddedItem(int itemIndex)
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


        private void OnExchangedItem(int prev, int next)
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


        private void OnMovedItem(int self, int target)
        {
            slotList[target].UpdateSlotImage(slotList[self].MyItemSprite);
            slotList[target].UpdateItemName(slotList[self].MyItemName);
        }


        // 아이템 장착 이벤트
        private void OnEquipedItem(EquipType equipType, EquipItem item)
        {
            // 해당 타입의 슬롯에 아이템 들어감...
            EquipSlot slot = FindEquipSlot(equipType);

            // 로드로 할 필요가 없을거 같은데....
            string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slot.UpdateSlotImage(sprite);
        }


        // 아이템 장착 해제 이벤트
        private void OnUnEquipedItem(EquipType equipType, EquipItem item)
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



        // Action 순서 때문에 StatController에 접근해서 추가함
        private void ChangePlayerStatText()
        {
            hpText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedHp().ToString();
            offensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedOffensivePower().ToString();
            defensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedDefensivePower().ToString();
        }
    }

}
