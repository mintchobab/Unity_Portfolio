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


        public event Action<int> onItemNewAdded;
        public event Action<int> onItemAdded;
        public event Action<int> onItemUsed;


        public readonly int StartSlotSize = 20;
        public readonly int AddSlotSize = 5;
        public readonly int MaxSlotSize = 50;

        public int CurrentSlotSize { get; private set; }


        public void Initialize()
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


        // ��ĭ�� �˻��� ������ ���� �߰� �� �������� ����


        // 1. �������� �ִ��� ã��
        // 2. ������ ���� Ȯ��
        //   - ������ �� Max���� ������ �ٷ� �߰�
        //   - ������ �� Max�� ������ ���� ����
        // 3. ������ ���� ����
        public void AddCountableItem(int itemId, int count)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                // ������ ���� ����
                if (ItemList[i].item == null)
                {
                    ItemList[i] = MakeNewInventoryItem(itemId, count);
                    onItemNewAdded?.Invoke(i);
                    return;
                }                    

                if (ItemList[i].item.id == itemId)
                {
                    int currentCount = ItemList[i].count + count;

                    if (currentCount > ItemList[i].item.maxCount)
                    {
                        ItemList[i].count = ItemList[i].item.maxCount;
                        count = currentCount - ItemList[i].item.maxCount;
                        onItemAdded?.Invoke(i);
                    }
                    else
                    {
                        ItemList[i].count = currentCount;
                        onItemAdded?.Invoke(i);
                        return;
                    }
                }
            }

            if (count > 0)
                UnityEngine.Debug.Log("�κ��丮 �ʰ�");
        }

        // InventoryItem�� ���� ����
        private InventoryItem MakeNewInventoryItem(int itemId, int count)
        {
            CountableItem itemData = Managers.Instance.ItemManager.GetConsumableItem(itemId);

            return new InventoryItem()
            {
                item = itemData,
                count = count
            };
        }


        public void UseItem(int index)
        {
            ItemList[index].count--;

            if (ItemList[index].count <= 0)
            {
                ItemList[index].item = null;
            }

            onItemUsed?.Invoke(index);

            // �ڡڡ� ���ȿ���� �߰��ϱ� �ڡڡ�
        }
    }
}


