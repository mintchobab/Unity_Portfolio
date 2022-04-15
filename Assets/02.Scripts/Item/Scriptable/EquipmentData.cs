using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Data", menuName = "Scriptable Object/Equipment Data", order = int.MaxValue)]

public class EquipmentData : ItemData
{
    [SerializeField]
    protected EquipmentType equipmentType;
    public EquipmentType EquipmentType { get => equipmentType; }
}
