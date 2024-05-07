using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class EquipmentInventory : SceneUI, IInventoryUIController
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

        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;


        protected override void Awake()
        {
            base.Awake();

            exitButton.onClick.AddListener(() => Hide());

            SetInitalizeSlots();
        }

        private void OnEnable()
        {
            inventoryManager.onChangedEquipmentList += UpdateSlot;
            inventoryManager.onChangedEquipedItem += UpdateEquipedSlot;

            // TODO
            //equipInventoryManager.onExchangedAllItems += OnExchangedAllItems;
            //equipInventoryManager.onAddedItem += OnAddedItem;
            //equipInventoryManager.onExchangedItem += OnExchangedItem;
            //equipInventoryManager.onMovedItem += OnMovedItem;
            //equipInventoryManager.onEquipedItem += OnEquipedItem;
            //equipInventoryManager.onUnEquipedItem += OnUnEquipedItem;
        }

        private void OnDisable()
        {
            if (inventoryManager != null)
            {
                inventoryManager.onChangedEquipmentList -= UpdateSlot;
                inventoryManager.onChangedEquipedItem -= UpdateEquipedSlot;
            }
        }


        private void Start()
        {
            PlayerController.Instance.StatController.onChangedStat += UpdatePlayerStatText;
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

            UpdatePlayerStatText();

            CameraController.Instance.LookPlayer();

            int slotSize = inventoryManager.EquipmentList.Count;

            for (int i = 0; i < slotSize; i++)
            {
                slotList[i].UnLock();
                slotList[i].Initialize(this);

                UpdateSlot(i);
            }

            for (int i = slotSize; i < slotList.Count; i++)
            {
                slotList[i].Lock();
                slotList[i].Initialize(this);
            }

        }


        // TODO
        public override void Hide(Action onHide = null)
        {
            base.Hide(onHide);

            equipInventoryPopup.gameObject.SetActive(false);
            Managers.Instance.UIManager.MainUIController.Show();

            CameraController.Instance.RestoreCamera();
        }


        private void UpdateSlot(int index)
        {
            if (!isActivate)
                return;

            InventoryManager.InventoryItem inventoryItem = inventoryManager.EquipmentList[index];

            if (inventoryItem.IsExist)
            {
                string path = $"{ResourcePath.EquipItem}/{Tables.EquipmentItemTable[inventoryItem.itemId].ResourceName}";
                Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

                slotList[index].UpdateSlotImage(sprite);
                slotList[index].UpdateItemName(StringManager.Get(Tables.EquipmentItemTable[inventoryItem.itemId].Name));
            }
            else
            {
                slotList[index].ClearSlot();
            }
        }


        private void UpdateEquipedSlot(EquipType equipType, int itemId)
        {
            EquipSlot slot = FindEquipSlot(equipType);

            if (itemId > 0)
            {
                string path = $"{ResourcePath.EquipItem}/{Tables.EquipmentItemTable[itemId].ResourceName}";
                Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

                slot.UpdateSlotImage(sprite);
            }
            else
            {
                slot.ClearSlot(); 
            }            
        }


        //private void OnEquipedItem(EquipType equipType, EquipItem item)
        //{
        //    EquipSlot slot = FindEquipSlot(equipType);

        //    string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
        //    Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

        //    slot.UpdateSlotImage(sprite);
        //}



        private void OnUnEquipedItem(EquipType equipType, EquipItem item)
        {
            EquipSlot slot = FindEquipSlot(equipType);
            slot.ClearSlot();
        }


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



        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (!inventoryManager.EquipmentList[index].IsExist)
                return;

            equipInventoryPopup.Show(true, inventoryManager.EquipmentList[index].itemId, index);
        }








        // -------------------------------------------------------------















        private void OnExchangedAllItems()
        {
            //List<EquipInventoryItem> itemList = equipInventoryManager.ItemList;

            //for (int i = 0; i < itemList.Count; i++)
            //{
            //    EquipItem item = itemList[i].item;

            //    if (item == null)
            //        continue;

            //    string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
            //    Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            //    slotList[i].UpdateSlotImage(sprite);

            //    string name = StringManager.GetLocalizedItemName(item.name);
            //    slotList[i].UpdateItemName(name);
            //}
        }





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






        private void UpdatePlayerStatText()
        {
            hpText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedHp().ToString();
            offensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedOffensivePower().ToString();
            defensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedDefensivePower().ToString();
        }
    }

}
