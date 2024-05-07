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
            if (Quest.task.collect.Count != 0)
            {
                QuestType = QuestType.Collect;
                GoalCount = Quest.task.collect[1];

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
        // TODO
        //private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;
        public CurrentQuest CurrentQuest { get; private set; }



        public void Init()
        {
            npcs = UnityEngine.Object.FindObjectsOfType<InteractNpc>().ToList();

            // TODO
            //inventoryManager.onItemAdded += OnChangedQuestItemCount;
            inventoryManager.onConsumableChanged += OnChangedQuestItemCount;
            inventoryManager.onItemUsed += OnChangedQuestItemCount;
        }


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


        public void TakeQuest(Quest quest)
        {
            CurrentQuest = new CurrentQuest(quest);
            onQuestChanged?.Invoke();
        }


        public void CompleteQuest()
        {
            List<RewardItem> rewards = CurrentQuest.Quest.reward.items;

            for (int i = 0; i < rewards.Count; i++)
            {
                int id = rewards[i].itemId;
                int count = rewards[i].itemCount;

                if (count > 0)
                {
                    //TODO
                    //inventoryManager.AddCountableItem(id, count);
                }
                else
                {
                    //TODO
                    //equipInventoryManager.AddEquipItem(id);
                }
            }

            InteractNpc npc = npcs.Find(x => x.NpcId == CurrentQuest.Quest.questNpcId);
            npc.ResetQuest();

            if (CurrentQuest.Quest.nextQuestId > 0)
            {
                SetQuestToNPC(CurrentQuest.Quest.nextQuestId);
            }

            CurrentQuest = null;
            onQuestChanged?.Invoke();
        }


        private void OnChangedQuestItemCount(int itemId, int itemIndex)
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
