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

                // 현재 아이템을 가지고 있을 수도 있어서 한번더 체크해야함
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

        // 특정 타이밍에 해당 NPC한테 퀘스트를 줄 수 있는 기능이 필요
        // 퀘스트가 완료됐을 때도 여기를 거쳐서 실행 될 수 있게하기
        // 현재 퀘스트 하나만 나오게 하기
        // 리스트 같은거 필요없음
        public void Initialize()
        {
            npcs = UnityEngine.Object.FindObjectsOfType<InteractNPC>().ToList();

            inventoryManager.onItemAdded += OnQuestItemChanged;
            inventoryManager.onItemChanged += OnQuestItemChanged;
            inventoryManager.onItemUsed += OnQuestItemChanged;
            //SetQuestToNPC();
        }


        // NPC에게 퀘스트 부여
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


        // 퀘스트를 받았을 떄 실행
        public void TakeQuest(Quest quest)
        {
            CurrentQuest = new CurrentQuest(quest);
            onQuestChanged?.Invoke();
        }


        // 현재 퀘스트 완료
        public void CompleteQuest()
        {
            // 보상 받기
            List<RewardItem> rewards = CurrentQuest.Quest.reward.items;

            for (int i = 0; i < rewards.Count; i++)
            {
                int id = rewards[i].itemId;
                int count = rewards[i].itemCount;

                // 소모품
                if (count > 0)
                {
                    inventoryManager.AddCountableItem(id, count);
                }
                // 장비
                else
                {
                    equipInventoryManager.AddEquipItem(id);
                }                
            }
            

            // 다음 퀘스트 받기 or NPC에게 부여하기

            // 현재 퀘스트 초기화
            CurrentQuest = null;
            onQuestChanged?.Invoke();
        }


        // 퀘스트 아이템이 추가될 때 체크
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
