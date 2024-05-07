using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lsy
{
    [RequireComponent(typeof(WorldUIName))]
    public class InteractNpc : MonoBehaviour, IInteractable
    {
        // ID ã�°� �̷������� �ϸ� �ȵɰ� ������.....
        // ex) �޸� ������ �ߺ��� ID ���� ������ �� ����
        // TODO : ��� �����ϱ�
        [field: SerializeField]
        public int NpcId { get; private set; }

        private GameObject exclamationMark;
        private GameObject questionMark;
        private Animator anim;
        private WorldUIName worldUIName;

        private int hashIdle = Animator.StringToHash("idle");
        private int hashTalk = Animator.StringToHash("talk");

        private readonly float npcDistance = 1f;

        public Quest MyQuest { get; private set; }
        public string NpcName { get; private set; }


        protected void Awake()
        {
            worldUIName = GetComponent<WorldUIName>();
            anim = GetComponentInChildren<Animator>();

            NpcName = StringManager.Get(Tables.NPCTable[NpcId].Name);
        }


        public void Interact()
        {
            Action action = () =>
            {
                anim.SetTrigger(hashTalk);
                worldUIName.Hide();
            };

            PlayerController.Instance.StartInteract(this, transform, action, null);
        }


        public void OnDialougeClosed()
        {
            anim.SetTrigger(hashIdle);
            worldUIName.Show();
        }


        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconDialogue);
        }


        public Transform GetTransform()
        {
            return transform;
        }


        public float GetInteractDistance()
        {
            return npcDistance;
        }


        public void SetQuest(Quest quest)
        {
            MyQuest = quest;
            CreateExclamationMark();
        }


        public void ResetQuest()
        {
            MyQuest = null;
        }

        private void CreateExclamationMark()
        {
            exclamationMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.ExclamationMark, transform);
            exclamationMark.transform.localPosition = new Vector3(0f, 2.65f, 0f);
        }


        public void DestoryExclamationMark()
        {
            if (exclamationMark)
            {
                Destroy(exclamationMark);
                exclamationMark = null;
            }
        }


        public void CreateQuestionMark()
        {
            if (!questionMark)
            {
                questionMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.QuestionMark, transform);
                questionMark.transform.localPosition = new Vector3(0f, 2.1f, 0f);
            }            
        }


        public void DestroyQuestionMark()
        {
            if (questionMark)
            {
                Destroy(questionMark);
                questionMark = null;
            }
        }


        public List<string> GetDialogues()
        {
            return Tables.NPCTable[NpcId].Dialogues.Split(',').ToList();
        }
    }
}
