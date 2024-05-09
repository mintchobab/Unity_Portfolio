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
                string path = $"{ResourcePath.EquipItem}/{Tables.EquipmentItemTable[inventoryItem.ItemId].ResourceName}";
                Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

                slotList[index].UpdateSlotImage(sprite);
                slotList[index].UpdateItemName(StringManager.Get(Tables.EquipmentItemTable[inventoryItem.ItemId].Name));
            }
            else
            {
                slotList[index].ClearSlot();
            }
        }


        private void UpdateEquipedSlot(EquipType equipType, int itemId)
        {
            EquipSlot slot = null;

            for (int i = 0; i < equipSlots.Length; i++)
            {
                if (equipSlots[i].EquipSlotType == equipType)
                {
                    slot = equipSlots[i];
                    break;
                }
            }

            if (slot == null)
            {
                Debug.LogError($"{nameof(EquipmentInventory)} : Slot Error");
                return;
            }


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


        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (!inventoryManager.EquipmentList[index].IsExist)
                return;

            equipInventoryPopup.Show(true, inventoryManager.EquipmentList[index].ItemId, index);
        }


        // TODO : 아이템 장착 시 스탯 변경 관련 작업 수정하기
        private void UpdatePlayerStatText()
        {
            hpText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedHp().ToString();
            offensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedOffensivePower().ToString();
            defensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedDefensivePower().ToString();
        }
    }
}
