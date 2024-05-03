using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    [System.Serializable]
    public class Stat
    {
        public int hp;
        public int offensivePower;
        public int defensivePower;
    }


    [System.Serializable]
    public class PlayerStat
    {
        public Stat playerBaseStat;

        public Dictionary<EquipType, Stat> playerEquipStat = new Dictionary<EquipType, Stat>()
        {
            { EquipType.Weapon, new Stat() },
            { EquipType.Shield, new Stat() },
            { EquipType.Helmet, new Stat() },
            { EquipType.Armor, new Stat() },
            { EquipType.Shoes, new Stat() }
        };


        public int GetAddedHp()
        {
            int addedHp = playerBaseStat.hp;

            foreach(KeyValuePair<EquipType, Stat> stat in playerEquipStat)
            {
                addedHp += stat.Value.hp;
            }

            return addedHp;
        }


        public int GetAddedOffensivePower()
        {
            int offensive = playerBaseStat.offensivePower;

            foreach (KeyValuePair<EquipType, Stat> stat in playerEquipStat)
            {
                offensive += stat.Value.offensivePower;
            }

            return offensive;
        }

                
        public int GetAddedDefensivePower()
        {
            int defensive = playerBaseStat.defensivePower;

            foreach (KeyValuePair<EquipType, Stat> stat in playerEquipStat)
            {
                defensive += stat.Value.defensivePower;
            }

            return defensive;
        }


        public void ResetStat(EquipType type)
        {
            playerEquipStat[type].hp = 0;
            playerEquipStat[type].offensivePower = 0;
            playerEquipStat[type].defensivePower = 0;
        }
    }
}
