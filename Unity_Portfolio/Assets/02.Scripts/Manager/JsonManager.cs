using System;
using UnityEngine;
using Newtonsoft.Json;

namespace lsy
{
    [Serializable]
    public class JsonManager : IManager
    {
        public JsonString jsonString;
        public JsonQuest jsonQuest;
        public JsonItem jsonItem;
        public JsonMonster jsonMonster;
        public JsonSkill jsonSkill;


        public void Init()
        {
            string json;

            json = Resources.Load<TextAsset>(ResourcePath.StringData).ToString();
            jsonString = JsonConvert.DeserializeObject<JsonString>(json);

            json = Resources.Load<TextAsset>(ResourcePath.QuestData).ToString();
            jsonQuest = JsonConvert.DeserializeObject<JsonQuest>(json);

            json = Resources.Load<TextAsset>(ResourcePath.ItemData).ToString();
            jsonItem = JsonConvert.DeserializeObject<JsonItem>(json);

            json = Resources.Load<TextAsset>(ResourcePath.MonsterData).ToString();
            jsonMonster = JsonConvert.DeserializeObject<JsonMonster>(json);

            json = Resources.Load<TextAsset>(ResourcePath.SkillData).ToString();
            jsonSkill = JsonConvert.DeserializeObject<JsonSkill>(json);
        }
    }
}
