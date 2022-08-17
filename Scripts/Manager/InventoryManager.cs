using System;
using System.Collections.Generic;


namespace lsy
{
    [Serializable]
    public class InventoryItem
    {
        public CountableItem item;
        public int count;
    }


    public class InventoryManager : IManager
    {
        public List<InventoryItem> ItemList { get; private set; } = new List<InventoryItem>();

        // int : itemId, int : itemListIndex
        public event Action<int, int> onItemAdded;
        public event Action<int, int> onItemChanged;
        public event Action<int, int> onItemUsed;

        public readonly int StartSlotSize = 20;
        public readonly int AddSlotSize = 5;
        public readonly int MaxSlotSize = 50;

        public int CurrentSlotSize { get; private set; }


        public void Init()
        {
            AddInventorySlot(StartSlotSize);
        }


        public void AddInventorySlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                InventoryItem inventoryItem = new InventoryItem();
                ItemList.Add(inventoryItem);
                CurrentSlotSize++;
            }
        }


        public int GetItemCountInInventory()
        {
            int itemCount = ItemList.Count;

            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].count == 0)
                {
                    itemCount = i;
                    break;
                }                
            }
            return itemCount;
        }


        public void AddCountableItem(int itemId, int count)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                // 아이템 새로 생성
                if (ItemList[i].item == null)
                {
                    ItemList[i] = MakeNewInventoryItem(itemId, count);
                    onItemAdded?.Invoke(itemId, i);
                    return;
                }                    

                if (ItemList[i].item.id == itemId)
                {
                    int currentCount = ItemList[i].count + count;

                    if (currentCount > ItemList[i].item.maxCount)
                    {
                        ItemList[i].count = ItemList[i].item.maxCount;
                        count = currentCount - ItemList[i].item.maxCount;
                        onItemChanged?.Invoke(itemId, i);
                    }
                    else
                    {
                        ItemList[i].count = currentCount;
                        onItemChanged?.Invoke(itemId, i);
                        return;
                    }
                }
            }

            if (count > 0)
                UnityEngine.Debug.Log("인벤토리 초과");
        }


        private InventoryItem MakeNewInventoryItem(int itemId, int count)
        {
            CountableItem itemData = Managers.Instance.ItemManager.GetCountableItem(itemId);

            return new InventoryItem()
            {
                item = itemData,
                count = count
            };
        }


        public void UseItem(int index)
        {
            ItemList[index].count--;

            onItemUsed?.Invoke(ItemList[index].item.id, index);

            if (ItemList[index].count <= 0)
                ItemList[index].item = null;
        }


        public int FindItemCount(int itemId)
        {
            int count = 0;

            foreach(InventoryItem item in ItemList)
            {
                if (item.item == null)
                    continue;

                if (item.item.id == itemId)
                {
                    count += item.count;
                }
            }

            return count;
        }
    }
}


