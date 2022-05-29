using System;
using System.Collections.Generic;

namespace lsy
{
    [Serializable]
    public class JsonItem
    {
        public List<CountableItem> consumables;
        public List<CountableItem> etcs;
    }


    public class Item
    {
        public int id;
        public string itemType;
        public string name;
        public string uniqueName;
        public string description;
    }


    [Serializable]
    public class CountableItem : Item
    {
        public int maxCount;
    }

}
