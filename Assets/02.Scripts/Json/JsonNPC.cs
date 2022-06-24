using System;
using System.Collections.Generic;


[Serializable]
public class JsonNPC
{
    public List<NPC> npcs;
}


[Serializable]
public class NPC
{
    public int id;
    public string name;
    public List<string> dialogues;
}
