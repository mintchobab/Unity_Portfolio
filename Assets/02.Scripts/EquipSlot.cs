using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : ItemSlot
{
    [field: SerializeField]
    public EquipmentType EquipmentType { get; private set; }


    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            EquipmentSystem.Instance.UnequipItem(EquipmentType);
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {

    }


    public override void OnDrag(PointerEventData eventData)
    {

    }


    public override void OnEndDrag(PointerEventData eventData)
    {

    }

}
