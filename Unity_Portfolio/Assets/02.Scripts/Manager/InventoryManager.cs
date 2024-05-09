using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lsy
{
    public class InventoryManager : IManager
    {
        public class InventoryItem
        {
            public ItemType ItemType { get; private set; }
            public EquipType EquipType { get; private set; }

            public int ItemId { get; private set; }
            public int ItemCount { get; private set; }

            public bool IsExist => ItemId != -1;

            public InventoryItem()
            {
                SetEmpty();
            }

            public void SetEmpty()
            {
                ItemType = ItemType.None;
                EquipType = EquipType.None;
                ItemId = -1;
                ItemCount = 0;
            }

            public void SetData(int id, int count)
            {
                if (Tables.ConsumableItemTable.IsExist(id))
                {
                    ItemType = ItemType.Consumable;
                    EquipType = EquipType.None;
                }
                else if (Tables.EquipmentItemTable.IsExist(id))
                {
                    ItemType = ItemType.Equipment;
                    EquipType = Utility.StringToEnum<EquipType>(Tables.EquipmentItemTable[id].Parts);
                }
                else
                {
                    Debug.LogError($"{nameof(InventoryItem)} : ItemType Error");
                }

                this.ItemId = id;
                this.ItemCount = count;
            }

            public void SetCount(int count)
            {
                if (ItemType != ItemType.Consumable)
                    return;

                int maxCount = Tables.ConsumableItemTable[ItemId].MaxCount;
                ItemCount = Mathf.Min(count, maxCount);
            }

            public void AddCount(int count)
            {
                int tmpCount = Mathf.Max(ItemCount + count, 0);
                SetCount(tmpCount);
            }
        }

        public List<InventoryItem> ConsumableList { get; private set; }
        public List<InventoryItem> EquipmentList { get; private set; }


        public readonly int StartConsumableSlotSize = 20;
        public readonly int StartEquipmentSlotSize = 28;

        // �Ҹ�ǰ
        public readonly int AddSlotSize = 5;
        public readonly int MaxSlotSize = 50;

        // TODO : itemID�� ���� �� �ִ� ���������...??
        // ItemID, index
        public event Action<int, int> onConsumableChanged;
        public event Action<int, int> onItemUsed;


        public event Action<int> onChangedEquipmentList;
        public event Action<EquipType, int> onChangedEquipedItem;



        // TODO : �̰� �÷��̾ �����ϰ� �־�� �� ����...?
        public int ConsumableSlotSize { get; private set; }
        public int EquipmentSlotSize { get; private set; }


        // �̰͵� �÷��̾ ������ �־�� �� ����
        // int : itemId
        public Dictionary<EquipType, int> EquipedItemDic { get; private set; } = new Dictionary<EquipType, int>()
        {
            { EquipType.Weapon, 0 },
            { EquipType.Shield, 0 },
            { EquipType.Helmet, 0 },
            { EquipType.Armor, 0 },
            { EquipType.Shoes, 0 },
        };




        public void Init()
        {
            // TODO : �ҷ�����
            ConsumableSlotSize = StartConsumableSlotSize;
            ConsumableList = Enumerable.Range(0, ConsumableSlotSize).Select(_ => new InventoryItem()).ToList();

            EquipmentSlotSize = StartEquipmentSlotSize;
            EquipmentList = Enumerable.Range(0, EquipmentSlotSize).Select(_ => new InventoryItem()).ToList();
        }


        // TODO : �̰� �� �����ؼ� �ɰ� �� ���� �� ������??
        // ����Ʈ Ÿ�� ��ü�� �����ϱ� �װɷ�....???
        public void AddItem(int itemId, int amount = 1)
        {
            if (Tables.ConsumableItemTable.IsExist(itemId))
            {
                int maxCount = Tables.ConsumableItemTable[itemId].MaxCount;

                for (int i = 0; i < ConsumableList.Count; i++)
                {
                    if (ConsumableList[i].ItemId == itemId && ConsumableList[i].ItemCount < maxCount)
                    {
                        if (ConsumableList[i].ItemCount + amount <= maxCount)
                        {
                            ConsumableList[i].AddCount(amount);
                            amount = 0;

                            onConsumableChanged?.Invoke(itemId, i);
                            break;
                        }
                        else
                        {
                            ConsumableList[i].SetCount(maxCount);
                            amount -= (maxCount - ConsumableList[i].ItemCount);

                            onConsumableChanged?.Invoke(itemId, i);
                            continue;
                        }
                    }
                }

                if (amount > 0)
                {
                    int index = ConsumableList.FindIndex(x => x.IsExist == false);

                    // TODO : ������ �߰��� ������ ��� �����
                    if (index == -1)
                    {
                        Debug.LogWarning("�߰� ���� : �κ��丮 ���� ��");
                        return;
                    }

                    ConsumableList[index].SetData(itemId, amount);

                    onConsumableChanged?.Invoke(itemId, index);
                }
            }
            else if (Tables.EquipmentItemTable.IsExist(itemId))
            {
                int index = EquipmentList.FindIndex(x => x.IsExist == false);

                if (index == -1)
                {
                    Debug.LogWarning("�߰� ���� : �κ��丮 ���� ��");
                    return;
                }

                EquipmentList[index].SetData(itemId, 1);

                onChangedEquipmentList?.Invoke(index);
            }
            else
            {
                UnityEngine.Debug.LogError($"{nameof(InventoryManager)} : This is Not Item ID ({itemId})");
            }
        }


        #region Consumable

        public void UseItem(int index)
        {
            ConsumableList[index].AddCount(-1);

            onConsumableChanged?.Invoke(ConsumableList[index].ItemId, index);

            if (ConsumableList[index].ItemCount <= 0)
                ConsumableList[index].SetEmpty();
        }


        public bool AddConsumableSlotSize()
        {
            if (ConsumableList.Count + AddSlotSize > MaxSlotSize)
            {
                Debug.LogWarning("���� Ȯ���� �ִ�ġ�Դϴ�.");
                return false;
            }

            for (int i = 0; i < AddSlotSize; i++)
            {
                InventoryItem inventoryItem = new InventoryItem();
                ConsumableList.Add(inventoryItem);
            }

            return true;
        }


        public int FindItemCount(int itemId)
        {
            int count = 0;

            foreach (InventoryItem item in ConsumableList)
            {
                if (!item.IsExist)
                    continue;

                if (item.ItemId == itemId)
                {
                    count += item.ItemCount;
                }
            }

            return count;
        }


        public int GetItemCountInInventory()
        {
            int count = 0;

            foreach (InventoryItem item in ConsumableList)
            {
                if (item.IsExist)
                    count++;
            }

            return count;
        }

        #endregion


        #region Equipment

        // TODO : ���� ��� Ÿ�Ժ��� ���� ����� �ִ�
        public void Equip(int index)
        {
            InventoryItem item = EquipmentList[index];

            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{Tables.EquipmentItemTable[item.ItemId].Parts}");

            int prevEquipItemId = EquipedItemDic[type];

            EquipedItemDic[type] = item.ItemId;
            onChangedEquipedItem?.Invoke(type, item.ItemId);

            EquipmentList[index].SetEmpty();
            onChangedEquipmentList?.Invoke(index);

            if (prevEquipItemId > 0)
                AddItem(prevEquipItemId);


            // NOTE : �ٲٴ� �������� �ڸ� �״�� �ϰ� �ʹٸ�
            //if (prevEquipItemId > 0)
            //{
            //    EquipmentList[index].itemId = prevEquipItemId;
            //    EquipmentList[index].itemCount = 1;
            //    EquipmentList[index].ItemType = ItemType.Equipment;
            //}

            //onChangedEquipmentList?.Invoke(index);
        }


        public void UnEquip(int itemId)
        {
            // TODO : ������ ������ ���Կ� ������ �־���ؼ� üũ�ϱ�
            KeyValuePair<EquipType, int> tmp = EquipedItemDic.First(x => x.Value == itemId);
            EquipedItemDic[tmp.Key] = 0;

            onChangedEquipedItem?.Invoke(tmp.Key, 0);

            // TODO : �κ��丮�� �ǵ�����
            AddItem(itemId);
        }

        public void SortEquipment(EquipType equipType = EquipType.None)
        {
            if (equipType == EquipType.None)
            {
                EquipmentList = EquipmentList
                    .OrderByDescending(x => x.ItemId > 0)
                    .ThenBy(x => x.ItemId)
                    .ToList();
            }
            else
            {
                EquipmentList = EquipmentList
                    .OrderByDescending(x => x.EquipType == equipType)
                    .ThenByDescending(x => x.ItemId > 0)
                    .ThenBy(x => x.ItemId)
                    .ToList();
            }            

            for (int i = 0; i < EquipmentList.Count; i++)
            {
                onChangedEquipmentList?.Invoke(i);
            }
        }

        #endregion
    }
}
