using UnityEngine;

[CreateAssetMenu(fileName = "Stat Data", menuName = "Scriptable Object/Stat Data", order = int.MaxValue)]
public class StatData : ScriptableObject
{
    [SerializeField]
    private int hp;
    public int Hp { get => hp; }

    [SerializeField]
    private int mp;
    public int Mp { get => mp; }

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
