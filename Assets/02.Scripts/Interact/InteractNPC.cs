using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class InteractNPC : InteractBase
    {
        // NPC ���� 
        [field: SerializeField]
        public int NpcId { get; private set; }

        private DialogueUIController dialogueController => Managers.Instance.UIManager.DialogueUIController;
        private QuestManager questManager => Managers.Instance.QuestManager;
        private NPC npc;
        //[SerializeField]
        private Quest currentQuest;

        private bool isQuestProgressing;
        private string npcName;


        protected void Awake()
        {
            SetNPCData();
            npcName = StringManager.GetLocalizedNPCName(npc.name);
        }



        public override void Interact()
        {
            Vector3 targetPos = transform.position + (transform.forward * 2) + Vector3.up;
            Quaternion targetRot = transform.rotation * Quaternion.Euler(0f, 180f, 0f);

            CameraController.Instance.LookTarget(targetPos, targetRot);
            dialogueController.onDialougeClosed += OnDialogueClosed;

            if (currentQuest == null)
                ShowDialogue();
            else
                ShowQuestDialogue();
        }


        // �Ϲ� ��ȭâ Ȱ��ȭ
        private void ShowDialogue()
        {
            List<string> dialogues = new List<string>();
            for (int i = 0; i < npc.dialogues.Count; i++)
            {
                dialogues.Add(StringManager.GetLocalizedNpcDialogue(npc.dialogues[i]));
            }

            dialogueController.SetInitializeInfo(false, npcName, dialogues);
            dialogueController.Show();
        }



        // ����Ʈ ��ȭâ Ȱ��ȭ
        private void ShowQuestDialogue()
        {
            // ����Ʈ �Ϸᵵ üũ....
            if (questManager.CurrentQuest != null && currentQuest == questManager.CurrentQuest.Quest)
            {
                if (questManager.CurrentQuest.IsCompleted)
                {
                    ShowQuestCompleteDialogue();
                    return;
                }
            }

            // ����Ʈ ���� ��
            if (!isQuestProgressing)
            {
                ShowQuestStartDialogue();
            }
            // ����Ʈ ��
            else if (isQuestProgressing)
            {
                ShowQuestProgressingDialogue();
            }
        }


        // ����Ʈ ���� ��ȭâ ���
        private void ShowQuestStartDialogue()
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = currentQuest.startDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetAcceptButtonEvent(() =>
            {
                questManager.TakeQuest(currentQuest);
                isQuestProgressing = true;
            });

            dialogueController.SetInitializeInfo(true, npcName, resultDialogues);
            dialogueController.Show();

        }


        // ����Ʈ ������ ��ȭâ ���
        private void ShowQuestProgressingDialogue()
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = currentQuest.progressingDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetInitializeInfo(false, npcName, resultDialogues);
            dialogueController.Show();
        }



        // ����Ʈ ����
        // ����Ʈ �Ŵ������� ����Ʈ�� �����ٴ°͵� �˷������
        private void ShowQuestCompleteDialogue()
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = currentQuest.endDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetInitializeInfo(false, npcName, resultDialogues);
            dialogueController.Show();

            questManager.CompleteQuest();
        }




        public override Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconDialogue);
        }


        // ����Ʈ �ο��ϱ�
        public void SetQuest(Quest quest)
        {
            currentQuest = quest;

            // ����Ʈ�� �ִ� NPC�� �Ӹ����� ����ǥ�� ���
        }



        private void SetNPCData()
        {
            JsonNPC dialogueData = Managers.Instance.JsonManager.jsonNPC;

            foreach (NPC npc in dialogueData.npcs)
            {
                if (npc.id.Equals(NpcId))
                    this.npc = npc;
            }

            if (npc == null)
                Debug.LogError("NPC Data Not Found");
        }


        // ��ȭâ�� ����� �� ī�޶� ���� �ڸ��� �̵�
        private void OnDialogueClosed()
        {
            CameraController.Instance.RestoreCamera();
        }
        
    }
}
