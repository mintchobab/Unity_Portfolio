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
        //public event Action<int> onItemRemoved;

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


        // 아이템 파츠에 따라 정렬되면서 아이템 추가하기
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
                // 한칸씩 밀어내기
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


        // 인벤토리에서 삭제
        public void RemoveEquipItem(int index)
        {
            int lastIndex = ItemList.Count - 1;

            for (int i = index; i < ItemList.Count - 1; i++)
            {
                // 마지막 아이템일 때 한번더하고 종료
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



        // 이미 장착된 아이템이 있는 경우
        public void Equip(int index)
        {
            EquipItem item = ItemList[index].item;
            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");

            // 이미 장착된 아이템 있는지 확인
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

            // 장착됐을때의 효과?? 스텟 변경같은거 여기에 쓰기
            onEquipedItem?.Invoke(type, item);
        }


        // 아이템 해제
        public void UnEquip(EquipItem item)
        {
            EquipType type = (EquipType)Enum.Parse(typeof(EquipType), $"{item._parts}");
            EquipedItemDic[type] = null;

            AddEquipItem(item.id);

            onUnEquipedItem?.Invoke(type, item);

            //PlayerController.Instance.EquipController.UnEquip(type);
        }


        // 탭 버튼이 눌렸을 때 리스트 재정렬
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


        // 모든 아이템 id 순으로 정렬
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



        // 무조건 데이터가 한개이상 존재해야함
        private void PushList(int startIndex)
        {
            int lastIndex = 0;

            // 가장 뒤에있는 빈칸 찾기
            for (int i = 0; i < ItemList.Count; i++)
            {
                if(ItemList[i].item == null)
                {
                    lastIndex = i;
                    break;
                }
            }

            // 밀어내기
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
