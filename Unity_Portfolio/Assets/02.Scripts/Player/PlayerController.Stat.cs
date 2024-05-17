using System;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public partial class PlayerController
    {
        public class Stat
        {
            public int hp;
            public int offensivePower;
            public int defensivePower;

            public void ResetValue()
            {
                hp = 0;
                offensivePower = 0;
                defensivePower = 0;
            }
        }
        
        
        public Stat BaseStat { get; private set; } = new();
        public Stat EquipmentStat { get; private set; } = new();
        public Stat TotalStat { get; private set; } = new();


        private void InitStat()
        {
            // TODO : 초기 스텟 불러오는 작업이 필요할듯??? 확인해보기
            BaseStat.hp = 1000;
            BaseStat.offensivePower = 100;
            BaseStat.defensivePower = 100;

            TotalStat.hp = BaseStat.hp + EquipmentStat.hp;
            TotalStat.offensivePower = BaseStat.offensivePower + EquipmentStat.offensivePower;
            TotalStat.defensivePower = BaseStat.defensivePower + EquipmentStat.defensivePower;
        }

        private void OnEnableStat()
        {
            inventoryManager.onChangedEquipedItem += OnChangedEquipedItem;
        }


        private void OnDisableStat()
        {
            inventoryManager.onChangedEquipedItem -= OnChangedEquipedItem;
        }


        private void OnChangedEquipedItem(EquipType equipType, int itemId)
        {
            OnChangedEquipedItemInternal();
        }


        private void OnChangedEquipedItemInternal()
        {
            EquipmentStat.ResetValue();

            foreach (KeyValuePair<EquipType, int> parts in inventoryManager.EquipedItemDic)
            {
                if (parts.Value <= 0)
                    continue;

                EquipmentItemTable.TableData tableData = Tables.EquipmentItemTable[parts.Value];

                EquipmentStat.hp += tableData.Hp;
                EquipmentStat.offensivePower += tableData.OffensivePower;
                EquipmentStat.defensivePower += tableData.DefensivePower;
            }

            TotalStat.hp = BaseStat.hp + EquipmentStat.hp;
            TotalStat.offensivePower = BaseStat.offensivePower + EquipmentStat.offensivePower;
            TotalStat.defensivePower = BaseStat.defensivePower + EquipmentStat.defensivePower;
        }


        public int GetMaxHp()
        {
            return TotalStat.hp;
        }

        public int GetDefensivePower()
        {
            return TotalStat.defensivePower;
        }
    }
}
