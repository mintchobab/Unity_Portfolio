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


    [Serializable]
    public class CountableItem
    {
        public int id;
        public string itemType;
        public string name;
        public string uniqueName;
        public string description;
        public int maxCount;
    }

}
