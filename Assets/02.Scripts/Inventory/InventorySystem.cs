using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemInSlot
{
    public Item item;
    public ItemSlot slot;

    public ItemInSlot(Item item, ItemSlot slot)
    {
        this.item = item;
        this.slot = slot;
    }
}


public class InventorySystem : Singleton<InventorySystem>
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private int slotRowCount = 10;

    [SerializeField]
    private Button expandButton;

    private List<ItemInSlot> slotList = new List<ItemInSlot>();

    private Canvas canvas;

    private const string PATH_SLOT_ROW = "UI/SlotRow";

    private bool isActive;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        expandButton.onClick.AddListener(() => CreateSlotRows(1));
        CreateSlotRows(slotRowCount);
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
        canvas.enabled = true;
    }


    public void Deactivate()
    {
        isActive = false;
        canvas.enabled = false;

        CommonSystem.ClearSelectedSlot();
    }


    private void CreateSlotRows(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject slotRow = Instantiate(Resources.Load<GameObject>(PATH_SLOT_ROW));
            slotRow.transform.SetParent(scrollContent);
            slotRow.transform.localScale = Vector3.one;

            for (int j = 0; j < slotRow.transform.childCount; j++)
            {
                ItemSlot slot = slotRow.transform.GetChild(j).GetComponent<ItemSlot>();

                ItemInSlot itemInSlot = new ItemInSlot(null, slot);
                slotList.Add(itemInSlot);
            }
        }
    }


    public void Add(ItemData itemData, int count = 0)
    {
        if (itemData.ItemType == ItemType.Equipment)
        {
            AddCountlessItem(itemData);
        }
        else if (itemData.ItemType == ItemType.Consumerable)
        {
            AddCountableItem(itemData, count);
        }
    }


    // 개수가 있는 아이템 획득
    // 아이템 생성 or 추가를 구분하기
    private void AddCountableItem(ItemData itemData, int count)
    {
        // 아이템을 가지고 있다면 개수 추가
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].item == null)
                continue;

            if (slotList[i].item.ItemData.Id == itemData.Id)
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

        ConsumerableData currentData = (ConsumerableData)slotList[index].item.ItemData;

        // 아이템의 최대치를 넘지 않을 때
        if (slotList[index].item.CurrentCount + count <= currentData.MaxCount)
        {
            slotList[index].item.IncreaseItemCount(count);
            count = 0;
            returnFlag = true;
        }
        // 아이템의 최대치를 넘을 때
        else
        {
            int current = currentData.MaxCount - slotList[index].item.CurrentCount;
            slotList[index].item.IncreaseItemCount(current);
            count -= current;
            returnFlag = false;
        }

        // UI 갱신
        slotList[index].slot.UpdateSlotCount(slotList[index].item);
        slotList[index].slot.UpdateSlotImage(slotList[index].item);

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
        for (int i = 0; i < slotList.Count; i++)
        { 
            if (slotList[i].item == null)
            {
                Item item = new Item();
                item.SetItemData(itemData);

                slotList[i].item = item;
                slotList[i].slot.UpdateSlotImage(item);
                slotList[i].slot.UpdateSlotCount(item);

                return;
            }
        }

        Debug.LogWarning("인벤토리가 가득찼습니다.");
    }


    // 아이템 제거
    public void RemoveItem(int index)
    {
        slotList[index].item = null;
        slotList[index].slot.ClearSlot();
    }


    #region 아이템 사용 or 장비 장착

    // 이름바꾸기
    public void ClickedInventorySlot(ItemSlot slot)
    {
        int index = FindSlotIndex(slot);

        Item item = slotList[index].item;

        if (item == null)
            return;

        if (item.ItemData.ItemType == ItemType.Consumerable)
        {
            UseItem(index);
        }
        else if (item.ItemData.ItemType == ItemType.Equipment)
        {
            RemoveItem(index);
            EquipmentSystem.Instance.EquipItem(item);
        }
        else
        {
            return;
        }
    }


    private int FindSlotIndex(ItemSlot slot)
    {
        return slotList.FindIndex(0, x => x.slot == slot);
    }


    private void UseItem(int index)
    {
        // 개수 1개 감소
        slotList[index].item.DecreseItemCount(1);

        // 개수 0개가 되면 슬롯 비우기
        if (slotList[index].item.CurrentCount == 0)
        {
            RemoveItem(index);
        }
        else
        {
            slotList[index].slot.UpdateSlotCount(slotList[index].item);
        }
    }

    #endregion

}
