using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : IManager
{
    // ��ü ����Ʈ ����Ʈ
    private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;

    // ���� �� �ִ� ������ ����Ʈ ID
    private List<int> beforeStartingIdList = new List<int>() { 0 };

    // �������� ����Ʈ ID
    private List<int> progressingIdList;

    // �Ϸ�� ����Ʈ ID
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



    // NPC���� ����Ʈ Ȱ��ȭ
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
