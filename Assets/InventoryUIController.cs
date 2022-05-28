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
        private Transform scrollContent;

        [SerializeField]
        private Button expandButton;

        [SerializeField]
        private Text itemCountText;


        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;

        public int ItemCount { get; private set; }

        private List<ItemSlot> slotList = new List<ItemSlot>();

        public ItemSlot SelectedSlot { get; private set; }



        protected override void Awake()
        {
            base.Awake();
            expandButton.onClick.AddListener(OnClickExpandButton);
            CreateSlotRows(Managers.Instance.InventoryManager.SlotRowCount);

            inventoryManager.onItemNewAdded += OnItemNewAdded;
            inventoryManager.onItemAdded += OnItemAdded;
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


        private void OnClickExpandButton()
        {
            inventoryManager.AddInventorySlot(4);
            CreateSlotRows(1);
        }


        private void CreateSlotRows(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject slotRow = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.SlotRow);
                slotRow.transform.SetParent(scrollContent);
                slotRow.transform.localScale = Vector3.one;

                for (int j = 0; j < slotRow.transform.childCount; j++)
                {
                    ItemSlot slot = slotRow.transform.GetChild(j).GetComponent<ItemSlot>();
                    slot.Initialize(this);
                    slotList.Add(slot);
                }
            }

            UpdateItemCountText();
        }


        public void ChangeItemCount(int value)
        {
            ItemCount = value;
            UpdateItemCountText();
        }

        public void UpdateItemCountText()
        {
            itemCountText.text = $"{ItemCount} / {slotList.Count}";
        }




        public void ClickedItemSlot(ItemSlot slot)
        {
            // 빈 슬롯체크
            int index = slotList.IndexOf(slot);

            if (inventoryManager.ItemList[index].item == null)
                return;

            if (slot != SelectedSlot)
                ClickedSlotOnce(slot);
            else
                ClickedSlotDouble(slot);
        }


        // 슬롯이 한번 클릭됐을 때
        private void ClickedSlotOnce(ItemSlot slot)
        {
            if (SelectedSlot != null)
            {
                SelectedSlot.DeactivateOutline();
            }

            SelectedSlot = slot;
            SelectedSlot.ActivateOutline();
        }


        // 슬롯이 두번 클릭됐을 때
        private void ClickedSlotDouble(ItemSlot slot)
        {
            SelectedSlot = null;
            slot.DeactivateOutline();
        }
    }
}
