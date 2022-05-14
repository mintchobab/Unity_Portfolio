using System;
using UnityEngine;

[Serializable]
public class JsonManager
{
    public JsonNPCData npcData;

    public void Init()
    {
        npcData = LoadJson<JsonNPCData>("Dialogue");
    }

    private T LoadJson<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<T>(textAsset.text);
    }
}