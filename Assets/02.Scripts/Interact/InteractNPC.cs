using UnityEngine;
using System;

namespace lsy
{
    [RequireComponent(typeof(WorldUIName))]
    public class InteractNpc : MonoBehaviour, IInteractable
    {
        // NPC ���� 
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


        // �÷��̾�� ��ȣ�ۿ��� �������� �� ó��
        public void Interact()
        {
            Action action = () =>
            {
                anim.SetTrigger(hashTalk);
                worldUIName.Hide();
            };

            PlayerController.Instance.StartInteract(this, transform, action, null);
        }


        // ��ȣ�ۿ� ������ �� ����Ǵ� �̺�Ʈ
        public void OnDialougeClosed()
        {
            anim.SetTrigger(hashIdle);
            worldUIName.Show();
        }


        // ��ȣ�ۿ� ��ư �̹��� Load
        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconDialogue);
        }


        // Transform ��ȯ
        public Transform GetTransform()
        {
            return transform;
        }


        // ��ȣ�ۿ� �Ÿ� ��ȯ
        public float GetInteractDistance()
        {
            return ValueData.NpcDistance;
        }


        // ����Ʈ �ο��ϱ�
        public void SetQuest(Quest quest)
        {
            MyQuest = quest;

            // ����ǥ ����
            CreateExclamationMark();
        }


        // ����Ʈ ���� �ʱ�ȭ
        public void ResetQuest()
        {
            MyQuest = null;
        }


        // NPC�� ������ ����
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


        // ����ǥ ����
        private void CreateExclamationMark()
        {
            exclamationMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.ExclamationMark, transform);
            exclamationMark.transform.localPosition = new Vector3(0f, 2.65f, 0f);
        }


        // ����ǥ ����
        public void DestoryExclamationMark()
        {
            if (exclamationMark)
            {
                Destroy(exclamationMark);
                exclamationMark = null;
            }
        }


        // ����ǥ ����
        public void CreateQuestionMark()
        {
            questionMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.QuestionMark, transform);
            questionMark.transform.localPosition = new Vector3(0f, 2.1f, 0f);
        }


        // ����ǥ ����
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
