using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class InventoryUIController : SceneUI
    {
        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Button expandButton;

        [SerializeField]
        private Text itemCountText;

        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private InventoryPopup inventoryPopup;

        [SerializeField]
        private List<ItemSlot> slotList;




        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;

        public ItemSlot SelectedSlot { get; private set; }

        private int totalItemCount;




        protected override void Awake()
        {
            base.Awake();
            expandButton.onClick.AddListener(OnClickExpandButton);
            exitButton.onClick.AddListener(OnClickExitButton);

            inventoryManager.onItemNewAdded += OnItemNewAdded;
            inventoryManager.onItemAdded += OnItemAdded;
            inventoryManager.onItemUsed += OnItemUsed;

            SetInitalizeSlots();
        }



        private void Start()
        {
            UpdateItemCountText();
        }



        public override void Show()
        {
            base.Show();
            scrollRect.verticalNormalizedPosition = 1f;
        }


        // 초기 슬롯 설정
        private void SetInitalizeSlots()
        {
            int startSize = inventoryManager.StartSlotSize;

            for (int i = 0; i < startSize; i++)
            {
                slotList[i].DeactivateLock();
                slotList[i].Initialize(this);
            }

            for (int i = startSize; i < slotList.Count; i++)
            {
                slotList[i].ActivateLock();
                slotList[i].Initialize(this);
            }
        }



        // 아이템이 새롭게 추가될 때
        private void OnItemNewAdded(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];

            // 이미지 변경
            string path = CombineItemPath(inventoryItem);
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);
            slotList[itemIndex].UpdateSlotImage(sprite);

            // 개수 변경
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }


        // 기존 아이템의 개수가 달라질 때
        private void OnItemAdded(int itemIndex)
        {
            // 아이템 개수 변경
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }


        // 아이템이 사용될 때
        private void OnItemUsed(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];

            if (inventoryItem.count <= 0)
            {
                slotList[itemIndex].ClearSlot();
            }
            else
            {
                slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
            }            
        }



        // 아이템 타입에 맞는 이미지 경로 반환
        private string CombineItemPath(InventoryItem inventoryItem)
        {
            string path = string.Empty;

            switch (inventoryItem.item.itemType)
            {
                case nameof(ItemType.Equipment):
                    path = ResourcePath.EquipmentItem;
                    break;

                case nameof(ItemType.Consumable):
                    path = ResourcePath.ConsumableItem;
                    break;

                case nameof(ItemType.Material):
                    path = ResourcePath.MaterialItem;
                    break;
            }

            path += $"/{inventoryItem.item.uniqueName}";
            return path;
        }



        // Exit 버튼 클릭
        private void OnClickExitButton()
        {
            Managers.Instance.UIManager.InputController.Show();
            Hide();
        }


        // 확장 버튼 클릭
        private void OnClickExpandButton()
        {
            if (inventoryManager.CurrentSlotSize >= inventoryManager.MaxSlotSize)
                return;

            // 슬롯 잠금 해제
            int count = inventoryManager.ItemList.Count;
            int addedCount = count + inventoryManager.AddSlotSize;

            for (int i = count; i < addedCount; i++)
            {
                slotList[i].DeactivateLock();
            }

            // 실제 인벤토리 슬롯 수 증가
            inventoryManager.AddInventorySlot(inventoryManager.AddSlotSize);
        }


        public void ChangeItemCount(int value)
        {
            totalItemCount = value;
            UpdateItemCountText();
        }


        public void UpdateItemCountText()
        {
            itemCountText.text = $"{totalItemCount} / <color=#4791E6>{inventoryManager.CurrentSlotSize}</color>";
        }



        // 슬롯이 클릭됐을 때
        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            // 빈 슬롯체크
            if (inventoryManager.ItemList[index].item == null)
                return;

            // 자물쇠 슬롯 체크
            if (index >= inventoryManager.CurrentSlotSize)
                return;

            // 1. 팝업 띄우기

            // 필요한것들
            // 슬롯에 들어있는 아이템 정보, 어떤 슬롯의 아이템인지 (슬롯에 대한 정보)

            CountableItem item = inventoryManager.ItemList[index].item;
            inventoryPopup.Show(item, index);
        }


        //private void CreateSlotRows(int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        GameObject slotRow = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.SlotRow);
        //        slotRow.transform.SetParent(scrollContent);
        //        slotRow.transform.localScale = Vector3.one;

        //        for (int j = 0; j < slotRow.transform.childCount; j++)
        //        {
        //            ItemSlot slot = slotRow.transform.GetChild(j).GetComponent<ItemSlot>();
        //            slot.Initialize(this);
        //            slotList.Add(slot);
        //        }
        //    }

        //    UpdateItemCountText();
        //}

    }
}
