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


        // �������� ���Ӱ� �߰��� ��
        private void OnItemNewAdded(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];

            // �̹��� ����
            string path = CombineItemPath(inventoryItem);
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);
            slotList[itemIndex].UpdateSlotImage(sprite);

            // ���� ����
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }


        // ���� �������� ������ �޶��� ��
        private void OnItemAdded(int itemIndex)
        {
            // ������ ���� ����
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];            
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }



        // ������ Ÿ�Կ� �´� �̹��� ��� ��ȯ
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
            // �� ����üũ
            int index = slotList.IndexOf(slot);

            if (inventoryManager.ItemList[index].item == null)
                return;

            if (slot != SelectedSlot)
                ClickedSlotOnce(slot);
            else
                ClickedSlotDouble(slot);
        }


        // ������ �ѹ� Ŭ������ ��
        private void ClickedSlotOnce(ItemSlot slot)
        {
            if (SelectedSlot != null)
            {
                SelectedSlot.DeactivateOutline();
            }

            SelectedSlot = slot;
            SelectedSlot.ActivateOutline();
        }


        // ������ �ι� Ŭ������ ��
        private void ClickedSlotDouble(ItemSlot slot)
        {
            SelectedSlot = null;
            slot.DeactivateOutline();
        }
    }
}
