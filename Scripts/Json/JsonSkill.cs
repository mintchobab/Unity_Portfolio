using System;
using System.Collections.Generic;

namespace lsy
{
    [Serializable]
    public class JsonSkill
    {
        public List<Skill> skills;
    }

    public class Skill
    {
        public int id;

        public string skillName;
        public string _resourceName;
        public float coolTime;

        public int minDamage;
        public int maxDamage;
    }    
}

