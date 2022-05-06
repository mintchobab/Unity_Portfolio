using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Data", menuName = "Scriptable Object/Equipment Data", order = int.MaxValue)]

public class EquipmentData : ItemData
{
    [Space(10)]
    [SerializeField]
    private EquipmentType equipmentType;
    public EquipmentType EquipmentType { get => equipmentType; }

    [SerializeField]
    private int hp;
    public int Hp { get => hp; }

    [SerializeField]
    private int damage;
    public int Damage { get => damage; }

    [SerializeField]
    private int armor;
    public int Armor { get => armor; }

    [SerializeField]
    private int moveSpeed;
    public int MoveSpeed { get => moveSpeed; }
}
