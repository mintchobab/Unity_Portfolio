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

            // ī�޶� �� ����
            CameraController.Instance.LookPlayer();
        }



        public override void Hide(Action onHide = null)
        {
            base.Hide(onHide);
            equipInventoryPopup.gameObject.SetActive(false);
            Managers.Instance.UIManager.MainUIController.Show();

            // ī�޶� �� ����
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

                // �ؿ����� ��
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

            // �̹��� ����
            string path = $"{ResourcePath.EquipItem}/{equipInventoryItem.item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slotList[itemIndex].UpdateSlotImage(sprite);

            // �̸� ����
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


        // ������ ���� �̺�Ʈ
        private void OnEquipedItem(EquipType equipType, EquipItem item)
        {
            // �ش� Ÿ���� ���Կ� ������ ��...
            EquipSlot slot = FindEquipSlot(equipType);

            // �ε�� �� �ʿ䰡 ������ ������....
            string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slot.UpdateSlotImage(sprite);
        }


        // ������ ���� ���� �̺�Ʈ
        private void OnUnEquipedItem(EquipType equipType, EquipItem item)
        {
            EquipSlot slot = FindEquipSlot(equipType);
            slot.ClearSlot();
        }



        // �κ��丮 ���� ��ư���� Ŭ������ ��
        public void ClickedItemSlot(ItemSlot slot)
        {
            int index = slotList.IndexOf(slot);

            if (equipInventoryManager.ItemList[index].item == null)
                return;

            // �˾� ����
            EquipItem item = equipInventoryManager.ItemList[index].item;
            equipInventoryPopup.Show(item, true, index);
        }


        // ��񽽷� ã��
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



        // Action ���� ������ StatController�� �����ؼ� �߰���
        private void ChangePlayerStatText()
        {
            hpText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedHp().ToString();
            offensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedOffensivePower().ToString();
            defensivePowerText.text = PlayerController.Instance.StatController.PlayerStat.GetAddedDefensivePower().ToString();
        }
    }

}
