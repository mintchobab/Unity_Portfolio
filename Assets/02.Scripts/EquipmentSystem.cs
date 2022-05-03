using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


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
    //[SerializeField]
    //private List<EquipSlot> slots = new List<EquipSlot>();
    private List<ItemInEquipSlot> slotList = new List<ItemInEquipSlot>();
    private CharacterEquipment characterEquipment;

    private bool isActive;



    private void Awake()
    {
        EquipSlot[] equipSlots = GetComponentsInChildren<EquipSlot>();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            ItemInEquipSlot slot = new ItemInEquipSlot(null, equipSlots[i]);
            slotList.Add(slot);
        }

        characterEquipment = FindObjectOfType<CharacterEquipment>();
    }


    // 아이템 장착
    public void EquipItem(Item item)
    {
        int index = FindSlotIndex(item);
        slotList[index].item = item;
        slotList[index].slot.UpdateSlotImage(item);

        // 실제 장비 변경
        characterEquipment.Equip(item);

        // 장비 효과도 추가하기???
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


    // 아이템 장착 해제
    public void UnequipItem(EquipSlot slot)
    {
        int index = FindSlotIndex(slot);        

        // 인벤토리로 이동
        ItemData data = slotList[index].item.ItemData;

        InventorySystem.Instance.Add(data);

        slotList[index].item = null;

        EquipmentData equipmentData = data as EquipmentData;
        characterEquipment.UnEquip(equipmentData.EquipmentType);

        slotList[index].slot.ClearSlot();
    }

}
