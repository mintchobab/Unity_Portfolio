using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    [Serializable]
    public class EquipInventoryItem
    {
        public EquipItem item;
    }

    public class EquipInventoryManager : IManager
    {
        public List<EquipInventoryItem> ItemList { get; private set; } = new List<EquipInventoryItem>();

        public event Action<int> onItemAdded;
        //public event Action<int> onItemRemoved;
        public event Action<int, int> onItemExchanged;
        public event Action<int, int> onItemMovedTo;
        public event Action<EquipType, EquipItem> onItemEquiped;
        public event Action<EquipType, EquipItem> onItemUnEquiped;

        public readonly int StartSlotSize = 28;

        private int currentItemCount;


        public Dictionary<EquipType, EquipItem> EquipedItemDic { get; private set; } = new Dictionary<EquipType, EquipItem>()
        {
            { EquipType.Weapon, null },
            { EquipType.Shield, null },
            { EquipType.Helmet, null },
            { EquipType.Armor, null },
            { EquipType.Shoes, null },
        };




        public void Initialize()
        {
            AddInventorySlot(StartSlotSize);
        }



        public void AddInventorySlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                EquipInventoryItem inventoryItem = new EquipInventoryItem();
                ItemList.Add(inventoryItem);
            }
        }


        // ������ ������ ���� ���ĵǸ鼭 ������ �߰��ϱ�
        public void AddEquipItem(int itemId)
        {
            if (currentItemCount >= StartSlotSize)
            {
                UnityEngine.Debug.Log("��� �κ��丮 �ʰ�");
                return;
            }


            EquipInventoryItem inventoryItem = MakeNewEquipItem(itemId);

            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].item == null)
                {
                    ItemList[i] = inventoryItem;
                    onItemAdded?.Invoke(i);
                    currentItemCount++;
                    break;
                }

                if (inventoryItem.item.id >= ItemList[i].item.id)
                {
                    continue;
                }
                // ��ĭ�� �о��
                else
                {
                    PushList(i);
                    ItemList[i] = inventoryItem;
                    onItemAdded?.Invoke(i);
                    currentItemCount++;
                    break;
                }
            }
        }


        // �κ��丮���� ����
        public void RemoveEquipItem(int index)
        {
            int lastIndex = ItemList.Count - 1;

            for (int i = index; i < ItemList.Count - 1; i++)
            {
                // ������ �������� �� �ѹ����ϰ� ����
                if (ItemList[i + 1].item == null)
                {
                    ItemList[i] = ItemList[i + 1];
                    onItemMovedTo(i + 1, i);
                    break;
                }
                else
                {
                    ItemList[i] = ItemList[i + 1];
                    onItemMovedTo(i + 1, i);
                }
            }

            currentItemCount--;
        }



        // �̹� ������ �������� �ִ� ���
        public void Equip(int index)
        {
            EquipItem item = ItemList[index].item;

            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");

            // �̹� ������ ������ �ִ��� Ȯ��
            if (EquipedItemDic[type] == null)
            {
                EquipedItemDic[type] = item;
                RemoveEquipItem(index);
            }
            else
            {
                EquipItem equipedItem = EquipedItemDic[type];

                EquipedItemDic[type] = item;
                RemoveEquipItem(index);

                AddEquipItem(equipedItem.id);
            }

            // ������������ ȿ��?? ���� ���氰���� ���⿡
            onItemEquiped?.Invoke(type, item);
        }


        // ������ ����
        public void UnEquip(EquipItem item)
        {
            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");
            EquipedItemDic[type] = null;

            AddEquipItem(item.id);

            onItemUnEquiped?.Invoke(type, item);
        }


        // ������ �����Ͱ� �Ѱ��̻� �����ؾ���
        private void PushList(int startIndex)
        {
            int lastIndex = 0;

            // ���� �ڿ��ִ� ��ĭ ã��
            for (int i = 0; i < ItemList.Count; i++)
            {
                if(ItemList[i].item == null)
                {
                    lastIndex = i;
                    break;
                }
            }

            // �о��
            for (int i = lastIndex; i > startIndex; i--)
            {
                ItemList[i] = ItemList[i - 1];
                onItemExchanged?.Invoke(i - 1, i);
            }
        }


        private EquipInventoryItem MakeNewEquipItem(int itemId)
        {
            EquipItem itemData = Managers.Instance.ItemManager.GetEquipItem(itemId);
            return new EquipInventoryItem() { item = itemData };
        }

    }
}
