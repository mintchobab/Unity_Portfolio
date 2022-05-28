using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy 
{
    public class QuestUIController : SceneUI
    {
        [SerializeField]
        private QuestTabPage progressQuestPage;

        [SerializeField]
        private QuestTabPage completedQuestPage;

        [SerializeField]
        private QuestDetailWindow questDetailWindow;

        private QuestManager questManager => Managers.Instance.QuestManager;


        private int progressingCount = 0;
        private int completedCount = 0;


        protected override void Awake()
        {
            base.Awake();
            Hide();
            Initialize();
        }


        // 1. 퀘스트 리스트의 카운트가 변경되면
        // 2. 추가 인지 삭제인지 구분하고
        // 3. 그거에 맞게 TabPage의 버튼 추가 / 삭제
        private void Initialize()
        {
            // 처음부터 퀘스트가 받아져 있는 상태 (Save & Load를 위해)
            progressQuestPage.AddQuestButton(questManager.ProgressingList, ShowQuestDetail);
            completedQuestPage.AddQuestButton(questManager.CompletedList, ShowQuestDetail);

            progressingCount = questManager.ProgressingList.Count;
            completedCount = questManager.CompletedList.Count;

            questManager.onChangeProgressingList += OnChangeProgressingList;

            questDetailWindow.gameObject.SetActive(false);
        }


        public void ShowQuestDetail(Quest quest)
        {
            questDetailWindow.SetQuest(quest);
            questDetailWindow.gameObject.SetActive(true);
        }


        private void OnChangeProgressingList(Quest quest, int count)
        {
            if (progressingCount < count)
            {
                progressQuestPage.AddQuestButton(quest, ShowQuestDetail);
                progressingCount = count;
            }
            else
            {
                progressQuestPage.RemoveQuestButton(quest);
                progressingCount = count;
            }
        }


        private void OnChangeCompletedList(Quest quest, int count)
        {
            //if ( > count)
            //    progressQuestPage.AddQuestButton(quest);
            //else
            //    progressQuestPage.RemoveQuestButton(quest);
        }
    }
}
