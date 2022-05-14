using System;
using System.Collections.Generic;


[Serializable]
public class JsonNPCData
{
    public List<NPC> npc;
}


[Serializable]
public class NPC
{
    public string id;
    public string name;
    public Contents[] contents;
}


[Serializable]
public class Contents
{
    public string[] contents;
}
