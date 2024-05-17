using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace lsy
{
    public class CurrentQuest
    {
        public int QuestId { get; private set; }
        //public Quest Quest { get; private set; }

        public QuestType QuestType { get; private set; }
        public bool IsCompleted { get; private set; }

        public int CurrentCount { get; private set; }
        public int GoalCount { get; private set; }

        public List<(int, int)> RewardList { get; private set; }

        // TODO : 퀘스트가 몬스터와 수집을 구분하지 않는 구조로 만들기..>???
        // 퀘스트 내용은 다르겠지만 처리하는 방법은 동일하게?
        public CurrentQuest(int id)
        {
            //this.Quest = quest;
            QuestId = id;
            IsCompleted = false;
            CurrentCount = 0;
            GoalCount = 0;

            QuestTable.TableData questInfo = Tables.QuestTable[QuestId];

            if (!string.IsNullOrEmpty(questInfo.Collect))
            {
                QuestType = QuestType.Collect;

                int[] collectInfo = questInfo.Collect.Split('/').ToInt();

                CurrentCount = Managers.Instance.InventoryManager.FindItemCount(collectInfo[0]);
                GoalCount = collectInfo[1];
            }
            else if (!string.IsNullOrEmpty(questInfo.Kill))
            {
                QuestType = QuestType.Kill;

                string[] killInfo = questInfo.Kill.Split('/');

                CurrentCount = 0;
                GoalCount = int.Parse(killInfo[1]);
            }

            // TOdo
            RewardList = new();

            string[] rewards = questInfo.Reward.Split('|');

            foreach (string reward in rewards)
            {
                int[] rewardInfo = reward.Split('/').ToInt();
                RewardList.Add((rewardInfo[0], rewardInfo[1]));
            }
        }

        public void ChangeCurrentCount(int count)
        {
            CurrentCount = count;
            IsCompleted = CurrentCount >= GoalCount ? true : false;
        }

        public QuestTable.TableData GetQuestTableData()
        {
            return QuestId > 0 ? Tables.QuestTable[QuestId] : null;
        }
    }


    public class QuestManager : IManager
    {
        private List<InteractNpc> npcs;

        public Action onQuestChanged;
        public Action onChangedCurrentQuestCount;

        private InteractNpc currentQuestNpc;

        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;
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
            int npcId = Tables.QuestTable[questId].NpcID;

            InteractNpc npc = npcs.Find(x => x.NpcId == npcId);
            npc.SetQuest(questId);

            currentQuestNpc = npc;
        }


        public void TakeQuest(int questId)
        {
            CurrentQuest = new CurrentQuest(questId);
            onQuestChanged?.Invoke();
        }


        public void CompleteQuest()
        {
            List<(int, int)> rewardList = CurrentQuest.RewardList;
            int questId = CurrentQuest.QuestId;

            CurrentQuest = null;

            for (int i = 0; i < rewardList.Count; i++)
            {
                int id = rewardList[i].Item1;
                int count = rewardList[i].Item2;

                inventoryManager.AddItem(id, count);
            }

            int currentNpcId = Tables.QuestTable[questId].NpcID;
            int nextQuestId = Tables.QuestTable[questId].NextQuestID;

            InteractNpc npc = npcs.Find(x => x.NpcId == currentNpcId);
            npc.ResetQuest();

            if (nextQuestId >= 0)
                SetQuestToNPC(nextQuestId);
            
            onQuestChanged?.Invoke();
        }


        private void OnChangedQuestItemCount(int itemId, int itemIndex)
        {
            if (CurrentQuest == null)
                return;

            if (CurrentQuest.QuestType != QuestType.Collect)
                return;

            //int questItemId = CurrentQuest.Quest.task.collect[0];

            //if (questItemId != itemId)
            //    return;

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

            //int targetMonsterid = CurrentQuest.Quest.task.kill[0];

            //if (targetMonsterid != monsterId)
            //    return;

            CurrentQuest.ChangeCurrentCount(CurrentQuest.CurrentCount + 1);

            if (CurrentQuest.IsCompleted)
            {
                currentQuestNpc.CreateQuestionMark();
            }

            onChangedCurrentQuestCount?.Invoke();
        }
    }
}
