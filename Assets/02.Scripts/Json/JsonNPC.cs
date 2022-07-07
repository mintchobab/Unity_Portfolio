using System;
using System.Collections.Generic;


[Serializable]
public class JsonNpc
{
    public List<Npc> npcs;
}


[Serializable]
public class Npc
{
    public int id;
    public string name;
    public List<string> dialogues;
}
