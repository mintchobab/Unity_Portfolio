using System;
using System.Collections.Generic;


[Serializable]
public class JsonQuest
{
    public List<Quest> quests;
}


[Serializable]
public class Quest
{
    public int questId;
    public int npcId;
    public string questName;
    public string description;
    public int nextQuest;
    public List<string> startDialogues;
    public List<string> endDialogues;
    public Task task;
    public Reward reward;
}


[Serializable]
public class Task
{
    public List<string> talk;
    public List<string> items;
    public List<string> kills;
}


[Serializable]
public class Reward
{
    public int exp;
    public int gold;
    public List<RewardItem> items;
}


[Serializable]
public class RewardItem
{
    public int id;
    public int amount;
}


