using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : IManager
{
    // 전체 퀘스트 리스트
    private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;

    // 받을 수 있는 상태의 퀘스트 ID
    private List<int> beforeStartingIdList = new List<int>() { 0 };

    // 진행중인 퀘스트 ID
    private List<int> progressingIdList;

    // 완료된 퀘스트 ID
    private List<int> completedIdList;

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
        Quest quest = allQuestList.Find(x => x.questId == questId);
        int questNpcId = quest.npcId;

        foreach(InteractQuestNPC npc in npcs)
        {
            if (npc.NPCId == questNpcId)
            {
                npc.SetCurrentQuest(quest);
                break;
            }
        }
    }
}
