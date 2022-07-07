using System;
using UnityEngine;
using Newtonsoft.Json;

namespace lsy
{
    [Serializable]
    public class JsonManager : IManager
    {
        public JsonNpc jsonNPC;
        public JsonString jsonString;
        public JsonQuest jsonQuest;
        public JsonItem jsonItem;


        public void Initialize()
        {
            string json;

            // Load String
            json = Resources.Load<TextAsset>(ResourcePath.StringData).ToString();
            jsonString = JsonConvert.DeserializeObject<JsonString>(json);

            // Load NPC
            json = Resources.Load<TextAsset>(ResourcePath.NPCData).ToString();
            jsonNPC = JsonConvert.DeserializeObject<JsonNpc>(json);

            // Load Quest
            json = Resources.Load<TextAsset>(ResourcePath.QuestData).ToString();
            jsonQuest = JsonConvert.DeserializeObject<JsonQuest>(json);

            // Load Item
            json = Resources.Load<TextAsset>(ResourcePath.ItemData).ToString();
            jsonItem = JsonConvert.DeserializeObject<JsonItem>(json);
        }


        private T LoadJson<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            return JsonUtility.FromJson<T>(textAsset.text);
        }
    }
}
