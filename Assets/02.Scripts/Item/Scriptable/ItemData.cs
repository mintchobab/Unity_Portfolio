using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField]
    protected int id;
    public int Id { get => id; }

    [SerializeField]
    protected Sprite image;
    public Sprite Image { get => image; }

    [SerializeField]
    protected ItemType itemType;
    public ItemType ItemType { get => itemType; }

    [SerializeField]
    protected new string name;
    public string Name { get => name; }

    [SerializeField]
    protected string description;
    public string Description { get => description; }
}
