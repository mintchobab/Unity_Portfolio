using System;
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
    public List<Quest> ProgressingList { get; private set; } = new List<Quest>();

    // 완료된 퀘스트 ID
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



    // NPC에게 퀘스트 부여
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


    // questId로 현재 Quest를 찾음
    public Quest FindQuest(int questId)
    {
        return allQuestList.Find(x => x.questId == questId);
    }



    //// ID 리스트를 진행중 Quest 리스트로 변환
    //public List<Quest> GetProgressingQuestList()
    //{
    //    List<Quest> questList = new List<Quest>();

    //    foreach (Quest quest in ProgressingList)
    //    {
    //        questList.Add(quest);
    //    }

    //    return questList;
    //}


    //// ID 리스트를 완료된 Quest 리스트로 변환
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
