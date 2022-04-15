using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : Singleton<InventorySystem>
{
    [SerializeField]
    private GameObject inventoryObj;

    [field: SerializeField]
    public ItemDragSlot DragSlot { get; private set; }

    [SerializeField]
    private InventorySlot[] slots;


    // 테스트 때문에 인스펙터에서 보이도록 설정
    [SerializeField]
    private List<Item> itemList = new List<Item>();

    private bool isActive;


    private void Awake()
    {
        int length = slots.Length;

        for (int i = 0; i < length; i++)
        {
            slots[i].SetSlotIndex(i);
            itemList.Add(null);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isActive)
                Activate();
            else
                Deactivate();
        }
    }


    public void Activate()
    {
        isActive = true;
        inventoryObj.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        inventoryObj.SetActive(false);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }


    // 1. 장비 아이템 획득
    //   1-1 인벤토리가 가득 찬 경우
    // 2. 소비 아이템 획득
    //   2-1 슬롯에 개수가 가득찬 경우
    //   2-2 인벤토리가 가득 찬 경우
    public void Add(ItemData itemData, int count)
    {
        if (itemData.ItemType == ItemType.Equipment)
            AddCountlessItem(itemData);
        else if (itemData.ItemType == ItemType.Consumerable)
            AddCountableItem(itemData, count);
    }


    // 개수가 있는 아이템 획득
    // 아이템 생성 or 추가를 구분하기
    private void AddCountableItem(ItemData itemData, int count)
    {
        // 아이템을 가지고 있다면 개수 추가
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
                continue;

            if (itemList[i].ItemData.Id == itemData.Id)
            {
                bool isSuccess = IncreaseItemCount(i, ref count);

                if (isSuccess)
                    return;
                else
                    continue;
            }
        }

        // 마지막 index에서 개수가 채워진 경우 반복문이 끝나기 때문에 한번 더 체크
        if (count == 0)
            return;

        // 아이템이 없거나 개수가 남으면 새로 생성
        AddNewItem(itemData, count);
    }


    // 개수가 있는 아이템 추가
    private bool IncreaseItemCount(int index, ref int count)
    {
        bool returnFlag = false;

        ConsumerableData currentData = (ConsumerableData)itemList[index].ItemData;

        // 아이템의 최대치를 넘지 않을 때
        if (itemList[index].CurrentCount + count <= currentData.MaxCount)
        {
            itemList[index].IncreaseItemCount(count);
            count = 0;
            returnFlag = true;
        }
        // 아이템의 최대치를 넘을 때
        else
        {
            int current = currentData.MaxCount - itemList[index].CurrentCount;
            itemList[index].IncreaseItemCount(current);
            count -= current;
            returnFlag = false;
        }

        // UI 갱신
        UpdateSlotCount(index);

        return returnFlag;
    }




    // 개수가 없는 아이템 획득
    // 무조건 아이템 생성
    private void AddCountlessItem(ItemData itemData)
    {
        AddNewItem(itemData, 1);
    }


    private void AddNewItem(ItemData itemData, int count)
    {
        Item item = new Item();
        item.SetItemData(itemData);

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                itemList[i].IncreaseItemCount(count);

                // UI 갱신하기

                UpdateSlotCount(i);
                ChangeSlotImage(i);
                return;
            }
        }

        // 인벤토리가 가득찬 경우 실행됨
        Debug.LogWarning("인벤토리가 가득찼습니다.");
    }


    // 아이템 제거
    public void RemoveItem(int index)
    {
        itemList[index] = null;

        slots[index].SetSlotEmpty();
    }


    private void UpdateSlotCount(int index)
    {
        var itemList = GetItemList();

        if (itemList[index].ItemData.ItemType == ItemType.Equipment)
        {
            if (slots[index].ItemCountText.gameObject.activeSelf)
            {
                slots[index].ItemCountText.gameObject.SetActive(false);
            }

            return;
        }
        else if (itemList[index].ItemData.ItemType == ItemType.Consumerable)
        {
            if (!slots[index].ItemCountText.gameObject.activeSelf)
            {
                slots[index].ItemCountText.gameObject.SetActive(true);
            }

            slots[index].SetItemCount(itemList[index].CurrentCount);
        }
    }

    private void ChangeSlotImage(int index)
    {
        var itemList = GetItemList();

        slots[index].SetItemImage(itemList[index].ItemData.Image);
    }


    public void ExchangeItem(int index, int index2)
    {
        (itemList[index], itemList[index2]) = (itemList[index2], itemList[index]);

        //UI 갱신

        if (itemList[index] == null)
        {
            RemoveItem(index);
        }
        else
        {
            UpdateSlotCount(index);
            ChangeSlotImage(index);
        }


        UpdateSlotCount(index2);
        ChangeSlotImage(index2);
    }



    #region 아이템 사용

    public void RightClickedSlot(int slotIndex)
    {
        Item item = itemList[slotIndex];

        if (item == null)
            return;

        if (item.ItemData.ItemType == ItemType.Consumerable)
        {
            UseItem(item, slotIndex);
        }
        else if (item.ItemData.ItemType == ItemType.Equipment)
        {
            EquipItem();
        }
        else
        {
            return;
        }
    }

    private void UseItem(Item item, int slotIndex)
    {
        // 개수 1개 감소
        item.DecreseItemCount(1);

        // 개수 0개가 되면 슬롯 비우기
        if (item.CurrentCount == 0)
        {
            RemoveItem(slotIndex);
        }
        else
        {
            UpdateSlotCount(slotIndex);
        }
    }

    private void EquipItem()
    {
        //EquipSystem.Instance
    }

    #endregion

}
