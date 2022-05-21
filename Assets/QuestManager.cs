using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : IManager
{
    // 전체 퀘스트 리스트
    private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;

    // 받을 수 있는 상태의 퀘스트 ID
    private List<int> beforeStartingIdList = new List<int>();

    // 진행중인 퀘스트 ID
    public List<int> ProgressingIdList { get; private set; } = new List<int>() { 2000, 2001 };

    // 완료된 퀘스트 ID
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



    // NPC에게 퀘스트 활성화
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



    // ID 리스트를 Quest 리스트로 변환
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


    // ID 리스트를 Quest 리스트로 변환
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
