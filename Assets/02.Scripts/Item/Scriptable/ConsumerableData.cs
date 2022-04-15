using UnityEngine;

[CreateAssetMenu(fileName = "Consumerable Data", menuName = "Scriptable Object/Consumerable Data", order = int.MaxValue)]
public class ConsumerableData : ItemData
{
    [SerializeField]
    protected int maxCount;
    public int MaxCount { get => maxCount; }
}
