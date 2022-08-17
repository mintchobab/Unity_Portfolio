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
    public int questNpcId;
    public string questName;
    public string goal;
    public string content;
    public int nextQuestId;
    public List<string> startDialogues;
    public List<string> progressingDialogues;
    public List<string> endDialogues;
    public Task task;
    public Reward reward;
}


[Serializable]
public class Task
{
    public List<int> collect;
    public List<int> kill;
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
    public int itemId;
    public int itemCount;
}
