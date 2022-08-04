using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class CurrentQuest
    {
        public Quest Quest { get; private set; }
        public QuestType QuestType { get; private set; }

        public bool IsCompleted { get; private set; }

        public int CurrentCount { get; private set; }
        public int GoalCount { get; private set; }


        public CurrentQuest(Quest quest)
        {
            this.Quest = quest;
            IsCompleted = false;
            CurrentCount = 0;
            GoalCount = 0;
            SetQuestType();
        }


        private void SetQuestType()
        {
            if (Quest.task.collect.Count != 0)
            {
                QuestType = QuestType.Collect;
                GoalCount = Quest.task.collect[1];

                // ���� �������� ������ ���� ���� �־ �ѹ��� üũ�ؾ���
                CurrentCount = Managers.Instance.InventoryManager.FindItemCount(Quest.task.collect[0]);
            }
            else if (Quest.task.kill.Count != 0)
            {
                QuestType = QuestType.Kill;
                CurrentCount = 0;
                GoalCount = Quest.task.kill[1];
            }
        }


        public void ChangeCurrentCount(int count)
        {
            CurrentCount = count;
            IsCompleted = CurrentCount >= GoalCount ? true : false;
        }
    }



    public class QuestManager : IManager
    {
        private List<InteractNpc> npcs;

        public Action onQuestChanged;
        public Action onChangedCurrentQuestCount;

        private InteractNpc currentQuestNpc;

        private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;
        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;
        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;

        public CurrentQuest CurrentQuest { get; private set; }



        public void Init()
        {
            npcs = UnityEngine.Object.FindObjectsOfType<InteractNpc>().ToList();

            inventoryManager.onItemAdded += OnQuestItemChanged;
            inventoryManager.onItemChanged += OnQuestItemChanged;
            inventoryManager.onItemUsed += OnQuestItemChanged;
            //SetQuestToNPC();
        }


        // NPC���� ����Ʈ �ο�
        // ����Ʈ�� �ִ� NPC���� ����ǥ ����
        public void SetQuestToNPC(int questId)
        {
            Quest quest = allQuestList.Find(x => x.questId == questId);

            foreach (InteractNpc npc in npcs)
            {
                if (npc.NpcId == quest.questNpcId)
                {
                    currentQuestNpc = npc;
                    currentQuestNpc.SetQuest(quest);
                    break;
                }
            }
        }


        // ����Ʈ�� ���� �� ����
        public void TakeQuest(Quest quest)
        {
            CurrentQuest = new CurrentQuest(quest);
            onQuestChanged?.Invoke();
        }


        // ���� ����Ʈ �Ϸ�
        public void CompleteQuest()
        {
            // ���� �ޱ�
            List<RewardItem> rewards = CurrentQuest.Quest.reward.items;

            for (int i = 0; i < rewards.Count; i++)
            {
                int id = rewards[i].itemId;
                int count = rewards[i].itemCount;

                // �Ҹ�ǰ
                if (count > 0)
                {
                    inventoryManager.AddCountableItem(id, count);
                }
                // ���
                else
                {
                    equipInventoryManager.AddEquipItem(id);
                }
            }

            // ���� npc�� ����Ʈ �ʱ�ȭ
            InteractNpc npc = npcs.Find(x => x.NpcId == CurrentQuest.Quest.questNpcId);
            npc.ResetQuest();

            // ���� ����Ʈ NPC���� �ο��ϱ�
            if (CurrentQuest.Quest.nextQuestId > 0)
            {
                SetQuestToNPC(CurrentQuest.Quest.nextQuestId);
            }

            // ���� ����Ʈ �ʱ�ȭ
            CurrentQuest = null;
            onQuestChanged?.Invoke();
        }


        // ����Ʈ �������� �߰��� �� üũ
        private void OnQuestItemChanged(int itemId, int itemIndex)
        {
            if (CurrentQuest == null)
                return;

            if (CurrentQuest.QuestType != QuestType.Collect)
                return;

            int questItemId = CurrentQuest.Quest.task.collect[0];

            if (questItemId != itemId)
                return;

            CurrentQuest.ChangeCurrentCount(inventoryManager.FindItemCount(itemId));

            if (CurrentQuest.IsCompleted)
            {
                currentQuestNpc.CreateQuestionMark();
            }

            onChangedCurrentQuestCount?.Invoke();
        }


        // ���Ͱ� ������� �� üũ
        public void KilledMonster(int monsterId)
        {
            if (CurrentQuest == null)
                return;

            if (CurrentQuest.QuestType != QuestType.Kill)
                return;

            int id = CurrentQuest.Quest.task.kill[0];

            if (id != monsterId)
                return;

            CurrentQuest.ChangeCurrentCount(CurrentQuest.CurrentCount + 1);

            if (CurrentQuest.IsCompleted)
            {
                currentQuestNpc.CreateQuestionMark();
            }

            onChangedCurrentQuestCount?.Invoke();
        }
    }
}