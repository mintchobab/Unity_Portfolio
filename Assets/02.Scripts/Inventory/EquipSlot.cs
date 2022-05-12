using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : ItemSlot
{
    [field: SerializeField]
    public EquipmentType EquipmentType { get; private set; }

    protected override void ClickedDouble()
    {
        isClicked = false;
        selectedObj.SetActive(false);

        EquipmentSystem.Instance.UnequipItem(this);
    }

    public override void ClearSlot()
    {
        IsEmpty = true;
        SlotImage.sprite = emptySprite;
    }
}
