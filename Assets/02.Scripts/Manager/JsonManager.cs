using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class JsonManager : IManager
{
    public JsonNPC jsonNPC;
    public JsonString jsonString;
    public JsonQuest jsonQuest;

    private readonly string NPCPath = "Json/JsonNPCData";
    private readonly string StringPath = "Json/JsonStringData";
    private readonly string QuestPath = "Json/JsonQuestData";


    public void Initialize()
    {
        string json;

        // Load String
        json = Resources.Load<TextAsset>(StringPath).ToString();
        jsonString = JsonConvert.DeserializeObject<JsonString>(json);

        // Load NPC
        json = Resources.Load<TextAsset>(NPCPath).ToString();
        jsonNPC = JsonConvert.DeserializeObject<JsonNPC>(json);

        // Loat Quest
        json = Resources.Load<TextAsset>(QuestPath).ToString();
        jsonQuest = JsonConvert.DeserializeObject<JsonQuest>(json);
    }


    private T LoadJson<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<T>(textAsset.text);
    }
}