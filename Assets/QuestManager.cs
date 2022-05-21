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
    public List<int> ProgressingIdList { get; private set; } = new List<int>() { 2000, 2001 };

    // �Ϸ�� ����Ʈ ID
    public List<int> CompletedIdList { get; private set; } = new List<int>() { 2002, 2003, 2004 };

    private List<InteractQuestNPC> npcs;



    public void Init()
    {
        SetNpcsInScene();
        //SetQuestByNPC();
    }


    public void SetNpcsInScene()
    {
        npcs = Object.FindObjectsOfType<InteractQuestNPC>().ToList();

        SetQuestByNPC(2000);
    }



    // NPC���� ����Ʈ Ȱ��ȭ
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


    public Quest FindQuest(int questId)
    {
        return allQuestList.Find(x => x.questId == questId);
    }



    // ID ����Ʈ�� Quest ����Ʈ�� ��ȯ
    public List<Quest> GetProgressingQuestList()
    {
        List<Quest> questList = new List<Quest>();

        foreach (int value in ProgressingIdList)
        {
            Quest quest = FindQuest(value);
            questList.Add(quest);
        }

        return questList;
    }


    // ID ����Ʈ�� Quest ����Ʈ�� ��ȯ
    public List<Quest> GetCompletedQuestList()
    {
        List<Quest> questList = new List<Quest>();

        foreach (int value in CompletedIdList)
        {
            Quest quest = FindQuest(value);
            questList.Add(quest);
        }

        return questList;
    }
}
