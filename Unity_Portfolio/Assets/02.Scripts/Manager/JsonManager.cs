using System;
using UnityEngine;
using Newtonsoft.Json;

namespace lsy
{
    [Serializable]
    public class JsonManager : IManager
    {
        public JsonMonster jsonMonster;
        public JsonSkill jsonSkill;


        public void Init()
        {
            string json;

            json = Resources.Load<TextAsset>(ResourcePath.MonsterData).ToString();
            jsonMonster = JsonConvert.DeserializeObject<JsonMonster>(json);

            json = Resources.Load<TextAsset>(ResourcePath.SkillData).ToString();
            jsonSkill = JsonConvert.DeserializeObject<JsonSkill>(json);
        }
    }
}
