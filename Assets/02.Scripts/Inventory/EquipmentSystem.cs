using System;
using System.Collections.Generic;


public class ItemInEquipSlot
{
    public Item item;
    public EquipSlot slot;

    public ItemInEquipSlot(Item item, EquipSlot slot)
    {
        this.item = item;
        this.slot = slot;
    }
}

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    private List<ItemInEquipSlot> slotList = new List<ItemInEquipSlot>();
    private PlayerEquipment characterEquipment;

    public event Action<EquipmentData> onEquipItem;
    public event Action<EquipmentData> onUnEquipItem;


    protected override void Awake()
    {
        base.Awake();

        EquipSlot[] equipSlots = GetComponentsInChildren<EquipSlot>();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            ItemInEquipSlot slot = new ItemInEquipSlot(null, equipSlots[i]);
            slotList.Add(slot);
        }

        characterEquipment = FindObjectOfType<PlayerEquipment>();
    }


    // 아이템 장착
    // 이미 아이템이 장착되어 있는 경우는????
    public void EquipItem(Item item)
    {
        int index = FindSlotIndex(item);

        // 슬롯에 아이템이 있을때만
        if (slotList[index].item != null)
            UnequipItem(slotList[index].slot);

        slotList[index].item = item;
        slotList[index].slot.UpdateSlotImage(item);

        // 플레이어 장비 변경
        characterEquipment.Equip(item);

        // 장비 효과 변경
        EquipmentData data = slotList[index].item.ItemData as EquipmentData;
        onEquipItem?.Invoke(data);

        InventorySystem.Instance.ItemCount--;
    }


    public void UnequipItem(EquipSlot slot)
    {
        int index = FindSlotIndex(slot);

        // 인벤토리로 이동
        ItemData data = slotList[index].item.ItemData;

        InventorySystem.Instance.Add(data);

        EquipmentData equipmentData = data as EquipmentData;
        characterEquipment.UnEquip(equipmentData.EquipmentType);

        slotList[index].slot.ClearSlot();

        // 장비 효과 변경
        onUnEquipItem?.Invoke(equipmentData);

        slotList[index].item = null;
    }


    private int FindSlotIndex(Item item)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            EquipmentData data = item.ItemData as EquipmentData;

            if (slotList[i].slot.EquipmentType == data.EquipmentType)
                return i;
        }

        return -1;
    }


    private int FindSlotIndex(EquipSlot slot)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].slot == slot)
                return i;
        }

        return -1;
    }

    public override void Init()
    {
        throw new NotImplementedException();
    }
}
