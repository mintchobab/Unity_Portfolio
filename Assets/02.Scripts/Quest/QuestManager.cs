using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : IManager
{
    // ��ü ����Ʈ ����Ʈ
    private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;

    // ���� �� �ִ� ������ ����Ʈ ID
    private List<int> beforeStartingIdList = new List<int>();

    // �������� ����Ʈ ID
    public List<Quest> ProgressingList { get; private set; } = new List<Quest>();

    // �Ϸ�� ����Ʈ ID
    public List<Quest> CompletedList { get; private set; } = new List<Quest>();

    private List<InteractQuestNPC> npcs;


    public event Action<Quest, int> onChangeProgressingList;
    public event Action<Quest, int> onChangeCompletedList; 




    public void Initialize()
    {
        SetNpcsInScene();
        //SetQuestByNPC();
    }


    public void SetNpcsInScene()
    {
        npcs = UnityEngine.Object.FindObjectsOfType<InteractQuestNPC>().ToList();

        SetQuestByNPC(2000);
    }


    public void AddProgressQuest(Quest quest)
    {
        ProgressingList.Add(quest);
        onChangeProgressingList?.Invoke(quest, ProgressingList.Count);
    }


    public void RemovePrgressQuest(Quest quest)
    {
        if (ProgressingList.Contains(quest))
            ProgressingList.Remove(quest);

        onChangeProgressingList?.Invoke(quest, ProgressingList.Count);
    }


    public void AddCompletedQuest(Quest quest)
    {
        CompletedList.Add(quest);
    }



    // NPC���� ����Ʈ �ο�
    private void SetQuestByNPC(int questId)
    {
        Quest quest = FindQuest(questId);
        int questNpcId = quest.npcId;

        foreach (InteractQuestNPC npc in npcs)
        {
            if (npc.NPCId == questNpcId)
            {
                npc.SetCurrentQuest(quest);
                break;
            }
        }
    }


    // questId�� ���� Quest�� ã��
    public Quest FindQuest(int questId)
    {
        return allQuestList.Find(x => x.questId == questId);
    }



    //// ID ����Ʈ�� ������ Quest ����Ʈ�� ��ȯ
    //public List<Quest> GetProgressingQuestList()
    //{
    //    List<Quest> questList = new List<Quest>();

    //    foreach (Quest quest in ProgressingList)
    //    {
    //        questList.Add(quest);
    //    }

    //    return questList;
    //}


    //// ID ����Ʈ�� �Ϸ�� Quest ����Ʈ�� ��ȯ
    //public List<Quest> GetCompletedQuestList()
    //{
    //    List<Quest> questList = new List<Quest>();

    //    foreach (Quest quest in CompletedList)
    //    {
    //        //Quest quest = FindQuest(value);
    //        questList.Add(quest);
    //    }

    //    return questList;
    //}
}
