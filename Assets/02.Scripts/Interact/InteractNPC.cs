using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class InteractNPC : InteractBase
    {
        // NPC 정보 
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


        // 일반 대화창 활성화
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



        // 퀘스트 대화창 활성화
        private void ShowQuestDialogue()
        {
            // 퀘스트 완료도 체크....
            if (questManager.CurrentQuest != null && currentQuest == questManager.CurrentQuest.Quest)
            {
                if (questManager.CurrentQuest.IsCompleted)
                {
                    ShowQuestCompleteDialogue();
                    return;
                }
            }

            // 퀘스트 시작 전
            if (!isQuestProgressing)
            {
                ShowQuestStartDialogue();
            }
            // 퀘스트 중
            else if (isQuestProgressing)
            {
                ShowQuestProgressingDialogue();
            }
        }


        // 퀘스트 시작 대화창 출력
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


        // 퀘스트 진행중 대화창 출력
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



        // 퀘스트 종료
        // 퀘스트 매니저한테 퀘스트가 끝났다는것도 알려줘야함
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


        // 퀘스트 부여하기
        public void SetQuest(Quest quest)
        {
            currentQuest = quest;

            // 퀘스트가 있는 NPC는 머리위에 느낌표가 뜬다
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


        // 대화창이 종료될 때 카메라 원래 자리로 이동
        private void OnDialogueClosed()
        {
            CameraController.Instance.RestoreCamera();
        }
        
    }
}
