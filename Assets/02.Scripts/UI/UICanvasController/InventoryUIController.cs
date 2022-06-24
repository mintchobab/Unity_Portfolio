using System;
using System.Collections;
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

        private int totalItemCount;



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



        public override void Show()
        {
            base.Show();
            scrollRect.verticalNormalizedPosition = 1f;
        }


        // �ʱ� ���� ����
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



        // �������� ���Ӱ� �߰��� ��
        private void OnItemAdded(int itemId, int itemIndex)
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
        private void OnItemChanged(int itemId, int itemIndex)
        {
            // ������ ���� ����
            InventoryItem inventoryItem = inventoryManager.ItemList[itemIndex];
            slotList[itemIndex].UpdateSlotCount(inventoryItem.count);
        }


        // �������� ���� ��
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
        }



        // ������ Ÿ�Կ� �´� �̹��� ��� ��ȯ
        private string CombineItemPath(InventoryItem inventoryItem)
        {
            string path = ResourcePath.GetItemSpritePathToTypeString(inventoryItem.item.itemType);
            path += $"/{inventoryItem.item._resourceName}";
            return path;
        }



        // Exit ��ư Ŭ��
        private void OnClickExitButton()
        {
            Managers.Instance.UIManager.InputUIController.Show();
            Hide();
        }


        // Ȯ�� ��ư Ŭ��
        private void OnClickExpandButton()
        {
            if (inventoryManager.CurrentSlotSize >= inventoryManager.MaxSlotSize)
                return;

            // ���� ��� ����
            int count = inventoryManager.ItemList.Count;
            int addedCount = count + inventoryManager.AddSlotSize;

            for (int i = count; i < addedCount; i++)
            {
                slotList[i].DeactivateLock();
            }

            // ���� �κ��丮 ���� �� ����
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



        // ������ Ŭ������ ��
        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            // �� ����üũ
            if (inventoryManager.ItemList[index].item == null)
                return;

            // �ڹ��� ���� üũ
            if (index >= inventoryManager.CurrentSlotSize)
                return;

            // �˾� ����
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
