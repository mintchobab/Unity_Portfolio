using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트를 가지고 있는 NPC
public class InteractQuestNPC : InteractNPC
{
    private Quest currentQuest;


    // 퀘스트 받기전 or 퀘스트 받은 후 구분해야 할듯
    // 대화가 끝난후에 퀘스트창 띄우기
    public override void Interact()
    {
        if (currentQuest == null)
            return;

        dialogueController.SetNameText(StringManager.GetLocalizedNPCName(npc.name));

        //// 퀘스트 진행 문구 입력
        List<string> dialogues = currentQuest.startDialogues;
        List<string> localizedDialogues = new List<string>();

        foreach (string str in dialogues)
        {
            localizedDialogues.Add(StringManager.GetLocalizedQuestDialogue(str));
        }

        dialogueController.SetCurrentDialogues(localizedDialogues);
        dialogueController.Show();
    }


    public void SetCurrentQuest(Quest quest)
    {
        currentQuest = quest;
    }

}
