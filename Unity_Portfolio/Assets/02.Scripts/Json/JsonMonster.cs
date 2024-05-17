using System;
using System.Collections.Generic;

namespace lsy 
{
    [Serializable]
    public class JsonMonster
    {
        public List<Monster> monsters;
    }


    [Serializable]
    public class Monster
    {
        public int id;
        public string monsterName;
        public PlayerController.Stat stat;
    }
}
