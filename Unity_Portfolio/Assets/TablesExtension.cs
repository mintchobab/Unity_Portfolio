using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public static class TablesExtension
    {
        public static bool IsExist(this ItemTable table, int id)
        {
            return Tables.ItemTable[id] != null;
        }

        public static bool IsExist(this EquipmentItemTable table, int id)
        {
            return Tables.EquipmentItemTable[id] != null;
        }
    }
}
