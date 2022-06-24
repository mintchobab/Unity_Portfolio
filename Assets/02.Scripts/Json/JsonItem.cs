using System;
using System.Collections.Generic;

namespace lsy
{
    [Serializable]
    public class JsonItem
    {
        public List<EquipItem> equips;
        public List<CountableItem> countables;
    }


    public abstract class Item
    {
        public int id;
        public string itemType;
        public string name;
        public string _resourceName;
        public string explanation;
    }


    [Serializable]
    public class CountableItem : Item
    {
        public int maxCount;
    }

    [Serializable]
    public class EquipItem : Item
    {
        public string _parts;
        public int power;
        public int healthPoint;
        public int manaPoint;
    }

}
