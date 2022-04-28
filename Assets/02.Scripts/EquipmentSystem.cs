using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSystem : Singleton<EquipmentSystem>
{
    [SerializeField]
    private GameObject equipObj;

    [SerializeField]
    private EquipSlot[] slots;

    [SerializeField]
    private Sprite emptySprite;

    private List<Item> itemList = new List<Item>();

    private CharacterEquipment characterEquipment;


    private bool isActive;


    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            itemList.Add(null);
        }

        characterEquipment = FindObjectOfType<CharacterEquipment>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        equipObj.SetActive(true);
    }


    public void Deactivate()
    {
        isActive = false;
        equipObj.SetActive(false);
    }


    // 아이템 장착
    // UI 장착 + 실제 캐릭터에도 바뀌어야함
    public void EquipItem(Item item)
    {
        EquipmentData data = item.ItemData as EquipmentData;

        if (data == null)
            return;

        // 슬롯에서 해당 타입을 찾아서 아이콘 넣기
        int index = FindSlotIndex(data.EquipmentType);
        slots[index].SlotImage.sprite = item.ItemData.Image;

        item.SetItemData(data);
        itemList[index] = item;

        // 실제 장비가 변경
        characterEquipment.Equip(item);

        // 장비 효과도 추가하기???
    }




    private int FindSlotIndex(EquipmentType equipType)
    {
        if (slots == null)
            slots = GetComponentsInChildren<EquipSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].EquipmentType == equipType)
            {
                return i;
            }
        }

        return -1;
    }


    // 아이템 장착 해제
    public void UnequipItem(EquipmentType equipType)
    {
        // 이미지 제거
        int index = FindSlotIndex(equipType);
        slots[index].SlotImage.sprite = emptySprite;

        // 인벤토리로 이동
        ItemData data = itemList[index].ItemData;

        InventorySystem.Instance.Add(data);

        itemList[index] = null;

        characterEquipment.UnEquip(equipType);
    }

}
