using System;
using System.Linq;
using System.Collections.Generic;

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
            if (Quest.task.talk.Count != 0)
            {
                QuestType = QuestType.Talk;
            }
            else if (Quest.task.collect.Count != 0)
            {
                QuestType = QuestType.Collect;
                GoalCount = Quest.task.collect[1];

                // ���� �������� ������ ���� ���� �־ �ѹ��� üũ�ؾ���
                CurrentCount = Managers.Instance.InventoryManager.FindItemCount(Quest.task.collect[0]);
            }
            else
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
        private List<InteractNPC> npcs;

        public Action onQuestChanged;
        public Action onCurrentQuestItemCountChanged;

        private List<Quest> allQuestList => Managers.Instance.JsonManager.jsonQuest.quests;
        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;
        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        public CurrentQuest CurrentQuest { get; private set; }

        // Ư�� Ÿ�ֿ̹� �ش� NPC���� ����Ʈ�� �� �� �ִ� ����� �ʿ�
        // ����Ʈ�� �Ϸ���� ���� ���⸦ ���ļ� ���� �� �� �ְ��ϱ�
        // ���� ����Ʈ �ϳ��� ������ �ϱ�
        // ����Ʈ ������ �ʿ����
        public void Initialize()
        {
            npcs = UnityEngine.Object.FindObjectsOfType<InteractNPC>().ToList();

            inventoryManager.onItemAdded += OnQuestItemChanged;
            inventoryManager.onItemChanged += OnQuestItemChanged;
            inventoryManager.onItemUsed += OnQuestItemChanged;
            //SetQuestToNPC();
        }


        // NPC���� ����Ʈ �ο�
        public void SetQuestToNPC(int questId, int npcId)
        {
            Quest quest = allQuestList.Find(x => x.questId == questId);

            foreach (InteractNPC npc in npcs)
            {
                if (npc.NpcId == npcId)
                {
                    npc.SetQuest(quest);
                    break;
                }
            }
        }


        // ����Ʈ�� �޾��� �� ����
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
            

            // ���� ����Ʈ �ޱ� or NPC���� �ο��ϱ�

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
            onCurrentQuestItemCountChanged?.Invoke();
        }
    }
}
