using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class ConsumableInventory : SceneUI, IInventoryUIController
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



        protected override void Awake()
        {
            base.Awake();

            expandButton.onClick.AddListener(OnClickExpandButton);
            exitButton.onClick.AddListener(OnClickExitButton);
        }


        private void OnEnable()
        {
            inventoryManager.onConsumableChanged += OnItemChanged;
        }


        private void OnDisable()
        {
            if (inventoryManager != null)
            {
                inventoryManager.onConsumableChanged -= OnItemChanged;
            }
        }


        public override void Show(Action onShow = null)
        {
            base.Show(onShow);

            scrollRect.verticalNormalizedPosition = 1f;
            UpdateItemCountText();

            int slotSize = inventoryManager.ConsumableList.Count;

            for (int i = 0; i < slotSize; i++)
            {
                slotList[i].UnLock();
                slotList[i].Initialize(this);

                UpdateSlot(i, false);
            }

            for (int i = slotSize; i < slotList.Count; i++)
            {
                slotList[i].Lock();
                slotList[i].Initialize(this);
            }
        }


        private void OnItemChanged(int itemId, int itemIndex)
        {
            UpdateSlot(itemIndex);
        }


        private void UpdateSlot(int index, bool isUpdateCount = true)
        {
            if (!isActivate)
                return;

            InventoryManager.InventoryItem inventoryItem = inventoryManager.ConsumableList[index];

            if (inventoryItem.IsExist)
            {
                // TODO : GetItemSpritePathToTypeString 없애기
                string path = $"{ResourcePath.GetItemSpritePathToTypeString(inventoryItem.ItemType)}/{Tables.ConsumableItemTable[inventoryItem.itemId].ResourceName}";
                Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

                slotList[index].UpdateSlotImage(sprite);
                slotList[index].UpdateSlotCount(inventoryItem.itemCount);
            }
            else
            {
                slotList[index].ClearSlot();
            }

            if (isUpdateCount)
                UpdateItemCountText();
        }


        public void UpdateItemCountText()
        {
            itemCountText.text = $"{inventoryManager.GetItemCountInInventory()} / <color=#4791E6>{inventoryManager.ConsumableList.Count}</color>";
        }


        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (!inventoryManager.ConsumableList[index].IsExist)
                return;

            if (index >= inventoryManager.ConsumableList.Count)
                return;

            inventoryPopup.Show(inventoryManager.ConsumableList[index], index);
        }


        // TODO : 장비창이랑 왜 다르지
        private void OnClickExitButton()
        {
            Managers.Instance.UIManager.MainUIController.Show();
            Hide();
        }


        private void OnClickExpandButton()
        {
            bool isAdded = inventoryManager.AddConsumableSlotSize();

            if (!isAdded)
                return;

            for (int i = 0; i < inventoryManager.ConsumableList.Count; i++)
            {
                if (slotList[i].IsLocked)
                    slotList[i].UnLock();
            }

            UpdateItemCountText();
        }
    }
}
