using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ʈ�� ������ �ִ� NPC
public class InteractQuestNPC : InteractNPC
{
    private Quest currentQuest;


    // ����Ʈ �ޱ��� or ����Ʈ ���� �� �����ؾ� �ҵ�
    // ��ȭ�� �����Ŀ� ����Ʈâ ����
    public override void Interact()
    {
        if (currentQuest == null)
            return;

        dialogueController.SetNameText(StringManager.GetLocalizedNPCName(npc.name));

        //// ����Ʈ ���� ���� �Է�
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
