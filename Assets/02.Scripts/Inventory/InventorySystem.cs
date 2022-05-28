using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;





public class InventorySystem : Singleton<InventorySystem>
{
    //public void Activate()
    //{
    //    isActive = true;
    //    canvas.enabled = true;
    //}


    //public void Deactivate()
    //{
    //    isActive = false;
    //    canvas.enabled = false;

    //    CommonSystem.ClearSelectedSlot();
    //}






    // 개수가 있는 아이템 획득
    // 아이템 생성 or 추가를 구분하기



    // 개수가 있는 아이템 추가
    //private bool IncreaseItemCount(int index, ref int count)
    //{
    //    bool returnFlag = false;

    //    ConsumerableData currentData = (ConsumerableData)slotList[index].item.ItemData;

    //    // 아이템의 최대치를 넘지 않을 때
    //    if (slotList[index].item.CurrentCount + count <= currentData.MaxCount)
    //    {
    //        slotList[index].item.IncreaseItemCount(count);
    //        count = 0;
    //        returnFlag = true;
    //    }
    //    // 아이템의 최대치를 넘을 때
    //    else
    //    {
    //        int current = currentData.MaxCount - slotList[index].item.CurrentCount;
    //        slotList[index].item.IncreaseItemCount(current);
    //        count -= current;
    //        returnFlag = false;
    //    }

    //    // UI 갱신
    //    slotList[index].slot.UpdateSlotCount(slotList[index].item);
    //    slotList[index].slot.UpdateSlotImage(slotList[index].item);

    //    return returnFlag;
    //}



    //// 아이템 제거
    //public void RemoveItem(int index)
    //{
    //    slotList[index].item = null;
    //    slotList[index].slot.ClearSlot();
    //}


    //private int FindSlotIndex(ItemSlot slot)
    //{
    //    return slotList.FindIndex(0, x => x.slot == slot);
    //}



    #region 아이템 사용 or 장비 장착

    // 이름바꾸기
    public void ClickedInventorySlot()
    {
        //int index = FindSlotIndex(slot);

        //Item item = slotList[index].item;

        //if (item == null)
        //    return;

        //if (item.ItemData.ItemType == ItemType.Consumerable)
        //{
        //    UseItem(index);
        //}
        //else if (item.ItemData.ItemType == ItemType.Equipment)
        //{
        //    RemoveItem(index);
        //    EquipmentSystem.Instance.EquipItem(item);
        //}
        //else
        //{
        //    return;
        //}
    }


    private void UseItem(int index)
    {
        // 개수 1개 감소
        //slotList[index].item.DecreseItemCount(1);

        //// 개수 0개가 되면 슬롯 비우기
        //if (slotList[index].item.CurrentCount == 0)
        //{
        //    RemoveItem(index);
        //}
        //else
        //{
        //    slotList[index].slot.UpdateSlotCount(slotList[index].item);
        //}
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    #endregion

}
