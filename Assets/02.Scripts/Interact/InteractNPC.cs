using UnityEngine;
using System;

namespace lsy
{
    [RequireComponent(typeof(WorldUIName))]
    public class InteractNpc : MonoBehaviour, IInteractable
    {
        // NPC 정보 
        [field: SerializeField]
        public int NpcId { get; private set; }

        private GameObject exclamationMark;
        private GameObject questionMark;
        private Animator anim;
        private WorldUIName worldUIName;

        private int hashIdle = Animator.StringToHash("idle");
        private int hashTalk = Animator.StringToHash("talk");

        public Npc Npc { get; private set; }
        public Quest MyQuest { get; private set; }
        public string NpcName { get; private set; }


        protected void Awake()
        {
            worldUIName = GetComponent<WorldUIName>();
            anim = GetComponentInChildren<Animator>();

            SetNPCData();
            NpcName = StringManager.GetLocalizedNPCName(Npc.name);
        }


        // 플레이어와 상호작용을 시작했을 때 처리
        public void Interact()
        {
            Action action = () =>
            {
                anim.SetTrigger(hashTalk);
                worldUIName.Hide();
            };

            PlayerController.Instance.StartInteract(this, transform, action, null);
        }


        // 상호작용 끝났을 때 실행되는 이벤트
        public void OnDialougeClosed()
        {
            anim.SetTrigger(hashIdle);
            worldUIName.Show();
        }


        // 상호작용 버튼 이미지 Load
        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconDialogue);
        }


        // Transform 반환
        public Transform GetTransform()
        {
            return transform;
        }


        // 상호작용 거리 반환
        public float GetInteractDistance()
        {
            return ValueData.NpcDistance;
        }


        // 퀘스트 부여하기
        public void SetQuest(Quest quest)
        {
            MyQuest = quest;

            // 느낌표 띄우기
            CreateExclamationMark();
        }


        // 퀘스트 정보 초기화
        public void ResetQuest()
        {
            MyQuest = null;
        }


        // NPC의 데이터 설정
        private void SetNPCData()
        {
            JsonNpc dialogueData = Managers.Instance.JsonManager.jsonNPC;

            foreach (Npc npc in dialogueData.npcs)
            {
                if (npc.id.Equals(NpcId))
                    this.Npc = npc;
            }

            if (Npc == null)
                Debug.LogError("NPC Data Not Found");
        }


        // 느낌표 생성
        private void CreateExclamationMark()
        {
            exclamationMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.ExclamationMark, transform);
            exclamationMark.transform.localPosition = new Vector3(0f, 2.65f, 0f);
        }


        // 느낌표 삭제
        public void DestoryExclamationMark()
        {
            if (exclamationMark)
            {
                Destroy(exclamationMark);
                exclamationMark = null;
            }
        }


        // 물음표 생성
        public void CreateQuestionMark()
        {
            questionMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.QuestionMark, transform);
            questionMark.transform.localPosition = new Vector3(0f, 2.1f, 0f);
        }


        // 물음표 삭제
        public void DestroyQuestionMark()
        {
            if (questionMark)
            {
                Destroy(questionMark);
                questionMark = null;
            }
        }

    }
}
