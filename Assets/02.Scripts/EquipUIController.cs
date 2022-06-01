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
        private Button[] tabButtons;

        [SerializeField]
        private EquipSlot[] equipSlots;


        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        protected override void Awake()
        {
            base.Awake();
            equipInventoryManager.onItemAdded += OnItemAdded;
            //equipInventoryManager.onItemRemoved += OnItemRemoved;
            equipInventoryManager.onItemExchanged += OnItemExchanged;
            equipInventoryManager.onItemMovedTo += OnItemMovedTo;
            equipInventoryManager.onItemEquiped += OnItemEquiped;
            equipInventoryManager.onItemUnEquiped += OnItemUnEquiped;

            exitButton.onClick.AddListener(Hide);

            AddTabButtonEvent();
            SetInitalizeSlots();
        }


        private void AddTabButtonEvent()
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                int j = i;
                tabButtons[j].onClick.AddListener(() => OnClickTabButton(j));
            }
        }


        private void SetInitalizeSlots()
        {
            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].Initialize(this);
            }
        }
  


        public override void Show()
        {
            base.Show();            
            scrollRect.verticalNormalizedPosition = 1f;
        }

        public override void Hide()
        {
            base.Hide();
            equipInventoryPopup.gameObject.SetActive(false);
            Managers.Instance.UIManager.InputController.Show();
        }


        private void OnItemAdded(int itemIndex)
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


        private void OnItemExchanged(int prev, int next)
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


        private void OnItemMovedTo(int self, int target)
        {
            slotList[target].UpdateSlotImage(slotList[self].MyItemSprite);
            slotList[target].UpdateItemName(slotList[self].MyItemName);
        }


        // ������ ���� �̺�Ʈ
        private void OnItemEquiped(EquipType equipType, EquipItem item)
        {
            // �ش� Ÿ���� ���Կ� ������ ��...
            EquipSlot slot = FindEquipSlot(equipType);

            // �ε�� �� �ʿ䰡 ������ ������....
            string path = $"{ResourcePath.EquipItem}/{item._resourceName}";
            Sprite sprite = Managers.Instance.ResourceManager.Load<Sprite>(path);

            slot.UpdateSlotImage(sprite);
        }


        // ������ ���� ���� �̺�Ʈ
        private void OnItemUnEquiped(EquipType equipType, EquipItem item)
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



        private void OnClickTabButton(int index)
        {
            switch (index)
            {
                // All
                case 0:
                    break;

                // ����
                case 1:
                    break;

                // ����
                case 2:
                    break;

                // ����
                case 3:
                    break;

                // ����
                case 4:
                    break;

                // �Ź�
                case 5:
                    break;
            }
        }

    }
}
