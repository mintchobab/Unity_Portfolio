using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public static class TablesExtension
    {
        public static bool IsExist(this ConsumableItemTable table, int id)
        {
            return Tables.ConsumableItemTable[id] != null;
        }

        public static bool IsExist(this EquipmentItemTable table, int id)
        {
            return Tables.EquipmentItemTable[id] != null;
        }
    }
}
