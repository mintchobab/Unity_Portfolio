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

        public event Action onExchangedAllItems;
        public event Action<int> onAddedItem;
        public event Action<int, int> onExchangedItem;
        public event Action<int, int> onMovedItem;
        public event Action<EquipType, EquipItem> onEquipedItem;
        public event Action<EquipType, EquipItem> onUnEquipedItem;

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


        public void Init()
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


        public void AddEquipItem(int itemId)
        {
            if (currentItemCount >= StartSlotSize)
            {
                Debug.Log("장비 인벤토리 초과");
                return;
            }

            EquipInventoryItem inventoryItem = MakeNewEquipItem(itemId);

            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].item == null)
                {
                    ItemList[i] = inventoryItem;
                    onAddedItem?.Invoke(i);
                    currentItemCount++;
                    break;
                }

                if (inventoryItem.item.id >= ItemList[i].item.id)
                {
                    continue;
                }
                else
                {
                    PushList(i);
                    ItemList[i] = inventoryItem;
                    onAddedItem?.Invoke(i);
                    currentItemCount++;
                    break;
                }
            }
        }


        public void RemoveEquipItem(int index)
        {
            int lastIndex = ItemList.Count - 1;

            for (int i = index; i < ItemList.Count - 1; i++)
            {
                if (ItemList[i + 1].item == null)
                {
                    ItemList[i] = ItemList[i + 1];
                    onMovedItem(i + 1, i);
                    break;
                }
                else
                {
                    ItemList[i] = ItemList[i + 1];
                    onMovedItem(i + 1, i);
                }
            }

            currentItemCount--;
        }



        public void Equip(int index)
        {
            EquipItem item = ItemList[index].item;
            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");

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

            onEquipedItem?.Invoke(type, item);
        }


        public void UnEquip(EquipItem item)
        {
            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");
            EquipedItemDic[type] = null;

            AddEquipItem(item.id);

            onUnEquipedItem?.Invoke(type, item);
        }


        public void SortInventory(EquipType tabType)
        {
            if (tabType == EquipType.Default)
            {
                SortInventoryAll();                
                return;
            }

            List<EquipInventoryItem> tmpList = new List<EquipInventoryItem>();

            for (int i = ItemList.Count - 1; i >= 0; i--)
            {
                if (ItemList[i].item == null)
                    continue;

                EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{ItemList[i].item._parts}");

                if (type.Equals(tabType))
                {
                    tmpList.Add(ItemList[i]);
                    ItemList.RemoveAt(i);
                }
            }

            tmpList.Sort((x, y) => x.item.id.CompareTo(y.item.id));

            for (int i = 0; i < tmpList.Count; i++)
            {
                ItemList.Insert(i, tmpList[i]);
            }

            onExchangedAllItems?.Invoke();
        }


        private void SortInventoryAll()
        {
            List<EquipInventoryItem> tmpList = new List<EquipInventoryItem>();

            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].item == null)
                    continue;

                tmpList.Add(ItemList[i]);
            }

            tmpList.Sort((x, y) => x.item.id.CompareTo(y.item.id));

            for (int i = 0; i < tmpList.Count; i++)
            {
                ItemList[i] = tmpList[i];
            }

            onExchangedAllItems?.Invoke();
        }


        private void PushList(int startIndex)
        {
            int lastIndex = 0;

            for (int i = 0; i < ItemList.Count; i++)
            {
                if(ItemList[i].item == null)
                {
                    lastIndex = i;
                    break;
                }
            }

            for (int i = lastIndex; i > startIndex; i--)
            {
                ItemList[i] = ItemList[i - 1];
                onExchangedItem?.Invoke(i - 1, i);
            }
        }


        private EquipInventoryItem MakeNewEquipItem(int itemId)
        {
            EquipItem itemData = Managers.Instance.ItemManager.GetEquipItem(itemId);
            return new EquipInventoryItem() { item = itemData };
        }
    }
}
