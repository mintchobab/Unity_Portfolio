using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class InventoryUIController : SceneUI, IInventoryUIController
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

            inventoryManager.onItemAdded += OnItemAdded;
            inventoryManager.onItemChanged += OnItemChanged;
            inventoryManager.onItemUsed += OnItemUsed;

            SetInitalizeSlots();
        }


        private void Start()
        {
            UpdateItemCountText();
        }


        public override void Show(Action onShow = null)
        {
            base.Show(onShow);
            scrollRect.verticalNormalizedPosition = 1f;
        }


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


        private void OnItemAdded(int itemId, int itemIndex)
        {
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];

            string path = CombineItemPath(inventoryItem);
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);
            slotList[itemIndex].UpdateSlotImage(sprite);
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);

            UpdateItemCountText();
        }


        private void OnItemChanged(int itemId, int itemIndex)
        {
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }


        private void OnItemUsed(int itemId, int itemIndex)
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

            UpdateItemCountText();
        }


        private string CombineItemPath(InventoryItem inventoryItem)
        {
            string path = ResourcePath.GetItemSpritePathToTypeString(inventoryItem.item.itemType);
            path += $"/{inventoryItem.item._resourceName}";
            return path;
        }


        private void OnClickExitButton()
        {
            Managers.Instance.UIManager.MainUIController.Show();
            Hide();
        }


        private void OnClickExpandButton()
        {
            if (inventoryManager.CurrentSlotSize >= inventoryManager.MaxSlotSize)
                return;

            int count = inventoryManager.ItemList.Count;
            int addedCount = count + inventoryManager.AddSlotSize;

            for (int i = count; i < addedCount; i++)
            {
                slotList[i].DeactivateLock();
            }

            inventoryManager.AddInventorySlot(inventoryManager.AddSlotSize);
            UpdateItemCountText();
        }


        public void UpdateItemCountText()
        {
            itemCountText.text = $"{inventoryManager.GetItemCountInInventory()} / <color=#4791E6>{inventoryManager.CurrentSlotSize}</color>";
        }


        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (inventoryManager.ItemList[index].item == null)
                return;

            if (index >= inventoryManager.CurrentSlotSize)
                return;

            CountableItem item = inventoryManager.ItemList[index].item;
            inventoryPopup.Show(item, index);
        }

    }
}
