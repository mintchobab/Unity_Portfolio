using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace lsy
{
    public partial class InventoryManager : IManager
    {
        public class InventoryItem
        {
            public ItemType ItemType = ItemType.None;
            public int itemId = -1;
            public int itemCount = 0;

            public bool IsExist => itemId != -1;

            public void SetEmpty()
            {
                ItemType = ItemType.None;
                itemId = -1;
                itemCount = 0;
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
            bool? isConsumable = IsConsumableItem(itemId);

            if (!isConsumable.HasValue)
                return;

            if (isConsumable.Value)
            {
                int maxCount = Tables.ConsumableItemTable[itemId].MaxCount;

                for (int i = 0; i < ConsumableList.Count; i++)
                {
                    if (ConsumableList[i].itemId == itemId && ConsumableList[i].itemCount < maxCount)
                    {
                        if (ConsumableList[i].itemCount + amount <= maxCount)
                        {
                            ConsumableList[i].itemCount += amount;
                            amount = 0;

                            onConsumableChanged?.Invoke(itemId, i);
                            break;
                        }
                        else
                        {
                            ConsumableList[i].itemCount = maxCount;
                            amount -= (maxCount - ConsumableList[i].itemCount);

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

                    ConsumableList[index].itemId = itemId;
                    ConsumableList[index].itemCount = amount;
                    ConsumableList[index].ItemType = ItemType.Consumable;

                    onConsumableChanged?.Invoke(itemId, index);
                }
            }
            else if (!isConsumable.Value)
            {
                int index = EquipmentList.FindIndex(x => x.IsExist == false);

                if (index == -1)
                {
                    Debug.LogWarning("�߰� ���� : �κ��丮 ���� ��");
                    return;
                }

                EquipmentList[index].itemId = itemId;
                EquipmentList[index].itemCount = 1;
                EquipmentList[index].ItemType = ItemType.Equipment;

                onChangedEquipmentList?.Invoke(index);
            }
        }


        #region Consumable

        public void UseItem(int index)
        {
            ConsumableList[index].itemCount--;

            onConsumableChanged?.Invoke(ConsumableList[index].itemId, index);

            if (ConsumableList[index].itemCount <= 0)
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


        public bool? IsConsumableItem(int itemId)
        {
            if (Tables.ConsumableItemTable[itemId] != null)
                return true;

            if (Tables.EquipmentItemTable[itemId] != null)
                return false;

            UnityEngine.Debug.LogError($"{nameof(InventoryManager)} : This is Not Item ID ({itemId})");

            return null;
        }


        public int FindItemCount(int itemId)
        {
            int count = 0;

            foreach (InventoryItem item in ConsumableList)
            {
                if (!item.IsExist)
                    continue;

                if (item.itemId == itemId)
                {
                    count += item.itemCount;
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

            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{Tables.EquipmentItemTable[item.itemId].Parts}");

            int prevEquipItemId = EquipedItemDic[type];

            EquipedItemDic[type] = item.itemId;
            onChangedEquipedItem?.Invoke(type, item.itemId);

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


        // ------------------------------------------------


        //    public void SortInventory(EquipType tabType)
        //    {
        //        if (tabType == EquipType.Default)
        //        {
        //            SortInventoryAll();                
        //            return;
        //        }

        //        List<EquipInventoryItem> tmpList = new List<EquipInventoryItem>();

        //        for (int i = ItemList.Count - 1; i >= 0; i--)
        //        {
        //            if (ItemList[i].item == null)
        //                continue;

        //            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{ItemList[i].item._parts}");

        //            if (type.Equals(tabType))
        //            {
        //                tmpList.Add(ItemList[i]);
        //                ItemList.RemoveAt(i);
        //            }
        //        }

        //        tmpList.Sort((x, y) => x.item.id.CompareTo(y.item.id));

        //        for (int i = 0; i < tmpList.Count; i++)
        //        {
        //            ItemList.Insert(i, tmpList[i]);
        //        }

        //        onExchangedAllItems?.Invoke();
        //    }


        //    private void SortInventoryAll()
        //    {
        //        List<EquipInventoryItem> tmpList = new List<EquipInventoryItem>();

        //        for (int i = 0; i < ItemList.Count; i++)
        //        {
        //            if (ItemList[i].item == null)
        //                continue;

        //            tmpList.Add(ItemList[i]);
        //        }

        //        tmpList.Sort((x, y) => x.item.id.CompareTo(y.item.id));

        //        for (int i = 0; i < tmpList.Count; i++)
        //        {
        //            ItemList[i] = tmpList[i];
        //        }

        //        onExchangedAllItems?.Invoke();
        //    }


        //    private void PushList(int startIndex)
        //    {
        //        int lastIndex = 0;

        //        for (int i = 0; i < ItemList.Count; i++)
        //        {
        //            if(ItemList[i].item == null)
        //            {
        //                lastIndex = i;
        //                break;
        //            }
        //        }

        //        for (int i = lastIndex; i > startIndex; i--)
        //        {
        //            ItemList[i] = ItemList[i - 1];
        //            onExchangedItem?.Invoke(i - 1, i);
        //        }
        //    }


        //    private EquipInventoryItem MakeNewEquipItem(int itemId)
        //    {
        //        //EquipmentItemTable.Instance[itemId].;

        //        EquipItem itemData = Managers.Instance.ItemManager.GetEquipItem(itemId);
        //        return new EquipInventoryItem() { item = itemData };
        //    }
        //}

        #endregion
    }
}
