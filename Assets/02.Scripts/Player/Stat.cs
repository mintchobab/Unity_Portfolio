using System;

[Serializable]
public class Stat
{
    private int baseValue;
    private int equipmentValue;
    private int buffValue;

    public string Name { get; private set; }
    public string Description { get; private set; }


    public Stat(string name, string description, int baseValue)
    {
        this.Name = name;
        this.Description = description;
        this.baseValue = baseValue;
    }


    public int GetValue()
    {
        return baseValue + equipmentValue;
    }

    public void IncreaseBaseValue(int value)
    {
        baseValue += value;
    }

    public void DecreaseBaseValue(int value)
    {
        baseValue -= value;
    }

    public void IncreaseEquipmentValue(int value)
    {
        equipmentValue += value;
    }

    public void DecreaseEquipmentValue(int value)
    {
        equipmentValue -= value;
    }

    public void IncreaseBuffValue(int value)
    {
        buffValue += value;
    }

    public void DecreaseBuffValue(int value)
    {
        buffValue -= value;
    }
}
